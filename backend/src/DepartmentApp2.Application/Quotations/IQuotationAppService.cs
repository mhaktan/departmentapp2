using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using DepartmentApp2.Analytics.Dto;
using DepartmentApp2.Quotations.Dto;

namespace DepartmentApp2.Quotations
{
    public interface IQuotationAppService : IAsyncCrudAppService<
        QuotationDto,
        long,
        PagedQuotationResultRequestDto,
        CreateQuotationDto,
        QuotationDto>
    {
        List<GroupCountDto> GetGroupedCount(QuotationGroupedCountInput input);
        decimal? GetStats(QuotationStatsInput input);
    }
}
