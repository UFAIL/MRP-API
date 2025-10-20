using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Models
{
    public class User
    {
        //[JsonProperty("username")]
        public required string Username { get; set; }

        //[JsonIgnore]
        public required string Password { get; set; }
    }
}
