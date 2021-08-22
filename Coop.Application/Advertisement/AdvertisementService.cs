using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coop.Application.Common;
using Coop.Domain.Common;

namespace Coop.Application.Advertisement
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.Advertisements.Advertisement> _repository;

        public AdvertisementService(IRepository<Domain.Advertisements.Advertisement> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public AdvertisementListViewModel GetPage(int page, int pageSize)
        {
            var ads = _repository.GetAll()
                .Where(a => a.IsActive && a.IsPublished)
                .OrderBy(a => a.CreatedAt);
            var count = ads.Count();
            return new AdvertisementListViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = PageViewModel<object>.CalculateTotalPages(pageSize, count),
                Items = ads.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<AdvertisementListItemViewModel>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }

        public AdvertisementListViewModel GetNewAdvertisements(int page, int pageSize)
        {
            var ads = _repository.GetAll()
                .Where(a => a.IsActive && !a.IsPublished)
                .OrderBy(a => a.CreatedAt);
            var count = ads.Count();
            return new AdvertisementListViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = PageViewModel<object>.CalculateTotalPages(pageSize, count),
                Items = ads.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<AdvertisementListItemViewModel>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }

        public AdvertisementListViewModel GetForUser(Guid userId, int page, int pageSize)
        {
            var ads = _repository.GetAll()
                .Where(a => a.IsActive)
                .Where(a => a.AuthorId == userId)
                .OrderBy(a => a.CreatedAt);
            var count = ads.Count();
            return new AdvertisementListViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = PageViewModel<object>.CalculateTotalPages(pageSize, count),
                Items = ads.Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<AdvertisementListItemViewModel>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }

        public async Task CreateAdvertisementAsync(CreateAdvertisementInputModel model, CancellationToken token)
        {
            var ad = Domain.Advertisements.Advertisement.Create(model.Title, model.Text, model.AuthorId);
            await _repository.AddAsync(ad, token);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }

        public Guid GetOwnerId(Guid adId)
        {
            var ad = _repository.Find(adId);
            Guard.Against.Null(ad, "Объявление не найдено");
            return ad.AuthorId;
        }

        public async Task ArchiveAsync(Guid adId, CancellationToken token)
        {
            var ad = _repository.Find(adId);
            Guard.Against.Null(ad, "Объявление не найдено");

            ad.Archive();
            _repository.Update(ad);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }


        public async Task PublishAsync(Guid adId, Guid userId, CancellationToken token)
        {
            var ad = _repository.Find(adId);
            Guard.Against.Null(ad, nameof(adId), "Не найдено объявление");
            ad.Publish(userId);
            _repository.Update(ad);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }
    }
}