using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
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
        [Route("getposts/{username}")]
        public ActionResult GetPosts(string username)
        {
            //var posts = _uow.Posts(username).GetAll().ToList();
            var posts=_uow.Posts.Find(p => p.UserId == _uow.Users.GetUserId(username));


            //var postsToReturn = _mapper.Map<IEnumerable<PostDto>>(posts);

            return Ok(posts);
        }

        [HttpPost]
        [Route("sharepost")]
        public ActionResult SharePost([FromBody]Post post)
        {

            //var currUploadImageDto = CloudinaryMethods.UploadImageToCloudinary(uploadImageDto);

            //if (currUploadImageDto.File.Length > 0)
            //{
            //    post.Photo = new Image();
            //    post.Photo.PublicId = currUploadImageDto.PublicId;
            //    post.Photo.Url = currUploadImageDto.Url;
            //}
            var userid= _uow.Users.GetUserId(User.Identity.Name);
            post.UserId = userid;

            _uow.Posts.Add(post);



            var thepost = _uow.Posts.Find(p => p.Header == post.Header).FirstOrDefault();
           _uow.Users.AddPost(userid, thepost.Id);
  

            //collection.UpdateOne()

            return Ok(thepost.Id.ToString());
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
