using AutoMapper;
using DepartmentApp2.Approvals.Dto;
using DepartmentApp2.Entities;

namespace DepartmentApp2.Approvals
{
    public class ApprovalMapProfile : Profile
    {
        public ApprovalMapProfile()
        {
            CreateMap<ApprovalRecord, ApprovalRecordDto>();
            CreateMap<StatusChangeLog, StatusChangeLogDto>();
        }
    }
}
