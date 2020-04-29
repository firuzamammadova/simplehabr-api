using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using SimpleHabr.DTOs;
using SimpleHabr.Models;

namespace SimpleHabr.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.Id.ToString());
                }).ForMember(dest => dest.Comments, opt =>
               {
                   opt.MapFrom(src => src.Comments == null ? new List<string>() : src.Comments.Select(i => i.ToString()));

               }).ForMember(dest => dest.UserId, opt =>
                {
                    opt.MapFrom(src => src.UserId.ToString());
                });
            CreateMap<PostDto, Post>()
               .ForMember(dest => dest.Id, opt =>
               {
                   opt.MapFrom(src => new ObjectId(src.Id));
               }).ForMember(dest => dest.Comments, opt =>
               {
                   opt.MapFrom(src => src.Comments == null ? new List<ObjectId>() : src.Comments.Select(i =>new ObjectId(i)));

               }).ForMember(dest => dest.UserId, opt =>
               {
                   opt.MapFrom(src => new ObjectId(src.UserId));
               });

        }
    }
}
