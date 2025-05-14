using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PABDCAFE
{
    public partial class Form1 : Form
    {

        // ganti "server" sesuai dengan SQL server anda
        private string connectionString = "Data Source=IDEAPAD5PRO\\LILA; Initial Catalog=ReservasiCafe; Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginPage lp = new LoginPage();
            lp.Show();
            this.Hide();
        }
    }
}