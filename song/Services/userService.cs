using user.Models;
using System.Collections.Generic;
using System.Linq;
using user.interfaces;

namespace user.Services
{
    public  class userService : Iuser
    {
        public List<userType> Users { get; }
        public int nextId = 3;
        public userService()
        {
            Users = new List<userType>
            {
                new userType { Id = 1, Name = "Aba", age = 20 },
                new userType { Id = 2, Name = "Ima", age = 18 }
            };
        }

        public  List<userType> GetAll() => Users;

        public  userType Get(int id) => Users.FirstOrDefault(p => p.Id == id);

        public  void Add(userType s)
        {
            s.Id = nextId++;
            Users.Add(s);
        }

        public  void Delete(int id)
        {
            var s = Get(id);
            if (s is null)
                return;

            Users.Remove(s);
        }

        public  void Update(userType s)
        {
            var index = Users.FindIndex(p => p.Id == s.Id);
            if (index == -1)
                return;

            Users[index] = s;
        }

        public  int Count => Users.Count();
    }
}