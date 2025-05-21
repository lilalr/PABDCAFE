using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
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
            // Tidak ada txtStatus yang perlu di-clear karena fieldnya tidak ada
            dgvAdminMeja.ClearSelection();
            txtNomor.Focus();
        }

        void LoadData()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Penting: Tetap SELECT Status_Meja agar bisa ditampilkan di DataGridView
                // Meskipun tidak bisa diedit langsung dari form ini.
                string query = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvAdminMeja.DataSource = dt;

                // Anda bisa mengatur AutoGenerateColumns = false dan mendefinisikan kolom di Designer
                // atau mengatur HeaderText secara programatik jika AutoGenerateColumns = true:
                if (dgvAdminMeja.Columns["Nomor_Meja"] != null)
                    dgvAdminMeja.Columns["Nomor_Meja"].HeaderText = "Nomor Meja";
                if (dgvAdminMeja.Columns["Status_Meja"] != null)
                    dgvAdminMeja.Columns["Status_Meja"].HeaderText = "Status"; // Akan ReadOnly di grid
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void AdminMeja_Load(object sender, EventArgs e)
        {
            // LoadData() sudah dipanggil di constructor.
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminPage ap = new AdminPage();
            ap.Show();
            this.Close();
        }

        // ValidasiInput sekarang hanya untuk nomorMeja dan kapasitas.
        // Tidak ada output untuk status.
        bool ValidasiInput(out string nomorMeja, out int kapasitas, out string errorMsg)
        {
            errorMsg = "";
            nomorMeja = txtNomor.Text.Trim();
            string kapasitasStr = txtKapasitas.Text.Trim();
            kapasitas = 0; // Inisialisasi default

            if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                errorMsg += "Nomor Meja harus terdiri dari 2 digit angka (contoh: 01, 12).\n";
            }

            if (!int.TryParse(kapasitasStr, out kapasitas) || kapasitas < 1 || kapasitas > 20)
            {
                errorMsg += "Kapasitas harus berupa angka antara 1 sampai 20.\n";
            }

            return string.IsNullOrEmpty(errorMsg);
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            // Hanya validasi nomor dan kapasitas
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // SP TambahMeja akan mengatur Status_Meja ke 'Tersedia' secara otomatis
                using (SqlCommand cmd = new SqlCommand("TambahMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                    cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                    // Tidak ada parameter @Status_Meja yang dikirim

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil ditambahkan! Status otomatis diatur ke 'Tersedia'.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        // Ini bisa terjadi jika SP melakukan validasi dan return tanpa error, atau RAISERROR tidak menghentikan di C#
                        MessageBox.Show("Data tidak berhasil ditambahkan. Periksa input atau kemungkinan nomor meja sudah ada.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException sqlEx) // Menangkap pesan dari RAISERROR di SP
            {
                MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            string nomorMeja = txtNomor.Text.Trim();
            if (string.IsNullOrEmpty(nomorMeja))
            {
                MessageBox.Show("Silakan pilih data meja yang ingin dihapus atau isi Nomor Meja pada field yang sesuai.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                MessageBox.Show("Nomor Meja yang akan dihapus harus terdiri dari 2 digit angka.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus meja dengan Nomor '{nomorMeja}'?",
                "Konfirmasi Penghapusan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No)
                return;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlCommand cmd = new SqlCommand("HapusMeja", conn)) // Pastikan SP HapusMeja ada
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau tidak ada data yang dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Validasi awal apakah ada data yang dipilih (Nomor Meja tidak boleh kosong)
            if (string.IsNullOrWhiteSpace(txtNomor.Text))
            {
                MessageBox.Show("Pilih data yang akan diubah atau pastikan Nomor Meja terisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hanya validasi nomor dan kapasitas
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // SP EditMeja sekarang diasumsikan hanya perlu Nomor_Meja dan Kapasitas.
                // Status TIDAK diubah melalui form ini.
                // Pastikan SP 'EditMeja' Anda HANYA mengupdate kolom yang diinginkan (misal Kapasitas)
                // dan TIDAK mengharapkan parameter @Status_Meja, atau jika iya, tidak menggunakannya
                // untuk mengubah data status.
                using (SqlCommand cmd = new SqlCommand("EditMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja); // Kunci untuk WHERE clause
                    cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);   // Nilai baru untuk kapasitas
                    // Tidak ada parameter @Status_Meja yang dikirim

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data kapasitas meja berhasil diperbarui! Status meja tidak diubah.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Muat ulang data untuk melihat perubahan kapasitas
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak berhasil diperbarui! Pastikan Nomor Meja ada.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void dgvAdminMeja_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Hanya mengisi txtNomor dan txtKapasitas dari baris yang dipilih.
            // Status akan terlihat di grid, tapi tidak dimuat ke field input karena tidak ada.
            if (e.RowIndex >= 0 && e.RowIndex < dgvAdminMeja.Rows.Count && dgvAdminMeja.Rows[e.RowIndex].Cells["Nomor_Meja"].Value != null)
            {
                // Hindari baris baru jika AllowUserToAddRows = true dan belum ada isinya
                if (dgvAdminMeja.Rows[e.RowIndex].IsNewRow) return;

                DataGridViewRow row = dgvAdminMeja.Rows[e.RowIndex];
                txtNomor.Text = row.Cells["Nomor_Meja"].Value?.ToString() ?? "";
                txtKapasitas.Text = row.Cells["Kapasitas"].Value?.ToString() ?? "";
                // Tidak ada txtStatus yang perlu diisi
            }
        }
    }
}