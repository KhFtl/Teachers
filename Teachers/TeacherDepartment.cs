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
    public partial class TeacherDepartment : Form
    {
        DAL dal = new DAL();

        public TeacherDepartment()
        {
            InitializeComponent();
        }

        private void TeacherDepartment_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'teachersDataSet.Subjects' table. You can move, or remove it, as needed.
            this.subjectsTableAdapter.Fill(this.teachersDataSet.Subjects);
            // TODO: This line of code loads data into the 'teachersDataSet.Departments' table. You can move, or remove it, as needed.
            this.departmentsTableAdapter.Fill(this.teachersDataSet.Departments);
            // TODO: This line of code loads data into the 'teachersDataSet.Teachers' table. You can move, or remove it, as needed.
            this.teachersTableAdapter.Fill(this.teachersDataSet.Teachers);

        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            this.teachersTableAdapter.Update(this.teachersDataSet.Teachers);
        }

        private void TeacherDepartment_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int? idTeacher = null;
            idTeacher = dataGridView1.CurrentRow.Cells["idDataGridViewTextBoxColumn"].Value as int?;
            FillSubjectTable(idTeacher);
            FillTeacherTxtBox();
        }

        private void FillSubjectTable(int? idTeacher)
        {
            if (idTeacher > 0 && idTeacher != null)
            {
                dgrv_subjects.Rows.Clear();
                List<Subject> subjects = dal.GetTeacherSubjects(idTeacher.Value);
                if (subjects.Count > 0)
                {
                    int i = 0;
                    foreach (var subject in subjects)
                    {
                        dgrv_subjects.Rows.Add();
                        dgrv_subjects.Rows[i].Cells["Subject"].Value = subject.Name;
                        dgrv_subjects.Rows[i].Cells["Id"].Value = subject.Id;
                        i++;
                    }
                }
            }
        }
        private void FillTeacherTxtBox()
        { 
            string teacherName = dataGridView1.CurrentRow.Cells["lastNameDataGridViewTextBoxColumn"].Value.ToString()+" "
                                +dataGridView1.CurrentRow.Cells["firstNameDataGridViewTextBoxColumn"].Value.ToString();
            txt_TeacherName.Text = teacherName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int subjectId = Convert.ToInt32(comboBox1.SelectedValue);
            int teacherId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["idDataGridViewTextBoxColumn"].Value);
            List<Subject> subjects = dal.GetTeacherSubjects(teacherId);
            if (subjects.Count > 0)
            {
                foreach (var subject in subjects)
                {
                    if (subject.Id == subjectId)
                    {
                        MessageBox.Show($"Предмет {subject.Name} вже додано викладачу", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            try
            {
                bool success = dal.SetTeacherSubject(teacherId, subjectId);
                if (success)
                { 
                    FillSubjectTable(teacherId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
