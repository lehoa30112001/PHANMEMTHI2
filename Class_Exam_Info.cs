﻿using System;
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
    public partial class Class_Exam_Info : Form
    {
        public Class_Exam_Info()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        DataTable dt3 = new DataTable();
        DataTable temp = new DataTable();
        string resultid, classname, examtype, examid;
        int numberquestion;
        public Class_Exam_Info(string classid, string stuid, string name, string birthday)
        {
            InitializeComponent();
            studentid.Text = stuid;
            studentname.Text = name;
            studentbirthday.Text = birthday;
            conn.Open();
            string query1 = "select * from Exams, Student_Exam_Result where exams.Exam_id = Student_Exam_Result.Exam_id and class_id = '" + classid + "'";
            string query = "select Exams.Exam_id as N'Mã đề thi', Classes.Class_name as N'Lớp', Exams.Exam_order as N'Loại bài thi' , number_question as N'Số câu hỏi', Score as N'Điểm', Access_time as N'Thời gian làm', times as N'Lần', Student_Exam_Result.Result_id as N'Mã kết quả'  from Exams, Student_Exam_Result, classes where exams.Exam_id = Student_Exam_Result.Exam_id and Classes.class_id = '" + classid + "'";
            SqlCommand cmd1 = new SqlCommand(query, conn);
            SqlCommand cmd2 = new SqlCommand(query1, conn);
            SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
            SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
            sda2.Fill(temp);
            sda1.Fill(dt3);
            classinfo.DataSource = dt3;           
            conn.Close();
        }

        private void hombut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Student_Login stlogin = new Student_Login(studentid.Text);
            stlogin.Show();
        }

        private void Chitiet_Click(object sender, EventArgs e)
        {
            this.Hide();
            ShowExamResult se = new ShowExamResult(resultid, studentname.Text, classname, examtype, numberquestion, studentid.Text, examid, "examinfo");
            se.Show();
        }

        private void classinfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            resultid = classinfo.SelectedRows[0].Cells[7].Value.ToString();
            examtype = classinfo.SelectedRows[0].Cells[2].Value.ToString();
            numberquestion = Convert.ToInt32(classinfo.SelectedRows[0].Cells[3].Value.ToString());
            foreach (DataRow dr1 in dt3.Rows)
            {
                examid = dr1[0].ToString();
                classname = dr1[1].ToString();
            }    
        }

        private void backbt_Click(object sender, EventArgs e)
        {
            this.Hide();
            Test_Result tr = new Test_Result(studentid.Text, studentname.Text, studentbirthday.Text);
            tr.Show();
        }
    }
}
