using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coop.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Coop.Application.RealtyOwner
{
    public class RealtyOwnerService : IRealtyOwnerService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Domain.Realties.RealtyOwner> _repository;

        public RealtyOwnerService(IRepository<Domain.Realties.RealtyOwner> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<RealtyOwnerViewModel> GetOwnersHistory(Guid realtyId)
        {
            return _repository.GetAll().AsNoTracking()
                .Where(r => r.RealtyId == realtyId)
                .OrderBy(r => r.TransferDate)
                .ProjectTo<RealtyOwnerViewModel>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}