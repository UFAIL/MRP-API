using MRP_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Services
{
    internal class MediaService
    {
        private static List<MediaEntry> _media = new List<MediaEntry>();

        public static MediaEntry CreateMediaEntry(string title, string type, string description, string releaseYear, string genre, string ageRestriction)
        {
            var newEntry = new MediaEntry
            {
                Id = _media.Count + 1,
                Title = title,
                Type = type,
                Description = description,
                ReleaseYear = releaseYear,
                Genre = genre,
                AgeRestriction = ageRestriction
            };

            _media.Add(newEntry);
            return newEntry;
        }

        public static List<MediaEntry> GetAllMedia()
        {
            return _media;
        }

        public static MediaEntry? GetMediaById(int id)
        {
            return _media.Find(media => media.Id == id);
        }

        public static bool UpdateMediaById(int id, string? title = null, string? type = null, string? description = null, string? releaseYear = null, string? genre = null, string? ageRestriction = null)
        {
            var media = GetMediaById(id);
            if (media != null)
            {
                if(title != null)
                {
                    media.Title = title;
                }

                if (type != null)
                {
                    media.Type = type;
                }

                if (description != null)
                {
                    media.Description = description;
                }

                if (releaseYear != null)
                {
                    media.ReleaseYear = releaseYear;
                }

                if (genre != null)
                {
                    media.Genre = genre;
                }

                if (ageRestriction != null)
                {
                    media.AgeRestriction = ageRestriction;
                }
                
                return true;
            }
            return false;
        }

        public static bool DeleteMediaById(int id)
        {
            var media = GetMediaById(id);
            if (media != null)
            {
                _media.Remove(media);
                return true;
            }
            return false;
        }
    }
}
