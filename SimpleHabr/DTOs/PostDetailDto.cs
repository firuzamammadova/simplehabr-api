using System;
using System.Collections.Generic;
using SimpleHabr.Models;

namespace SimpleHabr.DTOs
{
    public class PostDetailDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Header { get; set; }

        public string PhotoUrl { get; set; }

        public string Text { get; set; }

        public DateTime SharedTime { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
