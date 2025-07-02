using ClosedXML.Excel; // Tambahkan ini di bagian atas file .cs Anda
using System;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO; // Diperlukan untuk operasi File seperti File.ReadAllLines
using System.Collections.Generic; // Diperlukan untuk List<T>
using System.Runtime.Caching; // Diperlukan untuk Caching
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using LoginPage; // Untuk format .xlsx

namespace PABDCAFE
{
    public partial class AdminReservasi : Form
    {
        private readonly string connectionString;
        private SqlConnection conn;

        // Caching fields
        private readonly MemoryCache _reservasiDataCache = MemoryCache.Default;
        private const string ReservasiDataCacheKey = "AdminReservasiGridData";

        private readonly MemoryCache _availableMejaCache = MemoryCache.Default;
        private const string AvailableMejaCacheKey = "AdminReservasiAvailableMejaList";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        // DataTable untuk menyimpan data preview
        private DataTable PreviewDataTable; // Ini sudah diinisialisasi di InitializePreviewDataTable()

        public AdminReservasi(string connStr)
        {
            InitializeComponent(); // Panggil ini paling awal
            InitializePreviewDataTable(); // Kemudian inisialisasi DataTable

            dtpWaktuReservasi.Format = DateTimePickerFormat.Custom;
            dtpWaktuReservasi.CustomFormat = "dd/MM/yyyy HH:mm"; // Format: tanggal/bulan/tahun spasi jam:menit

            // Opsional: Menggunakan tombol panah (spinner) untuk memilih waktu
            dtpWaktuReservasi.ShowUpDown = true;

            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak ada atau kosong. Tidak dapat terhubung ke database.", "Kesalahan Konfigurasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException(nameof(connStr), "String koneksi database (connStr) tidak boleh null atau kosong.");
            }
            this.connectionString = connStr;
            this.conn = new SqlConnection(this.connectionString);


