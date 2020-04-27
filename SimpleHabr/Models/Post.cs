﻿using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace SimpleHabr.Models
{
    [BsonCollection("Posts")]

    public class Post: Document
    {

        [BsonRepresentation(BsonType.ObjectId)]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId UserId { get; set; }
        public string Header { get; set; }

        public string PhotoUrl { get; set; }

        public string Text { get; set; }

        public DateTime SharedTime { get; set; }

        public ICollection<ObjectId> Comments { get; set; }
    }
}
