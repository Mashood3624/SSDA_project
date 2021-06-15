using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Camera
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            String constr = ConfigurationManager.ConnectionStrings["CameraString"].ConnectionString;
            connection = new MySqlConnection(constr);
        }
        
        MySqlConnection connection;
 
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtPassword.Text == "")
            {
                MessageBox.Show("Please provide UserName and Password");
                return;
            }
            try
            {

                connection.Open();
                String check_query = "SELECT * FROM SSDA.images WHERE User_name ='" + txtUsername.Text + "' AND password ='" + txtPassword.Text + "'";
                MySqlCommand cmd = new MySqlCommand(check_query, connection);
                object result = cmd.ExecuteScalar();
                connection.Close();
                if (result != null)
                {
                    MessageBox.Show("Login Successful!");
                    this.Hide();
                    Form1 fm = new Form1(txtUsername.Text);
                    fm.Show();
                    
                }
                else
                {
                    MessageBox.Show("Login Failed!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void login_Load(object sender, EventArgs e)
        {

        }

       
    }
}
