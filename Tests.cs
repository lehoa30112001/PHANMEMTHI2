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
    public partial class Tests : Form
    {
        public Tests()
        {
            InitializeComponent();
        }
        string msv;
        string examid;
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        public Tests(string stuser)
        {
            msv = stuser;
            InitializeComponent();
            conn.Open();
            string query = "select * from students where Student_id = '" + stuser + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                studentid.Text = dr["Student_id"].ToString();
                studentname.Text = dr["Student_name"].ToString();
                string s = dr["Birthday"].ToString();
                string[] d = s.Split('/');
                studentbirthday.Text = d[0] + '/' + d[1] + '/' + d[2].Substring(0, 4);
            }
            conn.Close();
        }

        private void hombut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Student_Login stlogin = new Student_Login(msv);
            stlogin.Show();
        }
        private void loaddata()
        {
            conn.Open();
            string query = "select Classes.Class_name as N'Lớp học phần', Exams.Exam_id as N'Mã đề', Exams.Exam_order as N'Loại bài thi', Exams.Time as N'Thời gian', Exams.limited_times as N'Giới hạn' from Students, Classes, Student_Classes, Exams where Students.Student_id = '" + msv + "'";
            SqlCommand cmd1 = new SqlCommand(query, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            DataTable dt3 = new DataTable();
            sda1.Fill(dt3);
            testinfo.DataSource = dt3;
            conn.Close();
        }

        private void Tests_Load(object sender, EventArgs e)
        {
            loaddata();
        }
        DateTime startdate;
        DateTime enddate; 
        private void testinfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            examid = testinfo.SelectedRows[0].Cells[1].Value.ToString();
            conn.Open();
            string query1 = "select * from Students, Classes, Student_Classes, Exams, Subject where Students.Student_id = '" + msv + "' and Exams.Exam_id ='" + examid + "'";
            SqlCommand cmd2 = new SqlCommand(query1, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd2);
            DataTable dt4 = new DataTable();
            sda1.Fill(dt4);
            foreach (DataRow dr1 in dt4.Rows)
            {
                lbsubject.Text = dr1["Subject_name"].ToString();
                lborder.Text = dr1["Exam_order"].ToString();
                lbtime.Text = dr1["Time"].ToString();
                string a = dr1["Start_date"].ToString();
                startdate =Convert.ToDateTime(dr1["Start_date"].ToString());
                lbstart.Text = a.Substring(0, 10);
                string b = dr1["End_date"].ToString();
                enddate = Convert.ToDateTime(dr1["End_date"].ToString());
                lbend.Text = b.Substring(0, 10);
                lblimit.Text = dr1["limited_times"].ToString();
                lbnumber.Text = dr1["number_question"].ToString();

                lbsubject.Visible = true;
                lborder.Visible = true;
                lbtime.Visible = true;
                lbstart.Visible = true;
                lbend.Visible = true;
                lblimit.Visible = true;
                lbnumber.Visible = true;
            }    
            conn.Close();

        }

        private void vaothi_Click(object sender, EventArgs e)
        {
            if (lbsubject.Visible == false)
            {
                MessageBox.Show("Hay chon bai thi");
            }   
            else
            {
                DateTime curtdate = DateTime.Now;                
                int result1 = DateTime.Compare(startdate, curtdate);
                int result2 = DateTime.Compare(curtdate, enddate);
                if (result1 < 0 || result2 < 0)
                {
                    MessageBox.Show("Co the vao thi");
                }
                else
                {
                    MessageBox.Show("Da qua han thi hoac chua den lich thi");
                }
            }
        }
    }
}
