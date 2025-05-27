using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions; // Diperlukan untuk Regex (validasi input)
using System.Windows.Forms;
using System.Runtime.Caching;

namespace PABDCAFE
{
    public partial class AdminMeja : Form
    {
        // Menyimpan string koneksi database yang diterima dari form sebelumnya
        private readonly string connectionString;
        // Objek koneksi SQL yang akan digunakan untuk operasi database di form ini
        private SqlConnection conn;

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "AdminMeja";


        // Konstruktor AdminMeja, menerima string koneksi sebagai parameter
        public AdminMeja(string connStr) // Nama parameter diubah menjadi connStr
        {
            InitializeComponent(); // Inisialisasi komponen form (otomatis oleh Visual Studio)

            // Memeriksa apakah string koneksi valid (tidak null atau kosong)
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak ada atau kosong. Tidak dapat terhubung ke database.", "Kesalahan Konfigurasi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Melemparkan exception jika string koneksi tidak valid untuk menghentikan pembuatan form
                throw new ArgumentNullException(nameof(connStr), "String koneksi database (connStr) tidak boleh null atau kosong.");
            }

            this.connectionString = connStr; // Menyimpan string koneksi yang diterima
            this.conn = new SqlConnection(this.connectionString); // Membuat instance objek SqlConnection

            LoadData(); // Memuat data meja awal ke DataGridView
        }

        // Metode untuk membersihkan field input pada form
        void ClearForm()
        {
            txtNomor.Clear(); // Membersihkan TextBox Nomor Meja
            txtKapasitas.Clear(); // Membersihkan TextBox Kapasitas
            dgvAdminMeja.ClearSelection(); // Menghapus seleksi pada DataGridView
            txtNomor.Focus(); // Mengatur fokus ke TextBox Nomor Meja
        }

