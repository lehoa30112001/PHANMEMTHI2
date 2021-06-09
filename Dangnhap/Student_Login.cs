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
        function fn = new function();
        string query; 
        string msv, hoten, ngaysinh; 
        
        public Student_Login(string stuser)
        {
            msv = stuser;
            InitializeComponent();
            query = "select * from students where Student_id = '" + stuser + "'";
            DataTable dt = fn.getdt(query);
            foreach (DataRow dr in dt.Rows)
            {
                studentid.Text = dr["Student_id"].ToString();
                hoten = dr["Student_name"].ToString();
                studentname.Text = hoten;
                string s = dr["Birthday"].ToString();
                string[] d = s.Split('/');
                studentbirthday.Text = d[0] + '/' + d[1]+ '/' + d[2].Substring(0,4);
                ngaysinh = studentbirthday.Text;
            }
        } //Lấy thông tin up lên đầu

        private void testresult_Click(object sender, EventArgs e)
        {
            this.Hide();
            Test_Result tr = new Test_Result(msv, hoten, ngaysinh);
            tr.Show();
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
