using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Services
{
    internal class TokenService //sollte fuer die weitere Implementierung fuer das Token-System verantwortlich sein; z.B. die die Speicherung des Tokens ueber das ganze Programm oder die Ueberpruefung, ob das Token mit dem User uebereinstimmt; wird momentan nicht benutzt
    {
        private static Dictionary<string, string> _tokens = new Dictionary<string, string>();

        public static void StoreToken(string token, string username)
        {
            _tokens[token] = username;
        }

        public static bool IsTokenValid(string token)
        {
            return _tokens.ContainsKey(token);
        }

        public static string? GetUsernameFromToken(string token)
        {
            if(_tokens.TryGetValue(token, out var user))
            {
                return user;
            }
            
            return null;
            //return _tokens.TryGetValue(token, out var user) ? user : null;
        }

        public static void RevokeToken(string token)
        {
            _tokens.Remove(token);
        }
    }
}
