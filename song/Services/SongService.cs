using SONG.Models;
using System.Collections.Generic;
using System.Linq;
using SONG.interfaces;
using System.Text.Json;
using Generic.Interfaces;
using Generic.Services;
using Microsoft.AspNetCore.SignalR;
using SONG.Hubs;

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
        private readonly IHubContext<ActivityHub> _hubContext;

        public SongService(IGenericRepository<songType> repository, IHubContext<ActivityHub> hubContext)
        {
            this.repository = repository;
            this._hubContext = hubContext;
        }




        public List<songType> GetAll() => repository.GetAll();

        public songType Get(int id) => repository.Get(id);

        public void Add(songType song)
        {
            repository.Add(song);
            NotifyUser(song.UserId, "songAdded", song);
        }

        public void Delete(int id)
        {
            var song = repository.Get(id);
            if (song != null)
            {
                repository.Delete(id);
                NotifyUser(song.UserId, "songDeleted", new { id });
            }
        }

        public void Update(songType song)
        {
            repository.Update(song);
            NotifyUser(song.UserId, "songUpdated", song);
        }

        private void NotifyUser(int userId, string action, object data)
        {
            var connectionIds = ActivityHub.GetUserConnections(userId.ToString());
            _hubContext.Clients.Clients(connectionIds).SendAsync("ReceiveUpdate", action, data);
        }


        public int Count => repository.Count;
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