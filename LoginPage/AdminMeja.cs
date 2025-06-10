using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Microsoft.Reporting.WinForms;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

// Namespace disesuaikan dengan nama proyek Anda
namespace PABDCAFE
{
    public partial class AdminMeja : Form
    {
        // Variabel-variabel utama kelas
        private readonly string connectionString;
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "AdminMeja";

        // Konstruktor Form
        public AdminMeja(string connStr)
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak ada atau kosong.", "Kesalahan Konfigurasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException(nameof(connStr), "String koneksi tidak boleh null.");
            }
            this.connectionString = connStr;
        }


        private void AdminMeja_Load(object sender, EventArgs e)
        {
            try
            {
                SetupDataGridView();
                EnsureIndexes();
                LoadAndDisplayData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi error saat memuat form: " + ex.Message, "Kesalahan Form Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            dgvAdminMeja.AutoGenerateColumns = false;
            dgvAdminMeja.Columns.Clear();

            dgvAdminMeja.Columns.Add("Nomor_Meja", "Nomor Meja");
            dgvAdminMeja.Columns.Add("Kapasitas", "Kapasitas");
            dgvAdminMeja.Columns.Add("Status_Meja", "Status");

            dgvAdminMeja.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void LoadAndDisplayData()
        {
            DataTable dt = _cache.Get(CacheKey) as DataTable;
            if (dt == null)
            {
                try
                {
                    dt = new DataTable();
                    string query = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja ORDER BY Nomor_Meja ASC";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(query, connection);
                        da.Fill(dt);
                    }
                    _cache.Set(CacheKey, dt, _policy);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error memuat data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            TampilkanDataDiGrid(dt);
        }

        private void TampilkanDataDiGrid(DataTable dt)
        {
            dgvAdminMeja.Rows.Clear();
            if (dt != null)
            {
                foreach (DataRow barisData in dt.Rows)
                {
                    dgvAdminMeja.Rows.Add(
                        barisData["Nomor_Meja"],
                        barisData["Kapasitas"],
                        barisData["Status_Meja"]
                    );
                }
            }
        }

        private void EnsureIndexes()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var indexScript = @"
                    IF OBJECT_ID('dbo.Meja', 'U') IS NOT NULL
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_Meja_Kapasitas')
                            CREATE NONCLUSTERED INDEX idx_Meja_Kapasitas ON dbo.Meja(Kapasitas);
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'idx_Meja_Status_Meja')
                            CREATE NONCLUSTERED INDEX idx_Meja_Status_Meja ON dbo.Meja(Status_Meja);
                    END";
                    using (var cmd = new SqlCommand(indexScript, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memastikan indeks database: " + ex.Message, "Peringatan Optimasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AnalyzeQuery(string sqlQuery)
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
              
                    conn.InfoMessage += (s, e) => MessageBox.Show(e.Message, "Info Statistik Kinerja");

                    conn.Open();

                    // Membungkus query asli dengan perintah statistik
                    var wrappedQuery = $@"
                SET STATISTICS IO ON;
                SET STATISTICS TIME ON;
                {sqlQuery}
                SET STATISTICS TIME OFF;
                SET STATISTICS IO OFF;";

                    using (var cmd = new SqlCommand(wrappedQuery, conn))
                    {
                        // Menggunakan ExecuteNonQuery karena kita tidak mengharapkan data kembali,
                        // hanya pesan statistik dari InfoMessage.
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal melakukan analisis query: " + ex.Message, "Kesalahan Analisis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void InvalidateAdminMejaCache()
        {
            if (_cache.Contains(CacheKey))
            {
                _cache.Remove(CacheKey);
            }
        }

        void ClearForm()
        {
            txtNomor.Clear();
            txtKapasitas.Clear();
            dgvAdminMeja.ClearSelection();
            txtNomor.Focus();
        }

        bool ValidasiInput(out string nomorMeja, out int kapasitas, out string errorMsg)
        {
            errorMsg = "";
            nomorMeja = txtNomor.Text.Trim();
            string kapasitasStr = txtKapasitas.Text.Trim();
            kapasitas = 0;
            if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                errorMsg += "Nomor Meja harus terdiri dari 2 digit angka.\n";
            }
            if (!int.TryParse(kapasitasStr, out kapasitas) || kapasitas < 1 || kapasitas > 99)
            {
                errorMsg += "Kapasitas harus berupa angka antara 1 sampai 99.\n";
            }
            return string.IsNullOrEmpty(errorMsg);
        }



        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    using (SqlCommand cmd = new SqlCommand("TambahMeja", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    InvalidateAdminMejaCache();
                    LoadAndDisplayData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Error saat menambah data: " + ex.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Apakah Anda yakin ingin mengubah data meja '{nomorMeja}'?", "Konfirmasi Perubahan Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return; // Keluar dari method jika pengguna memilih 'No'
            }


            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    using (SqlCommand cmd = new SqlCommand("EditMeja", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                    MessageBox.Show("Kapasitas meja berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    InvalidateAdminMejaCache();
                    LoadAndDisplayData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Error saat mengubah data: " + ex.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            string nomorMeja = txtNomor.Text.Trim();
            if (string.IsNullOrEmpty(nomorMeja))
            {
                MessageBox.Show("Silakan pilih data meja yang ingin dihapus.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Apakah Anda yakin ingin menghapus meja nomor '{nomorMeja}'?", "Konfirmasi Penghapusan", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                SqlTransaction transaction = null;
                try
                { 
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    int rowsAffected;
                    using (SqlCommand cmd = new SqlCommand("HapusMeja", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    InvalidateAdminMejaCache();
                    LoadAndDisplayData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    MessageBox.Show("Error saat menghapus data: " + ex.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Filter diubah untuk mencari file Excel
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Pilih File Excel untuk Impor Data Meja"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = new DataTable();

                    // Menggunakan NPOI untuk membaca file .xlsx
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = new XSSFWorkbook(fs);
                        ISheet sheet = workbook.GetSheetAt(0); // Ambil sheet pertama

                        // Baca baris header untuk membuat kolom di DataTable
                        IRow headerRow = sheet.GetRow(0);
                        if (headerRow == null) throw new Exception("File Excel tidak memiliki baris header.");

                        foreach (ICell cell in headerRow.Cells)
                        {
                            dt.Columns.Add(cell.ToString().Trim());
                        }

                        // Baca sisa baris untuk data
                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            if (row == null || row.Cells.All(c => c.CellType == CellType.Blank)) continue;

                            DataRow dataRow = dt.NewRow();
                            for (int j = 0; j < headerRow.LastCellNum; j++)
                            {
                                ICell cell = row.GetCell(j);
                                dataRow[j] = cell?.ToString() ?? string.Empty;
                            }
                            dt.Rows.Add(dataRow);
                        }
                    }

                    // Tampilkan form preview dengan data dari Excel
                    PreviewDataMeja previewForm = new PreviewDataMeja(dt, this.connectionString);
                    if (previewForm.ShowDialog() == DialogResult.OK && previewForm.ImportConfirmed)
                    {
                        InvalidateAdminMejaCache();
                        LoadAndDisplayData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saat membaca file Excel: " + ex.Message, "Error Impor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            ReportViewerMeja form = new ReportViewerMeja();
            form.ShowDialog();
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            string queryToAnalyze = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja ORDER BY Nomor_Meja ASC";
            AnalyzeQuery(queryToAnalyze);

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
                AdminPage ap = new AdminPage(this.connectionString);
                ap.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal kembali ke halaman admin: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvAdminMeja_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvAdminMeja.Rows.Count)
            {
                DataGridViewRow row = dgvAdminMeja.Rows[e.RowIndex];
                txtNomor.Text = row.Cells["Nomor_Meja"].Value?.ToString() ?? "";
                txtKapasitas.Text = row.Cells["Kapasitas"].Value?.ToString() ?? "";
            }
        }

    }
}
