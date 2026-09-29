using AutoMapper;
using DepartmentApp2.Entities;
using DepartmentApp2.PurchaseOrders.Dto;

namespace DepartmentApp2.PurchaseOrders
{
    public class PurchaseOrderMapProfile : Profile
    {
        public PurchaseOrderMapProfile()
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>();
            CreateMap<CreatePurchaseOrderDto, PurchaseOrder>();
            CreateMap<PurchaseOrderDto, PurchaseOrder>();
        }
    }
}
