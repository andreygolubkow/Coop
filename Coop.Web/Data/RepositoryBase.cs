using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coop.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Coop.Web.Data
{
    public class RepositoryBase<T>: IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task AddAsync(T entity, CancellationToken token)
        {
            await _context.AddAsync(entity, token);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public T Find(Guid id)
        {
            return _context.Find<T>(id);
        }

        public async Task<bool> SaveAsync(CancellationToken token)
        {
            return (await _context.SaveChangesAsync(token)) != 0;
        }
    } 
}