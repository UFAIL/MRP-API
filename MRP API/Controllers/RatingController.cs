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

namespace MRP_API.Controllers
{
    internal static class RatingController
    {
        public static void HandleCreateRatingRequest(
            HttpListenerContext context,
            int mediaId,
            string username)
        {
            var body = new StreamReader(context.Request.InputStream).ReadToEnd();
            var rating = JsonConvert.DeserializeObject<Rating>(body);

            if (rating == null || rating.Stars < 1 || rating.Stars > 5)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    400,
                    new { error = "Invalid rating data (stars must be 1–5)" }
                );
                return;
            }

            try
            {
                var created = RatingService.Create(
                    mediaId,
                    username,
                    rating.Stars,
                    rating.Comment
                );

                HttpResponseHelper.WriteResponse(
                    context.Response,
                    201,
                    created
                );
            }
            catch (ArgumentException ex)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    404,
                    new { error = ex.Message }
                );
            }
        }

        public static void HandleLikeRatingRequest(
            HttpListenerContext context,
            int ratingId,
            string username)
        {
            var success = RatingService.Like(ratingId, username);

            if (!success)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    400,
                    new { error = "Rating not found or already liked" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                new { message = "Rating liked" }
            );
        }

        public static void HandleConfirmRatingRequest(
            HttpListenerContext context,
            int ratingId,
            string username)
        {
            var success = RatingService.Confirm(ratingId, username);

            if (!success)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    403,
                    new { error = "Only the author can confirm this rating" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                new { message = "Rating confirmed" }
            );
        }
    }
}
