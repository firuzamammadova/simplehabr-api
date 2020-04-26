using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("SimpleHabrDb");
            var collection = database.GetCollection<User>("Users");
            var user = collection.Find(u => u.Username == username).FirstOrDefault();


            //var postsToReturn = _mapper.Map<IEnumerable<PostDto>>(posts);

            return Ok(user.Posts);
        }

        [HttpPost]
        [Route("sharepost/{username}")]
        public ActionResult SharePost(string username,[FromBody]Post post)
        {

            //var currUploadImageDto = CloudinaryMethods.UploadImageToCloudinary(uploadImageDto);

            //if (currUploadImageDto.File.Length > 0)
            //{
            //    post.Photo = new Image();
            //    post.Photo.PublicId = currUploadImageDto.PublicId;
            //    post.Photo.Url = currUploadImageDto.Url;
            //}

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("SimpleHabrDb");
            var collection = database.GetCollection<User>("Users");
            var user=collection.Find(u => u.Username == username).FirstOrDefault();
            post.UserId = user.Id;
           // user.Posts = new List<Post>();
            user.Posts.Add(post);
            collection.ReplaceOne(book => book.Id == user.Id, user);

            //collection.UpdateOne()

            return Ok(post);
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
