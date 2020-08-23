using AutoMapper;
using FlutterApp.Api.DTOs;
using FlutterApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlutterApp.Api.Mapping
{
    public class MapProfile : Profile
    {
        // AutoMapper mapleme işlemleri.
        public MapProfile()
        {
            CreateMap<Categories, CategoryDto>().ReverseMap();
            CreateMap<Categories, CategoryWithQuestionsDto>().ReverseMap();
            CreateMap<Questions, QuestionsDto>().ReverseMap();
            CreateMap<Questions, QuestionsWithCategoryDto>().ReverseMap();
            CreateMap<Scores, ScoresDto>().ReverseMap();
        }
    }
}
