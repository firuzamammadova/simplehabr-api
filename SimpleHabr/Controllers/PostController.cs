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
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);
            var posts = _uow.Posts.Find(p => p.UserId == userid);
            var postsToReturn = _mapper.Map<IEnumerable<PostDetailDto>>(posts);
            return Ok(postsToReturn.OrderByDescending(d => d.SharedTime));
        }
        [HttpGet]
        [Route("getspecuserposts/{username}")]
        public ActionResult GetSpecUserPosts(string username)
        {
            var userid = _uow.Users.GetUserId(username);
            var posts = _uow.Posts.Find(p => p.UserId == userid);
            var postsToReturn = _mapper.Map<IEnumerable<PostDetailDto>>(posts);
            return Ok(postsToReturn.OrderByDescending(d => d.SharedTime));
        }
        [HttpGet]
        [Route("getallposts")]
        public ActionResult GetAllPosts()
        {
            var posts = _uow.Posts.GetAll();
            var postsToReturn = _mapper.Map<IEnumerable<PostDetailDto>>(posts);
            return Ok(postsToReturn.OrderByDescending(d=>d.SharedTime));
        }

        [HttpPost]
        [Route("sharepost")]
        public ActionResult SharePost([FromBody]PostDto thepost)
        {
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i=>i.Type=="UserId").Value);
            thepost.UserId = userid.ToString();
            thepost.SharedTime = DateTime.Now;
            var post = _mapper.Map<Post>(thepost);
            _uow.Posts.Add(post);
            _uow.Users.UpdatePosts(userid, _uow.Posts.GetAll().Where(i => i.UserId == userid).Select(i => i.Id).ToList());
            return Ok();
        }

        [HttpGet]
        [Route("detail/{id}")]
        public ActionResult GetPostById(string id)
        {
            var post = _uow.Posts.Get(new ObjectId(id));
            var thepost = _mapper.Map<PostDetailDto>(post);

            return Ok(new { thepost });
        }

        [HttpDelete]
        [Route("deletepost/{id}")]
        public ActionResult DeletePost(string id)
        {
            var userId = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);
            var postId = new ObjectId(id);
            var currPost = _uow.Posts.Get(postId);
            if (currPost.UserId == userId)
            {
                _uow.Posts.Delete(currPost);
                _uow.Users.DeletePost(userId, postId);

                return Ok();
            }
            else return Conflict();
        }
        [HttpPost]
        [Route("editpost")]
        public ActionResult EditPost([FromBody]PostDetailDto post)
        {
            var userId = new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value);

            Post thepost = _mapper.Map<Post>(post);
            if (thepost.UserId == userId)
            {
                _uow.Posts.Edit(thepost);
                return Ok(post);
            }
            else return Unauthorized();
        }
    }
}
