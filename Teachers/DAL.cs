using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teachers.Domains;

namespace Teachers
{
    public class DAL
    {
        private string _connectionString=null;
        public DAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Teachers.Properties.Settings.TeachersConnectionString"]?.ConnectionString;
            if (String.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("В файлі конфігураціїї App.config не знайдено рядок підключення до БД");
            }
        }
        public User GetUser(string Login)
        {
            User user;
            string query = $"SELECT * FROM Users WHERE Login = '{@Login}'";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Login = reader["Login"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    FullName = reader["FullName"].ToString()
                                };
                            }
                            else
                            {
                                user = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return user;
        }
        public bool AddUser(User user)
        {
            if (user == null)
            {
                return false;
            }
            User existUser = GetUser(user.Login);
            if (existUser != null)
            { 
                throw new Exception("Користувач з таким логіном вже існує");
            }
            string query = "INSERT INTO Users (Login, Password, FullName) VALUES (@Login, @Password, @FullName)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", user.Login);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@FullName", user.FullName);
                        int rowAffcted = command.ExecuteNonQuery();
                        return rowAffcted > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Помилка при реєстрації користувача: "+ex.Message);
                }
            }
        }
        public List<Subject> GetTeacherSubjects(int teacherId)
        { 
            List<Subject> subjects = new List<Subject>();
            string query = $"SELECT Subjects.Name, Subjects.Id FROM Teachers INNER JOIN(Subjects INNER JOIN TeacherSubjects ON Subjects.Id = TeacherSubjects.SubjectId)" +
                           $" ON Teachers.Id = TeacherSubjects.TeacherId WHERE Teachers.Id = {teacherId};";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand sql = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = sql.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subjects.Add(new Subject { Id = Convert.ToInt32(reader["Id"].ToString()), Name = reader["Name"].ToString() });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return subjects;
        }
        public bool SetTeacherSubject(int idTeacher, int idSubject)
        {
            if (idTeacher <= 0 || idSubject <= 0)
            {
                throw new Exception("Не вказано викладача або предмет");
            }
            string query = $"INSERT INTO TeacherSubjects (TeacherId, SubjectId) VALUES ({idTeacher}, {idSubject})";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int rowAffcted = command.ExecuteNonQuery();
                        return rowAffcted > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Помилка при додаванні предмета вчителю: " + ex.Message);
                }
            }
        }
    }
}
