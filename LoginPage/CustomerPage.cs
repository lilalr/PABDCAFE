using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; // Diperlukan untuk interaksi dengan SQL Server
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; // Diperlukan untuk Regex (validasi input)
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Caching; // Ditambahkan untuk Caching

namespace PABDCAFE
{
    // Form CustomerPage digunakan oleh customer untuk membuat dan melihat reservasi mereka.
    public partial class CustomerPage : Form
    {
        private readonly string connectionString;
        private SqlConnection conn;
        private int? selectedReservasiID = null;

        // Caching fields
        private readonly MemoryCache _reservasiCache = MemoryCache.Default;
        private const string ReservasiCacheKey = "CustomerPageReservasiData";

        private readonly MemoryCache _availableMejaCache = MemoryCache.Default;
        private const string AvailableMejaCacheKey = "CustomerPageAvailableMejaList";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(2); // Durasi cache bisa disesuaikan

        public CustomerPage(string connStr)
        {
            InitializeComponent();

            dtpCustWaktu.Format = DateTimePickerFormat.Custom;
            dtpCustWaktu.CustomFormat = "dd/MM/yyyy HH:mm"; // Format: tanggal/bulan/tahun spasi jam:menit

            // Opsional: Menggunakan tombol panah (spinner) untuk memilih waktu
            dtpCustWaktu.ShowUpDown = true;

            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak valid diterima oleh CustomerPage.", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException(nameof(connStr), "String koneksi tidak boleh null atau kosong.");
            }
            this.connectionString = connStr;
            this.conn = new SqlConnection(this.connectionString);


        }

