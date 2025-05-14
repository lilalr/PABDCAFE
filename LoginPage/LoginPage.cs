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
            string Username = txtUsername.Text;
            string Password = txtPassword.Text;

            try
            {
                string connectionString = baseconnectionString + $"User ID={Username};Password={Password}";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                 
                    if (Username == "admin")
                    {
                        // Masuk ke halaman admin
                        MessageBox.Show("Login berhasil sebagai admin.");
                        AdminPage admin = new AdminPage();
                        admin.Show();
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
            catch (Exception ex)
            {
                MessageBox.Show("Login gagal. Periksa username dan password!");
            }
        }
    }
}
