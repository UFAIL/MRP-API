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
    internal class MediaController
    {
        /*public static void HandleCreateMediaRequest(string requestBody)
        {
            var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);

            var newMedia = MediaService.CreateMediaEntry(media.Title, media.Type, media.Description, media.ReleaseYear, media.Genre, media.AgeRestriction);

            Console.WriteLine(string.Format("Media created with ID: {0}", newMedia.Id));
        }*/

        public static void HandleCreateMediaRequest(HttpListenerContext context)
        {
            var requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();
            var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);

            var newMedia = MediaService.CreateMediaEntry(media.Title, media.Type, media.Description, media.ReleaseYear, media.Genre, media.AgeRestriction);

            if (newMedia != null)
            {
                string response = string.Format("Media created with ID: {0}", newMedia.Id);

                HttpResponseHelper.WriteResponse(context.Response, 200, new { response });
            } else
            {
                HttpResponseHelper.WriteResponse(context.Response, 401, new { error = "Media could not be created." });
            }
        }

        /*public static void HandleGetAllMediaRequest()
        {
            var allMedia = MediaService.GetAllMedia();
            Console.WriteLine(JsonConvert.SerializeObject(allMedia, Formatting.Indented));
        }*/

        public static void HandleGetAllMediaRequest(HttpListenerContext context)
        {
            var requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();
            var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);

            var allMedia = MediaService.GetAllMedia();

            if (allMedia != null)
            {
                //string response = JsonConvert.SerializeObject(allMedia, Formatting.Indented);

                HttpResponseHelper.WriteResponse(context.Response, 200, new { allMedia });
            } else
            {
                HttpResponseHelper.WriteResponse(context.Response, 401, new { error = "Media could not be retrieved." });
            }
        }

        /*public static void HandleGetMediaByIdRequest(string requestBody)
        {
            var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);
            var mediaWithId = MediaService.GetMediaById(media.Id);

            if(mediaWithId != null)
            {
                Console.WriteLine(JsonConvert.SerializeObject(mediaWithId, Formatting.Indented));
            } else
            {
                Console.WriteLine(string.Format("Media with ID {0} does not exist.", media.Id));
            }
            
        }*/

        public static void HandleGetMediaByIdRequest(HttpListenerContext context)
        {
            var requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();
            var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);

            var mediaWithId = MediaService.GetMediaById(media.Id);

            if (mediaWithId != null)
            {
                //string response = JsonConvert.SerializeObject(mediaWithId, Formatting.Indented);

                HttpResponseHelper.WriteResponse(context.Response, 200, new { mediaWithId });
            } else
            {
                HttpResponseHelper.WriteResponse(context.Response, 401, new { error = string.Format("Media with ID {0} could not be retrieved.", media.Id) });
            }
        }

        public static void HandleUpdateMediaByIdRequest(string requestBody) //scheint gerade nicht zu funktionieren; muss noch bearbeitet werden
        {
            //var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);
            var media = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestBody);
            int id;

            if (!media.ContainsKey("id") || !int.TryParse(media["id"], out id))
            {
                Console.WriteLine("No ID specified or invalid request.");
                return;
            }

            //var updatedMedia = MediaService.UpdateMediaById(media.Id, media.Title, media.Type, media.Description, media.ReleaseYear, media.Genre, media.AgeRestriction);
            var updateMedia = MediaService.UpdateMediaById(
                id,
                media.ContainsKey("title") ? media["title"] : null,
                media.ContainsKey("type") ? media["type"] : null,
                media.ContainsKey("description") ? media["description"] : null,
                media.ContainsKey("releaseYear") ? media["releaseYear"] : null,
                media.ContainsKey("genre") ? media["genre"] : null,
                media.ContainsKey("ageRestriction") ? media["ageRestriction"] : null
            );
        }

        /*public static void HandleDeleteMediaByIdRequest(string requestBody)
        {
            var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);
            var mediaWithId = MediaService.DeleteMediaById(media.Id);
            Console.WriteLine(string.Format("Media with ID {0} deleted.", media.Id));
        }*/

        public static void HandleDeleteMediaByIdRequest(HttpListenerContext context)
        {
            var requestBody = new StreamReader(context.Request.InputStream).ReadToEnd();
            var media = JsonConvert.DeserializeObject<MediaEntry>(requestBody);

            var mediaWithId = MediaService.DeleteMediaById(media.Id);

            if (mediaWithId)
            {
                //string response = string.Format("Media with ID {0} deleted.", media.Id);

                HttpResponseHelper.WriteResponse(context.Response, 200, new { mediaWithId });
            }
            else
            {
                HttpResponseHelper.WriteResponse(context.Response, 401, new { error = string.Format("Media with ID {0} could not be deleted.", media.Id) });
            }
        }
    }
}
