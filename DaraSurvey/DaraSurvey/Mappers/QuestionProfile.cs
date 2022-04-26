using AutoMapper;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using System;

namespace DaraSurvey.Services.SurveryServices.Mappers
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<QuestionCreation, Question>()
               .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ----------------------

            CreateMap<QuestionUpdation, Question>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.Created, opt => opt.Ignore())
                .ForMember(o => o.Updated, opt => opt.MapFrom(src => DateTime.UtcNow));

            // ----------------------

            CreateMap<Question, SurveyQuestion>()
               .ForMember(o => o.QuestionId, opt => opt.MapFrom(src => src.Id))
               .ForMember(o => o.Text, opt => opt.MapFrom(src => src.Text))
               .ForMember(o => o.IsRequired, opt => opt.MapFrom(src => src.IsRequired));
            //.ForMember(o => o.WidgetData, opt => opt.MapFrom(src => src.Widget.Data));

            // ----------------------

            CreateMap<Question, QuestionRes>()
                .ForMember(o => o.SurveyTitle, opt => opt.MapFrom(src => src.Survey.Title));
        }
    }
}
