using System;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class UsernameResolver : IValueResolver<Like,LikeDto,string>
    {
        private IUnitOfWork _uow;

        public UsernameResolver(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public string Resolve(Like source, LikeDto destination, string destMember, ResolutionContext context)
        {
            return _uow.Users.GetUsername(source.UserId);
        }
    }
}
