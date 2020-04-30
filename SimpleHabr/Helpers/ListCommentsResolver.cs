using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

namespace SimpleHabr.Helpers
{
    public class ListCommentsResolver : IValueResolver<Post, PostDetailDto, ICollection<CommentDto>>
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public ListCommentsResolver(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public ICollection<CommentDto> Resolve(Post source, PostDetailDto destination, ICollection<CommentDto> destMember, ResolutionContext context)
        {
            var comments = _uow.Comments.GetAll().Where(i => i.PostId == source.Id);
            return _mapper.Map<IEnumerable<CommentDto>>(comments).ToList();
        }
    }
}
