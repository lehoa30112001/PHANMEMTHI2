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
    public partial class ShowExamResult : Form
    {
        public ShowExamResult()
        {
            InitializeComponent();
        }
        int truenumber;
        string reid;
        string studentid;
        string examid;
        DateTime startdate, enddate;
        int limited, lanthi;
        string huongve;
        string class_id, birthday;
        function fn = new function();        
        public ShowExamResult(string examresultid, string studentname, string classname, string examtype, int questionnumber, string stuid, string exam, string direct)
        {
            examid = exam;
            huongve = direct; 
            studentid = stuid;
            InitializeComponent();
            string countanswer = "select Student_Choice.Question_id,Question.Question, Answer.Anwer, is_true from Student_Choice, Answer, Question where Student_Choice.Result_id = '" + examresultid + "' and Answer.is_true = 1 and Answer.Anwer_id = Student_Choice.Answer_id and Question.Question_id = Student_Choice.Question_id";
            string access = "select Access_Time from Student_Exam_Result where Student_Exam_Result.Result_id = '" + examresultid + "'";
            DataTable resultinfo = fn.getdt(countanswer);
            DataTable accessinfo = fn.getdt(access);            
            reid = examresultid;
            truenumber = resultinfo.Rows.Count;
            stname.Text = studentname;
            clname.Text = classname;
            exorder.Text = examtype;
            lbnumber.Text = truenumber + "/" + questionnumber;
            foreach (DataRow dr in accessinfo.Rows)
            {
                int second = Convert.ToInt32(dr[0].ToString());
                int min = second / 60;
                second = second - min * 60;
                extime.Text = string.Format("{0}:{1}", min.ToString().PadLeft(2, '0'), second.ToString().PadLeft(2, '0'));
                //extime.Text = min + ":" + second;
            }
            double score = Convert.ToDouble(truenumber) * 10.0 / Convert.ToDouble(questionnumber);
            string query1 = "update Student_Exam_Result set Score = '" + score + "' where Result_id = '" + examresultid + "'";
            fn.setdata(query1);
        }
        public void loaddata()
        {
            string load = "select ROW_NUMBER() OVER (ORDER BY Question.Question_id) AS N'Số thứ tự' ,Question.Question as N'Câu hỏi', Answer.Anwer as N'Câu trả lời', is_true as N'Điểm' from Student_Choice, Answer, Question where Student_Choice.Result_id = '" + reid + "'  and Answer.Anwer_id = Student_Choice.Answer_id and Question.Question_id = Student_Choice.Question_id";            
            DataTable Result = fn.getdt(load);
            resulttb.DataSource = Result;
        }

        private void ShowExamResult_Load(object sender, EventArgs e)
        {
            loaddata();
        }

        private void backbutton_Click(object sender, EventArgs e)
        {
            string query = "Select Class_id from Exams where Exam_id = '" + examid + "'";
            string query2 = "select Birthday from Students where Student_id = '" + studentid + "'";             
            DataTable dt4 = fn.getdt(query);
            DataTable dt = fn.getdt(query2);
            class_id = dt4.Rows[0][0].ToString();
            foreach (DataRow dr in dt.Rows)
            {
                string s = dr[0].ToString();
                string[] d = s.Split('/');
                birthday = d[0] + '/' + d[1] + '/' + d[2].Substring(0, 4);
            }
            if (huongve == "dotest")
            {
                this.Close();
                Tests te = new Tests(studentid);
                te.Show();
            }    
            else if (huongve == "examinfo")
            {
                this.Close();
                Class_Exam_Info ce = new Class_Exam_Info(class_id, studentid, stname.Text, birthday);
                ce.Show();
            }
        }

        private void homebutton_Click(object sender, EventArgs e)
        {
            this.Close();
            Student_Login stlogin = new Student_Login(studentid);
            stlogin.Show();
        }

        private void redobutton_Click(object sender, EventArgs e)
        {
            string query = "select max(Times) as lan from Student_Exam_Result where Exam_id = '" + examid + "'";
            DataTable dt = fn.getdt(query);
            lanthi = Convert.ToInt32(dt.Rows[0][0].ToString());

            query = "Select start_date, end_date, limited_times from Exams where Exam_id = '" + examid + "'";
            DataTable dt4 = fn.getdt(query);

            foreach (DataRow dr in dt4.Rows)
            {
                startdate = Convert.ToDateTime(dr[0].ToString());
                enddate = Convert.ToDateTime(dr[1].ToString());
                limited = Convert.ToInt32(dr[2].ToString());
            }

            DateTime curtdate = DateTime.Now;
            int result1 = DateTime.Compare(startdate, curtdate);
            int result2 = DateTime.Compare(curtdate, enddate);
            if (result1 < 0 && result2 < 0)
            {
                if (lanthi < limited)
                {
                    this.Close();
                    Do_Test dtest = new Do_Test(studentid, examid, lanthi + 1);
                    dtest.Show();
                }
                else
                    MessageBox.Show("Đã hết lần thi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
    }
}
