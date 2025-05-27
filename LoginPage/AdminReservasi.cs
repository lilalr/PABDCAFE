using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO; // Diperlukan untuk operasi File seperti File.ReadAllLines
using System.Collections.Generic; // Diperlukan untuk List<T>

namespace PABDCAFE
{
    public partial class AdminReservasi : Form
    {
        // Menyimpan string koneksi database yang diterima dari form sebelumnya (misalnya Login atau AdminPage)
        private readonly string connectionString;
        // Objek koneksi SQL yang akan digunakan untuk semua operasi database di form ini
        private SqlConnection conn;

        // Konstruktor untuk AdminReservasi, menerima string koneksi sebagai parameter
        public AdminReservasi(string connStr)
        {
            InitializeComponent(); // Inisialisasi komponen-komponen form (otomatis oleh Visual Studio)

            // Validasi apakah string koneksi diberikan
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak ada atau kosong. Tidak dapat terhubung ke database.", "Kesalahan Konfigurasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Melemparkan exception jika string koneksi tidak valid untuk menghentikan pembuatan form
                throw new ArgumentNullException(nameof(connStr), "String koneksi database (connStr) tidak boleh null atau kosong.");
            }
            this.connectionString = connStr; // Menyimpan string koneksi
            this.conn = new SqlConnection(this.connectionString); // Membuat instance objek SqlConnection

            SetupDateTimePicker(); // Mengatur format dan nilai awal untuk DateTimePicker

