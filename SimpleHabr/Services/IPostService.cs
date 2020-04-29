using System;
using System.Collections.Generic;
using MongoDB.Bson;
using SimpleHabr.Models;

namespace SimpleHabr.Services
{
    public interface IPostService : IGenericService<Post>
    {
        Post AddComment(ObjectId postId, ObjectId commentId);
        void UpdateComments(ObjectId postId, List<ObjectId> commentIds);
    }
}
