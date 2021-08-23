using System;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Coop.Domain.Realties;

namespace Coop.Application.Realty
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<RealtyPay, RealtyPayViewModel>()
                .ForMember(r => r.Sum, m => m.MapFrom(
                    r => r.PaySum))
                .ForMember(r => r.DateTime, m => m.MapFrom(
                    r => r.DateTime))
                .ForMember(r => r.PayerName, m => m.MapFrom(
                    r => r.PayerName));

            CreateMap<Domain.Realties.Realty, RealtyListItemViewModel>()
                .ForMember(r => r.Id, m => m.MapFrom(
                    r => r.Id))
                .ForMember(r => r.InventoryNumber, m => m.MapFrom(
                    r => r.InventoryNumber))
                .ForMember(r => r.Balance, m => m.MapFrom(
                    r => r.Debts != null && r.Debts.Any()? r.Debts.FirstOrDefault(d=>d.DateTime==r.Debts.Max(rd=>rd.DateTime)).Sum.ToString():"Нет данных"))
                .ForMember(r => r.OwnerId, m => m.MapFrom(
                    r => r.GetCurrentOwnerId()));

            CreateMap<Domain.Realties.Realty, RealtyFullViewModel>()
                .ForMember(r => r.Id, m => m.MapFrom(
                    r => r.Id))
                .ForMember(r => r.Number, m => m.MapFrom(
                    r => r.InventoryNumber))
                .ForMember(r => r.OwnerId, m => m.MapFrom(
                    r => r.Owners == null || r.Owners.Count == 0
                        ? Guid.Empty
                        : r.Owners.First(d => d.TransferDate == r.Owners.Max(rd => rd.TransferDate)).UserId))
                .ForMember(r => r.CurrentDebt, m => m.MapFrom(
                    r => r.Debts == null || r.Debts.Count == 0
                        ? "Нет данных"
                        : r.Debts.First(d => d.DateTime == r.Debts.Max(rd => rd.DateTime)).Sum
                            .ToString(CultureInfo.InvariantCulture)))
                .ForMember(r => r.Pays, m => m.MapFrom(
                    r => r.Pays));
        }
    }
}