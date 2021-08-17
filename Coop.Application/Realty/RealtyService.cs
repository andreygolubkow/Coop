using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coop.Application.Common;
using Coop.Domain.Common;

namespace Coop.Application.Realty
{
    public class RealtyService:IRealtyService
    {
        private readonly IRepository<Domain.Realties.Realty> _repository;
        private readonly IMapper _mapper;

        public RealtyService(IRepository<Domain.Realties.Realty> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public RealtyListViewModel GetPage(int pageSize, int pageNum)
        {
            var articles = _repository.GetAll()
                .Where(a => a.IsActive)
                .OrderBy(a => a.InventoryNumber);
            var count = articles.Count();
            return new RealtyListViewModel()
            {
                PageSize = pageSize,
                CurrentPage = pageNum,
                TotalPages = count / pageSize + (count % pageSize == 0 ? 0 : 1),
                Items = articles
                    .Skip((pageNum-1) * pageSize)
                    .Take(pageSize)
                    .ProjectTo<RealtyListItemViewModel>(_mapper.ConfigurationProvider)
                    .ToList()
            };
        }

        public async Task AddRealty(NewRealtyInputModel model,CancellationToken token)
        {
            if (_repository.GetAll().Any(r => r.InventoryNumber == model.InventoryNumber && r.IsActive))
            {
                throw new ArgumentException("Такой объект уже есть, измените номер");
            }
            var realty = Domain.Realties.Realty.Create(model.InventoryNumber);
            await _repository.AddAsync(realty, token);
            if (!await _repository.SaveAsync(token)) throw new DatabaseException();
        }
    }
}