using System;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class PostUsernameResolver : IValueResolver<Post, PostDto, string>
    {
        private IUnitOfWork _uow;

        public PostUsernameResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public string Resolve(Post source, PostDto destination, string destMember, ResolutionContext context)
        {
            return _uow.Users.GetUsername(source.UserId);

        }
    }
}
