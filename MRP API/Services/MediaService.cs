using MRP_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Services
{
    internal static class MediaService
    {
        private static readonly List<MediaEntry> _media = new();
        private static int _nextId = 1;

        public static MediaEntry CreateMediaEntry(MediaEntry media, string creatorUsername)
        {
            media.Id = _nextId++;
            media.CreatorUsername = creatorUsername;
            media.Ratings = new List<Rating>();
            media.AverageScore = 0;

            _media.Add(media);

            return media;
        }

        public static List<MediaEntry> GetAllMedia()
        {
            return _media;
        }

        public static MediaEntry? GetMediaById(int id)
        {
            return _media.FirstOrDefault(media => media.Id == id);
        }

        public static bool UpdateMediaById(int id, MediaEntry update, string requester)
        {
            var media = GetMediaById(id);

            if (media == null)
            {
                return false;
            }
                

            if (media.CreatorUsername != requester)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(update.Title))
            {
                media.Title = update.Title;
            }

            if (!string.IsNullOrWhiteSpace(update.Description))
            {
                media.Description = update.Description;
            }

            if (!string.IsNullOrWhiteSpace(update.Type))
            {
                media.Type = update.Type;
            }

            if (update.ReleaseYear != 0)
            {
                media.ReleaseYear = update.ReleaseYear;
            }

            if (update.Genres != null && update.Genres.Any())
            {
                media.Genres = update.Genres;
            }
                

            if (update.AgeRestriction != 0)
            {
                update.AgeRestriction = update.AgeRestriction;
            }

            return true;
        }

        public static bool DeleteMediaById(int id, string requester)
        {
            var media = GetMediaById(id);

            if (media == null)
            {
                return false;
            }

            if (media.CreatorUsername != requester)
            {
                return false;
            }

            _media.Remove(media);

            return true;
        }

        internal static void RecalculateAverage(MediaEntry media)
        {
            media.AverageScore = media.Ratings.Any()
                ? media.Ratings.Average(r => r.Stars)
                : 0;
        }

        public static List<MediaEntry> SearchAndFilter(string? search, string? type, string? genre, int? year, int? minRating, string? sort)
        {
            IEnumerable<MediaEntry> query = _media;

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
                

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(m => m.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(m => m.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
            }
                
            if (year.HasValue)
            {
                query = query.Where(m => m.ReleaseYear == year.Value);
            }
                
            if (minRating.HasValue)
            {
                query = query.Where(m => m.AverageScore >= minRating.Value);
            }

            query = sort switch
            {
                "title" => query.OrderBy(m => m.Title),
                "year" => query.OrderBy(m => m.ReleaseYear),
                "score" => query.OrderByDescending(m => m.AverageScore),
                _ => query
            };

            return query.ToList();
        }

        public static void Clear()
        {
            _media.Clear();
            _nextId = 1;
        }
    }
}
