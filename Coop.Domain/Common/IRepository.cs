using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Coop.Domain.Common
{
    public interface IRepository<T> where T:class
    {
        IQueryable<T> GetAll();
        
        Task AddAsync(T entity, CancellationToken token);

        void Update(T entity);

        void Delete(T entity);

        [CanBeNull]
        T Find(Guid id);

        Task<bool> SaveAsync(CancellationToken token);
    }
}