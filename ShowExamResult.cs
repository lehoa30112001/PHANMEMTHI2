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
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        public ShowExamResult(string examresultid, string studentname, string classname, string examtype, int questionnumber, string stuid, string exam)
        {
            examid = exam;
            studentid = stuid;
            InitializeComponent();
            conn.Open();
            string countanswer = "select Student_Choice.Question_id,Question.Question, Answer.Anwer, is_true from Student_Choice, Answer, Question where Student_Choice.Result_id = '"+examresultid+"' and Answer.is_true = 1 and Answer.Anwer_id = Student_Choice.Answer_id and Question.Question_id = Student_Choice.Question_id";
            string access = "select Access_Time from Student_Exam_Result where Student_Exam_Result.Result_id = '" + examresultid + "'";
            SqlCommand cmd = new SqlCommand(countanswer, conn);
            SqlCommand cmd1 = new SqlCommand(access, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            DataTable resultinfo = new DataTable();
            DataTable accessinfo = new DataTable();
            sda1.Fill(accessinfo);
            sda.Fill(resultinfo);
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
                extime.Text = min + ":" + second;
            }
            conn.Close();
            double score = Convert.ToDouble(truenumber) * 10.0/Convert.ToDouble(questionnumber);
            string query1 = "update Student_Exam_Result set Score = '" + score + "' where Result_id = '" + examresultid + "'";
            conn.Open();
            SqlCommand cmd2 = new SqlCommand(query1, conn);
            cmd2.ExecuteNonQuery();
            conn.Close();
        }
        public void loaddata()
        {
            conn.Open();
            string load = "select ROW_NUMBER() OVER (ORDER BY Question.Question_id) AS N'Số thứ tự' ,Question.Question as N'Câu hỏi', Answer.Anwer as N'Câu trả lời', is_true as N'Điểm' from Student_Choice, Answer, Question where Student_Choice.Result_id = '" + reid +"'  and Answer.Anwer_id = Student_Choice.Answer_id and Question.Question_id = Student_Choice.Question_id";
            SqlCommand cmd = new SqlCommand(load, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable Result = new DataTable();
            sda.Fill(Result);
            resulttb.DataSource = Result;
            conn.Close();
        }

        private void ShowExamResult_Load(object sender, EventArgs e)
        {
            loaddata();
        }

        private void homebutton_Click(object sender, EventArgs e)
        {
            this.Close();
            Student_Login stlogin = new Student_Login(studentid);
            stlogin.Show();
        }

        private void redobutton_Click(object sender, EventArgs e)
        {
            this.Close();
            Do_Test dtest = new Do_Test(studentid, examid);
            dtest.Show();
        }
    }
}
