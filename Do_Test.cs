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
    public partial class Do_Test : Form
    {
        public Do_Test()
        {
            InitializeComponent();
        }
        int numberquestion;
        string exid, stuid;
        int currentquestion = 0; 
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        public Do_Test(string msv, string examid)
        {
            InitializeComponent();
            exid = examid;
            stuid = msv;
            conn.Open();
            string query = "select * from Students, Classes, Student_Classes, Exams, Subject where Students.Student_id = '" + msv + "' and Exams.Exam_id ='"+examid+"'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                string a = dr["number_question"].ToString();
                numberquestion = Convert.ToInt32(a);
                stid.Text = dr["Student_id"].ToString();
                stname.Text = dr["Student_name"].ToString();
                exorder.Text = dr["exam_order"].ToString();
                extime.Text = dr["time"].ToString();
                clname.Text = dr["class_name"].ToString();
            }
            conn.Close();            
        }
        DataTable question = new DataTable();
        DataTable answer = new DataTable();
        string resultid;
        string now;
        bool[] danhap = new bool[40];
        private void loadquestion(string examid)
        {
            string query1 = "select Question_id, Question as qt from Question, Exams where Exams.Exam_id = '"+examid+"'";
            SqlCommand cmd1 = new SqlCommand(query1, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            sda1.Fill(question);
        }
        private void loadanswer(string questionid)
        {
            answer.Clear();
            string query = "select Anwer_id, Anwer from Answer where Question_id = '" + questionid + "'";
            SqlCommand cmd1 = new SqlCommand(query, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            sda1.Fill(answer);
        }
        private void question_answer (int number)
        {
            causo.Text = "Câu " + (number + 1).ToString(); 
            lbquestion.Text = question.Rows[number][1].ToString();
            string questionid = question.Rows[number][0].ToString();
            loadanswer(questionid);
            answer1.Text = answer.Rows[0][1].ToString();
            answer2.Text = answer.Rows[1][1].ToString();
            answer3.Text = answer.Rows[2][1].ToString();
            answer4.Text = answer.Rows[3][1].ToString();
        }
        private void Do_Test_Load(object sender, EventArgs e)
        {
            lbnumber.Text = numberquestion.ToString();
            loadquestion(exid);
            if (currentquestion == 0)
            {
                btback.Enabled = false;
                btnext.Enabled = true;
                question_answer(currentquestion);
            }
            int i; 
            for (i=1;i<=numberquestion; i++)
            {
                danhap[i] = false;
            }
            now = DateTime.Now.ToString();
            resultid = "result" + stuid + exid + now;
            string query = "insert into Student_Exam_Result values ('"+resultid+"', '"+stuid+"', '"+exid+"', 0, '"+DateTime.Now+"', 0)";
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            currentquestion =  Convert.ToInt32(button.Text) - 1;
            question_answer(Convert.ToInt32(button.Text) - 1);
            if (currentquestion == 0)
                btback.Enabled = false;            
            else
                 btback.Enabled = true;            
            if (currentquestion == numberquestion - 1)
                btnext.Enabled = false;            
            else
                btnext.Enabled = true;            
        }

        private void btback_Click(object sender, EventArgs e)
        {
            if (currentquestion == 0)
                btback.Enabled = false;
            else
                btback.Enabled = true;
            btnext.Enabled = true;
            question_answer(currentquestion - 1);
            currentquestion = currentquestion - 1; 
        }        
        private void answer1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = (RadioButton)sender; 
            string questionidnow = question.Rows[currentquestion][0].ToString();
            string answeridnow = answer.Rows[button.TabIndex][0].ToString();
            string now = DateTime.Now.ToString();
            string choiceid = "choice" + resultid + answeridnow;             
            if (!danhap[currentquestion])
            {
                string query = "insert into Student_Choice values ('" + choiceid + "', '" + resultid + "', '" + answeridnow + "','" + questionidnow + "')";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                danhap[currentquestion] = true;
                conn.Close();
            }
            else
            {
                string query = "update Student_Choice set Answer_id = '"+answeridnow+"' where Choise_history_id = '"+choiceid+"'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }           
        }

        private void btnext_Click(object sender, EventArgs e)
        {
            btback.Enabled = true; 
            question_answer(currentquestion + 1);
            currentquestion = currentquestion + 1;
            if (currentquestion == numberquestion - 1)
                btnext.Enabled = false;
            else
                btnext.Enabled = true;
        }
    }
}
