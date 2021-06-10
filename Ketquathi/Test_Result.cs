using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace PHANMEMTHI
{
    public partial class Test_Result : Form
    {       
        public Test_Result()
        {
            InitializeComponent();
        }
        string stuid;
        string classid; 
        DataTable dt3 = new DataTable();
        function fn = new function();
        string query; 
        public Test_Result(string msv, string hoten, string ngaysinh)
        {
            InitializeComponent();
            studentid.Text = msv;
            studentname.Text = hoten;
            studentbirthday.Text = ngaysinh;
            stuid = msv;
            query = "select Classes.Class_id as N'Mã lớp học phần', Classes.Class_name as N'Tên LHP', Subject.Subject_name as N'Môn học', Subject.Credits as N'Số tín chỉ', Teachers.Teacher_name as N'Giáo viên phụ trách'  from Students, Classes, Student_Classes, Teachers, Subject where Students.Student_id = '" + msv + "' and Students.Student_id = Student_Classes.Student_id and Classes.Class_id = Student_Classes.Class_id and Teachers.Teacher_id = Classes.Teacher_id and Classes.Subject_id = Subject.Subject_id";
            dt3 = fn.getdt(query);
            classinfo.DataSource = dt3;
        }
        private void hombut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Student_Login stlogin = new Student_Login(stuid);
            stlogin.Show();
        }
        private void napdl(int i)
        {
            classid = classinfo[0, i].Value.ToString();
            lbsubject.Text = classinfo[2, i].Value.ToString();
            lbclass.Text = classinfo[1, i].Value.ToString();
            lbcredit.Text = classinfo[3, i].Value.ToString();
            lbteacher.Text = classinfo[4, i].Value.ToString();
        }
        private void classinfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = classinfo.CurrentRow.Index;
            napdl(i);
        }

        private void Chonlop_Click(object sender, EventArgs e)
        {            
                this.Hide();
                Class_Exam_Info ce = new Class_Exam_Info(classid, studentid.Text, studentname.Text, studentbirthday.Text);
                ce.Show();                         
        }

        private void Test_Result_Load(object sender, EventArgs e)
        {
            if (classinfo.Rows.Count > 0)
                napdl(0);
        }
    }
}
