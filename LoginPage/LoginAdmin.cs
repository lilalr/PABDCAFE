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
    public partial class LoginAdmin : Form
    {
        // ganti "server" sesuai dengan SQL server anda
        private string baseconnectionString = "Data Source=IDEAPAD5PRO\\LILA; Initial Catalog=ReservasiCafe;";

        public LoginAdmin()
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

            // Cek apakah username adalah "customer"
            if (Username.ToLower() == "customer")
            {
                MessageBox.Show("Akun customer tidak boleh login sebagai admin!");
                return;
            }

            try
            {
                string connectionString = baseconnectionString + $"User ID={Username};Password={Password}";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Misal cek apakah user adalah admin dari database atau hardcoded
                    if (Username.ToLower() == "admin") // contoh hardcoded
                    {
                        MessageBox.Show("Login admin berhasil!");
                        AdminPage admin = new AdminPage();
                        admin.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Akun ini bukan akun admin!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Login admin gagal. Cek username dan password!");
            }
        }
    }
}
