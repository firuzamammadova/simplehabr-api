using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleHabr.Models
{
    [BsonCollection("Users")]
    public class User: Document
    {

        public string Username { get; set; }
        public string ImgUrl { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<ObjectId> Posts { get; set; }
       // public ICollection<Like> Likes { get; set; }


    }
}
