using System;
using System.Data; // Diperlukan untuk DataTable
using System.Windows.Forms;
using System.Collections.Generic; // Diperlukan untuk List<string>
using System.Linq; // Diperlukan untuk .Take()

namespace PABDCAFE // Pastikan namespace ini sesuai dengan proyek Anda
{
    public partial class PreviewDataMeja : Form
    {
        // Properti untuk menyimpan data yang akan ditampilkan sebagai pratinjau
        public DataTable DataToPreview { get; private set; }
        // Properti untuk menunjukkan apakah pengguna mengonfirmasi impor
        public bool ImportConfirmed { get; private set; } = false;

        /// <summary>
        /// Konstruktor untuk form PreviewDataMeja.
        /// Menerima DataTable yang berisi data CSV yang telah divalidasi dan daftar detail kesalahan.
        /// </summary>
        /// <param name="previewData">DataTable yang berisi data yang akan ditampilkan.</param>
        /// <param name="errorDetails">Daftar string yang berisi detail kesalahan validasi baris.</param>
        public PreviewDataMeja(DataTable previewData, List<string> errorDetails)
        {
            InitializeComponent(); // Inisialisasi komponen form dari desainer

            DataToPreview = previewData; // Simpan data ke properti
            dgvPreview.DataSource = DataToPreview; // Atur sumber data DataGridView

            // Tampilkan detail kesalahan jika ada
            if (errorDetails != null && errorDetails.Count > 0)
            {
                // Bangun pesan kesalahan untuk ditampilkan kepada pengguna
                string errorMessage = "Beberapa baris tidak valid dan tidak akan diimpor. Detail kesalahan:\n" +
                                      string.Join("\n", errorDetails.Take(Math.Min(10, errorDetails.Count))); // Tampilkan hingga 10 kesalahan pertama
                if (errorDetails.Count > 10)
                {
                    errorMessage += $"\n...dan {errorDetails.Count - 10} kesalahan lainnya."; // Tambahkan indikator jika ada lebih banyak kesalahan
                }
                MessageBox.Show(errorMessage, "Peringatan Validasi Impor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Atur teks header kolom untuk kejelasan (asumsi "Nomor_Meja" dan "Kapasitas" adalah kolom)
            if (dgvPreview.Columns.Contains("Nomor_Meja"))
            {
                dgvPreview.Columns["Nomor_Meja"].HeaderText = "Nomor Meja";
            }
            if (dgvPreview.Columns.Contains("Kapasitas"))
            {
                dgvPreview.Columns["Kapasitas"].HeaderText = "Kapasitas";
            }
            // Pastikan kolom "Status_Meja" tidak dapat diedit dalam pratinjau jika ada.
            if (dgvPreview.Columns.Contains("Status_Meja"))
            {
                dgvPreview.Columns["Status_Meja"].HeaderText = "Status Meja (otomatis)"; // Perjelas bahwa ini otomatis
                dgvPreview.Columns["Status_Meja"].ReadOnly = true; // Jadikan kolom hanya-baca
            }

            dgvPreview.ReadOnly = true; // Jadikan seluruh DataGridView hanya-baca
            dgvPreview.AllowUserToAddRows = false; // Nonaktifkan penambahan baris baru oleh pengguna
            dgvPreview.AllowUserToDeleteRows = false; // Nonaktifkan penghapusan baris oleh pengguna
        }

        private void PreviewDataMeja_Load(object sender, EventArgs e)
        {
            // Logika pemuatan khusus jika diperlukan
        }

        private void dgvPreview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Event ini biasanya tidak diperlukan untuk tampilan pratinjau yang hanya-baca
        }

        /// Menangani klik tombol "OK". Mengatur bendera konfirmasi impor ke true dan menutup form.
        private void btnOk_Click(object sender, EventArgs e)
        {
            ImportConfirmed = true; // Atur bendera konfirmasi ke true
            this.DialogResult = DialogResult.OK; // Atur DialogResult ke OK
            this.Close(); // Tutup form pratinjau
        }

        /// <summary>
        /// Menangani klik tombol "Cancel". Mengatur bendera konfirmasi impor ke false dan menutup form.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ImportConfirmed = false; // Atur bendera konfirmasi ke false
            this.DialogResult = DialogResult.Cancel; // Atur DialogResult ke Cancel
            this.Close(); // Tutup form pratinjau
        }
    }
}