using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp2.Analytics.Dto;
using DepartmentApp2.PurchaseOrders.Dto;

namespace DepartmentApp2.PurchaseOrders
{
    public interface IPurchaseOrderAppService : IAsyncCrudAppService<
        PurchaseOrderDto,
        long,
        PagedPurchaseOrderResultRequestDto,
        CreatePurchaseOrderDto,
        PurchaseOrderDto>
    {
        List<GroupCountDto> GetGroupedCount(PurchaseOrderGroupedCountInput input);
        decimal? GetStats(PurchaseOrderStatsInput input);
    }
}
