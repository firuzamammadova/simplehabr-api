using System;
namespace SimpleHabr.DTOs
{
    public class CommentDto
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string PostId { get; set; }

        public string Text { get; set; }

        public DateTime SharedTime { get; set; }
    }
}
