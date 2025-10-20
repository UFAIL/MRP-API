using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Models
{
    public class MediaEntry
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Type { get; set; }
        public required string Description { get; set; }
        public required string ReleaseYear { get; set; }
        public required string Genre { get; set; }
        public required string AgeRestriction { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
