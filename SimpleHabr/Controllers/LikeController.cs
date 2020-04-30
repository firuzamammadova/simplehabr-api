using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleHabr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : Controller
    {
        private readonly IUnitOfWork _uow;
        private IMapper _mapper;

        public LikeController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getclikes/{postId}")]
        public ActionResult GetLikes(string postId)
        {
            var likes = _uow.Likes.Find(p => p.PostId == new ObjectId(postId));


            var likestoReturn = _mapper.Map<IEnumerable<LikeDto>>(likes);

            return Ok(likestoReturn);
        }

        [HttpPost]
        [Route("like/{postId}")]
        public ActionResult AddLike(string postId)
        {

            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);
            var thelike = new Like();

            thelike.PostId = new ObjectId(postId);
            thelike.UserId = userid;
            _uow.Likes.Add(thelike);



            _uow.Posts.UpdateLikes(thelike.PostId,
                _uow.Likes.GetAll().
                Where(i => i.PostId == thelike.PostId).
                Select(i => i.Id).
                ToList()
                );

            return Ok();
        }

        [HttpDelete]
        [Route("dislike/{id}")]
        public ActionResult Deletelike(string likeid)
        {
            var id = new ObjectId(likeid);
            var thelike = _uow.Likes.Get(id);
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);
            _uow.Likes.Delete(thelike);

            _uow.Posts.UpdateLikes(thelike.PostId,
               _uow.Likes.GetAll().
               Where(i => i.PostId == thelike.PostId ).
               Select(i => i.Id).
               ToList()
               );
            return Ok();
        }


    }
}
