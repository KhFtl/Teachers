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
    public partial class AddTeacher : Form
    {
        DAL dal = new DAL();
        TeacherDepartment teacherDepartment;
        public AddTeacher(TeacherDepartment teacherDepartment)
        {
            InitializeComponent();
            this.teacherDepartment = teacherDepartment;
        }

        private void AddTeacher_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'teachersDataSet.Departments' table. You can move, or remove it, as needed.
            this.departmentsTableAdapter.Fill(this.teachersDataSet.Departments);

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            teacherDepartment.firstFocus = true;
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Teacher teacher = new Teacher();
            teacher.FirstName = txt_FirstName.Text;
            teacher.LastName = txt_LastName.Text;
            try 
            {
                teacher.BirthDate = Convert.ToDateTime(txt_BirthDate.Text);
            }
            catch (Exception ex)
            {
                txt_BirthDate.Focus();
                MessageBox.Show("Помилка в даті народження:\n"+ex.Message,"Помилка",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            teacher.DepartmentId = Convert.ToInt32(cmb_Department.SelectedValue);
            if (teacher.FirstName == "" || teacher.LastName == "")
            {
                MessageBox.Show("Заповніть всі поля", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try 
            {
                bool success = dal.AddTeacher(teacher);
                if (success)
                {
                    teacherDepartment.firstFocus = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні викладача:\n" + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
