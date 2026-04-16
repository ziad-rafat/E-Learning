using AutoMapper;
using E_Learning.Dtos.Category;
using E_Learning.Dtos.Course;
using E_Learning.Dtos.Review;
using E_Learning.Dtos.SubCategory;
using E_Learning.Dtos.Topic;
using E_Learning.Dtos.User;
using E_Learning.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.Application.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<SubCategoryDto, SubCategory>().ReverseMap();
            CreateMap<CreateOrUpdateTopicDTO, Topic>().ReverseMap();
            CreateMap<GetAllTopicDTO, Topic>().ReverseMap();
            CreateMap<ReviewDTO, Review>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserLoginDTO, User>().ReverseMap();
            CreateMap<UserLoginInfo, User>().ReverseMap();
            CreateMap<RegisterDTO, User>().ReverseMap();
            CreateMap<CreateOrUpdateCourseDTO, Course>().ReverseMap();
            CreateMap<CourseListDTO, Course>().ReverseMap();
            CreateMap<CourseDTO, Course>().ReverseMap();
        }
    }
}
