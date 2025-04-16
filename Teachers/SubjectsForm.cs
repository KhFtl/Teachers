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
    public partial class SubjectsForm : Form
    {
        DAL dal;
        public SubjectsForm()
        {
            InitializeComponent();
            dal = new DAL();
        }

        private void Subjects_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'teachersDataSet.Subjects' table. You can move, or remove it, as needed.
            this.subjectsTableAdapter.Fill(this.teachersDataSet.Subjects);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.subjectsTableAdapter.Update(this.teachersDataSet.Subjects);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                string Name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txt_id.Text = Id.ToString();
                txt_name.Text = Name;
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            Subject subject = new Subject();
            subject.Name = txt_name.Text;
            try
            {
                if (dal.GetSubject(subject.Name) != null)
                { 
                    MessageBox.Show("Предмет з таким ім'я вже існує","Помилка",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                dal.AddSubject(subject);
                this.subjectsTableAdapter.Fill(this.teachersDataSet.Subjects);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            Subject subject = new Subject();
            subject.Id = Convert.ToInt32(txt_id.Text);
            subject.Name = txt_name.Text;
            try
            {
                List<int> Ids = dal.GetIdTeacherSubject(subject.Id);
                DialogResult result = MessageBox.Show($"Ви впевнені, що хочете видалити предмет він призначенний {Ids.Count} викладачам?", "Підтвердження", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    foreach (int id in Ids)
                    {
                        dal.DeleteTeacherSubject(id);
                    }
                    dal.DeleteSubject(subject.Id);
                    this.subjectsTableAdapter.Fill(this.teachersDataSet.Subjects);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            Subject subject = new Subject();
            subject.Id = Convert.ToInt32(txt_id.Text);
            subject.Name = txt_name.Text;
            try
            {
                dal.UpdateSubject(subject);
                this.subjectsTableAdapter.Fill(this.teachersDataSet.Subjects);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
