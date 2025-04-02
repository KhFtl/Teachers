using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Teachers.Domains
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public User()
        {

        }
        public User(int id, string login, string password, string fullName)
        {
            Id = id;
            Login = login;
            Password = password;
            FullName = fullName;
        }
        public bool ValidateUser(string password)
        {
            DAL dAL = new DAL();
            User user = dAL.GetUser(Login);
            if (user != null)
            {
                return user.Password == HashPassword(password);
            }
            else
            {
                return false;
            }
        }
        public bool RegisterUser()
        {
            if (!String.IsNullOrEmpty(Login) && !String.IsNullOrEmpty(Password) && !String.IsNullOrEmpty(FullName))
            {
                try
                {
                    DAL dal = new DAL();
                    this.Password = HashPassword(this.Password);
                    if (dal.AddUser(this))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return false;
            }
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
