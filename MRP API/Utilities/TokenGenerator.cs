using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Utilities
{
    internal class TokenGenerator
    {
        //private const string tokenKey = "TestKey";

        public static string GenerateToken(string username)
        {
            var token = string.Format("{0}:{1}", username, DateTime.UtcNow.Ticks);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var base64Token = Convert.ToBase64String(tokenBytes);
            return base64Token;
        }

        public static bool ValidateToken(string token)
        {
            try
            {
                var tokenBytes = Convert.FromBase64String(token);
                var tokenString = Encoding.UTF8.GetString(tokenBytes);
                return tokenString.Contains(":");
            } catch
            {
                return false;
            }
        }
    }
}
