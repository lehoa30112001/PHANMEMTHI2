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
        
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        public Test_Result(string msv, string hoten, string ngaysinh)
        {
            InitializeComponent();
            studentid.Text = msv;
            studentname.Text = hoten;
            studentbirthday.Text = ngaysinh;
            stuid = msv;
            conn.Open();
            string query = "select Classes.Class_id as N'Mã lớp học phần', Classes.Class_name as N'Tên LHP', Subject.Subject_name as N'Môn học', Subject.Credits as N'Số tín chỉ', Teachers.Teacher_name as N'Giáo viên phụ trách'  from Students, Classes, Student_Classes, Teachers, Subject where Students.Student_id = '" + msv + "'";
            SqlCommand cmd1 = new SqlCommand(query, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            sda1.Fill(dt3);
            classinfo.DataSource = dt3;
            conn.Close();
        }
        private void Test_Result_Load(object sender, EventArgs e)
        {
 
        }

        private void hombut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Student_Login stlogin = new Student_Login(stuid);
            stlogin.Show();
        }

        private void classinfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataRow dr1 in dt3.Rows)
            {
                classid = classinfo.SelectedRows[0].Cells[0].Value.ToString();
                lbsubject.Text = classinfo.SelectedRows[0].Cells[2].Value.ToString();
                lbclass.Text = classinfo.SelectedRows[0].Cells[1].Value.ToString();
                lbcredit.Text = classinfo.SelectedRows[0].Cells[3].Value.ToString();
                lbteacher.Text = classinfo.SelectedRows[0].Cells[4].Value.ToString();

                lbsubject.Visible = true;
                lbclass.Visible = true;
                lbcredit.Visible = true;
                lbteacher.Visible = true;
            }
        }

        private void Chonlop_Click(object sender, EventArgs e)
        {
            if (lbteacher.Visible == false)
                MessageBox.Show("Chon lop hoc !!");
            else
            {
                this.Hide();
                Class_Exam_Info ce = new Class_Exam_Info(classid, studentid.Text, studentname.Text, studentbirthday.Text);
                ce.Show();
            }            
        }
    }
}
