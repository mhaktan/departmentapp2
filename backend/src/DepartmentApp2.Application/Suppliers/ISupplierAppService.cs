using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp2.Analytics.Dto;
using DepartmentApp2.Suppliers.Dto;

namespace DepartmentApp2.Suppliers
{
    public interface ISupplierAppService : IAsyncCrudAppService<
        SupplierDto,
        long,
        PagedSupplierResultRequestDto,
        CreateSupplierDto,
        SupplierDto>
    {
        Task<SupplierReportDto> GetReportData(long id);
    }
}
