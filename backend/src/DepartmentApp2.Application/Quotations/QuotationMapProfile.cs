using AutoMapper;
using DepartmentApp2.Entities;
using DepartmentApp2.Quotations.Dto;

namespace DepartmentApp2.Quotations
{
    public class QuotationMapProfile : Profile
    {
        public QuotationMapProfile()
        {
            CreateMap<Quotation, QuotationDto>();
            CreateMap<CreateQuotationDto, Quotation>();
            CreateMap<QuotationDto, Quotation>();
        }
    }
}
