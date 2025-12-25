using user.Models;
using System.Collections.Generic;
using System.Linq;
using user.interfaces;
using System.Text.Json;

namespace user.Services
{
    public  class userService : Iuser
    {
        public List<userType> Users { get; }
        public int nextId = 1;
        

private string filePath;
        public userService(IWebHostEnvironment webHost)
        {
          
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Users.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                var content = jsonFile.ReadToEnd();
                Users = JsonSerializer.Deserialize<List<userType>>(content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            var text = JsonSerializer.Serialize(Users);
            File.WriteAllText(filePath, text);
        }



        public  List<userType> GetAll() => Users;

        public  userType Get(int id) => Users.FirstOrDefault(p => p.Id == id);

        public  void Add(userType s)
        {
            s.Id = nextId++;
            Users.Add(s);
              saveToFile();
        }

        public  void Delete(int id)
        {
            var s = Get(id);
            if (s is null)
                return;

            Users.Remove(s);
              saveToFile();
        }

        public  void Update(userType s)
        {
            var index = Users.FindIndex(p => p.Id == s.Id);
            if (index == -1)
                return;

            Users[index] = s;
              saveToFile();
        }

        public  int Count => Users.Count();
    }
}