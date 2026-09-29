using AutoMapper;
using DepartmentApp2.Entities;
using DepartmentApp2.PurchaseRequests.Dto;

namespace DepartmentApp2.PurchaseRequests
{
    public class PurchaseRequestMapProfile : Profile
    {
        public PurchaseRequestMapProfile()
        {
            CreateMap<PurchaseRequest, PurchaseRequestDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreatePurchaseRequestDto, PurchaseRequest>();
            CreateMap<PurchaseRequestDto, PurchaseRequest>();
        }
    }
}
