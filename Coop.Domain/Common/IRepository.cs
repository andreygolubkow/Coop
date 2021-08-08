using System.Linq;

namespace Coop.Domain.Common
{
    public interface IRepository<T> where T:class
    {
        IQueryable<T> GetAll();

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);
    }
}