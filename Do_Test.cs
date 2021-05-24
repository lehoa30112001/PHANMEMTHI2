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
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        public Do_Test(string msv, string examid)
        {
            InitializeComponent();
            conn.Open();
            string query = "select * from Students, Classes, Student_Classes, Exams, Subject where Students.Student_id = '" + msv + "' and Exams.Exam_id ='" + examid + "'";
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
        bool[] done = new bool[40];
        int pointnow = 0;
        private void btok_Click(object sender, EventArgs e)
        {
             
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            pointnow = Convert.ToInt32(button.Text);
        }

        private void Do_Test_Load(object sender, EventArgs e)
        {

        }
    }
}
