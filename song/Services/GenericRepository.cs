using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Generic.Interfaces;
using SONG.Models;
using user.Models;
using Microsoft.AspNetCore.Hosting;
using Entity.Interfaces;
using SongLog.Services;
using SongLog.Models;
using System.Threading.Tasks;

namespace Generic.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : IEntity
    {
        private readonly string filePath;
        private readonly List<T> list;
               public GenericRepository(IWebHostEnvironment env )
        {
            // The JSON files are named like "Users.json" and "Songs.json",
            // while the model types are named "userType" and "songType".
            // Normalize the type name to match the data file naming.
          
            var typeName = typeof(T).Name;
            if (typeName.EndsWith("Type", StringComparison.OrdinalIgnoreCase))
            {
                typeName = typeName.Substring(0, typeName.Length - 4);
            }

            typeName = char.ToUpperInvariant(typeName[0]) + typeName.Substring(1);
            filePath = Path.Combine(env.ContentRootPath, "Data", $"{typeName}s.json");

            if (!File.Exists(filePath))
            {
                list = new List<T>();
                return;
            }

            var content = File.ReadAllText(filePath);
            list = JsonSerializer.Deserialize<List<T>>(content,
                 new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new List<T>();
        }


        private void Save() => File.WriteAllText(filePath, JsonSerializer.Serialize(list));

        public List<T> GetAll() => list;

        public T? Get(int id) => list.FirstOrDefault(p => p.Id == id);

        public void Add(T obj)
        {
            obj.Id = list.Count == 0 ? 1 : list.Max(p => p.Id) + 1;
            list.Add(obj);
            Save();
        }

        public void Delete(int id)
        {
            var pizza = Get(id);
            if (pizza is null)
                return;

            list.Remove(pizza);
            Save();
        }

        public void Update(T obj)
        {
            var index = list.FindIndex(p => p.Id == obj.Id);
            if (index == -1)
                return;

            list[index] = obj;
            Save();
           
        }

        public int Count => list.Count;

       

    }
}
