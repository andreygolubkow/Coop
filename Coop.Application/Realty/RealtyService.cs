using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coop.Application.Common;
using Coop.Domain.Common;
using Coop.Domain.Realties;
using Microsoft.EntityFrameworkCore;

namespace Coop.Application.Realty
{
    public class RealtyService : IRealtyService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<RealtyDebt> _debtsRepository;
        private readonly IRepository<Domain.Realties.RealtyOwner> _realtyOwnerRepository;
        private readonly IRepository<Domain.Realties.Realty> _repository;

        public RealtyService(IRepository<Domain.Realties.Realty> repository,
            IRepository<Domain.Realties.RealtyOwner> realtyOwnerRepository, IMapper mapper,
            IRepository<RealtyDebt> debtsRepository)
        {
            _repository = repository;
            _realtyOwnerRepository = realtyOwnerRepository;
            _mapper = mapper;
            _debtsRepository = debtsRepository;
        }

        public RealtyListViewModel GetPage(int pageSize, int pageNum)
        {
            var articles = _repository.GetAll()
                .Where(a => a.IsActive)
                .OrderBy(a => a.InventoryNumber);
            var count = articles.Count();
            return new RealtyListViewModel
            {
                PageSize = pageSize,
                CurrentPage = pageNum,
                TotalPages = count / pageSize + (count % pageSize == 0 ? 0 : 1),
                Items = articles
                    .Skip((pageNum - 1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<RealtyListItemViewModel>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }

        public RealtyListViewModel GetForUser(Guid userId)
        {
            
            var realty =  _repository.GetAll().AsNoTracking()
                .Include(r => r.Owners)
                .Where(r => r.IsActive)
                .Where(r => r.Owners != null && r.Owners.Count>0)
                .Where(r => r.Owners.First(ro => r.Owners.Max(o => o.TransferDate) == ro.TransferDate).UserId == userId)
                .OrderBy(r => r.InventoryNumber)
                .ProjectTo<RealtyListItemViewModel>(_mapper.ConfigurationProvider);

            return new RealtyListViewModel()
            {
                CurrentPage = 1,
                Items = realty.ToList(),
                PageSize = realty.Count(),
                TotalPages = 1
            };
        }

        public async Task AddRealty(NewRealtyInputModel model, CancellationToken token)
        {
            if (_repository.GetAll().Any(r => r.InventoryNumber == model.InventoryNumber && r.IsActive == true))
                throw new ArgumentException("Такой объект уже есть, измените номер");
            var realty = Domain.Realties.Realty.Create(model.InventoryNumber);
            await _repository.AddAsync(realty, token);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }

        public async Task SetOwner(Guid realtyId, Guid ownerId, CancellationToken token)
        {
            var realty = _repository.GetAll()
                .Include(o => o.Owners)
                .FirstOrDefault(r => r.Id == realtyId);
            if (realty == null) throw new ArgumentException("Объект не найден");
            var ownerRecord = realty.SetOwner(ownerId, DateTime.Now);
            await _realtyOwnerRepository.AddAsync(ownerRecord, token);
            _repository.Update(realty);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }

        public async Task<RealtyFullViewModel> GetFullInfoAsync(Guid realtyId, CancellationToken token)
        {
            var realty = _repository.GetAll()
                .Include(o => o.Owners)
                .FirstOrDefault(r => r.Id == realtyId);
            if (realty == null) throw new ArgumentException("Такого объекта нет");

            return _mapper.Map<Domain.Realties.Realty, RealtyFullViewModel>(realty);
        }

        public async Task Archive(Guid realty, CancellationToken token)
        {
            var item = _repository.Find(realty);
            if (item == null) throw new ArgumentException("Не найден объект");
            item.Archive();
            _repository.Update(item);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }
        
        public async Task SetDebt(Guid realtyId, decimal amount, CancellationToken token)
        {
            var realty = _repository.GetAll()
                .Include(o => o.Owners)
                .FirstOrDefault(r => r.Id == realtyId);
            if (realty == null) throw new ArgumentException("Объект не найден");
            var record = realty.SetDebt(amount, DateTime.Now);
            await _debtsRepository.AddAsync(record, token);
            _repository.Update(realty);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }

        public Guid? FindActiveIdByName(string inventoryNumber)
        {
            return _repository.GetAll().AsNoTracking().Where(r => r.IsActive)
                .FirstOrDefault(r => r.InventoryNumber == inventoryNumber)?.Id;
        }
    }
}