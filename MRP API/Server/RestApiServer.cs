using MRP_API.Controllers;
using MRP_API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Server
{
    internal class RestApiServer
    {
        public void StartServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:7810/");
            listener.Start();
            Console.WriteLine("Server is listening...");

            while(true)
            {
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                //string clientInformation = "";

                if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/register")
                {
                    /*using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        var requestBody = reader.ReadToEnd();
                        UserController.HandleRegisterRequest(requestBody);
                    }*/

                    UserController.HandleRegisterRequest(context);
                } else if(request.HttpMethod == "POST" && request.Url.AbsolutePath == "/login")
                {
                    /*using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        var requestBody = reader.ReadToEnd();
                        UserController.HandleLoginRequest(requestBody);
                    }*/

                    UserController.HandleLoginRequest(context);
                } else if(request.HttpMethod == "POST" && request.Url.AbsolutePath == "/createMedia")
                {
                    /*using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        var requestBody = reader.ReadToEnd();
                        MediaController.HandleCreateMediaRequest(requestBody);
                    }*/

                    MediaController.HandleCreateMediaRequest(context);
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/getAllMedia")
                {
                    //MediaController.HandleGetAllMediaRequest();
                    MediaController.HandleGetAllMediaRequest(context);
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/getMediaById")
                {
                    /*using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        var requestBody = reader.ReadToEnd();
                        MediaController.HandleGetMediaByIdRequest(requestBody);
                    }*/
                    MediaController.HandleGetMediaByIdRequest(context);
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/updateMediaById")
                {
                    using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        var requestBody = reader.ReadToEnd();
                        MediaController.HandleUpdateMediaByIdRequest(requestBody);  //scheint gerade nicht zu funktionieren; muss noch bearbeitet werden
                    }
                } else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/deleteMediaById")
                {
                    /*using (var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding))
                    {
                        var requestBody = reader.ReadToEnd();
                        MediaController.HandleDeleteMediaByIdRequest(requestBody);
                    }*/

                    MediaController.HandleDeleteMediaByIdRequest(context);
                }

                //string clientInformation = ClientInformation(context);
                /*byte[] buffer = System.Text.Encoding.UTF8.GetBytes(string.Format("<HTML><BODY>{0}</BODY></HTML>", clientInformation));
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();*/

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Close();
            }
        }

        /*private static bool IsAuthenticated(HttpListenerRequest request)  //wird spaeter fuer die Authentifizierung mittels Tokens benutzt
        {
            var authHeader = request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return false;
            }

            var token = authHeader.Substring("Bearer ".Length);
            return TokenService.IsTokenValid(token);
        }*/
    }
}
