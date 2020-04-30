﻿using System;
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
            var postsToReturn = _mapper.Map<IEnumerable<PostDetailDto>>(posts);
            return Ok(postsToReturn);
        }
        [HttpGet]
        [Route("getallposts")]
        public ActionResult GetAllPosts()
        {
            var posts = _uow.Posts.GetAll();
            var postsToReturn = _mapper.Map<IEnumerable<PostDetailDto>>(posts);
            return Ok(postsToReturn);
        }

        [HttpPost]
        [Route("sharepost")]
        public ActionResult SharePost([FromBody]Post thepost)
        {
            var userid = new ObjectId(User.Claims.ToList().FirstOrDefault(i=>i.Type=="UserId").Value);
            thepost.UserId = userid;
            thepost.SharedTime = DateTime.Now;
            _uow.Posts.Add(thepost);
            _uow.Users.UpdatePosts(userid, _uow.Posts.GetAll().Where(i => i.UserId == userid).Select(i => i.Id).ToList());
            return Ok(thepost.Id.ToString());
        }

        [HttpGet]
        [Route("detail/{id}")]
        public ActionResult GetPostById(string id)
        {
            var post = _uow.Posts.Get(new ObjectId(id));
            var thepost = _mapper.Map<PostDetailDto>(post);

            return Ok(thepost);
        }

        [HttpDelete]
        [Route("deletepost/{id}")]
        public ActionResult DeletePost(string id)
        {
            var postId = new ObjectId(id);
            _uow.Posts.Delete(_uow.Posts.Get(postId));
            _uow.Users.DeletePost(new ObjectId(User.Claims.ToList().FirstOrDefault(i => i.Type == "UserId").Value), postId);

            return Ok();
        }
        [HttpPost]
        [Route("editpost")]
        public ActionResult EditPost([FromBody]PostDetailDto post)
        {
            Post thepost = _mapper.Map<Post>(post);
            _uow.Posts.Edit(thepost);
            return Ok(post);
        }
    }
}
