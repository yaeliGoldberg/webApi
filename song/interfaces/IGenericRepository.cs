using System.Collections.Generic;
using Entity.Interfaces;
namespace Generic.Interfaces
{
    public interface IGenericRepository<T> where T : IEntity
    {
        List<T> GetAll();
        T? Get(int id);
        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        int Count { get; }
    }
}
