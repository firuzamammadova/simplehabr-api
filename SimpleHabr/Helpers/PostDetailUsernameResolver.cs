using System;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class PostDetailUsernameResolver : IValueResolver<Post, PostDetailDto, string>
    {
        private IUnitOfWork _uow;

        public PostDetailUsernameResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public string Resolve(Post source, PostDetailDto destination, string destMember, ResolutionContext context)
        {
            return _uow.Users.GetUsername(source.UserId);

        }
    }
}
