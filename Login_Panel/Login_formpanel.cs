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
namespace PHANMEMTHI.Login_Panel
{
    public partial class Login_formpanel : UserControl
    {
        public Login_formpanel()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-1LOB8EI;Initial Catalog=phanmemthi;Integrated Security=True");
        bool teacher = false;
        bool student = false;
        private void gvbutton_Click(object sender, EventArgs e)
        {
            svbutton.FillColor = Color.White;
            svbutton.ForeColor = Color.CornflowerBlue;
            gvbutton.FillColor = Color.CornflowerBlue;
            gvbutton.ForeColor = Color.White;
            teacher = true;
            student = false;
        }

        private void svbutton_Click(object sender, EventArgs e)
        {
            gvbutton.FillColor = Color.White;
            gvbutton.ForeColor = Color.CornflowerBlue;
            svbutton.FillColor = Color.CornflowerBlue;
            svbutton.ForeColor = Color.White;
            teacher = false;
            student = true;
        }

        private void showpass_CheckedChanged(object sender, EventArgs e)
        {
            if (showpass.Checked == true)
            {
                passbox.UseSystemPasswordChar = false;
            }
            else
            {
                passbox.UseSystemPasswordChar = true;
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            userbox.Text = "";
            passbox.Text = "";
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (teacher)
            {
                string query = "select * from teachers where teacher_id = '"+userbox.Text+"' and Teacher_password = '"+ passbox.Text +"'";
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    MessageBox.Show("Dang nhap thanh cong");
                }  
                else
                {
                    MessageBox.Show("Nhap sai ten nguoi dung hoac mat khau");
                }    
            }   
            else if (student)
            {
                string query = "select * from students where student_id = '" + userbox.Text + "' and student_password = '" + passbox.Text + "'";
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    Login.ActiveForm.Hide();
                    Student_Login stlogin = new Student_Login(userbox.Text);                    
                    stlogin.Show();

                }
                else
                {
                    MessageBox.Show("Nhap sai ten nguoi dung hoac mat khau");
                }
            }
            else
            {
                MessageBox.Show("Chon dang nhap sinh vien hoac giao vien");
            } 
                
            
        }
    }
}
