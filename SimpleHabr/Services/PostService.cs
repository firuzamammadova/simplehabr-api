using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public class PostService : GenericService<Post>, IPostService
    {
        private readonly IMongoCollection<Post> _posts;


        public PostService(IMongoDatabase database) : base(database)
        {
            _posts = database.GetCollection<Post>("Posts");

        }
        public Post AddComment(ObjectId postId, ObjectId commentId)
        {
            var thepost = _posts.Find(u => u.Id == postId).FirstOrDefault();
            if (thepost.Comments == null)
            {
                thepost.Comments = new List<ObjectId>();
            }
            thepost.Comments.Add(commentId);
            _posts.ReplaceOne(book => book.Id == postId, thepost);
            return thepost;
        }
    }
}
