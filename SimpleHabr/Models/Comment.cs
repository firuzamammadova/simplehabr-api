using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SimpleHabr.Models
{
    public class Comment: Document
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId PostId { get; set; }

        public string Text { get; set; }

        public DateTime SharedTime { get; set; }
    }
}
