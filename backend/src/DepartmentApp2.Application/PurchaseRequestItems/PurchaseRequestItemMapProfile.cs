using AutoMapper;
using DepartmentApp2.Entities;
using DepartmentApp2.PurchaseRequestItems.Dto;

namespace DepartmentApp2.PurchaseRequestItems
{
    public class PurchaseRequestItemMapProfile : Profile
    {
        public PurchaseRequestItemMapProfile()
        {
            CreateMap<PurchaseRequestItem, PurchaseRequestItemDto>();
            CreateMap<CreatePurchaseRequestItemDto, PurchaseRequestItem>();
            CreateMap<PurchaseRequestItemDto, PurchaseRequestItem>();
        }
    }
}
