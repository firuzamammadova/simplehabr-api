using System;
using MongoDB.Bson;

namespace SimpleHabr.Models
{
    [BsonCollection("Likes")]

    public class Like: Document
    {
        public ObjectId UserId { get; set; }
        public ObjectId PostId { get; set; }

    }
}