            // Mencari ComboBox 'cbxNomorMeja' di form
            Control[] cbxControls = this.Controls.Find("cbxNomorMeja", true);
            if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
            {
                LoadAvailableMeja(cbx); // Memuat nomor meja yang tersedia ke ComboBox
            }
            else
            {
                // Menampilkan pesan error jika ComboBox tidak ditemukan
                MessageBox.Show("Error: ComboBox 'cbxNomorMeja' tidak ditemukan.", "Kesalahan Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadData(); // Memuat data reservasi awal ke DataGridView
        }

        // Metode untuk mengatur properti DateTimePicker
        private void SetupDateTimePicker()
        {
            // Mencari kontrol DateTimePicker dengan nama 'dtpWaktuReservasi'
            Control[] controls = this.Controls.Find("dtpWaktuReservasi", true);
            if (controls.Length > 0 && controls[0] is DateTimePicker dtp)
            {
                dtp.Format = DateTimePickerFormat.Custom; // Mengatur format menjadi kustom
                dtp.CustomFormat = "yyyy-MM-dd HH:mm";   // Menentukan format tanggal dan waktu
                dtp.Value = DateTime.Now;                // Mengatur nilai awal ke waktu saat ini
                // Properti ShowUpDown sebaiknya diatur ke False di Designer agar kalender muncul
            }
        }

        // Metode untuk memuat daftar nomor meja yang statusnya 'Tersedia' ke ComboBox
        private void LoadAvailableMeja(ComboBox cbx)
        {
            // Memastikan objek koneksi sudah diinisialisasi
            if (this.conn == null)
            {
                MessageBox.Show("Koneksi database belum diinisialisasi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // Membuka koneksi jika tertutup
                if (this.conn.State == ConnectionState.Closed)
                {
                    this.conn.Open();
                }

                // Perintah SQL untuk mengambil Nomor_Meja dari tabel Meja yang statusnya 'Tersedia'
                // Menggunakan 'this.conn' yang merupakan objek SqlConnection
                using (SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja WHERE Status_Meja = 'Tersedia' ORDER BY Nomor_Meja", this.conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader(); // Menjalankan query dan mendapatkan reader
                    List<string> mejaList = new List<string>(); // List untuk menyimpan nomor meja
                    while (reader.Read()) // Membaca setiap baris hasil query
                    {
                        mejaList.Add(reader["Nomor_Meja"].ToString()); // Menambahkan nomor meja ke list
                    }
                    reader.Close(); // Menutup reader setelah selesai membaca

                    // Mengatur DataSource ComboBox
                    cbx.DataSource = null; // Menghapus DataSource sebelumnya untuk refresh
                    cbx.Items.Clear();     // Menghapus item sebelumnya jika ada
                    cbx.DataSource = mejaList; // Mengisi ComboBox dengan daftar meja yang tersedia

                    if (mejaList.Count > 0)
                    {
                        cbx.SelectedIndex = -1; // Tidak ada item yang dipilih secara default
                    }
                    else
                    {
                        // Jika tidak ada meja tersedia, tambahkan item placeholder
                        cbx.Items.Add("Tidak ada meja tersedia");
                        cbx.SelectedIndex = 0; // Pilih item placeholder
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar meja yang tersedia: " + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Menutup koneksi jika terbuka
                if (this.conn.State == ConnectionState.Open)
                {
                    this.conn.Close();
                }
            }
        }

        // Metode untuk memuat data reservasi dari database ke DataGridView
        void LoadData()
        {
            if (this.conn == null)
            {
                MessageBox.Show("Koneksi database belum diinisialisasi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                // SqlDataAdapter dapat mengelola pembukaan/penutupan koneksi jika belum terbuka
                // Query untuk mengambil data reservasi, diurutkan berdasarkan waktu reservasi terbaru
                using (var da = new SqlDataAdapter(
                    "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi ORDER BY Waktu_Reservasi DESC", this.conn))
                {
                    var dt = new DataTable(); // Membuat DataTable untuk menampung data
                    da.Fill(dt); // Mengisi DataTable dengan data dari database
                    dgvAdminReservasi.DataSource = dt; // Menetapkan DataTable sebagai sumber data DataGridView

                    // Mengatur teks header kolom DataGridView
                    if (dgvAdminReservasi.Columns.Contains("ID_Reservasi"))
                        dgvAdminReservasi.Columns["ID_Reservasi"].HeaderText = "ID";
                    if (dgvAdminReservasi.Columns.Contains("Nama_Customer"))
                        dgvAdminReservasi.Columns["Nama_Customer"].HeaderText = "Nama Customer";
                    if (dgvAdminReservasi.Columns.Contains("No_Telp"))
                        dgvAdminReservasi.Columns["No_Telp"].HeaderText = "No. Telepon";
                    if (dgvAdminReservasi.Columns.Contains("Waktu_Reservasi"))
                    {
                        dgvAdminReservasi.Columns["Waktu_Reservasi"].HeaderText = "Waktu Reservasi";
                        // Mengatur format tampilan kolom tanggal dan waktu
                        dgvAdminReservasi.Columns["Waktu_Reservasi"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                    }
                    if (dgvAdminReservasi.Columns.Contains("Nomor_Meja"))
                        dgvAdminReservasi.Columns["Nomor_Meja"].HeaderText = "Nomor Meja";

                    dgvAdminReservasi.ClearSelection(); // Menghapus seleksi awal pada DataGridView
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Kesalahan Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handler yang dipanggil saat form AdminReservasi dimuat
        private void AdminReservasi_Load(object sender, EventArgs e)
        {
            // LoadData(), SetupDateTimePicker(), dan LoadAvailableMeja() sudah dipanggil di konstruktor.
            // Tidak ada kode tambahan yang diperlukan di sini kecuali ada inisialisasi lain yang spesifik.
        }

        // Metode untuk membersihkan input field pada form
        void ClearForm()
        {
            txtNama.Clear(); // Membersihkan TextBox nama
            txtTelepon.Clear(); // Membersihkan TextBox telepon

            // Mengatur ulang DateTimePicker ke waktu saat ini
            Control[] dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
            if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
            {
                dtp.Value = DateTime.Now;
            }

            // Membersihkan seleksi ComboBox dan memuat ulang daftar meja yang tersedia
            Control[] cbxControls = this.Controls.Find("cbxNomorMeja", true);
            if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
            {
                cbx.SelectedIndex = -1; // Menghapus seleksi
                LoadAvailableMeja(cbx); // Memuat ulang daftar meja untuk menyegarkan
            }

            dgvAdminReservasi.ClearSelection(); // Menghapus seleksi pada DataGridView
            txtNama.Focus(); // Mengatur fokus ke TextBox nama
        }

        // Metode untuk memvalidasi input dari pengguna
        bool ValidasiInput(out string err)
        {
            err = string.Empty; // Inisialisasi pesan error
            if (string.IsNullOrWhiteSpace(txtNama.Text))
                err += "Nama customer tidak boleh kosong.\n";

            // Validasi format nomor telepon Indonesia (mulai dengan +62 atau 0)
            // Regex ini disesuaikan dengan constraint CHECK di database
            if (!Regex.IsMatch(txtTelepon.Text.Trim(), @"^(\+62\d{8,12}|0\d{9,14})$"))
                err += "Format nomor telepon tidak valid (Contoh: +6281234567890 atau 081234567890).\n";

            // Validasi DateTimePicker
            var dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
            if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
            {
                // Validasi waktu reservasi tidak boleh di masa lalu (dengan toleransi 1 menit)
                if (dtp.Value < DateTime.Now.AddMinutes(-1))
                {
                    err += "Waktu reservasi tidak boleh di masa lalu.\n";
                }
                // Validasi tahun harus 2025 (sesuai constraint database di kode asli)
                if (dtp.Value.Year != 2025)
                {
                    err += "Waktu reservasi hanya diperbolehkan untuk tahun 2025.\n";
                }
            }
            else
            {
                err += "Kontrol DateTimePicker untuk waktu reservasi tidak ditemukan.\n";
            }

            // Validasi ComboBox Nomor Meja
            var cbxControls = this.Controls.Find("cbxNomorMeja", true);
            if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
            {
                // Memastikan item dipilih dan bukan placeholder "Tidak ada meja tersedia"
                if (cbx.SelectedItem == null || string.IsNullOrWhiteSpace(cbx.SelectedItem.ToString()) || cbx.SelectedItem.ToString() == "Tidak ada meja tersedia")
                    err += "Nomor meja yang valid harus dipilih.\n";
            }
            else
            {
                err += "Kontrol ComboBox untuk nomor meja tidak ditemukan.\n";
            }

            return string.IsNullOrEmpty(err); // Mengembalikan true jika tidak ada error
        }

        // Metode helper untuk memeriksa kesiapan koneksi database
        private bool IsConnectionReady()
        {
            if (this.conn == null || string.IsNullOrWhiteSpace(this.conn.ConnectionString))
            {
                MessageBox.Show("Koneksi database tidak dikonfigurasi atau hilang.", "Error Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Koneksi tidak siap
            }
            return true; // Koneksi siap
        }

        // Event handler untuk tombol Edit
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Periksa koneksi sebelum melanjutkan

            // Memastikan ada baris yang dipilih di DataGridView
            if (dgvAdminReservasi.CurrentRow == null || dgvAdminReservasi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih data reservasi yang ingin diedit dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Memvalidasi input sebelum proses edit
            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mengambil ID Reservasi dari baris yang dipilih
                int idReservasi = Convert.ToInt32(dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value);
                // Mengambil nilai dari kontrol input
                DateTime waktuReservasi = ((DateTimePicker)this.Controls.Find("dtpWaktuReservasi", true)[0]).Value;
                string selectedMeja = ((ComboBox)this.Controls.Find("cbxNomorMeja", true)[0]).SelectedItem.ToString();

                // Membuka koneksi jika tertutup
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Menggunakan Stored Procedure 'EditReservasi'
                using (SqlCommand cmd = new SqlCommand("EditReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // Menentukan tipe command adalah Stored Procedure
                    // Menambahkan parameter untuk Stored Procedure
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja_Baru", selectedMeja);

                    int rowsAffected = cmd.ExecuteNonQuery(); // Menjalankan Stored Procedure

                    if (rowsAffected > 0) // Jika ada baris yang terpengaruh (berhasil diupdate)
                    {
                        MessageBox.Show("Data reservasi berhasil diperbarui!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();  // Memuat ulang data di DataGridView
                        ClearForm(); // Membersihkan form input
                    }
                    else
                    {
                        MessageBox.Show("Data tidak diperbarui atau tidak ada perubahan data.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx) // Menangkap error spesifik SQL
            {
                // Pesan error dari RAISERROR di Stored Procedure akan ditampilkan di sini
                MessageBox.Show("Kesalahan SQL: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Menangkap error umum
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Menutup koneksi jika terbuka
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Event handler untuk tombol Kembali (Back)
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Membuat instance AdminPage dan menampilkannya
            // Jika AdminPage juga memerlukan string koneksi, pastikan konstruktornya diadaptasi
            AdminPage ap = new AdminPage(this.connectionString);
            ap.Show();
            this.Close(); // Menutup form AdminReservasi saat ini
        }

        // Event handler untuk tombol Tambah
        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Periksa koneksi

            // Memvalidasi input sebelum proses tambah
            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Mengambil nilai dari kontrol input
                DateTime waktuReservasi = ((DateTimePicker)this.Controls.Find("dtpWaktuReservasi", true)[0]).Value;
                string selectedMeja = ((ComboBox)this.Controls.Find("cbxNomorMeja", true)[0]).SelectedItem.ToString();

                // Membuka koneksi jika tertutup
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Menggunakan Stored Procedure 'TambahReservasi'
                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Menambahkan parameter untuk Stored Procedure
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja", selectedMeja);

                    cmd.ExecuteNonQuery(); // Menjalankan Stored Procedure
                    MessageBox.Show("Reservasi berhasil ditambahkan!");
                    LoadData();  // Memuat ulang data di DataGridView
                    ClearForm(); // Membersihkan form input
                }
            }
            catch (SqlException sqlEx) // Menangkap error spesifik SQL (misalnya dari trigger atau constraint)
            {
                MessageBox.Show("Gagal menambahkan reservasi: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Menangkap error umum
            {
                MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Menutup koneksi jika terbuka
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Event handler untuk tombol Hapus
        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Periksa koneksi

            // Memastikan ada baris yang dipilih di DataGridView
            if (dgvAdminReservasi.CurrentRow == null || dgvAdminReservasi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih data reservasi yang ingin dihapus dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi penghapusan dari pengguna
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No) // Jika pengguna memilih 'Tidak'
                return;

            try
            {
                // Mengambil ID Reservasi dari baris yang dipilih
                int idReservasi = Convert.ToInt32(dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value);

                // Membuka koneksi jika tertutup
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Menggunakan Stored Procedure 'HapusReservasi'
                using (SqlCommand cmd = new SqlCommand("HapusReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    // Menambahkan parameter ID Reservasi
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    int rowsAffected = cmd.ExecuteNonQuery(); // Menjalankan Stored Procedure

                    if (rowsAffected > 0) // Jika ada baris yang terpengaruh (berhasil dihapus)
                    {
                        MessageBox.Show("Reservasi berhasil dihapus.");
                        LoadData();  // Memuat ulang data di DataGridView
                        ClearForm(); // Membersihkan form input
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau gagal dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx) // Menangkap error spesifik SQL
            {
                MessageBox.Show("Gagal menghapus data: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Menangkap error umum
            {
                MessageBox.Show("Gagal menghapus data: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Menutup koneksi jika terbuka
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Event handler saat sel di DataGridView diklik (diganti dari CellContentClick untuk responsivitas yang lebih baik pada seleksi baris)
        private void dgvAdminReservasi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Memastikan klik terjadi pada baris yang valid dan bukan baris baru (jika ada)
            if (e.RowIndex < 0 || e.RowIndex >= dgvAdminReservasi.Rows.Count || dgvAdminReservasi.Rows[e.RowIndex].IsNewRow)
                return;

            DataGridViewRow row = dgvAdminReservasi.Rows[e.RowIndex]; // Mendapatkan baris yang diklik

            // Mengisi TextBox dengan data dari baris yang dipilih
            txtNama.Text = row.Cells["Nama_Customer"].Value?.ToString() ?? string.Empty;
            txtTelepon.Text = row.Cells["No_Telp"].Value?.ToString() ?? string.Empty;

            // Mengisi DateTimePicker
            Control[] dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
            if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
            {
                // Mengatur MinDate dan MaxDate untuk mencegah error jika nilai dari database di luar rentang DateTimePicker
                // Rentang ini sesuai dengan rentang DATETIME di SQL Server
                dtp.MinDate = new DateTime(1753, 1, 1);
                dtp.MaxDate = new DateTime(9998, 12, 31);

                if (row.Cells["Waktu_Reservasi"].Value != null && row.Cells["Waktu_Reservasi"].Value != DBNull.Value)
                {
                    DateTime nilai = Convert.ToDateTime(row.Cells["Waktu_Reservasi"].Value);
                    // Memastikan nilai berada dalam rentang yang valid untuk DateTimePicker sebelum di-set
                    dtp.Value = (nilai < dtp.MinDate) ? dtp.MinDate : (nilai > dtp.MaxDate ? dtp.MaxDate : nilai);
                }
                else
                {
                    dtp.Value = DateTime.Now; // Jika kosong, setel ke waktu saat ini
                }
            }

            // Mengisi ComboBox Nomor Meja
            Control[] cbxControls = this.Controls.Find("cbxNomorMeja", true);
            if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
            {
                string selectedMejaFromGrid = row.Cells["Nomor_Meja"].Value?.ToString() ?? string.Empty;

                // Logika untuk menangani ComboBox saat baris dipilih:
                // Jika meja dari grid tidak ada dalam daftar meja yang 'Tersedia' saat ini
                // (misalnya, karena meja tersebut adalah meja yang sedang diedit reservasinya),
                // kita perlu memastikan meja tersebut bisa ditampilkan dan dipilih di ComboBox.
                List<string> currentItems = new List<string>();
                if (cbx.DataSource is List<string> ds) // Jika DataSource adalah List<string>
                {
                    currentItems.AddRange(ds); // Salin item dari DataSource
                }
                else
                { // Fallback jika DataSource bukan List<string> (misalnya diisi manual)
                    foreach (var item in cbx.Items) currentItems.Add(item.ToString());
                }

                // Jika nomor meja dari grid tidak ada di daftar item ComboBox saat ini
                if (!string.IsNullOrEmpty(selectedMejaFromGrid) && !currentItems.Contains(selectedMejaFromGrid))
                {
                    // Buat list baru untuk DataSource yang menyertakan item yang dipilih dari grid
                    List<string> displayItems = new List<string>(currentItems);
                    if (!displayItems.Contains(selectedMejaFromGrid)) // Periksa lagi untuk menghindari duplikasi
                    {
                        displayItems.Add(selectedMejaFromGrid); // Tambahkan meja dari grid
                        displayItems.Sort(); // Opsional: urutkan daftar meja
                    }
                    cbx.DataSource = null; // Hapus DataSource lama
                    cbx.Items.Clear();     // Bersihkan item lama
                    cbx.DataSource = displayItems; // Set DataSource baru
                    cbx.SelectedItem = selectedMejaFromGrid; // Pilih meja dari grid
                }
                else if (cbx.Items.Contains(selectedMejaFromGrid)) // Jika meja ada di daftar item
                {
                    cbx.SelectedItem = selectedMejaFromGrid; // Langsung pilih
                }
                else // Jika meja kosong atau kasus lain
                {
                    cbx.SelectedIndex = -1; // Tidak ada yang dipilih
                }
            }
        }

        // Event handler untuk tombol Impor Data dari CSV
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Periksa koneksi

            OpenFileDialog openFileDialog = new OpenFileDialog(); // Membuat dialog untuk memilih file
            openFileDialog.Filter = "CSV files (.csv)|.csv|All files (.)|."; // Filter jenis file (hanya CSV)
            openFileDialog.Title = "Pilih File CSV untuk Impor"; // Judul dialog

            if (openFileDialog.ShowDialog() == DialogResult.OK) // Jika pengguna memilih file dan klik OK
            {
                string filePath = openFileDialog.FileName; // Mendapatkan path file yang dipilih
                int successCount = 0; // Penghitung data yang berhasil diimpor
                int failCount = 0;    // Penghitung data yang gagal diimpor
                string errorMessages = ""; // String untuk mengakumulasi pesan error

                try
                {
                    string[] lines = File.ReadAllLines(filePath); // Membaca semua baris dari file CSV
                    // Memeriksa apakah file kosong atau hanya berisi header (jika header dianggap tidak valid)
                    if (lines.Length == 0 || (lines.Length == 1 && string.IsNullOrWhiteSpace(lines[0])))
                    {
                        MessageBox.Show("File CSV kosong atau tidak valid.", "Informasi Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (conn.State == ConnectionState.Closed) conn.Open(); // Membuka koneksi jika tertutup

                    // Loop melalui setiap baris dalam file CSV
                    // Diasumsikan baris pertama bisa jadi header atau data.
                    // Jika baris pertama adalah header dan ingin dilewati, ubah 'i = 0' menjadi 'i = 1'.
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i]; // Mendapatkan baris saat ini
                        if (string.IsNullOrWhiteSpace(line)) continue; // Lewati baris kosong

                        string[] data = line.Split(','); // Memecah baris berdasarkan koma (delimiter CSV)
                        if (data.Length >= 4) // Memastikan ada setidaknya 4 kolom data (Nama, Telp, Waktu, Meja)
                        {
                            try
                            {
                                string namaCustomer = data[0].Trim(); // Data kolom pertama (Nama)
                                string noTelp = data[1].Trim();       // Data kolom kedua (No. Telepon)
                                DateTime waktuReservasi;
                                // Mencoba parsing tanggal dengan format spesifik "yyyy-MM-dd HH:mm"
                                if (!DateTime.TryParseExact(data[2].Trim(), "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out waktuReservasi))
                                {
                                    // Jika format spesifik gagal, coba parsing dengan format umum
                                    if (!DateTime.TryParse(data[2].Trim(), out waktuReservasi))
                                    {
                                        failCount++;
                                        errorMessages += $"Baris {i + 1}: Format waktu '{data[2]}' tidak valid. Diharapkan format yyyy-MM-dd HH:mm.\n";
                                        continue; // Lanjut ke baris berikutnya
                                    }
                                }
                                string nomorMeja = data[3].Trim(); // Data kolom keempat (Nomor Meja)

                                // Validasi data dari CSV sebelum dimasukkan ke database
                                string csvRowError = "";
                                if (string.IsNullOrWhiteSpace(namaCustomer)) csvRowError += "Nama kosong. ";
                                if (!Regex.IsMatch(noTelp, @"^(\+62\d{8,12}|0\d{9,14})$")) csvRowError += "Format telepon salah. ";
                                if (waktuReservasi < DateTime.Now.AddMinutes(-1)) csvRowError += "Waktu di masa lalu. "; // Validasi waktu
                                if (waktuReservasi.Year != 2025) csvRowError += "Tahun harus 2025. "; // Validasi tahun
                                if (string.IsNullOrWhiteSpace(nomorMeja) || nomorMeja.Length != 2 || !int.TryParse(nomorMeja, out _)) csvRowError += "Format Nomor Meja salah. ";

                                if (!string.IsNullOrEmpty(csvRowError)) // Jika ada error validasi untuk baris ini
                                {
                                    failCount++;
                                    errorMessages += $"Baris {i + 1}: Validasi gagal - {csvRowError.Trim()} (Data: {line}).\n";
                                    continue; // Lanjut ke baris berikutnya
                                }

                                // Menggunakan Stored Procedure 'TambahReservasi' untuk setiap baris data CSV
                                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Nama_Customer", namaCustomer);
                                    cmd.Parameters.AddWithValue("@No_Telp", noTelp);
                                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                                    cmd.ExecuteNonQuery(); // Menjalankan Stored Procedure
                                    successCount++; // Menambah hitungan sukses
                                }
                            }
                            catch (SqlException sqlEx) // Menangkap error SQL untuk baris tertentu
                            {
                                failCount++;
                                errorMessages += $"Baris {i + 1} (SQL Error): {sqlEx.Message} (Data: {line})\n";
                            }
                            catch (Exception ex) // Menangkap error lain (parsing, dll.) untuk baris tertentu
                            {
                                failCount++;
                                errorMessages += $"Baris {i + 1} (Error): {ex.Message} (Data: {line})\n";
                            }
                        }
                        else // Jika jumlah kolom tidak sesuai
                        {
                            failCount++;
                            errorMessages += $"Baris {i + 1}: Jumlah kolom tidak sesuai (Diharapkan minimal 4, ditemukan {data.Length}). Data: {line}\n";
                        }
                    }

                    // Menampilkan ringkasan hasil impor
                    MessageBox.Show($"Impor selesai.\nBerhasil: {successCount} data.\nGagal: {failCount} data." +
                                    (failCount > 0 ? "\n\nDetail Kegagalan:\n" + errorMessages : ""), // Tampilkan detail error jika ada
                                    "Hasil Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();  // Muat ulang data setelah impor
                    ClearForm(); // Bersihkan form
                }
                catch (IOException ioEx) // Menangkap error saat membaca file
                {
                    MessageBox.Show("Gagal membaca file: " + ioEx.Message, "Kesalahan I/O File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex) // Menangkap error umum saat proses impor
                {
                    MessageBox.Show("Gagal melakukan impor: " + ex.Message, "Kesalahan Impor Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close(); // Menutup koneksi
                }
            }
        }

        // Event handler dtpWaktuReservasi_ValueChanged_1 yang sebelumnya mungkin ada telah dihapus
        // karena bisa menyebabkan DateTimePicker mereset nilai secara terus-menerus jika tidak ditangani dengan benar.
        // Pengaturan awal di SetupDateTimePicker() dan pengambilan nilai saat submit sudah cukup.
    }
}