using AutoMapper;
using DepartmentApp2.Entities;
using DepartmentApp2.Departments.Dto;

namespace DepartmentApp2.Departments
{
    public class DepartmentMapProfile : Profile
    {
        public DepartmentMapProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<DepartmentDto, Department>();
        }
    }
}
