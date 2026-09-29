using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using DepartmentApp2.Entities;
using DepartmentApp2.PurchaseOrders.Dto;
using DepartmentApp2.Analytics.Dto;
using DepartmentApp2.Authorization;
using DepartmentApp2.Flows;

namespace DepartmentApp2.PurchaseOrders
{
    public class PurchaseOrderAppService : AsyncCrudAppService<
        PurchaseOrder,
        PurchaseOrderDto,
        long,
        PagedPurchaseOrderResultRequestDto,
        CreatePurchaseOrderDto,
        PurchaseOrderDto>,
        IPurchaseOrderAppService
    {
        private readonly IFlowEngine _flowEngine;

        public PurchaseOrderAppService(IRepository<PurchaseOrder, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.PurchaseOrder_Read;
            GetAllPermissionName = PermissionNames.PurchaseOrder_Read;
            CreatePermissionName = PermissionNames.PurchaseOrder_Create;
            UpdatePermissionName = PermissionNames.PurchaseOrder_Update;
            DeletePermissionName = PermissionNames.PurchaseOrder_Delete;
        }

        protected override IQueryable<PurchaseOrder> CreateFilteredQuery(PagedPurchaseOrderResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.OrderNumber != null && x.OrderNumber.Contains(input.Keyword)))
                .WhereIf(!input.OrderNumber.IsNullOrWhiteSpace(), x => x.OrderNumber != null && x.OrderNumber.Contains(input.OrderNumber))
                .WhereIf(input.OrderDate.HasValue, x => x.OrderDate == input.OrderDate.Value)
                .WhereIf(input.DeliveryDate.HasValue, x => x.DeliveryDate == input.DeliveryDate.Value)
                .WhereIf(input.OrderAmount.HasValue, x => x.OrderAmount == input.OrderAmount.Value)
                .WhereIf(input.OrderDateFrom.HasValue, x => x.OrderDate >= input.OrderDateFrom.Value)
                .WhereIf(input.OrderDateTo.HasValue, x => x.OrderDate <= input.OrderDateTo.Value)
                .WhereIf(input.DeliveryDateFrom.HasValue, x => x.DeliveryDate >= input.DeliveryDateFrom.Value)
                .WhereIf(input.DeliveryDateTo.HasValue, x => x.DeliveryDate <= input.DeliveryDateTo.Value)
                .WhereIf(input.OrderAmountFrom.HasValue, x => x.OrderAmount >= input.OrderAmountFrom.Value)
                .WhereIf(input.OrderAmountTo.HasValue, x => x.OrderAmount <= input.OrderAmountTo.Value)
                .WhereIf(input.PurchaseRequestId.HasValue, x => x.PurchaseRequestId == input.PurchaseRequestId.Value)
                .WhereIf(input.SupplierId.HasValue, x => x.SupplierId == input.SupplierId.Value);
        }

        public override async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "PurchaseOrder", result);
            return result;
        }

        public override async Task<PurchaseOrderDto> UpdateAsync(PurchaseOrderDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "PurchaseOrder", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "PurchaseOrder", new { Id = input.Id });
        }
        [Abp.Authorization.AbpAuthorize(PermissionNames.PurchaseOrder_Read)]
        public List<GroupCountDto> GetGroupedCount(PurchaseOrderGroupedCountInput input)
        {
            // Whitelist — istemciden gelen alan adı doğrudan sorguya girmez.
            var allowed = new[] { "PurchaseRequestId", "SupplierId" };
            if (input.GroupBy == null || !allowed.Contains(input.GroupBy))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Gruplanabilir alan degil: {input.GroupBy}. Izin verilenler: {string.Join(", ", allowed)}");
            }

            var query = CreateFilteredQuery(input);

            switch (input.GroupBy)
            {
                case "PurchaseRequestId":
                    return query
                        .GroupBy(x => new { Key = x.PurchaseRequestId, Label = x.PurchaseRequest == null ? null : x.PurchaseRequest.RequestNumber })
                        .Select(g => new GroupCountDto
                        {
                            Key = g.Key.Key.ToString(),
                            Label = g.Key.Label ?? "(bos)",
                            Count = g.Count(),
                        })
                        .ToList();
                case "SupplierId":
                    return query
                        .GroupBy(x => new { Key = x.SupplierId, Label = x.Supplier == null ? null : x.Supplier.Name })
                        .Select(g => new GroupCountDto
                        {
                            Key = g.Key.Key.ToString(),
                            Label = g.Key.Label ?? "(bos)",
                            Count = g.Count(),
                        })
                        .ToList();
                default:
                    return new List<GroupCountDto>();
            }
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.PurchaseOrder_Read)]
        public decimal? GetStats(PurchaseOrderStatsInput input)
        {
            var query = CreateFilteredQuery(input);

            if (input.Aggregate == "avgDayDiff")
            {
                var allowedDates = new[] { "OrderDate", "DeliveryDate" };
                if (!allowedDates.Contains(input.FromField) || !allowedDates.Contains(input.ToField))
                {
                    throw new Abp.UI.UserFriendlyException("avgDayDiff icin gecerli iki tarih alani gerekli.");
                }
                switch (input.FromField + "|" + input.ToField)
                {
                    case "OrderDate|DeliveryDate":
                    {
                        var pairsOrderDateDeliveryDate = query
                            .Where(x => x.DeliveryDate != null)
                            .Select(x => new { A = x.OrderDate, B = x.DeliveryDate.Value })
                            .ToList();
                        if (pairsOrderDateDeliveryDate.Count == 0) return null;
                        return (decimal)pairsOrderDateDeliveryDate.Average(p => (p.B - p.A).TotalDays);
                    }
                    case "DeliveryDate|OrderDate":
                    {
                        var pairsDeliveryDateOrderDate = query
                            .Where(x => x.DeliveryDate != null)
                            .Select(x => new { A = x.DeliveryDate.Value, B = x.OrderDate })
                            .ToList();
                        if (pairsDeliveryDateOrderDate.Count == 0) return null;
                        return (decimal)pairsDeliveryDateOrderDate.Average(p => (p.B - p.A).TotalDays);
                    }
                    default: return null;
                }
            }

            var allowedNumeric = new[] { "OrderAmount" };
            if (!allowedNumeric.Contains(input.Field))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Toplanabilir alan degil: {input.Field}. Izin verilenler: {string.Join(", ", allowedNumeric)}");
            }
            switch (input.Field)
            {
                        case "OrderAmount": return input.Aggregate == "sum" ? query.Sum(x => (decimal?)x.OrderAmount)
                            : input.Aggregate == "min" ? query.Min(x => (decimal?)x.OrderAmount)
                            : input.Aggregate == "max" ? query.Max(x => (decimal?)x.OrderAmount)
                            : query.Average(x => (decimal?)x.OrderAmount);
                        default: return null;
            }
        }

    }
}
