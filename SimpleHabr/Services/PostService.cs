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
            _posts.ReplaceOne(post => post.Id == postId, thepost);
            return thepost;
        }
        public void UpdateComments(ObjectId postId, List<ObjectId> commentIds)
        {
            var thepost = _posts.Find(u => u.Id == postId).FirstOrDefault();
            if (thepost.Comments == null)
            {
                thepost.Comments = new List<ObjectId>();
            }
            thepost.Comments = commentIds;
            _posts.ReplaceOne(post => post.Id == postId, thepost);
        }
        public void UpdateLikes(ObjectId postId, List<ObjectId> likeIds)
        {
            var thepost = _posts.Find(u => u.Id == postId).FirstOrDefault();
            if (thepost.Likes == null)
            {
                thepost.Likes = new List<ObjectId>();
            }
            thepost.Likes = likeIds;
            _posts.ReplaceOne(post => post.Id == postId, thepost);
        }
    }
}
