using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int MediaId { get; set; }
        public required string Username { get; set; }
        public int Stars { get; set; }
        public string? Comment { get; set; }
        public bool IsConfirmed { get; set; }
        public int Likes { get; set; }
        public List<string> LikedBy { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
