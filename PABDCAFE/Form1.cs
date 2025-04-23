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
    public partial class Form1: Form
    {

        private string connectionString = "Data Source=IDEAPAD5PRO\\LILA;Initial Catalog=ReservasiCafe;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ClearForm()
        {
            txtChoose.Clear();
            txtName.Clear();
            txtPhone.Clear();

            txtName.Focus();
        }

        private void LoadData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                try
                {
                    conn.Open();
                    string query = "SELECT Waktu_Reservasi, Nama_Customer, No_telp FROM Reservasi";
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

        private void btnRefresh(object sender, EventArgs e)
        {
            LoadData();

            MessageBox.Show($"Jumlah Kolom: {dgvKafe.ColumnCount}\nJumlah Baris: {dgvKafe.RowCount}",
                "Debugging DataGridView", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHapus(object sender, EventArgs e)
        {
            if (dgvKafe.SelectedRows.Count > 0)
            {
                DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            string nim = dgvKafe.SelectedRows[0].Cells["Nama_Customer"].Value.ToString();
                            conn.Open();
                            string query = "DELETE FROM Reservasi WHERE Nama_Customer = @Nama_Customer";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("Nama_Customer", nim);
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LoadData();
                                    ClearForm(); // Auto Clear setelah hapus data
                                }
                                else
                                {
                                    MessageBox.Show("Data tidak ditemukan atau gagal dihapus!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Pilih data yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit(object sender, EventArgs e)
        {

        }

        private void btnTambah(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    if (txtChoose.Text == "" || txtName.Text == "" || txtPhone.Text == "")
                    {
                        MessageBox.Show("Harap isi semua data!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    conn.Open();
                    string query = "INSERT INTO Reservasi (Waktu_Reservasi, Nama_Customer, No_telp) VALUES (@Waktu_Reservasi, @Nama_Customer, @No_Telp)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Waktu_Reservasi", txtChoose.Text.Trim());
                        cmd.Parameters.AddWithValue("@Nama_Customer", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@No_telp", txtPhone.Text.Trim());

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            ClearForm();
                        }
                        else
                        {
                            MessageBox.Show("Data tidak berhasil ditambahkan!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
