using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private IMapper _mapper;

        public PostController(IUnitOfWork uow,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getuserposts")]
        public ActionResult GetUserPosts()
        {
            var posts = _uow.Posts.Find(p => p.UserId == new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value));
            var postsToReturn = _mapper.Map<IEnumerable<PostDto>>(posts);
            return Ok(postsToReturn);
        }
        [HttpGet]
        [Route("getallposts")]
        public ActionResult GetAllPosts()
        {
            var posts = _uow.Posts.GetAll();
            var postsToReturn = _mapper.Map<IEnumerable<PostDto>>(posts);
            return Ok(postsToReturn);
        }

        [HttpPost]
        [Route("sharepost")]
        public ActionResult SharePost([FromBody]Post thepost)
        {
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i=>i.Type=="UserId").Value);
            thepost.UserId = userid;

            _uow.Posts.Add(thepost);
            _uow.Users.UpdatePosts(userid, _uow.Posts.GetAll().Where(i => i.UserId == userid).Select(i => i.Id).ToList());
            return Ok(thepost.Id.ToString());
        }

        [HttpGet]
        [Route("detail/{id}")]
        public ActionResult GetPostById(string id)
        {
            var post = _uow.Posts.Get(new ObjectId(id));
            var thepost = new PostDetailDto()
            {
                Id = post.Id.ToString(),
                Header = post.Header,
                Text = post.Text,
                SharedTime = post.SharedTime,
                PhotoUrl = post.PhotoUrl,
                Username= User.Identity.Name,
                Comments = _uow.Comments.GetAll().Where(i => i.PostId == post.Id).Select(i => new CommentDto()
                {
                    Id = i.Id.ToString(),
                    PostId = i.PostId.ToString(),
                    UserId = i.UserId.ToString(),
                    SharedTime = i.SharedTime,
                    Text = i.Text
                }).ToList()
            };

            return Ok(thepost);
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
            Post thepost = _mapper.Map<Post>(post);
            _uow.Posts.Edit(thepost);
            return Ok(post);
        }
    }
}
