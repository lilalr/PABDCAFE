using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace LoginPage
{
    public partial class Form1: Form
    {
        private string connectionString = "IDEAPAD5PRO\\LILA;Initial Catalog=ReservasiCafe";

        public Form1()
        {
            InitializeComponent();
        }

        private void Masukk_Click(object sender, EventArgs e)
        {
            string username = InUser.Text.Trim();
            string password = InPass.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Username dan password harus diisi!");
                return;
            }

            // Dummy login logika:
            if (username == "admin" && password == "admin123")
            {
                // Buka halaman admin
                PABDCAFE.Form1 adminForm = new PABDCAFE.Form1();
                adminForm.Show();
                this.Hide();
            }
            else if (username == "customer" && password == "cust123")
            {
                // Buka halaman customer
                CusTampil.Form1 customerForm = new CusTampil.Form1();
                customerForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username atau password salah.");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