            // Asumsikan cbxNomorMeja adalah nama kontrol ComboBox di designer
            if (this.cbxNomorMeja != null)
            {
                LoadAvailableMeja(this.cbxNomorMeja);
            }
            else
            {
                MessageBox.Show("Error: Kontrol ComboBox 'cbxNomorMeja' tidak ditemukan sebagai field.", "Kesalahan Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadData();

            this.dgvAdminReservasi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;         
        }



        private void InitializePreviewDataTable()
        {
            PreviewDataTable = new DataTable();
        }

        private void InvalidateReservasiDataCache()
        {
            _reservasiDataCache.Remove(ReservasiDataCacheKey);
        }

        private void InvalidateAvailableMejaCache()
        {
            _availableMejaCache.Remove(AvailableMejaCacheKey);
        }

        private void LoadAvailableMeja(ComboBox cbx)
        {
            // ... (logika cache dan pengecekan koneksi tetap sama) ...
            List<string> mejaList = _availableMejaCache.Get(AvailableMejaCacheKey) as List<string>;

            if (mejaList == null)
            {
                if (this.conn == null) { /* ... */ }
                else
                {
                    mejaList = new List<string>();
                    try
                    {
                        if (this.conn.State == ConnectionState.Closed)
                        {
                            this.conn.Open();
                        }

                        using (SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja ORDER BY Nomor_Meja", this.conn))

                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                mejaList.Add(reader["Nomor_Meja"].ToString());
                            }
                            reader.Close();
                            if (mejaList.Count > 0)
                            {
                                _availableMejaCache.Set(AvailableMejaCacheKey, mejaList, DateTimeOffset.Now.Add(_cacheDuration));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Gunakan ex.Message untuk menampilkan detail error yang sebenarnya
                        MessageBox.Show("Terjadi kesalahan saat mengekspor: " + ex.Message, "Error Ekspor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (this.conn != null && this.conn.State == ConnectionState.Open)
                        {
                            this.conn.Close();
                        }
                    }
                }
            }

            if (cbx == null) return;
            cbx.DataSource = null;
            cbx.Items.Clear();
            if (mejaList != null && mejaList.Count > 0)
            {
                cbx.DataSource = mejaList;
                cbx.SelectedIndex = -1;
            }
            else
            {
                cbx.Items.Add("Tidak ada data meja");
                if (cbx.Items.Count > 0)
                {
                    cbx.SelectedIndex = 0;
                }
            }
        }

        void LoadData()
        {
            DataTable dt = _reservasiDataCache.Get(ReservasiDataCacheKey) as DataTable;

            if (dt == null)
            {
                if (this.conn == null)
                {
                    MessageBox.Show("Koneksi database belum diinisialisasi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    using (var da = new SqlDataAdapter(
                        "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi ORDER BY Waktu_Reservasi DESC", this.conn))
                    {
                        dt = new DataTable();
                        da.Fill(dt);
                        _reservasiDataCache.Set(ReservasiDataCacheKey, dt, DateTimeOffset.Now.Add(_cacheDuration));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data: " + ex.Message, "Kesalahan Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (this.dgvAdminReservasi != null)
            {
                this.dgvAdminReservasi.DataSource = dt;

                if (this.dgvAdminReservasi.Columns.Contains("ID_Reservasi"))
                    this.dgvAdminReservasi.Columns["ID_Reservasi"].HeaderText = "ID";
                if (this.dgvAdminReservasi.Columns.Contains("Nama_Customer"))
                    this.dgvAdminReservasi.Columns["Nama_Customer"].HeaderText = "Nama Customer";
                if (this.dgvAdminReservasi.Columns.Contains("No_Telp"))
                    this.dgvAdminReservasi.Columns["No_Telp"].HeaderText = "No. Telepon";
                if (this.dgvAdminReservasi.Columns.Contains("Waktu_Reservasi"))
                {
                    this.dgvAdminReservasi.Columns["Waktu_Reservasi"].HeaderText = "Waktu Reservasi";
                    this.dgvAdminReservasi.Columns["Waktu_Reservasi"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                }
                if (this.dgvAdminReservasi.Columns.Contains("Nomor_Meja"))
                    this.dgvAdminReservasi.Columns["Nomor_Meja"].HeaderText = "Nomor Meja";

                this.dgvAdminReservasi.ClearSelection();
            }
        }

        private bool CekJadwalBentrok(string nomorMeja, DateTime waktu, int? reservasiIdToExclude = null)
        {
            // Query dasar untuk mengecek jadwal pada tanggal yang sama
            string query = "SELECT COUNT(*) FROM Reservasi WHERE Nomor_Meja = @NomorMeja AND CAST(Waktu_Reservasi AS DATE) = CAST(@WaktuReservasi AS DATE)";

            // Jika sedang mode edit, tambahkan kondisi untuk mengabaikan ID reservasi saat ini
            if (reservasiIdToExclude.HasValue)
            {
                query += " AND ID_Reservasi <> @ID_Reservasi_Exclude";
            }

            using (var tempConn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(query, tempConn))
                {
                    cmd.Parameters.AddWithValue("@NomorMeja", nomorMeja);
                    cmd.Parameters.AddWithValue("@WaktuReservasi", waktu);
                    if (reservasiIdToExclude.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@ID_Reservasi_Exclude", reservasiIdToExclude.Value);
                    }

                    tempConn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0; // Jika count > 0, berarti ada jadwal yang bentrok
                }
            }
        }

        private void AdminReservasi_Load(object sender, EventArgs e)
        {
            EnsureIndexes();
            LoadData();
            ClearForm();


        }

        void ClearForm()
        {
            txtNama.Clear();
            txtTelepon.Clear();

            if (this.dtpWaktuReservasi != null)
            {
                this.dtpWaktuReservasi.Value = DateTime.Now;
            }

            if (this.cbxNomorMeja != null)
            {
                this.cbxNomorMeja.SelectedIndex = -1;
                InvalidateAvailableMejaCache();
                LoadAvailableMeja(this.cbxNomorMeja);
            }

            this.dgvAdminReservasi.ClearSelection();
            txtNama.Focus();
        }

        bool ValidasiInput(out string err)
        {
            err = string.Empty;
            if (this.txtNama == null || string.IsNullOrWhiteSpace(this.txtNama.Text))
                err += "Nama customer tidak boleh kosong.\n";

            if (this.txtTelepon == null || !Regex.IsMatch(this.txtTelepon.Text.Trim(), @"^(\+62\d{8,12}|0\d{9,14})$"))
                err += "Format nomor telepon tidak valid (Contoh: +6281234567890 atau 081234567890).\n";

            if (this.dtpWaktuReservasi != null)
            {
                if (this.dtpWaktuReservasi.Value < DateTime.Now.AddMinutes(-1))
                {
                    err += "Waktu reservasi tidak boleh di masa lalu.\n";
                }
                
            }
            else
            {
                err += "Kontrol DateTimePicker untuk waktu reservasi tidak ditemukan.\n";
            }

            if (this.cbxNomorMeja != null)
            {
                if (this.cbxNomorMeja.SelectedItem == null || string.IsNullOrWhiteSpace(this.cbxNomorMeja.SelectedItem.ToString()) || this.cbxNomorMeja.SelectedItem.ToString() == "Tidak ada meja tersedia")
                    err += "Nomor meja yang valid harus dipilih.\n";
            }
            else
            {
                err += "Kontrol ComboBox untuk nomor meja tidak ditemukan.\n";
            }

            return string.IsNullOrEmpty(err);
        }

        private bool IsConnectionReady()
        {
            if (this.conn == null || string.IsNullOrWhiteSpace(this.conn.ConnectionString))
            {
                MessageBox.Show("Koneksi database tidak dikonfigurasi atau hilang.", "Error Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void EnsureIndexes()
        {
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var indexScript = @"
                    -- Memastikan tabel Reservasi ada sebelum membuat indeks di atasnya
                    IF OBJECT_ID('dbo.Reservasi', 'U') IS NOT NULL
                    BEGIN
                    -- Indeks untuk Foreign Key Nomor_Meja
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Nomor_Meja' AND object_id = OBJECT_ID('dbo.Reservasi'))
                    CREATE NONCLUSTERED INDEX IX_Reservasi_Nomor_Meja ON dbo.Reservasi(Nomor_Meja);

                    -- Indeks untuk Waktu_Reservasi
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Waktu' AND object_id = OBJECT_ID('dbo.Reservasi'))
                    CREATE NONCLUSTERED INDEX IX_Reservasi_Waktu ON dbo.Reservasi(Waktu_Reservasi);

                    -- Indeks untuk pencarian Nama_Customer
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Nama_Customer' AND object_id = OBJECT_ID('dbo.Reservasi'))
                    CREATE NONCLUSTERED INDEX IX_Reservasi_Nama_Customer ON dbo.Reservasi(Nama_Customer);

                    -- Indeks untuk pencarian No_Telp
                    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_No_Telp' AND object_id = OBJECT_ID('dbo.Reservasi'))
                    CREATE NONCLUSTERED INDEX IX_Reservasi_No_Telp ON dbo.Reservasi(No_Telp);
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;
            if (this.dgvAdminReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string errorMsg;
            if (!ValidasiInput(out errorMsg)) // Validasi input dasar
            {
                MessageBox.Show(errorMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idReservasi = Convert.ToInt32(this.dgvAdminReservasi.SelectedRows[0].Cells["ID_Reservasi"].Value);
                DateTime waktuReservasi = this.dtpWaktuReservasi.Value;
                string selectedMeja = this.cbxNomorMeja.SelectedItem.ToString();

                // Panggil metode validasi jadwal, sertakan ID reservasi yang sedang diedit
                if (CekJadwalBentrok(selectedMeja, waktuReservasi, idReservasi))
                {
                    MessageBox.Show($"Meja '{selectedMeja}' sudah dipesan untuk tanggal {waktuReservasi:dd-MM-yyyy} oleh reservasi lain.", "Jadwal Bentrok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Hentikan proses jika bentrok
                }

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("EditReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    cmd.Parameters.AddWithValue("@Nama_Customer", this.txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", this.txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja_Baru", selectedMeja);

                    // --- BAGIAN YANG HILANG DIMULAI DI SINI ---
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvalidateReservasiDataCache();
                        InvalidateAvailableMejaCache();
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau gagal diperbarui!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // --- BAGIAN YANG HILANG SELESAI DI SINI ---
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat mengubah data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void dgvAdminReservasi_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgvAdminReservasi.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = this.dgvAdminReservasi.SelectedRows[0];
                this.txtNama.Text = selectedRow.Cells["Nama_Customer"].Value?.ToString();
                this.txtTelepon.Text = selectedRow.Cells["No_Telp"].Value?.ToString();

                // --- AWAL PERBAIKAN ---
                // Ambil nilai dari sel ke dalam sebuah variabel objek
                object waktuValue = selectedRow.Cells["Waktu_Reservasi"].Value;

                // Periksa apakah nilainya valid sebelum dikonversi
                if (waktuValue != null && waktuValue != DBNull.Value)
                {
                    this.dtpWaktuReservasi.Value = Convert.ToDateTime(waktuValue);
                }
                else
                {
                    // Jika nilainya NULL, atur DateTimePicker ke waktu sekarang sebagai default
                    this.dtpWaktuReservasi.Value = DateTime.Now;
                }
                // --- AKHIR PERBAIKAN ---

                this.cbxNomorMeja.SelectedItem = selectedRow.Cells["Nomor_Meja"].Value?.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminPage ap = new AdminPage(this.connectionString);
            ap.Show();
            this.Close();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;

            string errorMsg;
            if (!ValidasiInput(out errorMsg)) // Validasi input dasar
            {
                MessageBox.Show(errorMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DateTime waktuReservasi = this.dtpWaktuReservasi.Value;
                string selectedMeja = this.cbxNomorMeja.SelectedItem.ToString();

                // Panggil metode validasi jadwal bentrok sebelum menambahkan
                if (CekJadwalBentrok(selectedMeja, waktuReservasi))
                {
                    MessageBox.Show($"Meja '{selectedMeja}' sudah dipesan untuk tanggal {waktuReservasi:dd-MM-yyyy}.", "Jadwal Bentrok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Hentikan proses jika bentrok
                }

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Customer", this.txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", this.txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja", selectedMeja);

                    // --- BAGIAN YANG HILANG DIMULAI DI SINI ---
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Menggunakan > 0 karena SP Admin mungkin tidak memakai SET NOCOUNT ON
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservasi berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Segarkan data dan form setelah berhasil
                        InvalidateReservasiDataCache();
                        InvalidateAvailableMejaCache();
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        // Jika SP Anda memakai SET NOCOUNT ON, pesan ini mungkin muncul meski data masuk.
                        // Jika itu terjadi, hapus saja blok if-else ini dan langsung jalankan logika sukses seperti di CustomerPage.
                        MessageBox.Show("Reservasi tidak berhasil ditambahkan!", "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // --- BAGIAN YANG HILANG SELESAI DI SINI ---
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat menambah data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;

            if (this.dgvAdminReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang akan dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.No)
            {
                return;
            }

            try
            {
                int idReservasi = Convert.ToInt32(this.dgvAdminReservasi.SelectedRows[0].Cells["ID_Reservasi"].Value);

                conn.Open();

                using (SqlCommand cmd = new SqlCommand("HapusReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvalidateReservasiDataCache();
                        InvalidateAvailableMejaCache();
                        LoadData();
                        ClearForm();
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
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Excel Workbook (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Title = "Pilih File Excel untuk Impor Data Reservasi"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    DataTable dt = new DataTable(); // Buat DataTable baru untuk data dari Excel

                    using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        IWorkbook workbook = new XSSFWorkbook(fs);
                        ISheet sheet = workbook.GetSheetAt(0); // Ambil sheet pertama

                        IRow headerRow = sheet.GetRow(0);
                        if (headerRow != null)
                        {
                            // === START: Bagian yang diubah/ditambahkan untuk menormalisasi nama kolom ===
                            // Mendapatkan nama header dari Excel dan menambahkannya ke DataTable
                            foreach (var cell in headerRow.Cells)
                            {
                                dt.Columns.Add(cell.ToString().Trim()); // Tambahkan kolom dengan nama asli dari Excel
                            }

                            // Menyesuaikan nama kolom DataTable agar sesuai dengan nama kolom di database
                            // Asumsi: Nama kolom database adalah Nama_Customer, No_Telepon, Waktu_Reservasi, Nomor_Meja
                            if (dt.Columns.Contains("Nama Customer"))
                            {
                                dt.Columns["Nama Customer"].ColumnName = "Nama_Customer";
                            }
                            if (dt.Columns.Contains("Nomor Telepon"))
                            {
                                dt.Columns["Nomor Telepon"].ColumnName = "No_Telp";
                            }
                            if (dt.Columns.Contains("Waktu Reservasi"))
                            {
                                dt.Columns["Waktu Reservasi"].ColumnName = "Waktu_Reservasi";
                            }
                            if (dt.Columns.Contains("Nomor Meja"))
                            {
                                dt.Columns["Nomor Meja"].ColumnName = "Nomor_Meja";
                            }
                            // === END: Bagian yang diubah/ditambahkan ===
                        }
                        else
                        {
                            MessageBox.Show("File Excel kosong atau tidak memiliki baris header.", "Informasi Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Inisialisasi FormulaEvaluator untuk menangani sel formula
                        IFormulaEvaluator evaluator = workbook.GetCreationHelper().CreateFormulaEvaluator(); // Tambahkan ini

                        for (int i = 1; i <= sheet.LastRowNum; i++) // Mulai dari baris ke-1 (indeks 1)
                        {
                            IRow dataRow = sheet.GetRow(i);
                            if (dataRow == null) continue; // Lewati baris kosong

                            DataRow newRow = dt.NewRow();
                            // Pastikan kita tidak melebihi jumlah kolom di DataTable
                            for (int cellIndex = 0; cellIndex < dt.Columns.Count; cellIndex++)
                            {
                                ICell cell = dataRow.GetCell(cellIndex);
                                if (cell != null)
                                {
                                    // Mengambil nilai sel berdasarkan tipe data
                                    switch (cell.CellType)
                                    {
                                        case CellType.String:
                                            newRow[cellIndex] = cell.StringCellValue;
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(cell))
                                            {
                                                newRow[cellIndex] = cell.DateCellValue;
                                            }
                                            else
                                            {
                                                newRow[cellIndex] = cell.NumericCellValue;
                                            }
                                            break;
                                        case CellType.Boolean:
                                            newRow[cellIndex] = cell.BooleanCellValue;
                                            break;
                                        case CellType.Formula: // Tangani sel formula
                                            CellValue cellValue = evaluator.Evaluate(cell); // Evaluasi formula
                                            switch (cellValue.CellType)
                                            {
                                                case CellType.Numeric:
                                                    if (DateUtil.IsCellDateFormatted(cell))
                                                    {
                                                        newRow[cellIndex] = cell.DateCellValue; // Gunakan DateCellValue jika itu tanggal
                                                    }
                                                    else
                                                    {
                                                        newRow[cellIndex] = cellValue.NumberValue;
                                                    }
                                                    break;
                                                case CellType.String:
                                                    newRow[cellIndex] = cellValue.StringValue;
                                                    break;
                                                case CellType.Boolean:
                                                    newRow[cellIndex] = cellValue.BooleanValue;
                                                    break;
                                                case CellType.Error:
                                                    newRow[cellIndex] = FormulaError.ForInt(cellValue.ErrorValue).String;
                                                    break;
                                                default:
                                                    newRow[cellIndex] = cell.ToString(); // Fallback
                                                    break;
                                            }
                                            break;
                                        default:
                                            newRow[cellIndex] = cell.ToString();
                                            break;
                                    }
                                }
                                else
                                {
                                    newRow[cellIndex] = DBNull.Value; // Jika sel kosong
                                }
                            }
                            dt.Rows.Add(newRow);
                        }
                    }

                    PreviewDataReservasi previewForm = new PreviewDataReservasi(dt, this.connectionString);

                    // Tampilkan form preview sebagai dialog modal
                    if (previewForm.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show("Data berhasil diimpor!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvalidateReservasiDataCache();
                        InvalidateAvailableMejaCache();
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Import data dibatalkan.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("Gagal membaca file: " + ioEx.Message + "\nPastikan file tidak sedang dibuka oleh program lain.", "Kesalahan File IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat mengimpor data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dtpWaktuReservasi_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnAnalisis_Click(object sender, EventArgs e)
        {
            string queryToAnalyze = "SELECT Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi ORDER BY Nama_Customer ASC";
            AnalyzeQuery(queryToAnalyze);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            // 1. Pastikan ada baris yang dipilih di DataGridView
            if (dgvAdminReservasi.CurrentRow == null)
            {
                MessageBox.Show("Silakan pilih salah satu data reservasi terlebih dahulu.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Hentikan proses jika tidak ada baris yang dipilih
            }

            // 2. Ambil ID Reservasi dari sel di baris yang dipilih
            //    Ganti "ID_Reservasi" dengan nama kolom ID Anda di DataGridView.
            string idReservasiDipilih = dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value.ToString();

            // 3. Buat instance form ReportViewerReservasi dengan memberikan ID
            ReportViewerReservasi form = new ReportViewerReservasi(idReservasiDipilih);
            form.ShowDialog();
        }

        
    }
}