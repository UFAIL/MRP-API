using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Models
{
    public class Rating
    {
        public required string Media { get; set; }
        public required string Username { get; set; }
        public int Value { get; set; }
        public string? Comment { get; set; }
    }
}
