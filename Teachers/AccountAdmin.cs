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
    public partial class AccountAdmin : Form
    {
        private readonly DapperDAL _da;
        public AccountAdmin()
        {
            InitializeComponent();
            _da = new DapperDAL();
            FillTable();
        }

        private void FillTable(string login = null)
        {
            List<User> users;
            if (login == null)
            {
               users = _da.GetAllUsers();
            }
            else
            {
               users = _da.GetAllUsers(login);
            }
            dataGridView1.Rows.Clear();
            int i=0;
            foreach (User user in users)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells["Id"].Value = user.Id.ToString();
                dataGridView1.Rows[i].Cells["Login"].Value = user.Login;
                dataGridView1.Rows[i].Cells["FullName"].Value = user.FullName;
                i++;
            }
        }

        private void AccountAdmin_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            { 
                string Login = dataGridView1.Rows[e.RowIndex].Cells["Login"].Value.ToString();
                string FullName = dataGridView1.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                int Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString());

                txt_editId.Text = Id.ToString();
                txt_fullname.Text = FullName;
                txt_loginEdit.Text = Login;
                txt_loginDelete.Text = Login;
                txt_deleteId.Text = Id.ToString();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            FillTable(txt_loginSearch.Text);
        }

        private void txt_loginSearch_TextChanged(object sender, EventArgs e)
        {
            if (txt_loginSearch.Text.All(char.IsLetter))
            {
                FillTable(txt_loginSearch.Text);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (txt_editId.Text.Length <= 0)
            {
                return;
            }
            User existUser = _da.GetUserById(Convert.ToInt32(txt_editId.Text));
            if (txt_editPassword.Text.Length > 0)
            {
                existUser.Password = existUser.GetHashPassword(txt_editPassword.Text);
            }
            existUser.Login = txt_loginEdit.Text.Length > 0 ? txt_loginEdit.Text : existUser.Login;
            existUser.FullName = txt_fullname.Text.Length > 0 ? txt_fullname.Text : existUser.FullName;
            if (_da.UpdateUser(existUser))
            {
                MessageBox.Show("Дані оновлено","Операція", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FillTable();
            }
            else
            {
                MessageBox.Show("Помилка оновлення даних", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (txt_deleteId.Text.Length <= 0)
            {
                return;
            }
            User existUser = _da.GetUserById(Convert.ToInt32(txt_deleteId.Text));
            if (existUser == null)
            {
                MessageBox.Show("Користувача не знайдено", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult result = MessageBox.Show($"Видалити користувача {existUser.Login}","Питання", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                { 
                    if (_da.DeleteUser(existUser.Id))
                    {
                        MessageBox.Show($"Дані користувача {existUser.Login} видалено", "Операція", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FillTable();
                    }
                    else
                    {
                        MessageBox.Show("Помилка видалення даних", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
