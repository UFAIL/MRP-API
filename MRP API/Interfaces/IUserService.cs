using MRP_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP_API.Interfaces
{
    public interface IUserService   //sollte fuer das Dependency Inversion Principle verwendet werden; ist momentan nicht implementiert
    {
        bool RegisterUser(User user);
        User AuthenticateUser(string username, string password);
    }
}
