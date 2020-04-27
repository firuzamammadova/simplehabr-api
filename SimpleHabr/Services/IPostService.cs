using System;
using MongoDB.Bson;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public interface IPostService : IGenericService<Post>
    {
        Post AddComment(ObjectId postId, ObjectId commentId);
    }
}
