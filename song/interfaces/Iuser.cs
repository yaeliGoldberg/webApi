using System.Collections.Generic;
using user.Models;
using System.Security.Cryptography.X509Certificates;



namespace user.interfaces;

public interface Iuser
{
    List<userType> GetAll();

    userType? Get(int id);

    void Add(userType user);

    void Update(userType user);

    void Delete(int id);
    int Count { get; }
}