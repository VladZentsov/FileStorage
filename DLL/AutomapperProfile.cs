using AutoMapper;
using BLL.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<UserProfile, UserProfileDto>()
                .ReverseMap();

            //CreateMap<ProgramLanguageEvaluation, ProgramLanguageEvaluationDto>()
            //    .ForMember(p => p.Id, c => c.MapFrom(model => model.Id))
            //    .ForMember(p => p.ProgramLanguage, c => c.MapFrom(model => model.ProgramLanguage))
            //    .ForMember(p => p.WorkExperience, c => c.MapFrom(model => model.levelDiscription.WorkExperience))
            //    .ForMember(p => p.Created, c => c.MapFrom(model => model.levelDiscription.Result.Created))
            //    .ForMember(p => p.Score, c => c.MapFrom(model => model.levelDiscription.Result.Score))
            //    .ForMember(p => p.MaxScore, c => c.MapFrom(model => model.levelDiscription.Result.MaxScore))
            //    .ReverseMap();

            //CreateMap<ProgramLanguageEvaluation, LanguageExp>()
            //    .ForMember(p => p.Language, c => c.MapFrom(model => model.ProgramLanguage))
            //    .ForMember(p => p.YearExp, c => c.MapFrom(model => model.levelDiscription.WorkExperience));

            //CreateMap<ProgramLanguageEvaluationDto, LanguageExp>()
            //    .ForMember(p => p.Language, c => c.MapFrom(model => model.ProgramLanguage))
            //    .ForMember(p => p.YearExp, c => c.MapFrom(model => model.WorkExperience));

            CreateMap<RegisterModel, UserProfile>()
                .ReverseMap();

            CreateMap<StFile, FileDto>()
                .ReverseMap();

            CreateMap<(RegisterModel,string), UserProfile>()
                .ForMember(p => p.Id, c => c.MapFrom(model => model.Item1.Id))
                .ForMember(p => p.Name, c => c.MapFrom(model => model.Item1.Name))
                .ForMember(p => p.Surname, c => c.MapFrom(model => model.Item1.Surname))
                .ForMember(p => p.Description, c => c.MapFrom(model => model.Item1.Description))
                .ForMember(p => p.SysIdentityId, c => c.MapFrom(model => model.Item2))
                .ReverseMap();

            CreateMap<(UserProfile, SysIdentityUser), UserGeneralInfo>()
                .ForMember(p => p.Name, c => c.MapFrom(model => model.Item1.Name))
                .ForMember(p => p.Surname, c => c.MapFrom(model => model.Item1.Surname))
                .ForMember(p => p.CreatedDate, c => c.MapFrom(model => model.Item1.CreatedDate))
                .ForMember(p => p.Description, c => c.MapFrom(model => model.Item1.Description))
                .ForMember(p => p.Email, c => c.MapFrom(model => model.Item2.Email))
                .ForMember(p => p.UserName, c => c.MapFrom(model => model.Item2.UserName));


            CreateMap<UserGeneralInfo, UserBriefInfo>();
        }
    }
}
