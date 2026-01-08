using MRP_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Services
{
    internal static class RatingService
    {
        private static readonly List<Rating> _ratings = new();
        private static int _nextId = 1;

        public static Rating Create(int mediaId, string username, int stars, string? comment)
        {
            if (stars < 1 || stars > 5)
                throw new ArgumentException("Stars must be between 1 and 5");

            var media = MediaService.GetMediaById(mediaId)
                ?? throw new ArgumentException("Media not found");

            var rating = new Rating
            {
                Id = _nextId++,
                MediaId = mediaId,
                Username = username,
                Stars = stars,
                Comment = comment,
                IsConfirmed = false,
                Likes = 0,
                CreatedAt = DateTime.UtcNow
            };

            _ratings.Add(rating);
            media.Ratings.Add(rating);

            MediaService.RecalculateAverage(media);

            return rating;
        }

        public static bool Like(int ratingId, string username)
        {
            var rating = _ratings.FirstOrDefault(r => r.Id == ratingId);

            if (rating == null)
                return false;

            if (rating.LikedBy.Contains(username))
                return false;

            rating.LikedBy.Add(username);
            rating.Likes++;

            return true;
        }

        public static bool Confirm(int ratingId, string username)
        {
            var rating = _ratings.FirstOrDefault(r => r.Id == ratingId);
            if (rating == null)
                return false;

            if (rating.Username != username)
                return false;

            rating.IsConfirmed = true;
            return true;
        }

        public static int GetRatingCountForUser(string username)
        {
            return _ratings.Count(r => r.Username == username);
        }

        public static double GetAverageScoreForUser(string username)
        {
            var userRatings = _ratings.Where(r => r.Username == username).ToList();
            return userRatings.Any()
                ? userRatings.Average(r => r.Stars)
                : 0;
        }

        public static string? GetFavoriteGenreForUser(string username)
        {
            var ratedMedia = _ratings
                .Where(r => r.Username == username)
                .Join(MediaService.GetAllMedia(),
                      r => r.MediaId,
                      m => m.Id,
                      (r, m) => m.Genres)
                .SelectMany(g => g);

            return ratedMedia
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();
        }

        public static List<object> GetLeaderboard()
        {
            return _ratings
                .GroupBy(r => r.Username)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    username = g.Key,
                    ratings = g.Count()
                })
                .ToList<object>();
        }

        public static void Clear()
        {
            _ratings.Clear();
        }

    }
}
