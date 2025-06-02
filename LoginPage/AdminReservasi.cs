using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO; // Diperlukan untuk operasi File seperti File.ReadAllLines
using System.Collections.Generic; // Diperlukan untuk List<T>
using System.Runtime.Caching; // Diperlukan untuk Caching

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

        public AdminReservasi(string connStr)
        {
            InitializeComponent();

            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak ada atau kosong. Tidak dapat terhubung ke database.", "Kesalahan Konfigurasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException(nameof(connStr), "String koneksi database (connStr) tidak boleh null atau kosong.");
            }
            this.connectionString = connStr;
            this.conn = new SqlConnection(this.connectionString);

            SetupDateTimePicker();

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
        }

        private void InvalidateReservasiDataCache()
        {
            _reservasiDataCache.Remove(ReservasiDataCacheKey);
            // System.Diagnostics.Debug.WriteLine("Cache ReservasiData invalidated.");
        }

        private void InvalidateAvailableMejaCache()
        {
            _availableMejaCache.Remove(AvailableMejaCacheKey);
            // System.Diagnostics.Debug.WriteLine("Cache AvailableMeja invalidated.");
        }

        private void SetupDateTimePicker()
        {
            // Asumsikan dtpWaktuReservasi adalah nama kontrol DateTimePicker di designer
            if (this.dtpWaktuReservasi != null)
            {
                this.dtpWaktuReservasi.Format = DateTimePickerFormat.Custom;
                this.dtpWaktuReservasi.CustomFormat = "yyyy-MM-dd HH:mm";
                this.dtpWaktuReservasi.Value = DateTime.Now;
            }
        }

        private void LoadAvailableMeja(ComboBox cbx)
        {
            List<string> mejaList = _availableMejaCache.Get(AvailableMejaCacheKey) as List<string>;

            if (mejaList == null) // Jika tidak ada di cache, ambil dari database
            {
                if (this.conn == null)
                {
                    MessageBox.Show("Koneksi database belum diinisialisasi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Inisialisasi mejaList sebagai list kosong agar tidak null di bagian selanjutnya
                    mejaList = new List<string>();
                }
                else // Koneksi ada, coba ambil data
                {
                    mejaList = new List<string>(); // Inisialisasi di sini untuk menampung hasil DB
                    try
                    {
                        if (this.conn.State == ConnectionState.Closed)
                        {
                            this.conn.Open();
                        }

                        using (SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja WHERE Status_Meja = 'Tersedia' ORDER BY Nomor_Meja", this.conn))
                        {
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                mejaList.Add(reader["Nomor_Meja"].ToString());
                            }
                            reader.Close();
                            // Simpan ke cache hanya jika berhasil mengambil data
                            if (mejaList.Count > 0)
                            {
                                _availableMejaCache.Set(AvailableMejaCacheKey, mejaList, DateTimeOffset.Now.Add(_cacheDuration));
                            }
                            // System.Diagnostics.Debug.WriteLine("Available Meja loaded from DB and cached.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal memuat daftar meja yang tersedia: " + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // mejaList sudah diinisialisasi sebagai list kosong di atas blok try
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
            // else
            // {
            //     // System.Diagnostics.Debug.WriteLine("Available Meja loaded from cache.");
            // }

            // Pastikan cbx merujuk ke kontrol yang valid
            if (cbx == null) return;

            // Reset ComboBox
            cbx.DataSource = null;
            cbx.Items.Clear();

            // Setelah mejaList didapatkan (dari cache atau DB, atau list kosong jika ada error)
            if (mejaList != null && mejaList.Count > 0)
            {
                // Hanya set DataSource jika ada item di mejaList
                cbx.DataSource = mejaList;
                cbx.SelectedIndex = -1; // Tidak ada item yang dipilih secara default
            }
            else
            {
                // Jika mejaList kosong atau null, DataSource tetap null.
                // Tambahkan placeholder secara manual ke Items collection.
                cbx.Items.Add("Tidak ada meja tersedia");
                if (cbx.Items.Count > 0) // Pastikan item berhasil ditambahkan
                {
                    cbx.SelectedIndex = 0; // Pilih item placeholder
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
                        // System.Diagnostics.Debug.WriteLine("Reservasi Data loaded from DB and cached.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data: " + ex.Message, "Kesalahan Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // else
            // {
            //    // System.Diagnostics.Debug.WriteLine("Reservasi Data loaded from cache.");
            // }

            // Asumsikan dgvAdminReservasi adalah nama kontrol DataGridView di designer
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

        private void AdminReservasi_Load(object sender, EventArgs e)
        {
            // Setup sudah di konstruktor
        }

        void ClearForm()
        {
            // Asumsikan txtNama, txtTelepon adalah nama kontrol TextBox di designer
            if (this.txtNama != null) this.txtNama.Clear();
            if (this.txtTelepon != null) this.txtTelepon.Clear();

            if (this.dtpWaktuReservasi != null)
            {
                this.dtpWaktuReservasi.Value = DateTime.Now;
            }

            if (this.cbxNomorMeja != null)
            {
                this.cbxNomorMeja.SelectedIndex = -1;
                InvalidateAvailableMejaCache(); // Invalidate cache sebelum load ulang
                LoadAvailableMeja(this.cbxNomorMeja);
            }

            if (this.dgvAdminReservasi != null) this.dgvAdminReservasi.ClearSelection();
            if (this.txtNama != null) this.txtNama.Focus();
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
                if (this.dtpWaktuReservasi.Value.Year != 2025) // Sesuai constraint di kode asli
                {
                    err += "Waktu reservasi hanya diperbolehkan untuk tahun 2025.\n";
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;
            if (this.dgvAdminReservasi == null || this.dgvAdminReservasi.CurrentRow == null || this.dgvAdminReservasi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih data reservasi yang ingin diedit dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idReservasi = Convert.ToInt32(this.dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value);
                DateTime waktuReservasi = this.dtpWaktuReservasi.Value;
                string selectedMeja = this.cbxNomorMeja.SelectedItem.ToString();

                if (conn.State == ConnectionState.Closed) conn.Open();

                using (SqlCommand cmd = new SqlCommand("EditReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    cmd.Parameters.AddWithValue("@Nama_Customer", this.txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", this.txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja_Baru", selectedMeja);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil diperbarui!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvalidateReservasiDataCache();
                        InvalidateAvailableMejaCache(); // Status meja mungkin berubah
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak diperbarui atau tidak ada perubahan data.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Kesalahan SQL: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
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

            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DateTime waktuReservasi = this.dtpWaktuReservasi.Value;
                string selectedMeja = this.cbxNomorMeja.SelectedItem.ToString();

                if (conn.State == ConnectionState.Closed) conn.Open();

                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Customer", this.txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", this.txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja", selectedMeja);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Reservasi berhasil ditambahkan!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InvalidateReservasiDataCache();
                    InvalidateAvailableMejaCache(); // Meja yg baru dipesan tidak lagi tersedia
                    LoadData();
                    ClearForm();
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Gagal menambahkan reservasi: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;
            if (this.dgvAdminReservasi == null || this.dgvAdminReservasi.CurrentRow == null || this.dgvAdminReservasi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih data reservasi yang ingin dihapus dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) return;

            try
            {
                int idReservasi = Convert.ToInt32(this.dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value);

                if (conn.State == ConnectionState.Closed) conn.Open();

                using (SqlCommand cmd = new SqlCommand("HapusReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservasi berhasil dihapus.", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        InvalidateReservasiDataCache();
                        InvalidateAvailableMejaCache(); // Meja yg dihapus reservasi nya jadi tersedia lagi
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau gagal dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Gagal menghapus data: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus data: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close();
            }
        }

        private void dgvAdminReservasi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvAdminReservasi == null || e.RowIndex < 0 || e.RowIndex >= this.dgvAdminReservasi.Rows.Count || this.dgvAdminReservasi.Rows[e.RowIndex].IsNewRow)
                return;

            DataGridViewRow row = this.dgvAdminReservasi.Rows[e.RowIndex];

            if (this.txtNama != null) this.txtNama.Text = row.Cells["Nama_Customer"].Value?.ToString() ?? string.Empty;
            if (this.txtTelepon != null) this.txtTelepon.Text = row.Cells["No_Telp"].Value?.ToString() ?? string.Empty;

            if (this.dtpWaktuReservasi != null)
            {
                this.dtpWaktuReservasi.MinDate = new DateTime(1753, 1, 1);
                this.dtpWaktuReservasi.MaxDate = new DateTime(9998, 12, 31);

                if (row.Cells["Waktu_Reservasi"].Value != null && row.Cells["Waktu_Reservasi"].Value != DBNull.Value)
                {
                    DateTime nilai = Convert.ToDateTime(row.Cells["Waktu_Reservasi"].Value);
                    this.dtpWaktuReservasi.Value = (nilai < this.dtpWaktuReservasi.MinDate) ? this.dtpWaktuReservasi.MinDate : (nilai > this.dtpWaktuReservasi.MaxDate ? this.dtpWaktuReservasi.MaxDate : nilai);
                }
                else
                {
                    this.dtpWaktuReservasi.Value = DateTime.Now;
                }
            }

            if (this.cbxNomorMeja != null)
            {
                string selectedMejaFromGrid = row.Cells["Nomor_Meja"].Value?.ToString() ?? string.Empty;

                // Dapatkan daftar item saat ini dari ComboBox (bisa dari cache atau yang baru di-load)
                List<string> currentDisplayItems = new List<string>();
                if (this.cbxNomorMeja.DataSource is List<string> ds)
                {
                    currentDisplayItems.AddRange(ds);
                }
                else // Fallback jika DataSource bukan List<string> (misalnya diisi manual atau Items)
                {
                    foreach (var item in this.cbxNomorMeja.Items) currentDisplayItems.Add(item.ToString());
                }

                // Jika meja dari grid tidak ada di daftar, tambahkan sementara untuk display edit
                if (!string.IsNullOrEmpty(selectedMejaFromGrid) && !currentDisplayItems.Contains(selectedMejaFromGrid))
                {
                    List<string> tempItems = new List<string>(currentDisplayItems);
                    tempItems.Add(selectedMejaFromGrid);
                    tempItems.Sort(); // Opsional

                    this.cbxNomorMeja.DataSource = null;
                    this.cbxNomorMeja.Items.Clear();
                    this.cbxNomorMeja.DataSource = tempItems; // Set DataSource baru dengan item tambahan
                }
                // Pilih item yang sesuai
                this.cbxNomorMeja.SelectedItem = selectedMejaFromGrid;
                if (this.cbxNomorMeja.SelectedItem == null && !string.IsNullOrEmpty(selectedMejaFromGrid))
                {
                    // Jika setelah set DataSource itemnya tidak terpilih (misal karena DataSource tidak langsung update Items), coba via Items
                    if (!this.cbxNomorMeja.Items.Contains(selectedMejaFromGrid)) this.cbxNomorMeja.Items.Add(selectedMejaFromGrid); // Pastikan ada
                    this.cbxNomorMeja.SelectedItem = selectedMejaFromGrid;
                }
                else if (string.IsNullOrEmpty(selectedMejaFromGrid))
                {
                    this.cbxNomorMeja.SelectedIndex = -1;
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return;

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                Title = "Pilih File CSV untuk Impor"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                int successCount = 0;
                int failCount = 0;
                List<string> errorDetails = new List<string>();

                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    if (lines.Length == 0)
                    {
                        MessageBox.Show("File CSV kosong.", "Informasi Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (conn.State == ConnectionState.Closed) conn.Open();

                    // Jika baris pertama adalah header, ubah i = 0 menjadi i = 1 untuk melewatinya.
                    // Diasumsikan CSV tidak memiliki header, atau header diproses sebagai data.
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] data = line.Split(',');
                        if (data.Length >= 4) // Nama, Telp, Waktu, Meja
                        {
                            try
                            {
                                string namaCustomer = data[0].Trim();
                                string noTelp = data[1].Trim();
                                string waktuReservasiStr = data[2].Trim();
                                string nomorMeja = data[3].Trim();
                                DateTime waktuReservasi;

                                if (!DateTime.TryParseExact(waktuReservasiStr, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out waktuReservasi) &&
                                    !DateTime.TryParse(waktuReservasiStr, out waktuReservasi))
                                {
                                    failCount++;
                                    errorDetails.Add($"Baris {i + 1}: Format waktu '{waktuReservasiStr}' tidak valid. Data: {line}");
                                    continue;
                                }

                                string csvRowError = "";
                                if (string.IsNullOrWhiteSpace(namaCustomer)) csvRowError += "Nama kosong. ";
                                if (!Regex.IsMatch(noTelp, @"^(\+62\d{8,12}|0\d{9,14})$")) csvRowError += "Format telepon salah. ";
                                if (waktuReservasi < DateTime.Now.AddMinutes(-1)) csvRowError += "Waktu di masa lalu. ";
                                if (waktuReservasi.Year != 2025) csvRowError += "Tahun harus 2025. ";
                                if (string.IsNullOrWhiteSpace(nomorMeja) || !Regex.IsMatch(nomorMeja, @"^\d{2}$")) csvRowError += "Format Nomor Meja salah (harus 2 digit angka). ";

                                if (!string.IsNullOrEmpty(csvRowError))
                                {
                                    failCount++;
                                    errorDetails.Add($"Baris {i + 1}: Validasi gagal - {csvRowError.Trim()} Data: {line}");
                                    continue;
                                }

                                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Nama_Customer", namaCustomer);
                                    cmd.Parameters.AddWithValue("@No_Telp", noTelp); // BUG FIX: Added
                                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi); // BUG FIX: Added
                                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja); // BUG FIX: Added
                                    cmd.ExecuteNonQuery();
                                    successCount++;
                                }
                            }
                            catch (SqlException sqlExInner)
                            {
                                failCount++;
                                errorDetails.Add($"Baris {i + 1}: Kesalahan SQL - {sqlExInner.Message.Split('\n')[0]}. Data: {line}");
                            }
                            catch (Exception exInner)
                            {
                                failCount++;
                                errorDetails.Add($"Baris {i + 1}: Kesalahan - {exInner.Message}. Data: {line}");
                            }
                        }
                        else
                        {
                            failCount++;
                            errorDetails.Add($"Baris {i + 1}: Jumlah kolom tidak cukup. Data: {line}");
                        }
                    }

                    InvalidateReservasiDataCache();
                    InvalidateAvailableMejaCache();
                    LoadData();
                    ClearForm();

                    string summaryMessage = $"Impor Selesai.\nBerhasil: {successCount}\nGagal: {failCount}";
                    if (failCount > 0)
                    {
                        summaryMessage += "\n\nDetail Kegagalan (maks 10 baris pertama):\n" + string.Join("\n", errorDetails.GetRange(0, Math.Min(errorDetails.Count, 10)));
                        // Pertimbangkan untuk menyimpan semua errorDetails ke file log jika jumlahnya banyak
                        // File.WriteAllLines("import_errors.log", errorDetails);
                    }
                    MessageBox.Show(summaryMessage, "Hasil Impor", MessageBoxButtons.OK, failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("Gagal membaca file: " + ioEx.Message, "Kesalahan File IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Terjadi kesalahan saat impor: " + ex.Message, "Kesalahan Impor Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }
    }
}