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
    public partial class AdminMeja : Form
    {
        private readonly string connectionString = "Data Source = LAPTOP-4FJGLBGI\\NANDA;" + "Initial Catalog =OrganisasiMahasiswa;" + "Integrated Security = true";
       

        public AdminMeja()
        {
            InitializeComponent();
            LoadData();
        }

        void ClearForm()
        {
            txtNomor.Clear();
            txtKapasitas.Clear();
            txtStatus.Clear();
        }

        void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Meja";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAdminMeja.DataSource = dt;
            }
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

        private void btnTambah_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("AddMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nomor_Meja", txtNomor.Text);
                    cmd.Parameters.AddWithValue("@Kapasitas", int.Parse(txtKapasitas.Text));
                    cmd.Parameters.AddWithValue("@Status_Meja", txtStatus.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil ditambahkan.");
                    LoadData();
                    ClearForm();
                }
            }

        }
    }
}
