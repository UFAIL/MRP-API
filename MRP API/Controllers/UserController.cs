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
    internal class UserController
    {
        /*public static void HandleRegisterRequest(string requestBody)
        {
            var user = JsonConvert.DeserializeObject<User>(requestBody);

            if (UserService.RegisterUser(user))
            {
                Console.WriteLine(string.Format("User {0} registered successfully.", user.Username));
            } else
            {
                Console.WriteLine("User already exists.");
            }
        }*/

        public static void HandleRegisterRequest(HttpListenerContext context)
        {
            var requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();
            var user = JsonConvert.DeserializeObject<User>(requestBody);

            if (UserService.RegisterUser(user))
            {
                string response = string.Format("User {0} registered successfully.", user.Username);

                HttpResponseHelper.WriteResponse(context.Response, 200, new { response });
            } else
            {
                HttpResponseHelper.WriteResponse(context.Response, 401, new { error = "User already exists." });
            }
        }

        /*public static void HandleLoginRequest(string requestBody)
        {
            var user = JsonConvert.DeserializeObject<User>(requestBody);

            var authenticatedUser = UserService.AuthenticateUser(user.Username, user.Password);

            if (authenticatedUser != null)
            {
                var token = string.Format("{0}-{1}", user.Username, TokenGenerator.GenerateToken(authenticatedUser.Username));
                Console.WriteLine("Login successful.");
                Console.WriteLine(string.Format("Token: {0}", token));    //zeigt den Token an, der fuer die Authentifizierung benutzt werden kann
            } else
            {
                Console.WriteLine("Invalid username or password.");
            }
        }*/

        public static void HandleLoginRequest(HttpListenerContext context)    //sollte, sobald richtig implementiert, HandleLoginRequest ersetzen
        {
            var requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();
            var user = JsonConvert.DeserializeObject<User>(requestBody);

            var authenticatedUser = UserService.AuthenticateUser(user.Username, user.Password);
            if (authenticatedUser != null)
            {
                var token = string.Format("{0}-{1}", user.Username, TokenGenerator.GenerateToken(authenticatedUser.Username));
                TokenService.StoreToken(token, authenticatedUser.Username);
                //string responseText = string.Format("Login successful. Token: {0}>", token);
                HttpResponseHelper.WriteResponse(context.Response, 200, new { token });
                //HttpResponseHelper.WriteResponse(context.Response, 200, new { responseText });
            } else
            {
                HttpResponseHelper.WriteResponse(context.Response, 401, new { error = "Invalid username or password." });
            }
        }
    }
}
