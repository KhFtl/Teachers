using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teachers
{
    public class DataBaseHelper
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["Teachers.Properties.Settings.TeachersConnectionString"]?.ConnectionString;
        public DataBaseHelper()
        {
            if (String.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("В файлі конфігураціїї App.config не знайдено рядок підключення до БД");
            }
        }

        public bool ExistDatabase(string databaseName)
        { 
            string MasterDb = "master";
            var builder = new SqlConnectionStringBuilder(_connectionString)
            {
                InitialCatalog = MasterDb
            };
            string masterConnectionString = builder.ToString();
            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{databaseName}'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    { 
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                } 
                catch(Exception ex) 
                {
                    throw new Exception("Не вдалось підключитись до БД " + ex.Message);
                }
            }
        }
        public bool ExistTable(string tableName)
        {
            string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Не вдалось підключитись до БД " + ex.Message);
                }
            }
        }
    }
}
