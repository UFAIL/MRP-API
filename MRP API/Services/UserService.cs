using MRP_API.Models;
using MRP_API.Repositories;
using MRP_API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MRP_API.Services
{
    internal static class UserService
    {
        //private static Dictionary<string, User> _users = new Dictionary<string, User>();
        private static readonly Dictionary<string, string> _users = new();

        /*public static bool RegisterUser(User user)
        {
            if (_users.ContainsKey(user.Username))
            {
                return false;
            }

            _users.Add(user.Username, user);*/

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

            /*return true;
        }*/

        public static bool RegisterUser(User user)
        {
            if (_users.ContainsKey(user.Username))
                return false;

            var hashedPassword = PasswordHasher.HashPassword(user.Password);
            _users[user.Username] = hashedPassword;

            return true;
        }

        /*public static User? AuthenticateUser(string username, string password)
        {
            if (_users.ContainsKey(username) && _users[username].Password == password)
            {
                return _users[username];
            }
            return null;
        }*/

        public static User? AuthenticateUser(string username, string password)
        {
            if (!_users.TryGetValue(username, out var storedHash))
                return null;

            if (!PasswordHasher.VerifyPassword(password, storedHash))
                return null;

            return new User
            {
                Username = username,
                Password = storedHash
            };
        }

        public static object? GetProfile(string username)
        {
            if (!_users.ContainsKey(username))
            {
                return null;
            }

            return new
            {
                username,
                totalRatings = RatingService.GetRatingCountForUser(username),
                averageScore = RatingService.GetAverageScoreForUser(username),
                favoriteGenre = RatingService.GetFavoriteGenreForUser(username)
            };
        }

        public static List<object> GetLeaderboard()
        {
            return RatingService.GetLeaderboard();
        }

        public static void Clear()
        {
            _users.Clear();
        }

    }
}
