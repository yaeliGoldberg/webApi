using System.Collections.Generic;
using SONG.Models;
using System.Security.Cryptography.X509Certificates;


namespace SONG.interfaces;

public interface Isong
{
    List<songType> GetAll();

    songType? Get(int id);

    void Add(songType song);

    void Update(songType song);

    void Delete(int id);
    int Count { get; }
}