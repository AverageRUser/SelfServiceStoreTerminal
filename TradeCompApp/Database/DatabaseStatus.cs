using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCompApp.Database
{
    public static class DatabaseStatus
    {
        public static bool IsConnected { get; private set; }
        public static async Task<string> GetConnectionString()
        {

            
            return await SecureStorage.GetAsync("mysql_connection");

        }
        public static async Task CheckConnectionAsync()
        {
            try
            {
                await using var connection = new MySqlConnection(await GetConnectionString());
                await connection.OpenAsync();
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
            }
        }
    }
}
