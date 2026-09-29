using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace DepartmentApp2.PurchaseOrders.Dto
{
    [AutoMapFrom(typeof(Entities.PurchaseOrder))]
    public class PurchaseOrderDto : EntityDto<long>
    {
        public string OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal OrderAmount { get; set; }

        public long PurchaseRequestId { get; set; }

        public long SupplierId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}