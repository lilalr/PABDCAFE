using PABDCAFE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class PreviewDataReservasi : Form
    {
        private readonly string connectionString;
        public bool ImportConfirmed { get; private set; } = false;

        public PreviewDataReservasi(DataTable data, string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            dgvPreviewReservasi.DataSource = data;
        }

        private void PreviewFormAdminReservasi_Load(object sender, EventArgs e)
        {
            dgvPreviewReservasi.AutoResizeColumns(); // Menyesuaikan ukuran kolom
        }

        public bool ValidateRow(DataRow row, int lineNumber, out string errorMessage)
        {
            errorMessage = "";

            // 1. Validasi Nama Customer
            if (string.IsNullOrWhiteSpace(row["Nama_Customer"]?.ToString()))
            {
                errorMessage = $"Baris {lineNumber}: Nama Customer tidak boleh kosong.";
                return false;
            }

            // 2. Validasi Nomor Telepon
            string noTelp = row["No_Telp"]?.ToString().Trim() ?? string.Empty;
            if (!Regex.IsMatch(noTelp, @"^(\+62\d{8,12}|0\d{9,14})$"))
            {
                errorMessage = $"Baris {lineNumber}: Format Nomor Telepon '{noTelp}' tidak valid.";
                return false;
            }

            // 3. Validasi Waktu Reservasi
            if (!DateTime.TryParse(row["Waktu_Reservasi"]?.ToString(), out DateTime waktuReservasi))
            {
                errorMessage = $"Baris {lineNumber}: Format Waktu Reservasi tidak valid.";
                return false;
            }
            // Tambahan: Sesuaikan dengan constraint CHECK di database Anda (hanya tahun 2025)
            if (waktuReservasi.Year != 2025)
            {
                errorMessage = $"Baris {lineNumber}: Reservasi hanya bisa untuk tahun 2025.";
                return false;
            }
            if (waktuReservasi < DateTime.Now)
            {
                errorMessage = $"Baris {lineNumber}: Waktu reservasi tidak boleh di masa lalu.";
                return false;
            }

            // 4. Validasi Nomor Meja
            string nomorMeja = row["Nomor_Meja"]?.ToString().Trim() ?? string.Empty;
            if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                errorMessage = $"Baris {lineNumber}: Format Nomor Meja '{nomorMeja}' tidak valid. Harus 2 digit.";
                return false;
            }

            return true;
        }

        private void ImportDataToDatabase()
        {
            DataTable dt = (DataTable)dgvPreviewReservasi.DataSource;
            List<string> errorList = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Loop melalui setiap baris di DataTable
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow row = dt.Rows[i];
                        // Validasi baris, jika gagal, tambahkan ke daftar error dan lanjut
                        if (!ValidateRow(row, i + 2, out string errorMsg)) // i + 2 untuk nomor baris di Excel
                        {
                            errorList.Add(errorMsg);
                            continue;
                        }

                        // Jika valid, panggil Stored Procedure TambahReservasi
                        using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn, transaction))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Nama_Customer", row["Nama_Customer"]);
                            cmd.Parameters.AddWithValue("@No_Telp", row["No_Telp"]);
                            cmd.Parameters.AddWithValue("@Waktu_Reservasi", Convert.ToDateTime(row["Waktu_Reservasi"]));
                            cmd.Parameters.AddWithValue("@Nomor_Meja", row["Nomor_Meja"]);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Jika ada error di daftar, batalkan transaksi
                    if (errorList.Count > 0)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Impor dibatalkan karena ditemukan data tidak valid:\n\n" + string.Join("\n", errorList),
                            "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Jika semua berhasil, commit transaksi
                    transaction.Commit();
                    MessageBox.Show("Data berhasil diimpor!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ImportConfirmed = true;
                    this.DialogResult = DialogResult.OK; // Set DialogResult menjadi OK
                    this.Close();
                }
                catch (Exception ex)
                {
                    // Jika ada error dari SQL (misal: meja tidak ada), batalkan transaksi
                    transaction.Rollback();
                    MessageBox.Show("Gagal mengimpor data ke database: \n\n" + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvPreviewAdminReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Yakin ingin mengimpor data ini?", "Konfirmasi Impor", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ImportDataToDatabase();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ImportConfirmed = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
