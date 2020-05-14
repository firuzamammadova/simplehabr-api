using System;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class CommentUsernameResolver : IValueResolver<Comment, CommentDto, string>
    {
        private IUnitOfWork _uow;

        public CommentUsernameResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public string Resolve(Comment source, CommentDto destination, string destMember, ResolutionContext context)
        {
            return _uow.Users.GetUsername(source.UserId);
        }
    }
}
