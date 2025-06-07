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
                            CREATE NON CLUSTERED INDEX IX_Reservasi_Nomor_Meja ON dbo.Reservasi(Nomor_Meja);

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_Waktu')
                            CREATE NON CLUSTERED INDEX IX_Reservasi_Waktu ON dbo.Reservasi(Waktu_Reservasi);

                        IF NOT EXISTS (SELECT 1 FROM sys,indexes WHERE name = 'IX_Reservasi_Nama_Customer')
                            CREATE NON CLUSTERED INDEX IX_Reservasi_Nama_Customer ON dbo.Reservasi(Nama_Customer);

                        IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Reservasi_No_Telp')
                            CREATE NON CLUSTERED INDEX IX_Reservasi_No_Telp ON dbo.Reservasi(No_Telp);
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

            List<string> mejaList = _availableMejaCache.Get(AvailableMejaCacheKey) as List<string>;

            if (mejaList == null)
            {
                // System.Diagnostics.Debug.WriteLine("Loading available tables from DB for CustomerPage");
                mejaList = new List<string>();
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja WHERE Status_Meja = 'Tersedia' ORDER BY Nomor_Meja", conn))
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
                    MessageBox.Show("Gagal memuat daftar meja yang tersedia: " + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
            // else { /* System.Diagnostics.Debug.WriteLine("Loading available tables from Cache for CustomerPage"); */ }


            cmb.DataSource = null;
            cmb.Items.Clear();
            cmb.DataSource = mejaList; // Langsung set DataSource dengan list dari cache atau DB
            if (mejaList.Count > 0)
            {
                cmb.SelectedIndex = -1;
            }
            else
            {
                // Jika DataSource kosong dan ingin menampilkan placeholder, harus lewat Items bukan DataSource
                cmb.DataSource = null; // Hapus DataSource jika ingin pakai Items untuk placeholder
                cmb.Items.Add("Tidak ada meja tersedia");
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

            if (string.IsNullOrWhiteSpace(nama) || nama.Length < 3)
            {
                MessageBox.Show("Nama customer tidak boleh kosong dan minimal 3 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!Regex.IsMatch(telp, @"^(\+62\d{8,12}|0\d{9,14})$"))
            {
                MessageBox.Show("Nomor telepon tidak valid. Contoh: +6281234567890 atau 081234567890.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (waktu < DateTime.Now.AddMinutes(-1))
            {
                MessageBox.Show("Waktu reservasi tidak boleh di masa lalu.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (waktu.Year != 2025) // Sesuai aturan bisnis di kode asli
            {
                MessageBox.Show("Reservasi hanya boleh di tahun 2025.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(meja) || meja == "Tidak ada meja tersedia")
            {
                MessageBox.Show("Silakan pilih nomor meja yang tersedia.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Cek status meja
                using (SqlCommand cekMejaCmd = new SqlCommand("SELECT Status_Meja FROM Meja WHERE Nomor_Meja = @NomorMeja", conn))
                {
                    cekMejaCmd.Parameters.AddWithValue("@NomorMeja", meja);
                    object statusMejaObj = cekMejaCmd.ExecuteScalar();
                    if (statusMejaObj == null)
                    {
                        MessageBox.Show($"Nomor meja '{meja}' tidak ditemukan di database.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    if (statusMejaObj.ToString() != "Tersedia")
                    {
                        MessageBox.Show($"Meja '{meja}' saat ini statusnya '{statusMejaObj.ToString()}', bukan 'Tersedia'. Silakan refresh daftar meja.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        InvalidateAvailableMejaCache(); // Penting untuk invalidasi cache meja
                        LoadComboBoxMeja(cmbCustMeja);
                        return false;
                    }
                }

                // Cek jadwal reservasi
                // Untuk Edit, kita harus mengecualikan reservasi yang sedang diedit
                string queryCekJadwal = "SELECT COUNT(*) FROM Reservasi WHERE Nomor_Meja = @NomorMeja AND Waktu_Reservasi = @WaktuReservasi";
                if (this.selectedReservasiID.HasValue) // Jika sedang mode edit (asumsi)
                {
                    // queryCekJadwal += " AND ID_Reservasi <> @CurrentReservasiID"; // Logika ini belum ada di CustomerPage
                }

                using (SqlCommand cekJadwalCmd = new SqlCommand(queryCekJadwal, conn))
                {
                    cekJadwalCmd.Parameters.AddWithValue("@NomorMeja", meja);
                    cekJadwalCmd.Parameters.AddWithValue("@WaktuReservasi", waktu);
                    // if (this.selectedReservasiID.HasValue) {
                    //    cekJadwalCmd.Parameters.AddWithValue("@CurrentReservasiID", this.selectedReservasiID.Value);
                    // }
                    int countReservasi = (int)cekJadwalCmd.ExecuteScalar();
                    if (countReservasi > 0)
                    {
                        MessageBox.Show($"Meja '{meja}' sudah direservasi pada waktu tersebut. Silakan pilih waktu atau meja lain.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            if (!IsConnectionReady() || !ValidasiInput()) return;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Menggunakan Stored Procedure 'TambahReservasi'
                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtCustNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtCustNoTelp.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", dtpCustWaktu.Value);
                    cmd.Parameters.AddWithValue("@Nomor_Meja", cmbCustMeja.SelectedItem.ToString());

                    int result = cmd.ExecuteNonQuery(); // SP biasanya mengembalikan rows affected atau nilai tertentu

                    MessageBox.Show(result >= 0 ? "Reservasi berhasil ditambahkan." : "Reservasi gagal ditambahkan.", // Perbaikan: SP TambahReservasi mungkin tidak mengembalikan rows affected jika ada trigger
                                    result >= 0 ? "Sukses" : "Gagal", MessageBoxButtons.OK,
                                    result >= 0 ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                    if (result >= 0) // Jika sukses (atau tidak ada error dari SP)
                    {
                        InvalidateReservasiCache();
                        InvalidateAvailableMejaCache();
                        LoadReservasi();
                        LoadComboBoxMeja(cmbCustMeja);
                        ClearForm();
                    }
                }
            }
            catch (SqlException sqlEx) // Tangkap error spesifik dari SQL Server (misal, dari trigger atau constraint)
            {
                MessageBox.Show("Gagal menambahkan reservasi: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menambahkan reservasi: " + ex.Message, "Error Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
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
            if (!IsConnectionReady() || dgvCustomer.SelectedRows.Count == 0)
            {
                if (dgvCustomer.SelectedRows.Count == 0)
                    MessageBox.Show("Pilih data reservasi yang ingin Anda hapus dari tabel.", "Pilih Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            object idReservasiObj = dgvCustomer.SelectedRows[0].Cells["ID_Reservasi"].Value;
            if (idReservasiObj == null || idReservasiObj == DBNull.Value)
            {
                MessageBox.Show("ID Reservasi tidak valid pada baris yang dipilih.", "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int idReservasi = Convert.ToInt32(idReservasiObj);

            DialogResult dr = MessageBox.Show($"Apakah Anda yakin ingin menghapus reservasi dengan ID {idReservasi}?",
                                              "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // Menggunakan Stored Procedure 'HapusReservasi'
                using (SqlCommand cmd = new SqlCommand("HapusReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);

                    int result = cmd.ExecuteNonQuery();
                    MessageBox.Show(result > 0 ? "Reservasi berhasil dihapus." : "Reservasi tidak ditemukan atau gagal dihapus.",
                                    result > 0 ? "Sukses" : "Informasi", MessageBoxButtons.OK,
                                    result > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    if (result > 0)
                    {
                        InvalidateReservasiCache();
                        InvalidateAvailableMejaCache();
                        LoadReservasi();
                        LoadComboBoxMeja(cmbCustMeja);
                        ClearForm();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Gagal menghapus reservasi: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menghapus reservasi: " + ex.Message, "Error Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
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
    }
}