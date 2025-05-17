using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class AdminMeja : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-4FJGLBGI\\NANDA;Initial Catalog=ReservasiCafe;Integrated Security=True;");

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
            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();

                string query = "SELECT * FROM Meja";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAdminMeja.AutoGenerateColumns = true;
                dgvAdminMeja.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
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

        bool ValidasiInput(out string errorMsg)
        {
            errorMsg = "";
            if (string.IsNullOrWhiteSpace(txtNomor.Text) || txtNomor.Text.Length != 2)
                errorMsg += "Nomor Meja harus 2 karakter.\n";

            if (!int.TryParse(txtKapasitas.Text, out int kapasitas) || kapasitas < 1 || kapasitas > 99)
                errorMsg += "Kapasitas harus antara 1 - 99.\n";

            if (txtStatus.Text != "Tersedia" && txtStatus.Text != "Dipesan")
                errorMsg += "Status Meja harus 'Tersedia' atau 'Dipesan'.\n";

            return string.IsNullOrEmpty(errorMsg);
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("AddMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NomorMeja", txtNomor.Text.Trim());
                    cmd.Parameters.AddWithValue("@Kapasitas", int.Parse(txtKapasitas.Text.Trim()));
                    cmd.Parameters.AddWithValue("@StatusMeja", txtStatus.Text.Trim());

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak berhasil ditambahkan!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            // 1. Validasi input Nomor Meja
            string nomorMeja = txtNomor.Text.Trim();
            if (string.IsNullOrEmpty(nomorMeja))
            {
                MessageBox.Show("Silakan pilih data meja yang ingin dihapus terlebih dahulu.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Konfirmasi penghapusan
            DialogResult result = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus meja dengan Nomor '{nomorMeja}'?",
                "Konfirmasi Penghapusan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            // 3. Eksekusi perintah DELETE dengan stored procedure
            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();

                using (SqlCommand cmd = new SqlCommand("DeleteMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NomorMeja", nomorMeja);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau sudah dihapus sebelumnya.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }



        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomor.Text))
            {
                MessageBox.Show("Pilih data yang akan diubah!");
                return;
            }

            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UpdateMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NomorMeja", txtNomor.Text.Trim());
                    cmd.Parameters.AddWithValue("@Kapasitas", int.Parse(txtKapasitas.Text));
                    cmd.Parameters.AddWithValue("@StatusMeja", txtStatus.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak berhasil diperbarui!", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

      

        private void dgvAdminMeja_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvAdminMeja.Rows.Count > e.RowIndex)
            {
                txtNomor.Text = dgvAdminMeja.Rows[e.RowIndex].Cells["Nomor_Meja"].Value.ToString();
                txtKapasitas.Text = dgvAdminMeja.Rows[e.RowIndex].Cells["Kapasitas"].Value.ToString();
                txtStatus.Text = dgvAdminMeja.Rows[e.RowIndex].Cells["Status_Meja"].Value.ToString();
            }

        }


        // (-) KALO PAS MENCET DATA YG DI DGV ITU MASIH SUSAH MASUK KE FORMNYA, TAPI TAMBAH EDIT HAPUS DH BISA
        // (-) UNTUK NOMER MEJA TERNYATA MASIH BISA BIMASUKIN HURUF
        // 
       
    }
}