        // Metode untuk memuat data meja dari database ke DataGridView
        void LoadData()
        {
            DataTable dt = _cache.Get(CacheKey) as DataTable;
            // Memastikan objek koneksi sudah diinisialisasi
            if (this.conn == null)
            {
                MessageBox.Show("Koneksi database belum diinisialisasi.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Query untuk mengambil data Nomor_Meja, Kapasitas, dan Status_Meja dari tabel Meja
                string query = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja";
                // SqlDataAdapter akan menangani pembukaan dan penutupan koneksi jika diperlukan
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                dt = new DataTable(); // Membuat DataTable untuk menampung data
                da.Fill(dt); // Mengisi DataTable dengan data dari database
                dgvAdminMeja.DataSource = dt; // Menetapkan DataTable sebagai sumber data DataGridView
                _cache.Set(CacheKey, dt, _policy);

                // Mengatur teks header kolom DataGridView
                if (dgvAdminMeja.Columns["Nomor_Meja"] != null)
                    dgvAdminMeja.Columns["Nomor_Meja"].HeaderText = "Nomor Meja";
                if (dgvAdminMeja.Columns["Status_Meja"] != null) // Kolom Status Meja ditampilkan tapi tidak diedit dari form ini
                    dgvAdminMeja.Columns["Status_Meja"].HeaderText = "Status";
                if (dgvAdminMeja.Columns["Kapasitas"] != null)
                    dgvAdminMeja.Columns["Kapasitas"].HeaderText = "Kapasitas";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error memuat data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Koneksi akan ditutup oleh SqlDataAdapter jika ia yang membukanya,
            // atau dikelola secara manual dalam operasi CRUD.
        }

        // Event handler yang dipanggil saat form AdminMeja dimuat
        private void AdminMeja_Load(object sender, EventArgs e)
        {
            // LoadData() sudah dipanggil di konstruktor, jadi tidak perlu dipanggil lagi di sini
            // kecuali ada logika inisialisasi tambahan yang spesifik untuk event Load.
        }

        // Event handler untuk tombol Kembali (Back)
        private void btnBack_Click(object sender, EventArgs e)
        {
            // Membuat instance AdminPage dan menampilkannya
            // String koneksi diteruskan kembali jika AdminPage memerlukannya
            AdminPage ap = new AdminPage(this.connectionString);
            ap.Show();
            this.Close(); // Menutup form AdminMeja saat ini
        }

        // Metode untuk memvalidasi input Nomor Meja dan Kapasitas
        // Status Meja tidak divalidasi/diinput dari form ini
        bool ValidasiInput(out string nomorMeja, out int kapasitas, out string errorMsg)
        {
            errorMsg = ""; // Inisialisasi pesan error
            nomorMeja = txtNomor.Text.Trim(); // Mengambil dan membersihkan Nomor Meja dari TextBox
            string kapasitasStr = txtKapasitas.Text.Trim(); // Mengambil dan membersihkan Kapasitas dari TextBox
            kapasitas = 0; // Inisialisasi kapasitas

            // Validasi Nomor Meja: harus terdiri dari 2 digit angka
            if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                errorMsg += "Nomor Meja harus terdiri dari 2 digit angka (contoh: 01, 12).\n";
            }

            // Validasi Kapasitas: harus berupa angka antara 1 sampai 20
            if (!int.TryParse(kapasitasStr, out kapasitas) || kapasitas < 1 || kapasitas > 20)
            {
                errorMsg += "Kapasitas harus berupa angka antara 1 sampai 20.\n";
            }

            return string.IsNullOrEmpty(errorMsg); // Mengembalikan true jika tidak ada error validasi
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

        // Event handler untuk tombol Tambah
        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Memeriksa kesiapan koneksi

            // Memvalidasi input sebelum menambahkan data
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Membuka koneksi jika sedang tertutup
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Menggunakan Stored Procedure 'TambahMeja'
                // Status_Meja akan diatur ke 'Tersedia' secara otomatis oleh Stored Procedure
                using (SqlCommand cmd = new SqlCommand("TambahMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // Menentukan tipe command adalah Stored Procedure
                    // Menambahkan parameter untuk Stored Procedure
                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                    cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);
                    // Parameter @Status_Meja tidak dikirim, SP yang akan mengaturnya

                    int rowsAffected = cmd.ExecuteNonQuery(); // Menjalankan Stored Procedure

                    if (rowsAffected > 0) // Jika ada baris yang terpengaruh (data berhasil ditambahkan)
                    {
                        MessageBox.Show("Data berhasil ditambahkan! Status otomatis diatur ke 'Tersedia'.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();  // Memuat ulang data di DataGridView
                        ClearForm(); // Membersihkan form input
                    }
                    else
                    {
                        // Bisa terjadi jika SP melakukan validasi dan return tanpa error, atau RAISERROR tidak menghentikan di C#
                        MessageBox.Show("Data tidak berhasil ditambahkan. Periksa input atau kemungkinan nomor meja sudah ada.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException sqlEx) // Menangkap error spesifik SQL (misalnya dari RAISERROR di SP)
            {
                MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Menangkap error umum
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Menutup koneksi jika sedang terbuka
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Event handler untuk tombol Hapus
        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Memeriksa kesiapan koneksi

            string nomorMeja = txtNomor.Text.Trim(); // Mengambil Nomor Meja dari TextBox
            // Validasi awal apakah Nomor Meja diisi
            if (string.IsNullOrEmpty(nomorMeja))
            {
                MessageBox.Show("Silakan pilih data meja yang ingin dihapus atau isi Nomor Meja pada field yang sesuai.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Validasi format Nomor Meja
            if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                MessageBox.Show("Nomor Meja yang akan dihapus harus terdiri dari 2 digit angka.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Konfirmasi penghapusan dari pengguna
            DialogResult result = MessageBox.Show(
                $"Apakah Anda yakin ingin menghapus meja dengan Nomor '{nomorMeja}'?",
                "Konfirmasi Penghapusan",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.No) // Jika pengguna memilih 'Tidak'
                return;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open(); // Membuka koneksi jika tertutup

                // Menggunakan Stored Procedure 'HapusMeja' (pastikan SP ini ada di database)
                using (SqlCommand cmd = new SqlCommand("HapusMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja); // Parameter untuk SP
                    int rowsAffected = cmd.ExecuteNonQuery(); // Menjalankan SP

                    if (rowsAffected > 0) // Jika ada baris yang terpengaruh (data berhasil dihapus)
                    {
                        MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();  // Memuat ulang data
                        ClearForm(); // Membersihkan form
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau tidak ada data yang dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx) // Menangkap error SQL
            {
                MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Menangkap error umum
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) conn.Close(); // Menutup koneksi
            }
        }

        // Event handler untuk tombol Edit
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Memeriksa kesiapan koneksi

            // Validasi awal apakah Nomor Meja diisi (karena digunakan sebagai kunci untuk update)
            if (string.IsNullOrWhiteSpace(txtNomor.Text))
            {
                MessageBox.Show("Pilih data yang akan diubah atau pastikan Nomor Meja terisi!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Memvalidasi input (Nomor Meja dan Kapasitas)
            // Status Meja tidak diubah melalui form ini
            if (!ValidasiInput(out string nomorMeja, out int kapasitas, out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) // Membuka koneksi jika tertutup
                {
                    conn.Open();
                }
                // Menggunakan Stored Procedure 'EditMeja'
                // SP ini diasumsikan hanya mengupdate kolom Kapasitas berdasarkan Nomor_Meja.
                // Status Meja tidak diubah.
                using (SqlCommand cmd = new SqlCommand("EditMeja", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja); // Kunci untuk klausa WHERE di SP
                    cmd.Parameters.AddWithValue("@Kapasitas", kapasitas);   // Nilai baru untuk kapasitas
                    // Tidak ada parameter @Status_Meja yang dikirim

                    int rowsAffected = cmd.ExecuteNonQuery(); // Menjalankan SP

                    if (rowsAffected > 0) // Jika ada baris yang terpengaruh (data berhasil diupdate)
                    {
                        MessageBox.Show("Data kapasitas meja berhasil diperbarui! Status meja tidak diubah.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData(); // Memuat ulang data untuk melihat perubahan kapasitas
                        ClearForm(); // Membersihkan form
                    }
                    else
                    {
                        MessageBox.Show("Data tidak berhasil diperbarui! Pastikan Nomor Meja ada.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (SqlException sqlEx) // Menangkap error SQL
            {
                MessageBox.Show("Error Database: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Menangkap error umum
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open) // Menutup koneksi
                {
                    conn.Close();
                }
            }
        }

        // Event handler saat sel di DataGridView diklik
        private void dgvAdminMeja_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Memastikan klik terjadi pada baris yang valid (bukan header atau baris baru)
            if (e.RowIndex >= 0 && e.RowIndex < dgvAdminMeja.Rows.Count && dgvAdminMeja.Rows[e.RowIndex].Cells["Nomor_Meja"].Value != null)
            {
                // Menghindari baris baru jika AllowUserToAddRows = true dan belum ada isinya
                if (dgvAdminMeja.Rows[e.RowIndex].IsNewRow) return;

                DataGridViewRow row = dgvAdminMeja.Rows[e.RowIndex]; // Mendapatkan baris yang diklik
                // Mengisi TextBox Nomor Meja dan Kapasitas dari data baris yang dipilih
                // ?.ToString() ?? "" digunakan untuk menangani nilai null dengan aman
                txtNomor.Text = row.Cells["Nomor_Meja"].Value?.ToString() ?? "";
                txtKapasitas.Text = row.Cells["Kapasitas"].Value?.ToString() ?? "";
                // TextBox untuk Status Meja tidak ada, jadi tidak diisi. Status terlihat di DataGridView.
            }
        }
    }
}