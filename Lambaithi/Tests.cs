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
        function fn = new function();
        string query;
        string msv;
        string examid;
        int lanthi;
        DateTime startdate;
        DateTime enddate;
        public Tests(string stuser)
        {
            msv = stuser;
            InitializeComponent();            
            query = "select * from students where Student_id = '" + stuser + "'";
            DataTable dt = fn.getdt(query);
            foreach (DataRow dr in dt.Rows)
            {
                studentid.Text = dr["Student_id"].ToString();
                studentname.Text = dr["Student_name"].ToString();
                string s = dr["Birthday"].ToString();
                string[] d = s.Split('/');
                studentbirthday.Text = d[0] + '/' + d[1] + '/' + d[2].Substring(0, 4);
            }
        } //Lấy thông tin up lên đầu

        private void hombut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Student_Login stlogin = new Student_Login(msv);
            stlogin.Show();
        }
        private void loaddata()
        {
            query = "select Classes.Class_name as N'Lớp học phần', Exams.Exam_id as N'Mã đề', Exams.Exam_order as N'Loại bài thi', Exams.Time as N'Thời gian', Exams.limited_times as N'Giới hạn' from Students, Classes, Student_Classes, Exams where Students.Student_id = '" + msv + "' and number_question > 0 and Students.Student_id = Student_Classes.Student_id and Classes.Class_id = Student_Classes.Class_id and Exams.Class_id = Classes.Class_id";
            DataTable dt = fn.getdt(query);
            testinfo.DataSource = dt;
        } // Load dữ liệu vào Data Grid

        private void Tests_Load(object sender, EventArgs e)
        {
            loaddata();
        }
        
        private void testinfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            examid = testinfo.SelectedRows[0].Cells[1].Value.ToString();
            query = "select * from Students, Classes, Student_Classes, Exams, Subject where Students.Student_id = '" + msv + "' and Exams.Exam_id ='" + examid + "' and Students.Student_id = Student_Classes.Student_id and Classes.Class_id = Student_Classes.Class_id and Exams.Class_id = Classes.Class_id and Subject.Subject_id = Classes.Subject_id";
            DataTable dt = fn.getdt(query);
            foreach (DataRow dr1 in dt.Rows)
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
        }
         // Lấy dữ liệu của từng dòng khi click vào Data Grid 
        private void vaothi_Click(object sender, EventArgs e)
        {
            query = "select max(Times) from Student_Exam_Result where Exam_id = '" + examid + "' and Student_Exam_Result.Student_id = '" + msv + "'";
            DataTable dt = fn.getdt(query);
            if (dt.Rows[0][0].ToString() == "")
                lanthi = 0;
            else 
                lanthi = Convert.ToInt32(dt.Rows[0][0].ToString());            
            int gioihan = Convert.ToInt32(lblimit.Text);
            if (lbsubject.Visible == false)
            {
                MessageBox.Show("Hay chon bai thi");
            }   
            else
            {
                DateTime curtdate = DateTime.Now;                
                int result1 = DateTime.Compare(startdate, curtdate);
                int result2 = DateTime.Compare(curtdate, enddate);
                if (result1 < 0 && result2 < 0) //Check xem có còn hạn thi không
                {
                    if (lanthi < gioihan)  //check xem đã làm quá số lần giới hạn chưa
                    {
                        this.Hide();
                        Do_Test dtest = new Do_Test(msv, examid, lanthi + 1);
                        dtest.Show();
                    }
                    else
                        MessageBox.Show("Đã hết lần thi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Chưa đến lịch thi hoặc đã quá hạn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        } //Vào thi
    }
}
