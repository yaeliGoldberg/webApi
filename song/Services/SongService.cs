using SONG.Models;
using System.Collections.Generic;
using System.Linq;
using SONG.interfaces;

namespace SONG.Services
{
    public  class SongService : Isong
    {
        public List<songType> Songs { get; }
        public int nextId = 3;
        public SongService()
        {
            Songs = new List<songType>
            {
                new songType { Id = 1, Name = "Aba", singer = "motti" },
                new songType { Id = 2, Name = "Ima", singer = "yaakov" }
            };
        }

        public  List<songType> GetAll() => Songs;

        public  songType Get(int id) => Songs.FirstOrDefault(p => p.Id == id);

        public  void Add(songType s)
        {
            s.Id = nextId++;
            Songs.Add(s);
        }

        public  void Delete(int id)
        {
            var s = Get(id);
            if (s is null)
                return;

            Songs.Remove(s);
        }

        public  void Update(songType s)
        {
            var index = Songs.FindIndex(p => p.Id == s.Id);
            if (index == -1)
                return;

            Songs[index] = s;
        }

        public  int Count => Songs.Count();
    }
}