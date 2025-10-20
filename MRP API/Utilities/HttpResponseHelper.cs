using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Utilities
{
    internal class HttpResponseHelper   //sollte fuer die weitere Uberpruefung und Ausgabe der HTTP Codes benutzt werden; wird momentan nicht benutzt
    {
        public static void WriteResponse(HttpListenerResponse response, int statusCode, object body)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            var json = JsonConvert.SerializeObject(body);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            response.ContentLength64 = buffer.Length;
            using (var output = response.OutputStream)
            {
                output.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
