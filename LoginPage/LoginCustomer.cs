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
    public partial class LoginCustomer : Form
    {
        // ganti "server" sesuai dengan SQL server anda
        private string baseconnectionString = "Data Source=IDEAPAD5PRO\\LILA; Initial Catalog=ReservasiCafe; Integrated Security=True";

        public LoginCustomer()
        {
            InitializeComponent();
        }

        private void LoginCustomer_Load(object sender, EventArgs e)
        {
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string Username = txtUsername.Text;
            string Password = txtPassword.Text;

            if (Username == "customer")
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
                    MessageBox.Show("Login berhasil!");
                    CustomerPage customer = new CustomerPage();
                    customer.Show();
                    this.Hide();
                }
            }
            catch
            {
                MessageBox.Show("Login customer gagal. Cek username dan password!");
            }
        }
    }
}
