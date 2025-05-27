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

namespace PABDCAFE
{
    // Form CustomerPage digunakan oleh customer untuk membuat dan melihat reservasi mereka.
    public partial class CustomerPage : Form
    {
        // Field untuk menyimpan string koneksi yang diterima dari LoginPage.
        // 'readonly' berarti nilainya hanya bisa diatur sekali dalam konstruktor.
        private readonly string connectionString;
        // Objek koneksi SQL yang akan digunakan untuk semua operasi database dalam form ini.
        private SqlConnection conn;

        private int? selectedReservasiID = null;

        // Konstruktor CustomerPage, menerima string koneksi sebagai parameter.
        public CustomerPage(string connStr)
        {
            InitializeComponent(); // Inisialisasi komponen UI.

            // Validasi string koneksi yang diterima.
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak valid diterima oleh CustomerPage.", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentNullException(nameof(connStr), "String koneksi tidak boleh null atau kosong.");
            }
            this.connectionString = connStr; // Menyimpan string koneksi.
            this.conn = new SqlConnection(this.connectionString); // Membuat instance SqlConnection.
        }

        // Konstruktor default (tanpa parameter).
        // Sebaiknya dihindari jika string koneksi selalu dibutuhkan dari LoginPage.
        // Jika terpanggil, fungsionalitas database mungkin tidak berjalan.
        public CustomerPage()
        {
            InitializeComponent();
            MessageBox.Show("CustomerPage dibuat tanpa string koneksi. Fitur database mungkin tidak berfungsi.", "Peringatan Konstruktor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            // Inisialisasi this.connectionString ke null atau string kosong agar pengecekan nanti bisa menangani kasus ini.
            this.connectionString = null;
            // this.conn akan tetap null jika konstruktor ini yang dipanggil.
        }


        // Event handler yang dipanggil saat form CustomerPage dimuat.
        private void CustomerPage_Load(object sender, EventArgs e)
        {
            // Memeriksa apakah koneksi sudah siap sebelum melakukan operasi database.
            if (!IsConnectionReady())
            {
                // Menonaktifkan kontrol input jika koneksi tidak siap untuk mencegah error.
                txtCustNama.Enabled = false;
                txtCustNoTelp.Enabled = false;
                dtpCustWaktu.Enabled = false;
                cmbCustMeja.Enabled = false;
                btnCustTambah.Enabled = false;
                btnCustHapus.Enabled = false;
                return;
            }

            LoadReservasi(); // Memuat data reservasi yang ada ke DataGridView.
            LoadComboBoxMeja(cmbCustMeja); // Memuat nomor meja yang tersedia ke ComboBox.

            // Mengatur format untuk DateTimePicker.
            dtpCustWaktu.Format = DateTimePickerFormat.Custom;
            dtpCustWaktu.CustomFormat = "yyyy-MM-dd HH:mm"; // Format tanggal dan waktu.
            dtpCustWaktu.ShowUpDown = false; // Agar kalender muncul saat diklik (bukan up-down).
        }

        // Metode untuk memuat nomor meja yang tersedia ke ComboBox.
        private void LoadComboBoxMeja(ComboBox cmb)
        {
            if (!IsConnectionReady()) return; // Pastikan koneksi siap.

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open(); // Membuka koneksi jika tertutup.
                }

                // Query untuk mengambil Nomor_Meja dari tabel Meja yang statusnya 'Tersedia'.
                using (SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja WHERE Status_Meja = 'Tersedia' ORDER BY Nomor_Meja", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader(); // Menjalankan query.
                    List<string> mejaList = new List<string>(); // List untuk menyimpan nomor meja.
                    while (reader.Read())
                    {
                        mejaList.Add(reader["Nomor_Meja"].ToString()); // Menambahkan nomor meja ke list.
                    }
                    reader.Close(); // Selalu tutup DataReader setelah selesai.

                    cmb.DataSource = null; // Hapus datasource lama untuk refresh.
                    cmb.Items.Clear();
                    cmb.DataSource = mejaList; // Mengisi ComboBox dengan daftar meja.
                    if (mejaList.Count > 0)
                    {
                        cmb.SelectedIndex = -1; // Tidak ada item yang dipilih secara default.
                    }
                    else
                    {
                        cmb.Items.Add("Tidak ada meja tersedia");
                        cmb.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat daftar meja yang tersedia: " + ex.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close(); // Menutup koneksi di blok finally.
                }
            }
        }

