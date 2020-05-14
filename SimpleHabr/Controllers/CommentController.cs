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
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _uow;
        private IMapper _mapper;

        public CommentController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getcomments/{postId}")]
        public ActionResult GetComments(string postId)
        {
            var comments = _uow.Comments.Find(p => p.PostId == new ObjectId(postId));


            var commentstoReturn = _mapper.Map<IEnumerable<CommentDto>>(comments);

            return Ok(commentstoReturn);
        }

        [HttpPost]
        [Route("addcomment")]
        public ActionResult AddComment( [FromBody]CommentDto thecomment)
        {

            var username = User.Identity.Name;

                thecomment.SharedTime = DateTime.Now;
                thecomment.Username = username;
                var comment = _mapper.Map<Comment>(thecomment);
                _uow.Comments.Add(comment);



                _uow.Posts.UpdateComments(comment.PostId,
                    _uow.Comments.GetAll().
                    Where(i => i.PostId == comment.PostId).
                    Select(i => i.Id).
                    ToList()
                    );

                return Ok(thecomment);
         
           
        }

        [HttpDelete]
        [Route("deletecomment/{commentid}")]
        public ActionResult DeleteComment(string commentid)
        {

            var id = new ObjectId(commentid);
            var thecomment = _uow.Comments.Get(id);
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);
            if (thecomment.UserId == userid || _uow.Posts.Get(thecomment.PostId).UserId==userid)
            {
                _uow.Comments.Delete(thecomment);
                _uow.Posts.UpdateComments(thecomment.PostId,
                   _uow.Comments.GetAll().
                   Where(i => i.PostId == thecomment.PostId).
                   Select(i => i.Id).
                   ToList()
                   );
                return Ok();
            }
            else return Unauthorized();
            
        }
        [HttpGet]
        [Route("getusercomments")]
        public ActionResult GetUserComments()
        {
            var userid= new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);
            var comments = _uow.Comments.GetAll().Where(i => i.UserId == userid);
            var commentstoReturn = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentstoReturn);
        }
        [HttpPost]
        [Route("editcomment")]
        public ActionResult EditComment([FromBody]CommentDto comment)
        {
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);

            Comment thecomment = _mapper.Map<Comment>(comment);
            if (thecomment.UserId == userid)
            {
                _uow.Comments.Edit(thecomment);
                return Ok(comment);
            }
            else return Unauthorized();
        }

    }
}
