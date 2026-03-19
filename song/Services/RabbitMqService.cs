using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SongLog.Models;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;



namespace SongLog.Services
{
    public interface IRabbitMqService
    {
        Task PublishSongLog(SongLogMessage message);
    }

    public class RabbitMqService : IRabbitMqService, IDisposable
    {
        private IConnection connection;
        private IChannel channel;
        private const string QueueName = "song-logs";

        public RabbitMqService()
        {
            InitializeAsync().GetAwaiter().GetResult();
        }

        private async System.Threading.Tasks.Task InitializeAsync()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            // Declare queue (idempotent - creates if doesn't exist)
            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,      // Survives broker restart
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async Task PublishSongLog(SongLogMessage message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: QueueName,
                body: body);
        }

        public void Dispose()
        {
            channel?.CloseAsync().Wait();
            connection?.CloseAsync().Wait();
        }


        
    }
//שאלה לגבי ה  RabbitMqService - האם צריך להוסיף את ה  IActiveUser כדי לקבל את ה  activeUserId וה  activeUsername ולשלוח אותם ב  SongLogMessage? או שעדיף להשאיר את זה כמו שזה ולקבל את המידע הזה ב  SongLogWorker מהמסד נתונים או ממקור אחר?
    public static partial class KsPizzaExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddHostedService<SongLogWorker>();
            return services;
        }
    }
}
