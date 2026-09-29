using AutoMapper;
using DepartmentApp2.Entities;
using DepartmentApp2.Employees.Dto;

namespace DepartmentApp2.Employees
{
    public class EmployeeMapProfile : Profile
    {
        public EmployeeMapProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<EmployeeDto, Employee>();
        }
    }
}
