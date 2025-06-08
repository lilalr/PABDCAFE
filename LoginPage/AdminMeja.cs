using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace PABDCAFE
{
    public partial class AdminMeja : Form
    {
        private readonly string connectionString;
        private SqlConnection conn;
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "AdminMeja";

        public AdminMeja(string connStr)
        {
            InitializeComponent();
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak ada atau kosong.", "Kesalahan Konfigurasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException(nameof(connStr), "String koneksi tidak boleh null.");
            }
            this.connectionString = connStr;
            this.conn = new SqlConnection(this.connectionString);

             
        }

        void ClearForm()
        {
            txtNomor.Clear();
            txtKapasitas.Clear();
            dgvAdminMeja.ClearSelection();
            txtNomor.Focus();
        }

        private void AdminMeja_Load(object sender, EventArgs e)
        {
            EnsureIndexes();

            DataTable dt = _cache.Get(CacheKey) as DataTable;
            if (dt == null)
            {
                if (this.conn == null)
                {
                    MessageBox.Show("Koneksi database belum diinisialisasi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    string query = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja ORDER BY Nomor_Meja ASC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    dt = new DataTable();
                    da.Fill(dt);
                    dgvAdminMeja.DataSource = dt;
                    _cache.Set(CacheKey, dt, _policy);

                    if (dgvAdminMeja.Columns.Contains("Nomor_Meja"))
                        dgvAdminMeja.Columns["Nomor_Meja"].HeaderText = "Nomor Meja";
                    if (dgvAdminMeja.Columns.Contains("Status_Meja"))
                        dgvAdminMeja.Columns["Status_Meja"].HeaderText = "Status";
                    if (dgvAdminMeja.Columns.Contains("Kapasitas"))
                        dgvAdminMeja.Columns["Kapasitas"].HeaderText = "Kapasitas";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error memuat data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                dgvAdminMeja.DataSource = dt;
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
                    // Baris ini akan menangkap pesan statistik dari SQL Server
                    // dan menampilkannya di MessageBox
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

        private bool IsConnectionReady()
        {
            if (this.conn == null || string.IsNullOrWhiteSpace(this.conn.ConnectionString))
            {
                MessageBox.Show("Koneksi database tidak dikonfigurasi.", "Error Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminPage ap = new AdminPage(this.connectionString);
            ap.Show();
            this.Close();
        }

        private void dgvAdminMeja_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvAdminMeja.Rows.Count && !dgvAdminMeja.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow row = dgvAdminMeja.Rows[e.RowIndex];
                txtNomor.Text = row.Cells["Nomor_Meja"].Value?.ToString() ?? "";
                txtKapasitas.Text = row.Cells["Kapasitas"].Value?.ToString() ?? "";
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // C# menangani transaksi
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Memulai transaksi

                try
                {
                    using (SqlCommand cmd = new SqlCommand("TambahMeja", conn, transaction)) // Kaitkan command dengan transaksi
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit(); // Commit jika berhasil

                    MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InvalidateAdminMejaCache();
                    AdminMeja_Load(null, null);
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    transaction.Rollback(); // Batalkan (rollback) jika ada error
                    MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Batalkan (rollback) jika ada error
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Memulai transaksi

                try
                {
                    using (SqlCommand cmd = new SqlCommand("EditMeja", conn, transaction)) // Kaitkan command dengan transaksi
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit(); // Commit jika berhasil

                    MessageBox.Show("Kapasitas meja berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InvalidateAdminMejaCache();
                    AdminMeja_Load(null, null);
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    transaction.Rollback(); // Batalkan (rollback) jika ada error
                    MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Batalkan (rollback) jika ada error
                    MessageBox.Show("Error: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;
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
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Memulai transaksi
                try
                {
                    using (SqlCommand cmd = new SqlCommand("HapusMeja", conn, transaction)) // Kaitkan command dengan transaksi
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Data tidak ditemukan atau tidak ada data yang dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                    transaction.Commit(); // Commit jika berhasil

                    InvalidateAdminMejaCache();
                    AdminMeja_Load(null, null);
                    ClearForm();
                }
                catch (SqlException sqlEx)
                {
                    transaction.Rollback(); // Batalkan (rollback) jika ada error
                    MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Batalkan (rollback) jika ada error
                    MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Filter diatur untuk file Excel (.xlsx), sesuai modul 
                Filter = "CSV File (*.csv)|*.csv",
                Title = "Pilih File CSV untuk Impor Data Meja"
            };

            // Jika pengguna memilih file dan klik OK, panggil metode PreviewData 
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    // 2. Baca semua baris dari file CSV
                    string[] lines = System.IO.File.ReadAllLines(filePath);

                    if (lines.Length == 0)
                    {
                        MessageBox.Show("File CSV kosong.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // 3. Buat DataTable secara manual dari data CSV
                    DataTable dt = new DataTable();

                    // Ambil header dari baris pertama dan buat kolom
                    string[] headers = lines[0].Split(',');
                    foreach (string header in headers)
                    {
                        dt.Columns.Add(header.Trim());
                    }

                    // Isi baris data (mulai dari baris kedua)
                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] fields = lines[i].Split(',');
                        DataRow row = dt.NewRow();
                        for (int j = 0; j < headers.Length; j++)
                        {
                            // Pastikan tidak error jika baris data lebih pendek dari header
                            if (j < fields.Length)
                            {
                                row[j] = fields[j].Trim();
                            }
                        }
                        dt.Rows.Add(row);
                    }

                    // 4. Kirim DataTable yang sudah jadi ke form preview
                    // Form PreviewDataMeja tidak perlu tahu datanya dari mana (Excel/CSV)
                    PreviewDataMeja previewForm = new PreviewDataMeja(dt, this.connectionString);

                    if (previewForm.ShowDialog() == DialogResult.OK && previewForm.ImportConfirmed)
                    {
                        // Jika import berhasil dari form preview, refresh data di sini
                        InvalidateAdminMejaCache();
                        AdminMeja_Load(null, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saat membaca file CSV: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PreviewDataMeja(string filePath)
        {
            try
            {
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(fs); // Membuka workbook Excel 
                    ISheet sheet = workbook.GetSheetAt(0);     // Mendapatkan worksheet pertama 
                    DataTable dt = new DataTable();

                    // Membaca header kolom dari baris pertama Excel 
                    IRow headerRow = sheet.GetRow(0);
                    foreach (var cell in headerRow.Cells)
                    {
                        dt.Columns.Add(cell.ToString());
                    }

                    // Membaca sisa data dari baris berikutnya 
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow dataRow = sheet.GetRow(i);
                        if (dataRow == null) continue;

                        DataRow newRow = dt.NewRow();
                        // Iterasi berdasarkan header agar jumlah kolom konsisten
                        for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                        {
                            newRow[cellIndex] = dataRow.GetCell(cellIndex)?.ToString() ?? "";
                        }
                        dt.Rows.Add(newRow);
                    }

                    // Membuka form preview dan mengirimkan DataTable 
                    PreviewDataMeja previewForm = new PreviewDataMeja(dt, this.connectionString);

                    // Tampilkan form preview sebagai dialog 
                    if (previewForm.ShowDialog() == DialogResult.OK && previewForm.ImportConfirmed)
                    {
                        // Jika import berhasil, refresh data di grid utama
                        InvalidateAdminMejaCache();
                        AdminMeja_Load(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                // Menangani error jika gagal membaca file Excel 
                MessageBox.Show("Error saat membaca file Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvAdminMeja.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk diekspor.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV File (*.csv)|*.csv",
                Title = "Simpan Data Meja sebagai CSV",
                FileName = $"DataMeja_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    IEnumerable<string> columnHeaders = dgvAdminMeja.Columns.Cast<DataGridViewColumn>().Select(column => $"\"{column.HeaderText}\"");
                    sb.AppendLine(string.Join(",", columnHeaders));

                    foreach (DataGridViewRow row in dgvAdminMeja.Rows)
                    {
                        if (row.IsNewRow) continue;
                        IEnumerable<string> fields = row.Cells.Cast<DataGridViewCell>().Select(cell => $"\"{cell.Value?.ToString()?.Replace("\"", "\"\"") ?? ""}\"");
                        sb.AppendLine(string.Join(",", fields));
                    }

                    System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Data berhasil diekspor!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saat mengekspor data: " + ex.Message, "Kesalahan Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    

     
        private bool ValidateCsvRow(string[] data, int lineNumber, out string nomorMejaCsv, out int kapasitasCsv, ref List<string> errorDetails)
        {
            nomorMejaCsv = "";
            kapasitasCsv = 0;
            bool isValid = true;
            if (data.Length < 2)
            {
                errorDetails.Add($"Baris {lineNumber}: Format tidak valid. Harusnya 'Nomor_Meja,Kapasitas'.");
                return false;
            }
            string tempNomorMeja = data[0].Trim();
            string tempKapasitasStr = data[1].Trim();
            if (!Regex.IsMatch(tempNomorMeja, @"^\d{2}$"))
            {
                errorDetails.Add($"Baris {lineNumber}: Nomor Meja '{tempNomorMeja}' tidak valid.");
                isValid = false;
            }
            else
            {
                nomorMejaCsv = tempNomorMeja;
            }
            if (!int.TryParse(tempKapasitasStr, out kapasitasCsv) || kapasitasCsv < 1 || kapasitasCsv > 99)
            {
                errorDetails.Add($"Baris {lineNumber}: Kapasitas '{tempKapasitasStr}' tidak valid.");
                isValid = false;
            }
            return isValid;
        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            string queryToAnalyze = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja ORDER BY Nomor_Meja ASC";
            AnalyzeQuery(queryToAnalyze);

        }

        
    }
}