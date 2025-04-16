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

        #region Users
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
        #endregion

        #region Teachers
        public List<Subject> GetTeacherSubjects(int teacherId)
        { 
            List<Subject> subjects = new List<Subject>();
            string query = $"SELECT TeacherSubjects.Id as RecordId, Subjects.Name, Subjects.Id FROM Teachers INNER JOIN(Subjects INNER JOIN TeacherSubjects ON Subjects.Id = TeacherSubjects.SubjectId)" +
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
                                subjects.Add(new Subject { RecordId = Convert.ToInt32(reader["RecordId"].ToString()), Id = Convert.ToInt32(reader["Id"].ToString()), Name = reader["Name"].ToString() });
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
        public bool DeleteTeacherSubject(int Id)
        {
            if (Id <= 0 )
            {
                throw new Exception("Не вірно вказано Id запису");
            }
            string query = $"DELETE FROM TeacherSubjects WHERE Id = {Id}";
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
                    throw new Exception("Помилка при видалені предмета у вчителя: " + ex.Message);
                }
            }
        }
        public List<int> GetIdTeacherSubject(int subjectId)
        {
            if (subjectId <= 0)
            {
                throw new Exception("Не вірно вказано Id предмета");
            }
            List<int> Ids = new List<int>();
            string query = $"SELECT Id FROM TeacherSubjects WHERE SubjectId = {subjectId}";
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
                                Ids.Add(Convert.ToInt32(reader["Id"].ToString()));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return Ids;
        }
        public bool AddTeacher(Teacher teacher)
        {
            if (String.IsNullOrEmpty(teacher.FirstName) || String.IsNullOrEmpty(teacher.LastName) || teacher.DepartmentId <= 0)
            { 
                throw new Exception("Не вірно вказано дані викладача");
            }
            string query = $"INSERT INTO Teachers (LastName, FirstName, BirthDate, DepartmentId) VALUES (@LastName, @FirstName, @BirthDate, @DepartmentId)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LastName", teacher.LastName);
                        command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                        command.Parameters.AddWithValue("@BirthDate", teacher.BirthDate);
                        command.Parameters.AddWithValue("@DepartmentId", teacher.DepartmentId);
                        int rowAffcted = command.ExecuteNonQuery();
                        return rowAffcted > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Помилка при додаванні викладача: " + ex.Message);
                }
            }
        }
        #endregion

        #region Subjects
        public bool AddSubject(Subject subject)
        {
            if (subject == null)
            {
                return false;
            }
            string query = "INSERT INTO Subjects (Name) VALUES (@Name)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", subject.Name);
                        int rowAffcted = command.ExecuteNonQuery();
                        return rowAffcted > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Помилка при додаванні предмету: " + ex.Message);
                }
            }
        }
        public bool DeleteSubject(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Не вірно вказано Id запису предмета");
            }
            string query = $"DELETE FROM Subjects WHERE Id = {id}";
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
                    throw new Exception("Помилка при видалені предмета: " + ex.Message);
                }
            }
        }
        public Subject GetSubject(int id)
        {
            Subject subject;
            string query = $"SELECT * FROM Subjects WHERE Id = '{@id}'";
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
                                subject = new Subject
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString(),
                                };
                            }
                            else
                            {
                                subject = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return subject;
        }
        public Subject GetSubject(string name)
        {
            Subject subject;
            if (String.IsNullOrEmpty(name))
            {
                throw new Exception("Ім'я предмету пусте");
            }
            string query = $"SELECT * FROM Subjects WHERE Name = '{@name}'";
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
                                subject = new Subject
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Name = reader["Name"].ToString(),
                                };
                            }
                            else
                            {
                                subject = null;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return subject;
        }
        public bool UpdateSubject(Subject updatedSubject)
        {
            if (updatedSubject == null || String.IsNullOrEmpty(updatedSubject.Name) || updatedSubject.Id <= 0 )
            {
                throw new Exception("Не вірно вказано дані предмета вони пусті");
            }
            Subject subject = GetSubject(updatedSubject.Id);
            if (subject == null)
            { 
                throw new Exception("Не вірно вказано Id предмета");
            }
            string query = $"UPDATE Subjects SET Name = @Name WHERE Id = {updatedSubject.Id}";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", updatedSubject.Name);
                        int rowAffcted = command.ExecuteNonQuery();
                        return rowAffcted > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Помилка при редагуванні предмету: " + ex.Message);
                }
            }
        }
        #endregion
    }
}
