using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace SimpleHabr.Models
{
    [BsonCollection("Users")]
    public class User: Document
    {

        public string Username { get; set; }
        public string ImgUrl { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Post> Posts { get; set; }
       // public ICollection<Like> Likes { get; set; }


    }
}
