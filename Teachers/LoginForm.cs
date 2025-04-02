using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Teachers.Domains;

namespace Teachers
{
    public partial class LoginForm: Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DAL dal;
            User user;
            string login = txt_login.Text;
            string password = txt_password.Text;
            try
            {
                dal = new DAL();
                user = dal.GetUser(login);
                if (user != null)
                {
                    if (user.ValidateUser(password))
                    {
                        //MessageBox.Show("Ви увійшли в систему", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TeacherDepartment teacherDepartment = new TeacherDepartment();
                        this.Visible = false;
                        teacherDepartment.Show();
                    }
                    else
                    {
                        MessageBox.Show("Невірний пароль", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Користувача не знайдено", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.ShowDialog();
        }
    }
}
