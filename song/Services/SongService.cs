using SONG.Models;
using System.Collections.Generic;
using System.Linq;
using SONG.interfaces;
using System.Text.Json;
using Generic.Interfaces;
using Generic.Services;
using SongLog.Services;
using SongLog.Models;
using System;

namespace SONG.Services
{
    public class SongService : Isong
    {
        //  public List<songType> Songs { get; }
        //    private string filePath;
        //    public SongService(IWebHostEnvironment webHost)
        //     {
        //         //this.webHost = webHost;
        //         this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Songs.json");
        //         using (var jsonFile = File.OpenText(filePath))
        //         {
        //             var content = jsonFile.ReadToEnd();
        //             Songs = JsonSerializer.Deserialize<List<songType>>(content,
        //             new JsonSerializerOptions
        //             {
        //                 PropertyNameCaseInsensitive = true
        //             });
        //         }
        //     }
        // private void saveToFile()
        // {
        //     var text = JsonSerializer.Serialize(Songs);
        //     File.WriteAllText(filePath, text);
        // }


        private readonly IGenericRepository<songType> repository;
        private readonly IRabbitMqService rabbitMqService;
       // private readonly int activeUserId;
      //  private readonly string activeUsername;

        public SongService(IGenericRepository<songType> repository, IRabbitMqService rabbitMqService)
        {
            this.repository = repository;
            this.rabbitMqService = rabbitMqService;
        }




        public List<songType> GetAll() => repository.GetAll();

        public songType Get(int id) => repository.Get(id);

        public void Add(songType song) => repository.Add(song);

        public void Delete(int id) => repository.Delete(id);

        public void Update(songType song) => repository.Update(song);


        public int Count => repository.Count;


//                                 שאלה
//לשאול  את פניני לגבי המשתנים של ה activeUserId וה activeUsername


        private void QueueActivityBroadcast(songType song)
        {
            var message = new SongLogMessage
            {
                UserId =
                Username = Log.Username,
                SongName = Log.SongName,
                Timestamp = DateTime.UtcNow
            };

            rabbitMqService.PublishSongLog(message).Wait();
        }

    }






    public static partial class SongExtensions
    {
        public static IServiceCollection AddSong(this IServiceCollection services)
        {
            services.AddSingleton<IGenericRepository<songType>, GenericRepository<songType>>();
            services.AddScoped<Isong, SongService>();
            return services;
        }
    }
}