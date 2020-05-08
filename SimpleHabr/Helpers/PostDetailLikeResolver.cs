using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class PostDetailLikeResolver : IValueResolver<Post, PostDetailDto, List<LikeDto>>
    {

        private IUnitOfWork _uow;
        private IMapper _mapper;


        public PostDetailLikeResolver(IUnitOfWork uow ,IMapper mapper) { _uow = uow; _mapper = mapper; }



        public List<LikeDto> Resolve(Post source, PostDetailDto destination, List<LikeDto> destMember, ResolutionContext context)
        {
            var likes= _uow.Likes.GetAll().Where(i => i.PostId == source.Id);
            return _mapper.Map<IEnumerable<LikeDto>>(likes).ToList();
        }

       
    }
}
