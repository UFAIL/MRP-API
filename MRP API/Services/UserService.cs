using MRP_API.Models;
using MRP_API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MRP_API.Services
{
    internal class UserService
    {
        private static Dictionary<string, User> _users = new Dictionary<string, User>();

        public static bool RegisterUser(User user)
        {
            if (_users.ContainsKey(user.Username))
            {
                return false;
            }

            _users.Add(user.Username, user);

            /*CREATE TABLE users(
                id SERIAL PRIMARY KEY,
                username VARCHAR(50) UNIQUE NOT NULL,
                password VARCHAR(255) NOT NULL
            );*/

            /*string connStr = "Host=localhost;Port=5432;Username=Test;Password=TestPassword;Database=mrp_db";
            var userRepo = new UserRepository(connStr);
            userRepo.CreateUser(user);*/

            //var connStr = "Host=localhost;Port=5432;Username=mrp_user;Password=mrp_pass;Database=mrp_db";
            //var userRepo = new UserRepository(connStr);
            //var user = new User { Username = "Test", Password = "TestPassword" };
            //userRepo.CreateUser(user);

            //var connStr = Environment.GetEnvironmentVariable("MRP_DB_CONN");  //Environment Variable; Im Docker File: MRP_DB_CONN=Host=localhost;Port=5432;Username=mrp_user;Password=mrp_pass;Database=mrp_db

            return true;
        }

        public static User? AuthenticateUser(string username, string password)
        {
            if (_users.ContainsKey(username) && _users[username].Password == password)
            {
                return _users[username];
            }
            return null;
        }
    }
}
