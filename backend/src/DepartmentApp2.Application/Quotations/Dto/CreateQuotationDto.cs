using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace DepartmentApp2.Quotations.Dto
{
    [AutoMapTo(typeof(Entities.Quotation))]
    public class CreateQuotationDto
    {
        public decimal QuotationAmount { get; set; }

        public DateTime QuotationDate { get; set; }

        public bool IsSelected { get; set; }

        public long PurchaseRequestId { get; set; }

        public long SupplierId { get; set; }

    }
}