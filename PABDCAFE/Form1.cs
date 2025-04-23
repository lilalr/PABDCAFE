using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class Form1: Form
    {

        private string connectionString = "Data Source=IDEAPAD5PRO\\LILA;Initial Catalog=ReservasiCafe;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();

            // Tambahkan ComboBox cmbMeja secara manual
            this.cmbMeja = new System.Windows.Forms.ComboBox();
            this.cmbMeja.Items.AddRange(new object[] {
            "01", "02", "03", "04", "05"});
            this.cmbMeja.Location = new System.Drawing.Point(150, 100); // sesuaikan posisi
            this.cmbMeja.Size = new System.Drawing.Size(121, 21); // sesuaikan ukuran
            this.Controls.Add(this.cmbMeja);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxMeja();
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtNoTelp.Clear();

            if (cmbMeja != null)
                cmbMeja.SelectedIndex = -1;

            dtpReservasii.Value = DateTime.Now;

            txtName.Focus();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                try
                {
                    conn.Open();
                    string query = "SELECT ID_Reservasi, Nama_Customer, No_telp, Number_Table, Waktu_Reservasi FROM Reservasi";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvKafe.AutoGenerateColumns = true;
                    dgvKafe.DataSource = dt;

                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadComboBoxMeja()
        {
            cmbMeja.Items.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Number_Table FROM Meja WHERE Status_Meja = 'Tersedia'";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbMeja.Items.Add(reader["Number_Table"].ToString());
                }
            }
        }

        private void btnTambah(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
            string.IsNullOrWhiteSpace(txtNoTelp.Text) ||
            cmbMeja.SelectedItem == null)
            {
                MessageBox.Show("Harap isi semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {

                    conn.Open();
                    string query = "INSERT INTO Reservasi (Nama_Customer, No_telp, Waktu_Reservasi, Number_Table) " +
                                   "VALUES (@Nama, @NoTelp, @Waktu, @Meja)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nama", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@NoTelp", txtNoTelp.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu", dtpReservasii.Value);
                    cmd.Parameters.AddWithValue("@Meja", cmbMeja.SelectedItem.ToString());

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit(object sender, EventArgs e)
        {
            if (dgvKafe.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diedit!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvKafe.SelectedRows[0].Cells["ID_Reservasi"].Value);


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Reservasi SET Nama_Customer = @Nama, No_telp = @NoTelp, " +
                                   "Waktu_Reservasi = @Waktu, Number_Table = @Meja WHERE ID_Reservasi = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nama", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@NoTelp", txtNoTelp.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu", dtpReservasii.Value);
                    cmd.Parameters.AddWithValue("@Meja", cmbMeja.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@ID", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnRefresh(object sender, EventArgs e)
        {
            LoadData();

            MessageBox.Show($"Jumlah Kolom: {dgvKafe.ColumnCount}\nJumlah Baris: {dgvKafe.RowCount}",
                "Debugging DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHapus(object sender, EventArgs e)
        {
            if (dgvKafe.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int id = Convert.ToInt32(dgvKafe.SelectedRows[0].Cells["ID_Reservasi"].Value);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Reservasi WHERE ID_Reservasi = @ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }





        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChoose_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
