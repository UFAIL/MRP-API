using MRP_API.Server;

namespace MRP_API
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RestApiServer apiServer = new RestApiServer();
            apiServer.StartServer();
        }
    }
}
