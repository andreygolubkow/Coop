using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coop.Application.Realty
{
    public interface IRealtyService
    {
        public RealtyListViewModel GetPage(int pageSize, int pageNum);

        public Task AddRealty(NewRealtyInputModel model, CancellationToken token);

        public Task<RealtyFullViewModel> GetFullInfoAsync(Guid realtyId, CancellationToken token);

        public Task Archive(Guid realty, CancellationToken token);
        public Task SetOwner(Guid realtyId, Guid ownerId, CancellationToken token);
    }
}