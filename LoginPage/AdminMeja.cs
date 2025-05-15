using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABDCAFE 
{
    public partial class AdminMeja : Form
    {
        public AdminMeja()
        {
            InitializeComponent();
        }

        private void AdminMeja_Load(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminPage ap = new AdminPage();
            ap.Show();
            this.Close();
        }
    }
}
