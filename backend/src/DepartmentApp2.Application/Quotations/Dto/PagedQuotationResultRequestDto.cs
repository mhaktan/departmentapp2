using System;
using Abp.Application.Services.Dto;

namespace DepartmentApp2.Quotations.Dto
{
    public class PagedQuotationResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? PurchaseRequestId { get; set; }
        public long? SupplierId { get; set; }
        public decimal? QuotationAmount { get; set; }
        public DateTime? QuotationDate { get; set; }
        public bool? IsSelected { get; set; }
        public decimal? QuotationAmountFrom { get; set; }
        public decimal? QuotationAmountTo { get; set; }
        public DateTime? QuotationDateFrom { get; set; }
        public DateTime? QuotationDateTo { get; set; }
    }
}
