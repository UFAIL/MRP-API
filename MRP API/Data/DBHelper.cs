using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace MRP_API.Data
{
    internal static class DbHelper
    {
        //private static readonly string ConnectionString = "Host=localhost;Port=5432;Username=postgres;Password=yourpassword;Database=MRP_DB";
        private static readonly string ConnectionString = "Host=localhost;Port=5432;Username=Test;Password=TestPassword;Database=MRP_DB";

        public static NpgsqlConnection GetConnection()
        {
            var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
    }
}

