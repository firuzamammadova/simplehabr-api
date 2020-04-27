﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace SimpleHabr.Models
{
    public class Document : IDocument
    {
        public ObjectId Id { get; set; }
    }

    public interface IDocument
    {
        //[BsonId]
        ObjectId Id { get; set; }
    }
}
