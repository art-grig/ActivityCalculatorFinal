using ActivityCalculator.Data.Entities;
using ActivityCalculator.Services.Extensions;
using ActivityCalculator.Services.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActivityCalculator.Services
{
    public class OrmMappingProfile : Profile
    {
        public OrmMappingProfile()
        {
            CreateMap<DatasetBaseModel, ActivityDataset>()
                .ForMember(e => e.ActivityLogs, _ => _.Ignore())
                .ForMember(e => e.CreatedOn, _ => _.Ignore())
                .ReverseMap();

            CreateMap<DatasetModel, ActivityDataset>()
                .IncludeBase<DatasetBaseModel, ActivityDataset>()
                .ReverseMap()
                .IncludeBase<ActivityDataset, DatasetBaseModel>()
                .ForMember(m => m.DeletedIds, _ => _.Ignore());

            CreateMap<ActivityLogModel, ActivityLog>()
                .ForMember(e => e.Lifetime, _ => _.MapFrom(m => m.RegistrationDate.DaysDiff(m.LastActivityDate)))
                .ForMember(e => e.Dataset, _ => _.Ignore())
                .ForMember(e => e.DatasetId, _ => _.Ignore())
                .ForMember(e => e.CreatedOn, _ => _.Ignore())
                .ReverseMap();
        }
    }
}
