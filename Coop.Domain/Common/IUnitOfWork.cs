using System.Threading;
using System.Threading.Tasks;

namespace Coop.Domain.Common
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangesAsync(CancellationToken token);
    }
}