using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SimpleHabr.Models
{
    [BsonCollection("Comments")]

    public class Comment: Document
    {
        public ObjectId PostId { get; set; }

        public string Text { get; set; }

        public DateTime SharedTime { get; set; }
    }
}
