using System;
using System.Linq;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class PostDetailLikeResolver : IValueResolver<Post, PostDetailDto, int>
    {

        private IUnitOfWork _uow;

        public PostDetailLikeResolver(IUnitOfWork uow) => _uow = uow;



        public int Resolve(Post source, PostDetailDto destination, int destMember, ResolutionContext context)
        {
            return _uow.Likes.GetAll().Where(i => i.PostId == source.Id).Count();
        }
    }
}
