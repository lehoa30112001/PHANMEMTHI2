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
    public partial class Student_Login : Form
    {
        public Student_Login()
        {
            InitializeComponent();
        }
        string msv; 
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        public Student_Login(string stuser)
        {
            msv = stuser;
            InitializeComponent();
            string query = "select * from students where Student_id = '" + stuser + "'";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt2 = new DataTable();
            sda.Fill(dt2);
            foreach (DataRow dr in dt2.Rows)
            {
                studentid.Text = dr["Student_id"].ToString();
                studentname.Text = dr["Student_name"].ToString();
                string s = dr["Birthday"].ToString();
                string[] d = s.Split('/');
                studentbirthday.Text = d[0] + '/' + d[1]+ '/' + d[2].Substring(0,4);
            }
        }
        private void logoutbutton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login log = new Login();
            log.Show();
        }


        private void dotest_Click(object sender, EventArgs e)
        {
            this.Hide();
            Tests ts = new Tests(msv);
            ts.Show();
        }
    }
}
