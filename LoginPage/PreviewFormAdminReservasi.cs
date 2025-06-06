using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoginPage
{
    public partial class PreviewFormAdminReservasi : Form
    {
        private readonly string connectionString;
        private SqlConnection conn;
        // Konstruktor menerima DataTable dan menampilkan data di DataGridView

        public PreviewFormAdminReservasi(DataTable data)
        {
            InitializeComponent();
            // Menetapkan data source DataGridView ke DataTable yang diterima
            dgvPreviewAdminReservasi.DataSource = data;
        }

        private void PreviewFormAdminReservasi_Load(object sender, EventArgs e)
        {
            // Opsional: Sesuaikan DataGridView jika perlu
            dgvPreviewAdminReservasi.AutoResizeColumns(); // Menyesuaikan ukuran kolom
        }

        // Event ketika tombol OK ditekan
        // Event ketika tombol OK ditekan
        

        private void btnOK_Click(object sender, EventArgs e)
        {
            // Menanyakan pengguna jika mereka ingin mengimpor data
            DialogResult result = MessageBox.Show("Apakah Anda ingin mengimpor data ini ke database?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Mengimpor data dari DataGridView ke database
                ImportDataToDatabase();
            }
        }

        private bool ValidateRow(DataRow row)
        {
            // Validasi Nama Customer
            string namaCustomer = row["Nama_Customer"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(namaCustomer))
            {
                MessageBox.Show("Nama Customer tidak boleh kosong.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Contoh validasi tambahan: panjang nama
            if (namaCustomer.Length < 3)
            {
                MessageBox.Show("Nama Customer minimal 3 karakter.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validasi Nomor Telepon
            string noTelp = row["No_Telp"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(noTelp))
            {
                MessageBox.Show("Nomor Telepon tidak boleh kosong.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Contoh validasi tambahan: hanya angka
            if (!System.Text.RegularExpressions.Regex.IsMatch(noTelp, @"^\d+$"))
            {
                MessageBox.Show("Nomor Telepon hanya boleh mengandung angka.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Contoh validasi panjang nomor telepon
            if (noTelp.Length < 8 || noTelp.Length > 15) // Sesuaikan rentang panjang yang valid
            {
                MessageBox.Show("Panjang Nomor Telepon harus antara 8 hingga 15 digit.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validasi Waktu Reservasi
            // Periksa apakah konversi ke DateTime berhasil
            if (!DateTime.TryParse(row["Waktu_Reservasi"].ToString(), out DateTime waktuReservasi))
            {
                MessageBox.Show("Format Waktu Reservasi tidak valid.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Contoh validasi: Waktu reservasi tidak boleh di masa lalu
            if (waktuReservasi < DateTime.Now.Date) // Membandingkan hanya tanggal, abaikan waktu
            {
                MessageBox.Show("Waktu Reservasi tidak boleh di masa lalu.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Contoh validasi: Reservasi hanya untuk hari ini atau masa depan
            if (waktuReservasi.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Waktu Reservasi harus hari ini atau di masa depan.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            // Validasi Nomor Meja
            string nomorMeja = row["Nomor_Meja"].ToString().Trim();
            if (string.IsNullOrWhiteSpace(nomorMeja))
            {
                MessageBox.Show("Nomor Meja tidak boleh kosong.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Contoh validasi: Nomor Meja harus angka dan dalam rentang tertentu
            if (!int.TryParse(nomorMeja, out int mejaNum))
            {
                MessageBox.Show("Nomor Meja harus berupa angka.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            // Asumsi nomor meja antara 1 dan 20, sesuaikan jika berbeda
            if (mejaNum < 1 || mejaNum > 20)
            {
                MessageBox.Show("Nomor Meja harus antara 1 dan 20.", "Kesalahan Validasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Jika semua validasi berhasil, kembalikan true
            return true;
        }

        private void ImportDataToDatabase()
        {
            try
            {
                // Ambil data dari DataGridView ke DataTable
                DataTable dt = (DataTable)dgvPreviewAdminReservasi.DataSource;

                // Gunakan SqlConnection yang baru untuk operasi impor ini, sesuai pola gambar
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open(); // Buka koneksi

                    // Loop melalui setiap baris di DataTable
                    foreach (DataRow row in dt.Rows)
                    {
                        // Validasi setiap baris sebelum diimpor
                        // Panggil ValidateRow yang sudah disesuaikan untuk data reservasi
                        if (!ValidateRow(row))
                        {
                            // Jika validasi gagal, lewati baris ini dan lanjut ke baris berikutnya
                            continue;
                        }

                        // Gunakan SqlCommand untuk memanggil Stored Procedure TambahReservasi
                        using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Tambahkan parameter dari setiap kolom baris ke Stored Procedure
                            // Pastikan nama kolom sesuai dengan nama di DataTable Anda
                            cmd.Parameters.AddWithValue("@Nama_Customer", row["Nama_Customer"].ToString().Trim());
                            cmd.Parameters.AddWithValue("@No_Telp", row["No_Telp"].ToString().Trim());
                            cmd.Parameters.AddWithValue("@Waktu_Reservasi", Convert.ToDateTime(row["Waktu_Reservasi"]));
                            cmd.Parameters.AddWithValue("@Nomor_Meja", row["Nomor_Meja"].ToString().Trim());

                            // Eksekusi Stored Procedure
                            cmd.ExecuteNonQuery();
                        }
                    }
                } // Koneksi akan otomatis ditutup di sini karena 'using'

                MessageBox.Show("Data berhasil diimpor ke database!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Tutup PreviewForm setelah data diimpor
                              // Setelah mengimpor, mungkin Anda juga ingin memuat ulang data utama
                              // LoadData(); // Panggil ini jika LoadData() dapat diakses dari sini atau melalui event
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengimpor data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPreviewAdminReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
