using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Teachers.Domains;

namespace Teachers
{
    public class DapperDAL
    {
        private string _connectionString = null;
        public DapperDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Teachers.Properties.Settings.TeachersConnectionString"]?.ConnectionString;
            if (String.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("В файлі конфігураціїї App.config не знайдено рядок підключення до БД");
            }
        }

        public List<User> GetAllUsers(string login=null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql="";
                if (login == null)
                {
                    sql = "SELECT * FROM Users";
                }
                else
                {
                    sql = $"SELECT * FROM Users WHERE Login Like '%{login}%'";
                }
                List<User> users = connection.Query<User>(sql).ToList();
                return users;
            }
            
        }
        public User GetUserById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = $"SELECT * FROM Users WHERE Id={id}";
                User user = connection.QueryFirst<User>(sql);
                return user;
            }
        }
        public User GetUserByLogin(string login)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = $"SELECT * FROM Users WHERE Login={login}";
                User user = connection.QueryFirst<User>(sql);
                return user;
            }
        }
        public bool UpdateUser(User user)
        { 
            using (var connection = new SqlConnection(_connectionString))
            { 
                //string sql = $"UPDATE Users SET Login='{user.Login}', Password='{user.Password}', FullName='{user.FullName}' WHERE Id={user.Id}";
                int rows = connection.Execute("UPDATE Users SET Login=@Login, Password=@Password, FullName=@FullName WHERE Id=@Id", user);
                return rows > 0;
            }
        }
        public bool DeleteUser(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = $"DELETE FROM Users WHERE Id={id}";
                int rows = connection.Execute(sql);
                return rows > 0;
            }
        }
    }
}
