using AutoMapper;
using DepartmentApp2.Entities;
using DepartmentApp2.Suppliers.Dto;

namespace DepartmentApp2.Suppliers
{
    public class SupplierMapProfile : Profile
    {
        public SupplierMapProfile()
        {
            CreateMap<Supplier, SupplierDto>();
            CreateMap<CreateSupplierDto, Supplier>();
            CreateMap<SupplierDto, Supplier>();
        }
    }
}
