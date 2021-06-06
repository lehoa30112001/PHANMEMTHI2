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
using System.Timers;

namespace PHANMEMTHI
{
    public partial class Do_Test : Form
    {
        public Do_Test()
        {
            InitializeComponent();
        }
        int numberquestion, examtime;
        string exid, stuid, examtype;
        int currentquestion = 0;
        bool[] danhap = new bool[120]; //Kiểm tra xem câu i đã nhập câu trả lời chưa
        int[] dapan = new int[120]; //Lưu đáp án câu i = 0, 1, 2, 3; 
        int totalpage;
        int currentpage; //Trang số câu hỏi hiện tại (có 20 ô số nhỏ ở bên phải, mỗi trang 20 ô)
        string classname;
        int h, m, s;
        int times;
        function fn = new function();
        string query;         
        public Do_Test(string msv, string examid, int lanthi)
        {
            InitializeComponent();
            times = lanthi; 
            exid = examid;
            stuid = msv;
            query = "select * from Students, Classes, Student_Classes, Exams, Subject where Students.Student_id = '" + msv + "' and Exams.Exam_id ='" + examid + "'";
            DataTable dt = fn.getdt(query);
            foreach (DataRow dr in dt.Rows)
            {
                string a = dr["number_question"].ToString();
                numberquestion = Convert.ToInt32(a);
                stid.Text = dr["Student_id"].ToString();
                stname.Text = dr["Student_name"].ToString();
                exorder.Text = dr["exam_order"].ToString();
                examtype = dr["exam_order"].ToString();
                extime.Text = dr["time"].ToString();
                examtime = Convert.ToInt32(dr["time"].ToString());
                clname.Text = dr["class_name"].ToString();
                classname = dr["class_name"].ToString();
            }
        }
        
        DataTable question = new DataTable();
        DataTable answer = new DataTable();
        string resultid;
        string now;
        private void loadquestion(string examid)
        {
            query = "select Question_id, Question as qt from Question, Exams where Exams.Exam_id = '" + examid + "'";
            question = fn.getdt(query);
        } //Load cau hoi vao bang question
        private void loadanswer(string questionid)
        {
            answer.Clear();
            query = "select Anwer_id, Anwer from Answer where Question_id = '" + questionid + "'";
            answer = fn.getdt(query);
        } //Load cau tra loi vao bang answer
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
                switch (dapan[number])
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
            btback.Visible = false;
            backpage.Visible = false;
            question_answer(currentquestion);
            currentpage = 1;

            if (numberquestion % 20 == 0)
            {
                totalpage = numberquestion / 20;
            }
            else
            {
                totalpage = numberquestion / 20 + 1;
            }       //Tính số trang câu hỏi (mỗi trang 20 câu) 
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
            } //Chỉ hiện số button = tổng số câu hỏi

            now = DateTime.Now.ToString();
            resultid = "result" + stuid + exid + now;
            query = "insert into Student_Exam_Result values ('" + resultid + "', '" + stuid + "', '" + exid + "', 0, '" + DateTime.Now + "', 0, '" + times + "')";
            fn.setdata(query);  //Set mã đề thi
            h = examtime / 60;
            m = examtime - (h * 60);
            s = 0;
            elaptime.Text = h + ":" + m + ":" + s;
            if (numberquestion <= 20)
                nextpage.Visible = false;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            currentquestion = Convert.ToInt32(button.Text) - 1;
            question_answer(Convert.ToInt32(button.Text) - 1);
            if (currentquestion == 0)
                btback.Visible = false;
            else
                btback.Visible = true;
            if (currentquestion == numberquestion - 1)
                btnext.Visible = false;
            else
                btnext.Visible = true;
        }
        private void btback_Click(object sender, EventArgs e)
        {
            currentquestion--;
            btnext.Visible = true;
            if (currentquestion == 0)
                btback.Visible = false;
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
            } // Chuyển các câu hỏi đã nhập sang đỏ
            string questionidnow = question.Rows[question0][0].ToString();
            string answeridnow = answer.Rows[answer0][0].ToString();
            string choiceid = "choice" + resultid + questionidnow + answer0;
            if (!danhap[question0])  
            {
                query = "insert into Student_Choice values ('" + choiceid + "', '" + resultid + "', '" + answeridnow + "','" + questionidnow + "')";
                fn.setdata(query);
                danhap[currentquestion] = true;
                dapan[currentquestion] = answer0;
            } //Nếu như đáp án chưa được nhập, có nghĩa câu đó làm lần đầu thì sẽ tạo mới (insert)
            else
            {
                choiceid = "choice" + resultid + questionidnow + dapan[question0];
                query = "update Student_Choice set Answer_id = '" + answeridnow + "'  where Choise_history_id = '" + choiceid + "'";
                fn.setdata(query);
                dapan[question0] = answer0;
            } // Nếu đã nhập trước đó thì cập nhật vào cái choiceid cũ
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m < 5)
                elaptime.ForeColor = Color.Red;
            elaptime.Text = h + ":" + m + ":" + s;
            if (s == 0)
            {
                if (m == 0)
                {
                    if (h == 0)
                    {
                        timer1.Enabled = false;
                        takeaccesstime();
                        MessageBox.Show("Đã lưu kết quả kiểm tra", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                        ShowExamResult se = new ShowExamResult(resultid, stname.Text, classname, examtype, numberquestion, stuid, exid, "dotest");
                        se.Show();
                    }
                    else
                    {
                        m = 59;
                        h--;
                    }
                }
                else
                {
                    s = 59;
                    m--;
                }

            }
            else s--;            
        }

        private void loadpage(int number)
        {
            foreach (Control item in Panel2.Controls)
            {
                int i;
                for (i = 1; i <= 20; i++)
                {
                    string buttonname = "button" + i;
                    if (item.Name == buttonname)
                    {
                        Button b = item as Button;
                        b.Text = ((number - 1) * 20 + i).ToString();
                        if (danhap[Convert.ToInt32(b.Text.ToString()) - 1]  == false)
                            b.BackColor = Color.White;
                        else b.BackColor = Color.Red;
                    }
                }
                page.Text = number + "/" + totalpage;
            }
        }

        private void backpage_Click(object sender, EventArgs e)
        {
            currentpage--;
            nextpage.Visible = true;
            if (currentpage == 1)
                backpage.Visible = false;
            loadpage(currentpage);
        }
        private void nextpage_Click(object sender, EventArgs e)
        {
            currentpage = currentpage + 1;
            backpage.Visible = true;
            if (currentpage == totalpage)
                nextpage.Visible = false;
            loadpage(currentpage);
        }
        private void btsubmit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Kết thúc bài kiểm tra và lưu kết quả?", "Xác nhận nộp bài", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                timer1.Enabled = false;
                takeaccesstime();
                MessageBox.Show("Đã lưu kết quả kiểm tra", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                ShowExamResult se = new ShowExamResult(resultid, stname.Text, classname, examtype, numberquestion, stuid, exid, "dotest");
                se.Show();
            }    
            
        }

        private void btnext_Click(object sender, EventArgs e)
        {
            
            currentquestion = currentquestion + 1;
            btback.Visible = true; 
            question_answer(currentquestion);            
            if (currentquestion == numberquestion - 1)
                btnext.Visible = false;
        }
        private void takeaccesstime()
        {
            int elap = h * 3600 + m * 60 + s;
            int access = examtime * 60;
            int second = access - elap;
            string query = "update Student_Exam_Result set Access_time = '" + second + "' where Result_id = '" + resultid + "'";
            fn.setdata(query);
        } // Lấy tổng thời gian làm bài
    }
}
