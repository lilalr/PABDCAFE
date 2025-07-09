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
using NPOI.SS.Formula.Functions;

// Namespace disesuaikan dengan nama proyek Anda
namespace PABDCAFE
{
    public partial class AdminMeja : Form
    {
        // Variabel-variabel utama kelas
        //private readonly string connectionString;
        private readonly string activeConnectionString;
        // Objek 'cache' untuk menyimpan data sementara. Ini seperti catatan contekan
        // agar tidak perlu bertanya ke database terus-menerus, sehingga aplikasi lebih cepat.
        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            // Data di cache akan otomatis terhapus setelah 5 menit.
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "AdminMeja"; // Kunci unik untuk cache data meja.

        // Konstruktor Form
        public AdminMeja(string connStr)
        {
            InitializeComponent();
            this.activeConnectionString = connStr;
        }

        // Method ini berjalan otomatis saat form pertama kali ditampilkan.
        private void AdminMeja_Load(object sender, EventArgs e)
        {
            try
            {
                // Urutan kerja: Siapkan tabel, optimalkan database, lalu muat dan tampilkan datanya.
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

        // Ini adalah method inti untuk menampilkan data.

        private void LoadAndDisplayData()
        {
            // 1. Cek dulu "catatan contekan" (cache). Apakah datanya sudah ada?
            DataTable dt = _cache.Get(CacheKey) as DataTable;
            // 2. Jika tidak ada di cache (pertama kali load atau cache sudah kadaluwarsa)...
            if (dt == null)
            {
                try
                {
                    // ...baru kita ambil data dari database.
                    dt = new DataTable();
                    string query = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja ORDER BY Nomor_Meja ASC";
                    using (var conn = new SqlConnection(this.activeConnectionString))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(query, conn);
                        da.Fill(dt);
                    }
                    // 3. Simpan data yang baru diambil ke cache agar load berikutnya lebih cepat.
                    _cache.Set(CacheKey, dt, _policy);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error memuat data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // 4. Tampilkan datanya ke tabel di layar.
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

        // Fitur optimasi untuk memastikan pencarian data di database lebih cepat.
        private void EnsureIndexes()
        {
            // Method ini membuat 'indeks' di database jika belum ada, mirip daftar isi pada buku.
            // Tidak wajib, tapi sangat membantu performa.

            try
            {
                using (var conn = new SqlConnection(this.activeConnectionString))
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
                using (var conn = new SqlConnection(this.activeConnectionString))
                {
                    // Ini bagian terpenting:
                    // Baris ini menyuruh program untuk 'mendengarkan' semua pesan informasi
                    // yang dikirim oleh database. Hasil statistik performa akan dikirim
                    // sebagai pesan ini, yang kemudian kita tampilkan dalam sebuah MessageBox.

                    conn.InfoMessage += (s, e) => MessageBox.Show(e.Message, "Info Statistik Kinerja");

                    conn.Open();


                    // Kita 'membungkus' query asli yang ingin dianalisis dengan perintah
                    // untuk menyalakan dan mematikan statistik dari SQL Server.
                    var wrappedQuery = $@"
                SET STATISTICS IO ON;
                SET STATISTICS TIME ON;
                {sqlQuery}
                SET STATISTICS TIME OFF;
                SET STATISTICS IO OFF;";

                    // Jalankan seluruh perintah yang sudah dibungkus tadi.
                    // Hasilnya tidak diambil sebagai data, melainkan ditangkap oleh 'conn.InfoMessage' di atas.
                    using (var cmd = new SqlCommand(wrappedQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal melakukan analisis query: " + ex.Message, "Kesalahan Analisis", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Method ini penting untuk menjaga data tetap update.
        // Setiap kali kita menambah, mengubah, atau menghapus data, cache lama harus dibuang
        // agar aplikasi mengambil data terbaru dari database.
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

        // Fungsi penting untuk memastikan data yang diinput pengguna itu benar formatnya
        // sebelum disimpan ke database. Ini mencegah data sampah masuk.
        bool ValidasiInput(out string nomorMeja, out int kapasitas, out string errorMsg)
        {
            var errorMessages = new List<string>();
            nomorMeja = txtNomor.Text.Trim();
            string kapasitasStr = txtKapasitas.Text.Trim();
            kapasitas = 0;

            // Cek: Nomor Meja
            if (string.IsNullOrWhiteSpace(nomorMeja))
            {
                errorMessages.Add("Field Nomer Meja Wajib Diisi.");
            }
            else if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                errorMessages.Add("Nomor Meja harus terdiri dari 2 digit angka.");
            }

            // Cek: Kapasitas
            if (string.IsNullOrWhiteSpace(kapasitasStr))
            {
                errorMessages.Add("Field Kapasitas Wajib Diisi.");
            }
            else if (!int.TryParse(kapasitasStr, out kapasitas) || kapasitas < 1 || kapasitas > 99)
            {
                errorMessages.Add("Kapasitas harus berupa angka antara 1 sampai 99.");
            }

            errorMsg = string.Join("\n", errorMessages);
            return !errorMessages.Any();
        }


        // Aksi saat tombol "Tambah" diklik.
        private void btnTambah_Click(object sender, EventArgs e)
        {
            // 1. Validasi dulu inputnya.
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Membuka koneksi ke database.
            using (var conn = new SqlConnection(this.activeConnectionString))
            {
                //Memulai SqlTransaction.
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();

                    //Menjalankan stored procedure (TambahMeja) yang mengubah data di database.
                    using (SqlCommand cmd = new SqlCommand("TambahMeja", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                        cmd.ExecuteNonQuery();
                    }
                    // Jika berhasil, ia melakukan Commit.
                    transaction.Commit();
                    MessageBox.Show("Data berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //invalidate cache
                    InvalidateAdminMejaCache();
                    LoadAndDisplayData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    //Jika gagal, ia melakukan Rollback.
                    transaction?.Rollback();
                    MessageBox.Show("Error saat menambah data: " + ex.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // 1. Validasi dulu inputnya.
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Apakah Anda yakin ingin mengubah data meja '{nomorMeja}'?", "Konfirmasi Perubahan Data", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return; // Keluar dari method jika pengguna memilih 'No'
            }

            // Membuka koneksi ke database.
            using (var conn = new SqlConnection(this.activeConnectionString))
            {
                //Memulai SqlTransaction.
                SqlTransaction transaction = null;
                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    //Menjalankan stored procedure (EditMeja) yang mengubah data di database.
                    using (SqlCommand cmd = new SqlCommand("EditMeja", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                        cmd.ExecuteNonQuery();
                    }
                    // Jika berhasil, ia melakukan Commit.
                    transaction.Commit();
                    MessageBox.Show("data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //invalidate cache
                    InvalidateAdminMejaCache();
                    LoadAndDisplayData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    // Jika gagal, ia melakukan Rollback.
                    transaction?.Rollback();
                    MessageBox.Show("Error saat mengubah data: " + ex.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            // konfirmasi validasi
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


            using (var conn = new SqlConnection(this.activeConnectionString))
            {
                //Memulai SqlTransaction.
                SqlTransaction transaction = null;
                try
                { 
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    int rowsAffected;
                    //Menjalankan stored procedure (HapusMeja) yang mengubah data di database.
                    using (SqlCommand cmd = new SqlCommand("HapusMeja", conn, transaction))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    // Jika berhasil, ia melakukan Commit.
                    transaction.Commit();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    //invalidate cache
                    InvalidateAdminMejaCache();
                    LoadAndDisplayData();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    // Jika gagal, ia melakukan Rollback.
                    transaction?.Rollback();
                    MessageBox.Show("Error saat menghapus data: " + ex.Message, "Transaksi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // 1. Munculkan dialog untuk user memilih file Excel.
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Pilih File Excel untuk Impor Data Meja"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    DataTable dt = new DataTable();

                    // 2. Baca file Excel menggunakan library NPOI.
                    // Data dari Excel akan dimasukkan ke dalam sebuah DataTable.
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

                    // 3. Tampilkan form 'Preview' agar user bisa cek data sebelum disimpan.
                    // Ini mencegah salah impor data.
                    PreviewDataMeja previewForm = new PreviewDataMeja(dt, this.activeConnectionString);

                    if (previewForm.ShowDialog() == DialogResult.OK && previewForm.ImportConfirmed)
                    {
                        // Jika user menekan OK di form preview, baru datanya di-refresh.
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
            // Membuka halaman baru untuk menampilkan laporan.
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
            // Kembali ke halaman admin utama.
            try
            {
                AdminPage ap = new AdminPage(this.activeConnectionString);
                ap.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal kembali ke halaman admin: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Aksi saat salah satu baris di tabel diklik.
        private void dgvAdminMeja_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Tujuannya untuk menyalin data dari baris yang diklik ke kotak isian (TextBox),
            // agar mudah untuk diedit atau dihapus.
            if (e.RowIndex >= 0 && e.RowIndex < dgvAdminMeja.Rows.Count)
            {
                DataGridViewRow row = dgvAdminMeja.Rows[e.RowIndex];
                txtNomor.Text = row.Cells["Nomor_Meja"].Value?.ToString() ?? "";
                txtKapasitas.Text = row.Cells["Kapasitas"].Value?.ToString() ?? "";
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                
                InvalidateAdminMejaCache();

               
                LoadAndDisplayData();

               
                ClearForm();

                MessageBox.Show("Data berhasil disegarkan dari database.", "Refresh Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi error saat menyegarkan data: " + ex.Message, "Kesalahan Refresh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
