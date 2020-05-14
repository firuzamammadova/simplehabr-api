using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MongoDB.Bson;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class AutoMapperProfiles : Profile
    {


        public AutoMapperProfiles()
        {





            CreateMap<PostDto, Post>().ForMember(dest => dest.Comments, opt =>
                   {
                       opt.MapFrom(src => src.Comments == null ? new List<ObjectId>() : src.Comments.Select(i => new ObjectId(i)));

                   }).ForMember(dest => dest.UserId, opt =>
                   {
                       opt.MapFrom(src => new ObjectId(src.UserId));
                   }).ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

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
               })
              .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            /*
        CreateMap<PostDto, Post>()
               .ForMember(dest => dest.Id, opt =>
               {
                   opt.MapFrom(src => new ObjectId(src.Id));
               }).ForMember(dest => dest.Comments, opt =>
               {
                   opt.MapFrom(src => src.Comments == null ? new List<ObjectId>() : src.Comments.Select(i => new ObjectId(i)));

               }).ForMember(dest => dest.UserId, opt =>
               {
                   opt.MapFrom(src => uow.Users.GetUserId(src.Username));
               }).ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

*/

            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.Id.ToString());
                }).ForMember(dest => dest.PostId, opt =>
                  {
                      opt.MapFrom(src => src.PostId.ToString());
                  }).ForMember(dest => dest.Username, opt =>
                  {
                      opt.MapFrom<CommentUsernameResolver>();
                  }).ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CommentDto, Comment>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => new ObjectId(src.Id));
                })
            .ForMember(dest => dest.PostId, opt =>
               {
                   opt.MapFrom(src => new ObjectId(src.PostId));
               }).ForMember(dest => dest.UserId, opt =>
               {
                   opt.MapFrom<CommentUserIdResolver>();
               }).ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Post, PostDetailDto>()
               .ForMember(dest => dest.Id, opt =>
               {
                   opt.MapFrom(src => src.Id.ToString());
               }).ForMember(dest => dest.Comments, opt =>
               {
                   opt.MapFrom<ListCommentsResolver>();

               }).ForMember(dest => dest.Username, opt =>
               {
                   opt.MapFrom<PostDetailUsernameResolver>();
               }).ForMember(dest => dest.Likes, opt =>
               opt.MapFrom<PostDetailLikeResolver>()).
               ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PostDetailDto, Post>()
              .ForMember(dest => dest.Id, opt =>
              {
                  opt.MapFrom(src => new ObjectId(src.Id));
              }).ForMember(dest => dest.Comments, opt =>
              {
                  opt.MapFrom(src => src.Comments.Select(i => new ObjectId(i.Id)));

              }).ForMember(dest => dest.UserId, opt =>
              {
                  opt.MapFrom<UserIdPostResolver>();
              }).ForMember(dest => dest.Likes, opt =>
              {
                  opt.MapFrom(src => src.Likes.Select(i => new ObjectId(i.Id)));
              }).ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));




            CreateMap<LikeDto, Like>()
               .ForMember(dest => dest.Id, opt =>
               {
                   opt.MapFrom(src => new ObjectId(src.Id));
               }).ForMember(dest => dest.PostId, opt =>
               {
                   opt.MapFrom(src => new ObjectId(src.PostId));
               })
               .ForMember(obj => obj.UserId,
                exp => exp.MapFrom<UserIdLikeResolver>());


            CreateMap<Like, LikeDto>()
               .ForMember(dest => dest.Id, opt =>
               {
                   opt.MapFrom(src => src.Id.ToString());
               }).ForMember(dest => dest.PostId, opt =>
               {
                   opt.MapFrom(src => src.PostId.ToString());
               })
               .ForMember(obj => obj.Username,
                exp => exp.MapFrom<UsernameResolver>());
        }
    }
}
