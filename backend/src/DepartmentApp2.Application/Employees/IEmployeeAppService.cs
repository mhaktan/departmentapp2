using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp2.Analytics.Dto;
using DepartmentApp2.Employees.Dto;

namespace DepartmentApp2.Employees
{
    public interface IEmployeeAppService : IAsyncCrudAppService<
        EmployeeDto,
        long,
        PagedEmployeeResultRequestDto,
        CreateEmployeeDto,
        EmployeeDto>
    {
        List<GroupCountDto> GetGroupedCount(EmployeeGroupedCountInput input);
        Task<EmployeeReportDto> GetReportData(long id);
    }
}
