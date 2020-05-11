using System;
using AutoMapper;
using MongoDB.Bson;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class UserIdPostResolver : IValueResolver<PostDetailDto, Post, ObjectId>
    {
        private IUnitOfWork _uow;

        public UserIdPostResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ObjectId Resolve(PostDetailDto source, Post destination, ObjectId destMember, ResolutionContext context)
        {
            return _uow.Users.GetUserId(source.Username);
        }
    }
}
