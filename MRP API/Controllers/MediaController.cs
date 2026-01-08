using MRP_API.Models;
using MRP_API.Services;
using MRP_API.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MRP_API.Controllers
{
    internal static class MediaController
    {
        public static void HandleCreateMediaRequest(HttpListenerContext context, string username)
        {
            var body = new StreamReader(context.Request.InputStream).ReadToEnd();
            var media = JsonConvert.DeserializeObject<MediaEntry>(body);

            if (media == null)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    400,
                    new { error = "Invalid request body" }
                );
                return;
            }

            var created = MediaService.CreateMediaEntry(media, username);

            HttpResponseHelper.WriteResponse(
                context.Response,
                201,
                created
            );
        }

        public static void HandleGetAllMediaRequest(HttpListenerContext context)
        {
            var list = MediaService.GetAllMedia();

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                list
            );
        }

        public static void HandleGetMediaByIdRequest(HttpListenerContext context, int id)
        {
            var media = MediaService.GetMediaById(id);

            if (media == null)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    404,
                    new { error = "Media not found" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                media
            );
        }

        public static void HandleUpdateMediaByIdRequest(HttpListenerContext context, int id, string username)
        {
            var body = new StreamReader(context.Request.InputStream).ReadToEnd();
            var update = JsonConvert.DeserializeObject<MediaEntry>(body);

            if (update == null)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    400,
                    new { error = "Invalid request body" }
                );
                return;
            }

            var success = MediaService.UpdateMediaById(id, update, username);

            if (!success)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    403,
                    new { error = "You are not allowed to update this media entry" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                MediaService.GetMediaById(id)
            );
        }

        public static void HandleDeleteMediaByIdRequest(HttpListenerContext context, int id, string username)
        {
            var success = MediaService.DeleteMediaById(id, username);

            if (!success)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    403,
                    new { error = "You are not allowed to delete this media entry" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                204,
                new { }
            );
        }

        public static void HandleSearchMediaRequest(HttpListenerContext context)
        {
            var query = context.Request.QueryString;

            string? search = query["search"];
            string? type = query["type"];
            string? genre = query["genre"];
            string? sort = query["sort"];

            int? year = int.TryParse(query["year"], out var y) ? y : null;
            int? minRating = int.TryParse(query["minRating"], out var r) ? r : null;

            var results = MediaService.SearchAndFilter(
                search, type, genre, year, minRating, sort);

            HttpResponseHelper.WriteResponse(context.Response, 200, results);
        }
    }
}
