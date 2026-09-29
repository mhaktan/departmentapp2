using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp2.Analytics.Dto;
using DepartmentApp2.StateMachine.Dto;
using DepartmentApp2.PurchaseRequests.Dto;

namespace DepartmentApp2.PurchaseRequests
{
    public interface IPurchaseRequestAppService : IAsyncCrudAppService<
        PurchaseRequestDto,
        long,
        PagedPurchaseRequestResultRequestDto,
        CreatePurchaseRequestDto,
        PurchaseRequestDto>
    {
        Task<PurchaseRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input);
        List<GroupCountDto> GetGroupedCount(PurchaseRequestGroupedCountInput input);
        decimal? GetStats(PurchaseRequestStatsInput input);
        Task<PurchaseRequestReportDto> GetReportData(long id);
    }
}
