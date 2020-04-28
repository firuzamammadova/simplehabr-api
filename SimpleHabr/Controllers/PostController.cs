using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using SimpleHabr.DTOs;
using SimpleHabr.Models;
using SimpleHabr.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleHabr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : Controller
    {
        private readonly IUnitOfWork _uow;

        public PostController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("getuserposts")]
        public ActionResult GetUserPosts()
        {
            var posts = _uow.Posts.Find(p => p.UserId == _uow.Users.GetUserId(User.Identity.Name));
            return Ok(posts);
        }
        [HttpGet]
        [Route("getallposts")]
        public ActionResult GetAllPosts()
        {
            var posts = _uow.Posts.GetAll(); return Ok(posts);
        }

        [HttpPost]
        [Route("sharepost")]
        public ActionResult SharePost([FromBody]Post post)
        {
            var userid = _uow.Users.GetUserId(User.Identity.Name);
            post.UserId = userid;

            _uow.Posts.Add(post);

            var thepost = _uow.Posts.Find(p => p.Header == post.Header).FirstOrDefault();
            _uow.Users.AddPost(userid, thepost.Id);

            return Ok(thepost.Id.ToString());
        }

        [HttpGet]
        [Route("detail/{id}")]
        public ActionResult GetPostById(string id)
        {
            var post = _uow.Posts.Get(new ObjectId(id));

            return Ok(post);
        }

        [HttpDelete]
        [Route("deletepost/{id}")]
        public ActionResult DeletePost(string id)
        {
            var postId = new ObjectId(id);
            _uow.Posts.Delete(_uow.Posts.Get(postId));
            _uow.Users.DeletePost(_uow.Users.GetUserId(User.Identity.Name), postId);
            return Ok();
        }
        [HttpPost]
        [Route("editpost")]
        public ActionResult EditPost([FromBody]PostDto post)
        {
            var thepost = _uow.Posts.Get(new ObjectId(post.Id));
            thepost.PhotoUrl = post.PhotoUrl;
            thepost.SharedTime = post.SharedTime;
            thepost.Text = post.Text;
            thepost.Header = post.Header;
            _uow.Posts.Edit(thepost);
            return Ok(post);
        }
    }
}
