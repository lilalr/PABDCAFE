using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Caching;
using DocumentFormat.OpenXml.Office.Word;

namespace PABDCAFE
{
    public partial class CustomerPage : Form
    {
        private readonly string connectionString;

        // Caching fields
        private readonly MemoryCache _reservasiCache = MemoryCache.Default;
        private const string ReservasiCacheKey = "CustomerPageReservasiData";
        private readonly MemoryCache _availableMejaCache = MemoryCache.Default;
        private const string AvailableMejaCacheKey = "CustomerPageAvailableMejaList";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public CustomerPage(string connStr)
        {
            InitializeComponent();

            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak valid.", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException(nameof(connStr));
            }
            this.connectionString = connStr;

            dtpCustWaktu.Format = DateTimePickerFormat.Custom;
            dtpCustWaktu.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpCustWaktu.ShowUpDown = true;
        }

        public CustomerPage()
        {
            InitializeComponent();
            MessageBox.Show("CustomerPage dibuat tanpa string koneksi.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            this.connectionString = null;
        }

        private void CustomerPage_Load(object sender, EventArgs e)
        {
            if (!IsConnectionReady())
            {
                foreach (Control c in this.Controls) { c.Enabled = false; }
                return;
            }

            EnsureIndexes();
            LoadAllReservasi();
            LoadComboBoxMeja(cmbCustMeja);
            ClearForm();
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
                        -- Indeks untuk pencarian berdasarkan Nomor_Meja dan Waktu_Reservasi
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Meja_Waktu' AND object_id = OBJECT_ID('dbo.Reservasi'))
                            CREATE NONCLUSTERED INDEX IX_Reservasi_Meja_Waktu ON dbo.Reservasi(Nomor_Meja, Waktu_Reservasi);

                        -- Indeks untuk pencarian Nama_Customer
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Nama_Customer' AND object_id = OBJECT_ID('dbo.Reservasi'))
                            CREATE NONCLUSTERED INDEX IX_Reservasi_Nama_Customer ON dbo.Reservasi(Nama_Customer);
                    END

                    -- Memastikan tabel Meja ada
                    IF OBJECT_ID('dbo.Meja', 'U') IS NOT NULL
                    BEGIN
                        -- Indeks untuk mempercepat pengambilan Nomor_Meja
                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Meja_Nomor_Meja' AND object_id = OBJECT_ID('dbo.Meja'))
                            CREATE NONCLUSTERED INDEX IX_Meja_Nomor_Meja ON dbo.Meja(Nomor_Meja);
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


        private bool IsConnectionReady()
        {
            if (string.IsNullOrWhiteSpace(this.connectionString))
            {
                MessageBox.Show("Koneksi database tidak diinisialisasi.", "Error Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void InvalidateAllCaches()
        {
            _reservasiCache.Remove(ReservasiCacheKey);
            _availableMejaCache.Remove(AvailableMejaCacheKey);
        }

        private void LoadAllReservasi()
        {
            if (!IsConnectionReady()) return;

            DataTable dt = _reservasiCache.Get(ReservasiCacheKey) as DataTable;

            if (dt == null)
            {
                dt = new DataTable();
                try
                {
                    string query = "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi ORDER BY Waktu_Reservasi DESC";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, this.connectionString))
                    {
                        da.Fill(dt);
                    }
                    _reservasiCache.Set(ReservasiCacheKey, dt, DateTimeOffset.Now.Add(_cacheDuration));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menampilkan data reservasi: " + ex.Message, "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            dgvCustomer.DataSource = dt;
            if (dgvCustomer.Columns.Count > 0)
            {
                if (dgvCustomer.Columns.Contains("ID_Reservasi")) dgvCustomer.Columns["ID_Reservasi"].HeaderText = "ID";
                if (dgvCustomer.Columns.Contains("Nama_Customer")) dgvCustomer.Columns["Nama_Customer"].HeaderText = "Nama";
                if (dgvCustomer.Columns.Contains("No_Telp")) dgvCustomer.Columns["No_Telp"].HeaderText = "No. Telepon";
                if (dgvCustomer.Columns.Contains("Waktu_Reservasi"))
                {
                    dgvCustomer.Columns["Waktu_Reservasi"].HeaderText = "Waktu";
                    dgvCustomer.Columns["Waktu_Reservasi"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                }
                if (dgvCustomer.Columns.Contains("Nomor_Meja")) dgvCustomer.Columns["Nomor_Meja"].HeaderText = "Meja";
            }
        }

        private void LoadComboBoxMeja(ComboBox cmb)
        {
            if (!IsConnectionReady()) return;

            List<string> mejaList = _availableMejaCache.Get(AvailableMejaCacheKey) as List<string>;
            if (mejaList == null)
            {
                mejaList = new List<string>();
                try
                {
                    using (var conn = new SqlConnection(this.connectionString))
                    using (var cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja ORDER BY Nomor_Meja", conn))
                    {
                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                mejaList.Add(reader["Nomor_Meja"].ToString());
                            }
                        }
                    }
                    _availableMejaCache.Set(AvailableMejaCacheKey, mejaList, DateTimeOffset.Now.Add(_cacheDuration));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat daftar meja: " + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            cmb.DataSource = mejaList;
            cmb.SelectedIndex = -1;
        }

        private bool ValidasiInput(out string err)
        {
            if (string.IsNullOrWhiteSpace(txtCustNama.Text) &&
                string.IsNullOrWhiteSpace(txtCustNoTelp.Text) &&
                string.IsNullOrWhiteSpace(cmbCustMeja.Text))
            {
                err = "Field wajib diisi";
                return false;
            }

            var errorMessages = new List<string>();

            if (string.IsNullOrWhiteSpace(txtCustNama.Text))
                errorMessages.Add("Nama customer tidak boleh kosong.");
            else if (!Regex.IsMatch(txtCustNama.Text, @"^[a-zA-Z\s]+$"))
                errorMessages.Add("Nama customer tidak boleh mengandung angka dan simbol.");

            if (string.IsNullOrWhiteSpace(txtCustNoTelp.Text))
                errorMessages.Add("Nomor telepon tidak boleh kosong.");
            else if (!Regex.IsMatch(txtCustNoTelp.Text.Trim(), @"^(\+62\d{8,12}|0\d{9,14})$"))
                errorMessages.Add("Format nomor telepon tidak sesuai (Contoh: 081234567890).");

            if (dtpCustWaktu.Value < DateTime.Now.AddMinutes(-5))
                errorMessages.Add("Waktu reservasi tidak boleh di masa lalu.");

            if (string.IsNullOrWhiteSpace(cmbCustMeja.Text))
                errorMessages.Add("Nomor meja tidak boleh kosong.");
            else if (!cmbCustMeja.Items.Contains(cmbCustMeja.Text))
                errorMessages.Add("Nomer meja yang valid harus dipilih.");

            err = string.Join("\n", errorMessages);
            return !errorMessages.Any();
        }

        private bool CekJadwalBentrok(string nomorMeja, DateTime waktu)
        {
            // Query untuk memeriksa reservasi pada rentang satu hari penuh (dari jam 00:00 hingga 23:59)
            string query = "SELECT COUNT(*) FROM Reservasi WHERE Nomor_Meja = @NomorMeja AND Waktu_Reservasi BETWEEN @StartOfDay AND @EndOfDay";

            using (var tempConn = new SqlConnection(this.connectionString))
            using (var cmd = new SqlCommand(query, tempConn))
            {
                // Mendefinisikan awal hari (contoh: 09/11/2025 00:00:00)
                DateTime startOfDay = waktu.Date;

                // Mendefinisikan akhir hari (contoh: 09/11/2025 23:59:59)
                DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1);

                // Menambahkan parameter ke query
                cmd.Parameters.AddWithValue("@NomorMeja", nomorMeja);
                cmd.Parameters.AddWithValue("@StartOfDay", startOfDay);
                cmd.Parameters.AddWithValue("@EndOfDay", endOfDay);

                tempConn.Open();
                int count = (int)cmd.ExecuteScalar();

                // Jika count > 0, berarti sudah ada reservasi di tanggal tersebut.
                return count > 0;
            }
        }

        private void btnCustTambah_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput(out string errorMsg))
            {
                MessageBox.Show(errorMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime waktuReservasi = dtpCustWaktu.Value;
            string selectedMeja = cmbCustMeja.Text;

            /*
            if (CekJadwalBentrok(selectedMeja, waktuReservasi))
            {
                MessageBox.Show($"Meja '{selectedMeja}' sudah dipesan untuk tanggal {waktuReservasi:dd-MM-yyyy}.", "Jadwal Bentrok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            */

            try
            {
                using (var conn = new SqlConnection(this.connectionString))
                using (var cmd = new SqlCommand("TambahReservasiCustomer", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtCustNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtCustNoTelp.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", dtpCustWaktu.Value);
                    cmd.Parameters.AddWithValue("@Nomor_Meja", cmbCustMeja.Text);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Reservasi berhasil ditambahkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                InvalidateAllCaches();
                LoadAllReservasi();
                ClearForm();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Terjadi error database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Tangkap error umum lainnya
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan umum: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCustHapus_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data reservasi yang ingin Anda batalkan.", "Pilih Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string namaDiGrid = dgvCustomer.SelectedRows[0].Cells["Nama_Customer"].Value.ToString();
            string telpDiGrid = dgvCustomer.SelectedRows[0].Cells["No_Telp"].Value.ToString();
            string namaDiForm = txtCustNama.Text.Trim();
            string telpDiForm = txtCustNoTelp.Text.Trim();

            if (string.IsNullOrWhiteSpace(namaDiForm) || string.IsNullOrWhiteSpace(telpDiForm))
            {
                MessageBox.Show("Untuk membatalkan, silakan isi nama dan nomor telepon Anda terlebih dahulu agar kami bisa memverifikasi.", "Verifikasi Diperlukan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (namaDiGrid.ToLower() != namaDiForm.ToLower() || telpDiGrid != telpDiForm)
            {
                MessageBox.Show("Anda hanya dapat membatalkan reservasi milik Anda sendiri.\nPastikan nama dan nomor telepon yang Anda masukkan sesuai dengan data reservasi yang dipilih.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            int idReservasi = Convert.ToInt32(dgvCustomer.SelectedRows[0].Cells["ID_Reservasi"].Value);
            DialogResult dr = MessageBox.Show($"Apakah Anda yakin ingin membatalkan reservasi ini?", "Konfirmasi Pembatalan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes)
            {
                try
                {
                    using (var conn = new SqlConnection(this.connectionString))
                    using (var cmd = new SqlCommand("HapusReservasi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Reservasi berhasil dibatalkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InvalidateAllCaches();
                    LoadAllReservasi();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal membatalkan reservasi: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- LOGIKA DIPERBAIKI DI SINI ---
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvCustomer.Rows.Count)
            {
                DataGridViewRow row = dgvCustomer.Rows[e.RowIndex];
                txtCustNama.Text = row.Cells["Nama_Customer"].Value?.ToString();
                txtCustNoTelp.Text = row.Cells["No_Telp"].Value?.ToString();

                if (DateTime.TryParse(row.Cells["Waktu_Reservasi"].Value?.ToString(), out DateTime waktuDB))
                {
                    // Cek apakah tanggal dari DB lebih kecil dari MinDate picker
                    if (waktuDB < dtpCustWaktu.MinDate)
                    {
                        // Jika ya, atur picker ke MinDate untuk menghindari error
                        dtpCustWaktu.Value = dtpCustWaktu.MinDate;
                    }
                    else
                    {
                        // Jika tidak, atur ke tanggal dari DB
                        dtpCustWaktu.Value = waktuDB;
                    }
                }
                else
                {
                    // Fallback jika data tanggal tidak valid
                    dtpCustWaktu.Value = DateTime.Now;
                }
                cmbCustMeja.Text = row.Cells["Nomor_Meja"].Value?.ToString();
            }
        }

        private void ClearForm()
        {
            txtCustNama.Clear();
            txtCustNoTelp.Clear();
            cmbCustMeja.SelectedIndex = -1;
            cmbCustMeja.Text = "";
            dtpCustWaktu.Value = DateTime.Now;
            dgvCustomer.ClearSelection();
            txtCustNama.Focus();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            InvalidateAllCaches();
            LoadAllReservasi();
            LoadComboBoxMeja(cmbCustMeja);
            ClearForm();
            this.Cursor = Cursors.Default;
            MessageBox.Show("Data berhasil diperbarui.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
