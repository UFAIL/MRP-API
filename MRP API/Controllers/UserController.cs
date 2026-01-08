using MRP_API.Models;
using MRP_API.Services;
using MRP_API.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MRP_API.Controllers
{
    internal static class UserController
    {
        public static void HandleRegisterRequest(HttpListenerContext context)
        {
            var body = new StreamReader(context.Request.InputStream).ReadToEnd();
            var user = JsonConvert.DeserializeObject<User>(body);

            if (user == null ||
                string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    400,
                    new { error = "Invalid input" }
                );
                return;
            }

            bool created = UserService.RegisterUser(user);

            if (!created)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    409,
                    new { error = "User already exists" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                201,
                new { username = user.Username }
            );
        }

        public static void HandleLoginRequest(HttpListenerContext context)
        {
            var body = new StreamReader(context.Request.InputStream).ReadToEnd();
            var login = JsonConvert.DeserializeObject<User>(body);

            if (login == null ||
                string.IsNullOrWhiteSpace(login.Username) ||
                string.IsNullOrWhiteSpace(login.Password))
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    400,
                    new { error = "Invalid input" }
                );
                return;
            }

            var user = UserService.AuthenticateUser(login.Username, login.Password);

            if (user == null)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    401,
                    new { error = "Invalid credentials" }
                );
                return;
            }

            var token = TokenGenerator.GenerateToken(user.Username);
            TokenService.StoreToken(token, user.Username);

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                new { token }
            );
        }

        public static void HandleGetProfileRequest(HttpListenerContext context, string username)
        {
            var profile = UserService.GetProfile(username);

            if (profile == null)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    404,
                    new { error = "User not found" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                profile
            );
        }

        public static void HandleLeaderboardRequest(HttpListenerContext context)
        {
            var leaderboard = UserService.GetLeaderboard();

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                leaderboard
            );
        }
    }
}
