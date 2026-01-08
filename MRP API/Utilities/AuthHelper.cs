using MRP_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Utilities
{
    internal static class AuthHelper
    {
        public static string? Authenticate(HttpListenerRequest request)
        {
            var authHeader = request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(authHeader))
                return null;

            if (!authHeader.StartsWith("Bearer "))
                return null;

            var token = authHeader.Substring("Bearer ".Length);

            if (!TokenService.IsTokenValid(token))
                return null;

            return TokenService.GetUsernameFromToken(token);
        }
    }
}
