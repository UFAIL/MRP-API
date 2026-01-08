using MRP_API.Services;
using MRP_API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace MRP_API.Controllers
{
    internal static class FavoritesController
    {
        public static void HandleAddFavoriteRequest(HttpListenerContext context, int mediaId, string username)
        {
            var success = FavoritesService.AddFavorite(username, mediaId);

            if (!success)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    404,
                    new { error = "Media not found or already favorited" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                new { message = "Added to favorites" }
            );
        }

        public static void HandleRemoveFavoriteRequest(
            HttpListenerContext context,
            int mediaId,
            string username)
        {
            var success = FavoritesService.RemoveFavorite(username, mediaId);

            if (!success)
            {
                HttpResponseHelper.WriteResponse(
                    context.Response,
                    404,
                    new { error = "Favorite not found" }
                );
                return;
            }

            HttpResponseHelper.WriteResponse(
                context.Response,
                204,
                new { }
            );
        }

        public static void HandleGetFavoritesRequest(
            HttpListenerContext context,
            string username)
        {
            var favorites = FavoritesService.GetFavorites(username);

            HttpResponseHelper.WriteResponse(
                context.Response,
                200,
                favorites
            );
        }
    }
}
