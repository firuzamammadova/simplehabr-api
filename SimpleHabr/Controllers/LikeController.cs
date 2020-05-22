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
        [Route("getlikes/{postId}")]
        public ActionResult GetLikes(string postId)
        {
            var likes = _uow.Likes.Find(p => p.PostId == new ObjectId(postId));


            var likestoReturn = _mapper.Map<IEnumerable<LikeDto>>(likes);

            return Ok(likestoReturn);
        }
        [HttpGet]
        [Route("getuserlikes")]
        public ActionResult GetUserLikes()
        {
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);

            var likes = _uow.Likes.Find(p => p.UserId == userid);


            var likestoReturn = _mapper.Map<IEnumerable<LikeDto>>(likes);

            return Ok(likestoReturn);
        }
        [HttpGet]
        [Route("getspecuserlikes/{username}")]
        public ActionResult GetSpecUserLikes(string username)
        {
            var userid = _uow.Users.GetUserId(username);

            var likes = _uow.Likes.Find(p => p.UserId == userid);


            var likestoReturn = _mapper.Map<IEnumerable<LikeDto>>(likes);

            return Ok(likestoReturn);
        }


        [HttpPost]
        [Route("like/{postId}")]
        public ActionResult AddLike(string postId)
        {

            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);

            if (_uow.Likes.Find(i => i.PostId == new ObjectId(postId) && i.UserId == userid).Count() == 0)
            {
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
            else return Conflict();
        }

        [HttpDelete]
        [Route("dislike/{postid}")]
        public ActionResult Deletelike(string postid)
        {

            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);

            var id = new ObjectId(postid);
            var thelike = _uow.Likes.Find(i => i.PostId == id && i.UserId == userid).FirstOrDefault();

            if (_uow.Likes.Find(i => i.PostId == id && i.UserId == userid).Count() != 0)
            {

                _uow.Likes.Delete(thelike);

                _uow.Posts.UpdateLikes(thelike.PostId,
                   _uow.Likes.GetAll().
                   Where(i => i.PostId == thelike.PostId).
                   Select(i => i.Id).
                   ToList()
                   );
                return Ok();
            }
            else return Conflict();
        }


    }
}
