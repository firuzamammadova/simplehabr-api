using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

        public CommentController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("getcomments/{postId}")]
        public ActionResult GetComments(string postId)
        {
            //var posts = _uow.Posts(username).GetAll().ToList();
            var comments = _uow.Comments.Find(p => p.PostId == new ObjectId(postId));


            //var postsToReturn = _mapper.Map<IEnumerable<PostDto>>(posts);

            return Ok(comments);
        }

        [HttpPost]
        [Route("addcomment/{postId}")]
        public ActionResult AddComment(string postId, [FromBody]Comment comment)
        {

         
           
            comment.PostId =new ObjectId( postId);
            
            _uow.Comments.Add(comment);



            var thecomment = _uow.Comments.Find(p => p.Text == comment.Text).FirstOrDefault();
            _uow.Posts.AddComment(new ObjectId(postId), thecomment.Id);


            //collection.UpdateOne()

            return Ok(thecomment);
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
