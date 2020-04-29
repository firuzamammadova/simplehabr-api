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
            //var posts = _uow.Posts(username).GetAll().ToList();
            var comments = _uow.Comments.Find(p => p.PostId == new ObjectId(postId));


            var commentstoReturn = _mapper.Map<IEnumerable<CommentDto>>(comments);

            return Ok(commentstoReturn);
        }

        [HttpPost]
        [Route("addcomment/{postId}")]
        public ActionResult AddComment(string postId, [FromBody]Comment thecomment)
        {

            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);


            thecomment.PostId =new ObjectId(postId);
            thecomment.SharedTime = DateTime.Now;
            //var thecomment = _mapper.Map<Comment>(comment);
            thecomment.UserId = userid;
            _uow.Comments.Add(thecomment);



            _uow.Posts.UpdateComments(thecomment.PostId,
                _uow.Comments.GetAll().
                Where(i => i.PostId == thecomment.PostId && i.UserId == userid).
                Select(i => i.Id).
                ToList()
                );

            //collection.UpdateOne()

            return Ok(thecomment);
        }

        [HttpDelete]
        [Route("/deletecomment/{id}")]
        public ActionResult DeleteComment(string commentid)
        {
            var id = new ObjectId(commentid);
            var thecomment = _uow.Comments.Get(id);
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);

            _uow.Posts.UpdateComments(thecomment.PostId,
               _uow.Comments.GetAll().
               Where(i => i.PostId == thecomment.PostId && i.UserId == userid).
               Select(i => i.Id).
               ToList()
               );
            return Ok();
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
        [Route("editcomment/{id}")]
        public ActionResult EditComment([FromBody]CommentDto comment)
        {
            Comment thecomment = _mapper.Map<Comment>(comment);
            _uow.Comments.Edit(thecomment);
            return Ok(comment);
        }
        /*
        [HttpGet]
        [Route("detail/{id}")]
        public ActionResult GetPostById(int id)
        {
            var post = _context.Posts.Include(c => c.Comments).ThenInclude(c => c.User).Include(u => u.User).Include(p => p.Photo).FirstOrDefault(i => i.Id == id);

            var postToReturn = _mapper.Map<PostDetailsDto>(post);

            return Ok(postToReturn);
        }*/
    }
}
