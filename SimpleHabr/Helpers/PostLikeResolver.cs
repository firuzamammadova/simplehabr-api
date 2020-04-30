using System;
using System.Linq;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class PostLikeResolver : IValueResolver<Post, PostDto, int>
    {

        private IUnitOfWork _uow;

        public PostLikeResolver(IUnitOfWork uow) => _uow = uow;

      

        public int Resolve(Post source, PostDto destination, int destMember, ResolutionContext context)
        {
            return _uow.Likes.GetAll().Where(i => i.PostId == source.Id).Count();
        }
    }
}
