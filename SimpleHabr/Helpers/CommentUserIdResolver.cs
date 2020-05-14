using System;
using AutoMapper;
using MongoDB.Bson;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class CommentUserIdResolver : IValueResolver<CommentDto, Comment, ObjectId>
    {
        private IUnitOfWork _uow;

        public CommentUserIdResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public ObjectId Resolve(CommentDto source, Comment destination, ObjectId destMember, ResolutionContext context)
        {
            return _uow.Users.GetUserId(source.Username);
        }
    }
}
