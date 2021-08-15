using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coop.Application.Advertisement
{
    public interface IAdvertisementService
    {
        AdvertisementListViewModel GetPage(int page,int pageSize);

        AdvertisementListViewModel GetNewAdvertisements(int page, int pageSize);

        AdvertisementListViewModel GetForUser(Guid userId, int page, int pageSize);

        Task CreateAdvertisementAsync(CreateAdvertisementInputModel model, CancellationToken token);

        Guid GetOwnerId(Guid adId);
        
        Task ArchiveAsync(Guid adId, CancellationToken token);
        
        
        Task PublishAsync(Guid adId, Guid userId, CancellationToken token);
    }
}