using System;
using System.Collections.Generic;

namespace SimpleHabr.DTOs
{
    public class PostDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Header { get; set; }

        public string PhotoUrl { get; set; }

        public string Text { get; set; }

        public DateTime SharedTime { get; set; }
        public ICollection<string> Comments { get; set; }
        public int Likes { get; set; }

    }
}
