using SONG.Models;
using System.Collections.Generic;
using System.Linq;
using SONG.interfaces;
using System.Text.Json;

namespace SONG.Services
{
    public  class SongService : Isong
    {
        public List<songType> Songs { get; }
          



    private string filePath;
        public SongService(IWebHostEnvironment webHost)
        {
            //this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Songs.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                var content = jsonFile.ReadToEnd();
                Songs = JsonSerializer.Deserialize<List<songType>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            var text = JsonSerializer.Serialize(Songs);
            File.WriteAllText(filePath, text);
        }

        public int nextId = 1;
       

        public  List<songType> GetAll() => Songs;

        public  songType Get(int id) => Songs.FirstOrDefault(p => p.Id == id);

        public  void Add(songType s)
        {
            s.Id = nextId++;
            Songs.Add(s);
             saveToFile();
        }

        public  void Delete(int id)
        {
            var s = Get(id);
            if (s is null)
                return;

            Songs.Remove(s);
             saveToFile();
        }

        public  void Update(songType s)
        {
            var index = Songs.FindIndex(p => p.Id == s.Id);
            if (index == -1)
                return;

            Songs[index] = s;
             saveToFile();
        }

        public  int Count => Songs.Count();
    }
}