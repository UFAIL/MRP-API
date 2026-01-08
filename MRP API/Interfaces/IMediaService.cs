using MRP_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Interfaces
{
    public interface IMediaService   //sollte fuer das Dependency Inversion Principle verwendet werden; ist momentan nicht implementiert
    {
        MediaEntry CreateMediaEntry(string title, string type, string description, string releaseYear, string genre, string ageRestriction);
        List<MediaEntry> GetAll();
        bool UpdateMedia(int id, string title, string type, string description, string releaseYear, string genre, string ageRestriction);
        bool DeleteMedia(int id);
    }
}
