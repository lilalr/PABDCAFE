using PABDCAFE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class LoginPage : Form
    {
        // ganti "server" sesuai dengan SQL server anda
        private string baseconnectionString = "Data Source=IDEAPAD5PRO\\LILA; Initial Catalog=ReservasiCafe;";

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginAdmin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text.Trim();
            string Password = txtPassword.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(baseconnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password", conn);
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    int userCount = (int)cmd.ExecuteScalar();

                    if (userCount > 0)
                    {
                        if (Username == "admin")
                        {
                            // Masuk ke halaman admin
                            MessageBox.Show("Login berhasil sebagai admin.");
                            AdminPage admin = new AdminPage();
                            admin.Show();
                            this.Hide();
                        }   
                    }
                    
                    else if (Username == "customer")
                    {
                        // Masuk ke halaman customer
                        MessageBox.Show("Login berhasil sebagai customer.");
                        CustomerPage customer = new CustomerPage();
                        customer.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Akun ini tidak dikenali.");
                    }
                }
            }
            catch (Exception) 
            {
                MessageBox.Show("Login gagal. Periksa username dan password!");
            }
        }
    }
}
