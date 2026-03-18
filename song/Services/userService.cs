using user.Models;
using System.Collections.Generic;
using System.Linq;
using user.interfaces;
using System.Text.Json;
using Generic.Interfaces;

namespace user.Services
{
    public class userService : Iuser
    {
        //         public List<userType> Users { get; }
        //         public int nextId = 1;


        // private string filePath;
        //         public userService(IWebHostEnvironment webHost)
        //         {

        //             this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Users.json");
        //             using (var jsonFile = File.OpenText(filePath))
        //             {
        //                 var content = jsonFile.ReadToEnd();
        //                 Users = JsonSerializer.Deserialize<List<userType>>(content,
        //                 new JsonSerializerOptions
        //                 {
        //                     PropertyNameCaseInsensitive = true
        //                 });
        //             }
        //         }

        //         private void saveToFile()
        //         {
        //             var text = JsonSerializer.Serialize(Users);
        //             File.WriteAllText(filePath, text);
        //         }

        private readonly IGenericRepository<userType> repository;

        public userService(IGenericRepository<userType> repository)
        {
            this.repository = repository;
        }

        public List<userType> GetAll() => repository.GetAll();
        public userType Get(int id) => repository.Get(id);
        public void Add(userType user) => repository.Add(user);
        public void Delete(int id) => repository.Delete(id);
        public void Update(userType user) => repository.Update(user);
        public int Count => repository.Count;
    }
}