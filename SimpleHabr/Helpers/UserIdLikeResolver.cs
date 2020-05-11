using System;
using AutoMapper;
using MongoDB.Bson;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class UserIdLikeResolver : IValueResolver<LikeDto, Like, ObjectId>
    {
        private IUnitOfWork _uow;

        public UserIdLikeResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ObjectId Resolve(LikeDto source, Like destination, ObjectId destMember, ResolutionContext context)
        {
            return _uow.Users.GetUserId(source.Username);
        }
    }
}
