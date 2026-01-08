using MRP_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Services
{
    internal static class FavoritesService
    {
        private static readonly Dictionary<string, HashSet<int>> _favorites = new();

        public static bool AddFavorite(string username, int mediaId)
        {
            var media = MediaService.GetMediaById(mediaId);
            if (media == null)
                return false;

            if (!_favorites.ContainsKey(username))
                _favorites[username] = new HashSet<int>();

            return _favorites[username].Add(mediaId);
        }

        public static bool RemoveFavorite(string username, int mediaId)
        {
            if (!_favorites.ContainsKey(username))
                return false;

            return _favorites[username].Remove(mediaId);
        }

        public static List<MediaEntry> GetFavorites(string username)
        {
            if (!_favorites.ContainsKey(username))
                return new List<MediaEntry>();

            return _favorites[username]
                .Select(id => MediaService.GetMediaById(id))
                .Where(m => m != null)
                .ToList()!;
        }

        public static void Clear()
        {
            _favorites.Clear();
        }

    }
}