        // Metode untuk memvalidasi input dari form reservasi customer.
        private bool ValidasiInput()
        {
            if (!IsConnectionReady()) return false; // Pastikan koneksi siap.

            string nama = txtCustNama.Text.Trim();
            string telp = txtCustNoTelp.Text.Trim();
            // Mengambil teks yang dipilih atau diketik di ComboBox.
            string meja = cmbCustMeja.SelectedItem?.ToString() ?? cmbCustMeja.Text.Trim();
            DateTime waktu = dtpCustWaktu.Value;

            // Validasi nama.
            if (string.IsNullOrWhiteSpace(nama) || nama.Length < 3)
            {
                MessageBox.Show("Nama customer tidak boleh kosong dan minimal 3 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validasi nomor telepon (format Indonesia).
            // Memperbolehkan +62 atau 0 di awal.
            if (!Regex.IsMatch(telp, @"^(\+62\d{8,12}|0\d{9,14})$"))
            {
                MessageBox.Show("Nomor telepon tidak valid. Contoh: +6281234567890 atau 081234567890.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validasi waktu reservasi tidak boleh di masa lalu.
            if (waktu < DateTime.Now.AddMinutes(-1)) // Toleransi 1 menit.
            {
                MessageBox.Show("Waktu reservasi tidak boleh di masa lalu.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validasi tahun reservasi (sesuai kebutuhan awal).
            if (waktu.Year != 2025)
            {
                MessageBox.Show("Reservasi hanya boleh di tahun 2025.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validasi pemilihan meja.
            if (string.IsNullOrWhiteSpace(meja) || meja == "Tidak ada meja tersedia")
            {
                MessageBox.Show("Silakan pilih nomor meja yang tersedia.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validasi tambahan ke database: Cek status meja dan jadwal.
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open(); // Buka koneksi untuk validasi database.
                }

                // 1. Cek apakah meja yang dipilih memang ada dan statusnya 'Tersedia'.
                // (Meskipun ComboBox sudah difilter, double check itu baik, terutama jika ada kemungkinan status berubah).
                SqlCommand cekMejaCmd = new SqlCommand("SELECT Status_Meja FROM Meja WHERE Nomor_Meja = @NomorMeja", conn);
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
                    LoadComboBoxMeja(cmbCustMeja); // Refresh ComboBox.
                    return false;
                }

                // 2. Cek apakah meja tersebut sudah direservasi pada waktu yang sama.
                // Pertimbangkan durasi reservasi jika ada (misal, +/- 1-2 jam dari waktu yang dipilih).
                // Untuk saat ini, kita cek tepat pada waktu yang dipilih.
                SqlCommand cekJadwalCmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservasi WHERE Nomor_Meja = @NomorMeja AND Waktu_Reservasi = @WaktuReservasi", conn);
                cekJadwalCmd.Parameters.AddWithValue("@NomorMeja", meja);
                cekJadwalCmd.Parameters.AddWithValue("@WaktuReservasi", waktu);
                int countReservasi = (int)cekJadwalCmd.ExecuteScalar();

                if (countReservasi > 0)
                {
                    MessageBox.Show($"Meja '{meja}' sudah direservasi pada waktu tersebut. Silakan pilih waktu atau meja lain.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                return true; // Semua validasi berhasil.
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kesalahan saat validasi data ke database: " + ex.Message, "Error Validasi DB", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close(); // Tutup koneksi setelah validasi database.
                }
            }
        }

        // Event handler untuk tombol "Tambah Reservasi" oleh customer.
        private void btnCustTambah_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Pastikan koneksi siap.
            if (!ValidasiInput()) return; // Lakukan validasi input.

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open(); // Buka koneksi.
                }

                // Perintah SQL INSERT menggunakan parameter untuk keamanan (mencegah SQL Injection).
                // Sebaiknya gunakan Stored Procedure untuk operasi ini.
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Reservasi (Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja) " +
                    "VALUES (@Nama, @Telp, @Waktu, @Meja); SELECT SCOPE_IDENTITY();", conn); // SCOPE_IDENTITY() untuk mendapatkan ID jika perlu.
                cmd.Parameters.AddWithValue("@Nama", txtCustNama.Text.Trim());
                cmd.Parameters.AddWithValue("@Telp", txtCustNoTelp.Text.Trim());
                cmd.Parameters.AddWithValue("@Waktu", dtpCustWaktu.Value);
                cmd.Parameters.AddWithValue("@Meja", cmbCustMeja.SelectedItem.ToString()); // Ambil dari SelectedItem agar pasti valid.

                // ExecuteNonQuery untuk INSERT, UPDATE, DELETE. Mengembalikan jumlah baris terpengaruh.
                // Jika menggunakan SELECT SCOPE_IDENTITY(), gunakan ExecuteScalar() untuk mendapatkan ID baru.
                int result = cmd.ExecuteNonQuery();

                MessageBox.Show(result > 0 ? "Reservasi berhasil ditambahkan." : "Reservasi gagal ditambahkan.",
                                result > 0 ? "Sukses" : "Gagal", MessageBoxButtons.OK,
                                result > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                // Muat ulang data dan bersihkan form setelah berhasil.
                LoadReservasi();
                LoadComboBoxMeja(cmbCustMeja); // Refresh daftar meja, karena status meja mungkin berubah.
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menambahkan reservasi: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close(); // Tutup koneksi.
                }
            }
        }

        // Metode untuk memuat data reservasi ke DataGridView.
        private void LoadReservasi()
        {
            if (!IsConnectionReady()) return; // Pastikan koneksi siap.

            try
            {
                // Menggunakan SqlDataAdapter untuk mengisi DataTable.
                // SqlDataAdapter bisa mengelola open/close koneksi jika koneksi yang diberikan dalam keadaan closed.
                // Query SELECT * mengambil semua kolom. Lebih baik sebutkan kolom spesifik jika tidak semua dibutuhkan.
                string query = "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi ORDER BY Waktu_Reservasi DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn); // Menggunakan koneksi kelas 'conn'.

                if (da.SelectCommand != null)
                {
                    da.SelectCommand.CommandTimeout = 120;
                }

                DataTable dt = new DataTable();
                da.Fill(dt); // Mengisi DataTable.
                dgvCustomer.DataSource = dt; // Menetapkan DataSource untuk DataGridView.

                // Mengatur header kolom jika perlu (opsional, karena nama kolom dari DB mungkin sudah cukup).
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
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data reservasi: " + ex.Message, "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // Koneksi tidak perlu ditutup di sini jika SqlDataAdapter yang mengelolanya,
            // atau jika 'conn' dikelola per operasi di metode lain.
        }

        // Event handler untuk tombol "Hapus Reservasi" oleh customer.
        private void btnCustHapus_Click(object sender, EventArgs e)
        {
            if (!IsConnectionReady()) return; // Pastikan koneksi siap.

            // Memastikan ada baris yang dipilih di DataGridView untuk dihapus.
            if (dgvCustomer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data reservasi yang ingin Anda hapus dari tabel.", "Pilih Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Mengambil ID_Reservasi dari baris yang dipilih (lebih aman daripada nama dan waktu).
            // Diasumsikan kolom "ID_Reservasi" ada dan merupakan primary key.
            object idReservasiObj = dgvCustomer.SelectedRows[0].Cells["ID_Reservasi"].Value;
            if (idReservasiObj == null || idReservasiObj == DBNull.Value)
            {
                MessageBox.Show("ID Reservasi tidak valid pada baris yang dipilih.", "Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int idReservasi = Convert.ToInt32(idReservasiObj);

            // Konfirmasi penghapusan.
            DialogResult dr = MessageBox.Show($"Apakah Anda yakin ingin menghapus reservasi dengan ID {idReservasi}?",
                                               "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No) return;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open(); // Buka koneksi.
                }

                // Menggunakan ID_Reservasi untuk menghapus (lebih presisi).
                // Sebaiknya gunakan Stored Procedure.
                SqlCommand cmd = new SqlCommand("DELETE FROM Reservasi WHERE ID_Reservasi = @IDReservasi", conn);
                cmd.Parameters.AddWithValue("@IDReservasi", idReservasi);

                int result = cmd.ExecuteNonQuery(); // Menjalankan perintah DELETE.
                MessageBox.Show(result > 0 ? "Reservasi berhasil dihapus." : "Reservasi tidak ditemukan atau gagal dihapus.",
                                result > 0 ? "Sukses" : "Informasi", MessageBoxButtons.OK,
                                result > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                // Muat ulang data dan bersihkan form.
                LoadReservasi();
                LoadComboBoxMeja(cmbCustMeja); // Status meja mungkin berubah menjadi 'Tersedia'.
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menghapus reservasi: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close(); // Tutup koneksi.
                }
            }
        }

        // Event handler saat sel di DataGridView diklik.
        // Digunakan untuk mengisi form input dengan data dari baris yang dipilih.
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Memastikan klik pada baris yang valid.
            if (e.RowIndex >= 0 && e.RowIndex < dgvCustomer.Rows.Count && !dgvCustomer.Rows[e.RowIndex].IsNewRow)
            {
                DataGridViewRow row = dgvCustomer.Rows[e.RowIndex];
                txtCustNama.Text = row.Cells["Nama_Customer"].Value?.ToString();
                txtCustNoTelp.Text = row.Cells["No_Telp"].Value?.ToString();

                // Mengatur DateTimePicker dengan aman.
                if (row.Cells["Waktu_Reservasi"].Value != null && row.Cells["Waktu_Reservasi"].Value != DBNull.Value)
                {
                    DateTime waktuDB = Convert.ToDateTime(row.Cells["Waktu_Reservasi"].Value);
                    // Pastikan nilai dalam rentang valid DateTimePicker
                    dtpCustWaktu.MinDate = new DateTime(1753, 1, 1);
                    dtpCustWaktu.MaxDate = new DateTime(9998, 12, 31);
                    dtpCustWaktu.Value = (waktuDB < dtpCustWaktu.MinDate) ? dtpCustWaktu.MinDate : (waktuDB > dtpCustWaktu.MaxDate ? dtpCustWaktu.MaxDate : waktuDB);
                }
                else
                {
                    dtpCustWaktu.Value = DateTime.Now; // Default jika null
                }

                // Mengatur ComboBox. Meja yang dipilih mungkin tidak lagi 'Tersedia'.
                // ComboBox hanya menampilkan meja 'Tersedia'. Jika ingin menampilkan meja yang sudah dipesan
                // pada baris ini, ComboBox perlu di-refresh atau item ditambahkan secara manual.
                // Untuk kesederhanaan, kita set teksnya saja, tapi pilihan mungkin tidak ada di daftar 'Tersedia'.
                cmbCustMeja.Text = row.Cells["Nomor_Meja"].Value?.ToString();

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

        // Metode untuk membersihkan semua input field di form.
        private void ClearForm()
        {
            txtCustNama.Clear();
            txtCustNoTelp.Clear();
            cmbCustMeja.SelectedIndex = -1; // Hapus pilihan di ComboBox.
            cmbCustMeja.Text = ""; // Bersihkan teks ComboBox jika ada.
            dtpCustWaktu.Value = DateTime.Now; // Set DateTimePicker ke waktu saat ini.
            txtCustNama.Focus();
        }

        // Event handler untuk tombol "Logout".
        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                new LoginPage().Show(); // Membuat instance baru LoginPage dan menampilkannya.
                this.Close(); // Menutup form CustomerPage saat ini.
            }
        }

        // Metode helper untuk memeriksa kesiapan koneksi.
        private bool IsConnectionReady()
        {
            // Jika konstruktor default yang dipanggil, this.conn akan null.
            if (this.conn == null || string.IsNullOrWhiteSpace(this.connectionString))
            {
                MessageBox.Show("Koneksi database tidak diinisialisasi dengan benar. Silakan restart aplikasi atau hubungi administrator.", "Error Koneksi Kritis", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}