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
        //Створення таблиці Предмети
        public bool CreateSubjectTable()
        {
            string query = @"CREATE TABLE [dbo].[Subjects]([Id] [int] IDENTITY(1,1) NOT NULL,[Name] [nvarchar](50) NOT NULL,CONSTRAINT [PK_Subjects] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY]";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Не вдалося створити таблицю Предмети " + ex.Message);
                }
            }
        }
        //Створення таблиці Кафедри
        public bool CreateDepartmentTable()
        {
            string query = @"CREATE TABLE [dbo].[Departments]([Id] [int] IDENTITY(1,1) NOT NULL,[Name] [nvarchar](50) NOT NULL,[Phone] [nchar](10) NULL,
                             CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) 
                             ON [PRIMARY]) ON [PRIMARY]";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Не вдалося створити таблицю Кафедри " + ex.Message);
                }
            }
        }
        //Створення таблиці Викладачі
        public bool CreateTeacherTable()
        {
            string query = @"CREATE TABLE [dbo].[Teachers]([Id] [int] IDENTITY(1,1) NOT NULL,[LastName] [nvarchar](50) NOT NULL,
	                         [FirstName] [nvarchar](50) NULL, [BirthDate] [date] NOT NULL,[DepartmentId] [int] NOT NULL,
                             CONSTRAINT [PK_Teachers] PRIMARY KEY CLUSTERED ([Id] ASC)
                             WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
                             ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY]";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Не вдалося створити таблицю Вчителі " + ex.Message);
                }
            }
        }
        //Створення таблиці Викладачі_Предмети
        public bool CreateTeacherSubjectTable()
        {
            string query = @"CREATE TABLE [dbo].[TeacherSubjects]([TeacherId] [int] NOT NULL,[SubjectId] [int] NOT NULL,
	                         [Id] [int] IDENTITY(1,1) NOT NULL) ON [PRIMARY]";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Не вдалося створити таблицю Вчителі-Предмети " + ex.Message);
                }
            }
        }
        //Створення тиаблиці Користувачі
        public bool CreateUsersTable()
        {
            string query = @"CREATE TABLE [dbo].[Users]([Id] [int] IDENTITY(1,1) NOT NULL,[Login] [nvarchar](50) NOT NULL,
	                         [Password] [nvarchar](100) NOT NULL, [FullName] [nvarchar](50) NOT NULL,
                             CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
                             WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, 
                             ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) ON [PRIMARY]";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Не вдалося створити таблицю Користувачі " + ex.Message);
                }
            }
        }
    }
}
