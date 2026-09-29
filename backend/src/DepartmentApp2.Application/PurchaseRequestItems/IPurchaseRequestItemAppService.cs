using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp2.Analytics.Dto;
using DepartmentApp2.PurchaseRequestItems.Dto;

namespace DepartmentApp2.PurchaseRequestItems
{
    public interface IPurchaseRequestItemAppService : IAsyncCrudAppService<
        PurchaseRequestItemDto,
        long,
        PagedPurchaseRequestItemResultRequestDto,
        CreatePurchaseRequestItemDto,
        PurchaseRequestItemDto>
    {
        List<GroupCountDto> GetGroupedCount(PurchaseRequestItemGroupedCountInput input);
        decimal? GetStats(PurchaseRequestItemStatsInput input);
    }
}
