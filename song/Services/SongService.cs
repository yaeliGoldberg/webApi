using SONG.Models;
using System.Collections.Generic;
using System.Linq;
using SONG.interfaces;
using System.Text.Json;
using Generic.Interfaces;
using Generic.Services;

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

        public SongService(IGenericRepository<songType> repository)
        {
            this.repository = repository;
        }




        public List<songType> GetAll() => repository.GetAll();

        public songType Get(int id) => repository.Get(id);

        public void Add(songType song) => repository.Add(song);

        public void Delete(int id) => repository.Delete(id);

        public void Update(songType song) => repository.Update(song);


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