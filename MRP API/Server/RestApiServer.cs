using MRP_API.Controllers;
using MRP_API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Server
{
    internal class RestApiServer
    {
        /*public void StartServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:7810/");
            listener.Start();
            Console.WriteLine("Server is listening...");

            while(true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                //string clientInformation = "";

                if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/api/users/register")
                {
                    UserController.HandleRegisterRequest(context);
                } else if(request.HttpMethod == "POST" && request.Url.AbsolutePath == "api/users/login")
                {
                    UserController.HandleLoginRequest(context);
                } else if(request.HttpMethod == "POST" && request.Url.AbsolutePath == "api/users/createMedia")
                {
                    MediaController.HandleCreateMediaRequest(context);
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/getAllMedia")
                {
                    MediaController.HandleGetAllMediaRequest(context);
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/getMediaById")
                {
                    MediaController.HandleGetMediaByIdRequest(context);
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/updateMediaById")
                {
                    using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        var requestBody = reader.ReadToEnd();
                        MediaController.HandleUpdateMediaByIdRequest(requestBody);  //scheint gerade nicht zu funktionieren; muss noch bearbeitet werden
                    }
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/deleteMediaById")
                {
                    MediaController.HandleDeleteMediaByIdRequest(context);
                }
            }
        }*/

        public void StartServer()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:7810/");
            listener.Start();

            Console.WriteLine("MRP API running on http://localhost:7810");

            while (true)
            {
                var context = listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                try
                {
                    RouteRequest(context);
                }
                catch (Exception ex)
                {
                    HttpResponseHelper.WriteResponse(
                        response,
                        500,
                        new { error = "Internal server error", details = ex.Message }
                    );
                }
            }
        }

        private void RouteRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var path = request.Url!.AbsolutePath.TrimEnd('/');
            var method = request.HttpMethod;

            // Public Endpoints
            if (method == "POST" && path == "/api/users/register")
            {
                UserController.HandleRegisterRequest(context);
                return;
            }

            if (method == "POST" && path == "/api/users/login")
            {
                UserController.HandleLoginRequest(context);
                return;
            }

            // Authentication
            var username = AuthHelper.Authenticate(request);
            if (username == null)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    401,
                    new { error = "Unauthorized" }
                );
                return;
            }

            // User
            if (method == "GET" && path.StartsWith("/api/users/") && path.EndsWith("/profile"))
            {
                UserController.HandleGetProfileRequest(context, username);
                return;
            }

            if (method == "GET" && path == "/api/users/leaderboard")
            {
                UserController.HandleLeaderboardRequest(context);
                return;
            }

            // Media
            if (method == "POST" && path == "/api/media")
            {
                MediaController.HandleCreateMediaRequest(context, username);
                return;
            }

            /*if (method == "GET" && path == "/api/media")
            {
                MediaController.HandleGetAllMediaRequest(context);
                return;
            }*/

            if (method == "GET" && path == "/api/media")
            {
                MediaController.HandleSearchMediaRequest(context);
                return;
            }

            if (path.StartsWith("/api/media/"))
            {
                var parts = path.Split('/');
                if (parts.Length == 4 && int.TryParse(parts[3], out int mediaId))
                {
                    if (method == "GET")
                    {
                        MediaController.HandleGetMediaByIdRequest(context, mediaId);
                        return;
                    }

                    if (method == "PUT")
                    {
                        MediaController.HandleUpdateMediaByIdRequest(context, mediaId, username);
                        return;
                    }

                    if (method == "DELETE")
                    {
                        MediaController.HandleDeleteMediaByIdRequest(context, mediaId, username);
                        return;
                    }
                }
            }

            // Rating
            if (method == "POST" && path.StartsWith("/api/media/") && path.EndsWith("/ratings"))
            {
                var parts = path.Split('/');
                if (parts.Length == 5 && int.TryParse(parts[3], out int mediaId))
                {
                    RatingController.HandleCreateRatingRequest(context, mediaId, username);
                    return;
                }
            }

            if (method == "POST" && path.StartsWith("/api/ratings/") && path.EndsWith("/like"))
            {
                var parts = path.Split('/');
                if (parts.Length == 5 && int.TryParse(parts[3], out int ratingId))
                {
                    RatingController.HandleLikeRatingRequest(context, ratingId, username);
                    return;
                }
            }

            if (method == "POST" && path.StartsWith("/api/ratings/") && path.EndsWith("/confirm"))
            {
                var parts = path.Split('/');
                if (parts.Length == 5 && int.TryParse(parts[3], out int ratingId))
                {
                    RatingController.HandleConfirmRatingRequest(context, ratingId, username);
                    return;
                }
            }

            // Favorites
            if (method == "POST" && path.StartsWith("/api/media/") && path.EndsWith("/favorite"))
            {
                var parts = path.Split('/');
                if (parts.Length == 5 && int.TryParse(parts[3], out int mediaId))
                {
                    FavoritesController.HandleAddFavoriteRequest(context, mediaId, username);
                    return;
                }
            }

            if (method == "DELETE" && path.StartsWith("/api/media/") && path.EndsWith("/favorite"))
            {
                var parts = path.Split('/');
                if (parts.Length == 5 && int.TryParse(parts[3], out int mediaId))
                {
                    FavoritesController.HandleRemoveFavoriteRequest(context, mediaId, username);
                    return;
                }
            }

            if (method == "GET" && path.StartsWith("/api/users/") && path.EndsWith("/favorites"))
            {
                var parts = path.Split('/');
                if (parts.Length == 5)
                {
                    var requestedUser = parts[3];
                    FavoritesController.HandleGetFavoritesRequest(context, requestedUser);
                    return;
                }
            }

            // Falls Endpoint nicht gefunden/nicht existiert
            HttpResponseHelper.WriteResponse(
                context.Response,
                404,
                new { error = "Endpoint not found" }
            );
        }

        /*private static bool IsAuthenticated(HttpListenerRequest request)  //wird spaeter fuer die Authentifizierung mittels Tokens benutzt
        {
            var authHeader = request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return false;
            }

            var token = authHeader.Substring("Bearer ".Length);
            return TokenService.IsTokenValid(token);
        }*/
    }
}
