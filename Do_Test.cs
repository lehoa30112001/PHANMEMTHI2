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
        bool[] danhap = new bool[40];
        int[] dapan = new int[40];
        int totalpage;
        int currentpage; 
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
        private void question_answer(int number)
        {
            causo.Text = "Câu " + (number + 1).ToString();
            lbquestion.Text = question.Rows[number][1].ToString();
            string questionid = question.Rows[number][0].ToString();
            loadanswer(questionid);
            if (danhap[number] == false)
            {
                answer1.Checked = false;
                answer2.Checked = false;
                answer3.Checked = false;
                answer4.Checked = false;
            }
            else
            {
                switch(dapan[number])
                {
                    case 0:
                        answer1.Checked = true;
                        break;
                    case 1:
                        answer2.Checked = true;
                        break;
                    case 2:
                        answer3.Checked = true;
                        break;
                    case 3:
                        answer4.Checked = true;
                        break;
                    default:
                        break;
                }                
            }                 
            answer1.Text = "A." + answer.Rows[0][1].ToString();
            answer2.Text = "B." + answer.Rows[1][1].ToString();
            answer3.Text = "C." + answer.Rows[2][1].ToString();
            answer4.Text = "D." + answer.Rows[3][1].ToString();
        }
        private void Do_Test_Load(object sender, EventArgs e)
        {
            answer1.Checked = false;
            answer2.Checked = false;
            answer3.Checked = false;
            answer4.Checked = false;
            lbnumber.Text = numberquestion.ToString();
            loadquestion(exid);
            btback.Enabled = false;
            backpage.Enabled = false;
            question_answer(currentquestion);

            currentpage = 1; 
            if (numberquestion % 20 == 0)
            {
                totalpage = numberquestion / 20;
            }    
            else
            {
                totalpage = numberquestion / 20 + 1;
            }
            page.Text = currentpage + "/" + totalpage;

            foreach (Control item in Panel2.Controls)
            {
                int i; 
                for (i = 1; i <= numberquestion; i++)
                {
                    string buttonname = "button" + i;
                    if (item.Name == buttonname)
                    {
                        Button b = item as Button;
                        if (b != null)
                        {
                            b.Visible = true;
                        }
                    }
                }    
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
            currentquestion = currentquestion - 1;
            btnext.Enabled = true;
            if (currentquestion == 0)
                btback.Enabled = false;
            question_answer(currentquestion);            
        }        
        private void saveanswer(int question0, int answer0)
        {
            foreach (Control item in Panel2.Controls)
            {
                string buttonname = "button" + (question0 + 1);
                if (item.Name == buttonname)
                {
                    Button b = item as Button;
                    if (b != null)
                    {
                        b.BackColor = Color.Red;
                    }
                }
            }    
            string questionidnow = question.Rows[question0][0].ToString();
            string answeridnow = answer.Rows[answer0][0].ToString();
            string now = DateTime.Now.ToString();
            string choiceid = "choice" + resultid + questionidnow + answer0;
            if (!danhap[question0])
            {
                string query = "insert into Student_Choice values ('" + choiceid + "', '" + resultid + "', '" + answeridnow + "','" + questionidnow + "')";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                danhap[currentquestion] = true;
                dapan[currentquestion] = answer0;
            }
            else
            {
                choiceid = "choice" + resultid + questionidnow + dapan[question0];
                string query = "update Student_Choice set Answer_id = '" + answeridnow + "' where Choise_history_id = '" + choiceid + "'";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                dapan[question0] = answer0;
            }
        }

        private void answer1_Click(object sender, EventArgs e)
        {
            answer1.Checked = true;
            saveanswer(currentquestion, 0);
        }

        private void answer2_Click(object sender, EventArgs e)
        {
            answer2.Checked = true;
            saveanswer(currentquestion, 1);
        }

        private void answer3_Click(object sender, EventArgs e)
        {
            answer3.Checked = true;
            saveanswer(currentquestion, 2);
        }

        private void answer4_Click(object sender, EventArgs e)
        {
            answer4.Checked = true;
            saveanswer(currentquestion, 3);
        }
        private void loadpage (int number)
        {
            foreach (Control item in Panel2.Controls)
            {
                int i;
                for (i=1; i<=20; i++)
                {
                    string buttonname = "button" + i;
                    if (item.Name == buttonname)
                    {
                        Button b = item as Button;
                        b.Text = ((number - 1) * 20 + i).ToString(); 
                    }
                }
                page.Text = number + "/" + totalpage;
            }
        }

        private void backpage_Click(object sender, EventArgs e)
        {
            currentpage = currentpage - 1; 
            nextpage.Enabled = true;
            if (currentpage == 1)
                backpage.Enabled = false;
            loadpage(currentpage);
        }

        private void nextpage_Click(object sender, EventArgs e)
        {
            currentpage = currentpage + 1;
            backpage.Enabled = true;
            if (currentpage == totalpage)
                nextpage.Enabled = false;
            loadpage(currentpage);
        }

        private void btnext_Click(object sender, EventArgs e)
        {
            
            currentquestion = currentquestion + 1;
            btback.Enabled = true; 
            question_answer(currentquestion);            
            if (currentquestion == numberquestion - 1)
                btnext.Enabled = false;
        }


    }
}
