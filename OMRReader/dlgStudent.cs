using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CSedu.OMR
{
    public partial class dlgStudent : Form
    {
        public string StudentID = "";        
        public DataSet students = null;
        public dlgStudent()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string select = "1=1 ";

            if (this.txtStudentNo.Text.Trim() != "")
            {
                select += " and [StudentNo] like '%" + this.txtStudentNo.Text + "%'";
            }

            if ( this.txtStudentName.Text.Trim() != "" )
            {
                if (this.txtStudentName.Text.Split(' ').Length > 1)
                {
                    select += " and [FirstName] like '%" + this.txtStudentName.Text.Split(' ')[0] + "%' ";
                    select += " and [LastName] like '%" + this.txtStudentName.Text.Split(' ')[1] + "%' ";
                }
                else
                {
                    select += " and (  [FirstName]+[LastName] like '%" + this.txtStudentName.Text + "%' ) ";
                }
            }

            if (this.txtBranch.Text.Trim() != "")
            {
                select += " and [branch] like '%" + this.txtBranch.Text + "%'";
            }

            if (this.txtGrade.Text.Trim() != "")
            {
                select += " and [grade] like '%" + this.txtGrade.Text + "%'";
            }


            List<Student> list = new List<Student>();
            foreach (DataRow dr in students.Tables[0].Select(select) )
            {
                Student std = new Student();
                std.Branch = dr["branch"].ToString();
                std.Grade = dr["grade"].ToString();
                std.StudentNo = dr["StudentNo"].ToString();
                std.FirstName = dr["FirstName"].ToString();
                std.LastName = dr["LastName"].ToString();

                list.Add(std);
            }

            this.grdStudent.DataSource = list;
            
            this.grdStudent.Columns[0].Width = 50;
            this.grdStudent.Columns[1].Width = 50;
            this.grdStudent.Columns[2].Width = 100;

        }

        private void grdStudent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && grdStudent.DataSource != null && grdStudent.Rows.Count > 0)
            {
                this.StudentID = grdStudent.Rows[e.RowIndex].Cells[2].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (grdStudent.DataSource != null && grdStudent.Rows.Count > 0)
                this.StudentID = grdStudent.SelectedRows[0].Cells[2].Value.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dlgStudent_Load(object sender, EventArgs e)
        {

        }
    }

    class Student
    {
        public string Branch { get; set; }
        public string Grade { get; set; }
        public string StudentNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