        public CustomerPage()
        {
            InitializeComponent();
            MessageBox.Show("CustomerPage dibuat tanpa string koneksi. Fitur database mungkin tidak berfungsi.", "Peringatan Konstruktor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.connectionString = null;
        }

        private void CustomerPage_Load(object sender, EventArgs e)
        {
            if (!IsConnectionReady())
            {
                txtCustNama.Enabled = false;
                txtCustNoTelp.Enabled = false;
                dtpCustWaktu.Enabled = false;
                cmbCustMeja.Enabled = false;
                btnCustTambah.Enabled = false;
                btnCustHapus.Enabled = false;
                return;
            }

            EnsureIndexes();

            LoadReservasi();
            LoadComboBoxMeja(cmbCustMeja);

            dtpCustWaktu.Format = DateTimePickerFormat.Custom;
            dtpCustWaktu.CustomFormat = "yyyy-MM-dd HH:mm";
            dtpCustWaktu.ShowUpDown = false;
        }

        private void EnsureIndexes()
        {
            if (!IsConnectionReady()) return;

            try
            {
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();
                    var indexScript = @"
                    IF OBJECT_ID('dbo.Reservasi', 'U') IS NOT NULL
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Nomor_Meja')
                            CREATE NONCLUSTERED INDEX IX_Reservasi_Nomor_Meja ON dbo.Reservasi(Nomor_Meja);

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Waktu')
                            CREATE NONCLUSTERED INDEX IX_Reservasi_Waktu ON dbo.Reservasi(Waktu_Reservasi);

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Nama_Customer')
                            CREATE NONCLUSTERED INDEX IX_Reservasi_Nama_Customer ON dbo.Reservasi(Nama_Customer);

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_No_Telp')
                            CREATE NONCLUSTERED INDEX IX_Reservasi_No_Telp ON dbo.Reservasi(No_Telp);
                    END

                    IF OBJECT_ID('dbo.Meja', 'U') IS NOT NULL
                    BEGIN
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Meja_Status')
                            CREATE NONCLUSTERED INDEX IX_Meja_Status ON dbo.Meja(Status_Meja);
                    END";

                    using (var cmd = new SqlCommand(indexScript, sqlConnection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memastikan index dalam database: " + ex.Message, "Peringatan Performa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InvalidateReservasiCache()
        {
            _reservasiCache.Remove(ReservasiCacheKey);
        }

        private void InvalidateAvailableMejaCache()
        {
            _availableMejaCache.Remove(AvailableMejaCacheKey);
        }

        private void LoadComboBoxMeja(ComboBox cmb)
        {
            if (!IsConnectionReady()) return;

            // Cache di sini mungkin tidak lagi relevan karena kita memuat semua meja,
            // tapi kita biarkan untuk konsistensi. Anda bisa juga menghapus logika cache khusus untuk ini.
            List<string> mejaList = _availableMejaCache.Get(AvailableMejaCacheKey) as List<string>;

            if (mejaList == null)
            {
                mejaList = new List<string>();
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    // --- AWAL PERUBAHAN ---
                    // Query diubah untuk mengambil SEMUA nomor meja, tanpa filter status.
                    using (SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja ORDER BY Nomor_Meja", conn))
                    // --- AKHIR PERUBAHAN ---
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            mejaList.Add(reader["Nomor_Meja"].ToString());
                        }
                        reader.Close();
                    }
                    _availableMejaCache.Set(AvailableMejaCacheKey, mejaList, DateTimeOffset.Now.Add(_cacheDuration));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat daftar meja: " + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }

            cmb.DataSource = null;
            cmb.Items.Clear();
            cmb.DataSource = mejaList;
            if (mejaList.Count > 0)
            {
                cmb.SelectedIndex = -1;
            }
            else
            {
                cmb.Items.Add("Tidak ada data meja");
                if (cmb.Items.Count > 0) cmb.SelectedIndex = 0;
            }
        }

        private bool ValidasiInput()
        {
            if (!IsConnectionReady()) return false;

            string nama = txtCustNama.Text.Trim();
            string telp = txtCustNoTelp.Text.Trim();
            string meja = cmbCustMeja.SelectedItem?.ToString() ?? cmbCustMeja.Text.Trim();
            DateTime waktu = dtpCustWaktu.Value;

            if (string.IsNullOrWhiteSpace(nama) || nama.Length < 3) { /*...*/ return false; }
            if (!Regex.IsMatch(telp, @"^(\+62\d{8,12}|0\d{9,14})$")) { /*...*/ return false; }
            if (waktu < DateTime.Now.AddMinutes(-1)) { /*...*/ return false; }
            if (waktu.Year != 2025) { /*...*/ return false; }
            if (string.IsNullOrWhiteSpace(meja) || meja.Contains("Tidak ada")) { /*...*/ return false; }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

               
                string queryCekJadwal = "SELECT COUNT(*) FROM Reservasi WHERE Nomor_Meja = @NomorMeja AND CAST(Waktu_Reservasi AS DATE) = CAST(@WaktuReservasi AS DATE)";

                using (SqlCommand cekJadwalCmd = new SqlCommand(queryCekJadwal, conn))
                {
                    cekJadwalCmd.Parameters.AddWithValue("@NomorMeja", meja);
                    cekJadwalCmd.Parameters.AddWithValue("@WaktuReservasi", waktu);

                    int countReservasi = (int)cekJadwalCmd.ExecuteScalar();
                    if (countReservasi > 0)
                    {
                        MessageBox.Show($"Meja '{meja}' sudah direservasi untuk tanggal {waktu:dd-MM-yyyy}. Silakan pilih meja atau tanggal lain.", "Jadwal Bentrok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kesalahan saat validasi data ke database: " + ex.Message, "Error Validasi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void btnCustTambah_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady() || !ValidasiInput())
            {
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = new SqlCommand("TambahReservasiCustomer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtCustNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtCustNoTelp.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", dtpCustWaktu.Value);
                    cmd.Parameters.AddWithValue("@Nomor_Meja", cmbCustMeja.Text.Trim());

                    
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Reservasi berhasil ditambahkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    InvalidateReservasiCache();
                    InvalidateAvailableMejaCache();
                    LoadReservasi();
                    LoadComboBoxMeja(cmbCustMeja);
                    ClearForm();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Gagal menambahkan reservasi: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menambahkan reservasi: " + ex.Message, "Error Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void LoadReservasi()
        {
            if (!IsConnectionReady()) return;

            DataTable dt = _reservasiCache.Get(ReservasiCacheKey) as DataTable;

            if (dt == null)
            {
                // System.Diagnostics.Debug.WriteLine("Loading reservations from DB for CustomerPage");
                dt = new DataTable();
                try
                {
                    string query = "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi ORDER BY Waktu_Reservasi DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    if (da.SelectCommand != null)
                    {
                        da.SelectCommand.CommandTimeout = 120;
                    }
                    da.Fill(dt);
                    _reservasiCache.Set(ReservasiCacheKey, dt, DateTimeOffset.Now.Add(_cacheDuration));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menampilkan data reservasi: " + ex.Message, "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Jangan set DataSource jika gagal
                }
            }
            // else { /* System.Diagnostics.Debug.WriteLine("Loading reservations from Cache for CustomerPage"); */ }


            dgvCustomer.DataSource = dt;

            if (dgvCustomer.Columns.Contains("ID_Reservasi")) dgvCustomer.Columns["ID_Reservasi"].HeaderText = "ID Reservasi";
            if (dgvCustomer.Columns.Contains("Nama_Customer")) dgvCustomer.Columns["Nama_Customer"].HeaderText = "Nama";
            if (dgvCustomer.Columns.Contains("No_Telp")) dgvCustomer.Columns["No_Telp"].HeaderText = "No. Telepon";
            if (dgvCustomer.Columns.Contains("Waktu_Reservasi"))
            {
                dgvCustomer.Columns["Waktu_Reservasi"].HeaderText = "Waktu";
                dgvCustomer.Columns["Waktu_Reservasi"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            }
            if (dgvCustomer.Columns.Contains("Nomor_Meja")) dgvCustomer.Columns["Nomor_Meja"].HeaderText = "Meja";
        }

        private void btnCustHapus_Click(object sender, EventArgs e)
        {
            // 1. Validasi: Pastikan koneksi siap dan ada baris yang dipilih
            if (!IsConnectionReady() || dgvCustomer.SelectedRows.Count == 0)
            {
                if (dgvCustomer.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Pilih data reservasi yang ingin Anda hapus dari tabel.", "Pilih Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }

            // 2. Ambil ID dari baris yang dipilih dengan aman
            object idReservasiObj = dgvCustomer.SelectedRows[0].Cells["ID_Reservasi"].Value;
            if (idReservasiObj == null || idReservasiObj == DBNull.Value)
            {
                MessageBox.Show("ID Reservasi tidak valid pada baris yang dipilih.", "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int idReservasi = Convert.ToInt32(idReservasiObj);

            // 3. Minta konfirmasi dari pengguna sebelum menghapus
            DialogResult dr = MessageBox.Show($"Apakah Anda yakin ingin menghapus reservasi dengan ID {idReservasi}?",
                                             "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Jika pengguna memilih "No", batalkan proses
            if (dr == DialogResult.No)
            {
                return;
            }

            // 4. Lakukan operasi database
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Pastikan nama Stored Procedure 'HapusReservasiCustomer' sudah benar
                using (SqlCommand cmd = new SqlCommand("HapusReservasiCustomer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);

                    // [Implementasi Solusi 3]
                    // Eksekusi perintah tanpa memeriksa nilai kembaliannya.
                    // Jika terjadi error, program akan langsung loncat ke blok 'catch'.
                    cmd.ExecuteNonQuery();

                    // Jika tidak ada error, langsung tampilkan pesan sukses dan segarkan UI.
                    MessageBox.Show("Reservasi berhasil dihapus.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Segarkan data dan form
                    InvalidateReservasiCache();
                    InvalidateAvailableMejaCache();
                    LoadReservasi();
                    LoadComboBoxMeja(cmbCustMeja);
                    ClearForm();
                }
            }
            catch (SqlException sqlEx)
            {
                // Tangkap error spesifik dari SQL Server
                MessageBox.Show("Gagal menghapus reservasi: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Tangkap error umum lainnya
                MessageBox.Show("Terjadi kesalahan saat menghapus reservasi: " + ex.Message, "Error Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 5. Pastikan koneksi selalu ditutup setelah operasi selesai
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCustomer.Rows.Count && !dgvCustomer.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow row = dgvCustomer.Rows[e.RowIndex];
                txtCustNama.Text = row.Cells["Nama_Customer"].Value?.ToString();
                txtCustNoTelp.Text = row.Cells["No_Telp"].Value?.ToString();

                if (row.Cells["Waktu_Reservasi"].Value != null && row.Cells["Waktu_Reservasi"].Value != DBNull.Value)
                {
                    DateTime waktuDB = Convert.ToDateTime(row.Cells["Waktu_Reservasi"].Value);
                    dtpCustWaktu.MinDate = new DateTime(1753, 1, 1); // Set min/max for safety
                    dtpCustWaktu.MaxDate = new DateTime(9998, 12, 31);
                    dtpCustWaktu.Value = (waktuDB < dtpCustWaktu.MinDate) ? dtpCustWaktu.MinDate : (waktuDB > dtpCustWaktu.MaxDate ? dtpCustWaktu.MaxDate : waktuDB);
                }
                else
                {
                    dtpCustWaktu.Value = DateTime.Now;
                }

                // Set ComboBox text. If the table is booked, it might not be in the 'Tersedia' list.
                // This approach is fine for display. If editing were allowed, more complex ComboBox handling would be needed.
                string nomorMejaDipesan = row.Cells["Nomor_Meja"].Value?.ToString();
                cmbCustMeja.Text = nomorMejaDipesan; // Ini akan menampilkan nomor meja, meskipun mungkin tidak ada di daftar dropdown jika meja itu sudah tidak 'Tersedia'


                if (row.Cells["ID_Reservasi"].Value != null && row.Cells["ID_Reservasi"].Value != DBNull.Value)
                {
                    this.selectedReservasiID = Convert.ToInt32(row.Cells["ID_Reservasi"].Value);
                }
                else
                {
                    this.selectedReservasiID = null;
                }
            }
        }

        private void ClearForm()
        {
            txtCustNama.Clear();
            txtCustNoTelp.Clear();
            cmbCustMeja.SelectedIndex = -1;
            cmbCustMeja.Text = "";
            dtpCustWaktu.Value = DateTime.Now;
            txtCustNama.Focus();
            this.selectedReservasiID = null; // Reset selected ID
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                new LoginPage().Show();
                this.Close();
            }
        }

        private bool IsConnectionReady()
        {
            if (this.conn == null || string.IsNullOrWhiteSpace(this.connectionString))
            {
                MessageBox.Show("Koneksi database tidak diinisialisasi dengan benar. Silakan restart aplikasi atau hubungi administrator.", "Error Koneksi Kritis", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Periksa koneksi terlebih dahulu
            if (!IsConnectionReady())
            {
                return;
            }

            try
            {
                // Untuk memberikan feedback visual, ubah cursor menjadi ikon loading
                this.Cursor = Cursors.WaitCursor;

                // 1. Hapus cache agar data diambil langsung dari database
                InvalidateReservasiCache();
                InvalidateAvailableMejaCache();

                // 2. Panggil kembali metode untuk memuat data
                LoadReservasi();        // Memuat ulang data ke tabel DataGridView
                LoadComboBoxMeja(cmbCustMeja); // Memuat ulang daftar meja yang tersedia

                // 3. Bersihkan semua kolom input
                ClearForm();

              
                MessageBox.Show("Data berhasil diperbarui.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Tangani jika ada error saat proses refresh
                MessageBox.Show("Terjadi kesalahan saat menyegarkan data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Kembalikan cursor ke bentuk semula, baik proses berhasil maupun gagal
                this.Cursor = Cursors.Default;
            }
        }
    }
}