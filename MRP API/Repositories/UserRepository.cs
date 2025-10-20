using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using MRP_API.Models;
using MRP_API.Utilities;

namespace MRP_API.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public bool CreateUser(User user)
        async public void CreateUser(User user)
        {
            //using var conn = new NpgsqlConnection(_connectionString);
            await using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            string phc = PasswordHasher.HashPassword(user.Password);
            
            var cmd = new NpgsqlCommand("INSERT INTO users (username, password) VALUES (@u, @p)", conn);
            cmd.Parameters.AddWithValue("u", user.Username);
            //cmd.Parameters.AddWithValue("p", user.Password);
            cmd.Parameters.AddWithValue("p", phc);

            var rows = cmd.ExecuteNonQuery();
            //return rows > 0;
        }

        public User? GetUser(string username, string password)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();
            
            var cmd = new NpgsqlCommand("SELECT username, password FROM users WHERE username = @u AND password = @p", conn);
            cmd.Parameters.AddWithValue("u", username);
            cmd.Parameters.AddWithValue("p", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Username = reader.GetString(0),
                    Password = reader.GetString(1)
                };
            }

            return null;
        }
    }
}
