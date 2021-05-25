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
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        public ShowExamResult(string examresultid)
        {
            InitializeComponent();
            conn.Open();
            string countanswer = "select * from Student_Choice, Answer where Student_Choice.Result_id = '"+examresultid+"' and Answer.is_true = 1 and Answer.Anwer_id = Student_Choice.Answer_id";
            string access = "select Access_Time from Student_Exam_Result where Student_Exam_Result = '"+ examresultid + "'";
            SqlCommand cmd = new SqlCommand(countanswer, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            truenumber = dt.Rows.Count;
        }
    }
}
