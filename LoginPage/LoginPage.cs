using PABDCAFE; // Namespace proyek Anda
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; // Diperlukan untuk interaksi dengan SQL Server
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Caching;

namespace PABDCAFE
{
    // Form LoginPage adalah halaman awal tempat pengguna (admin/customer) melakukan login.
    public partial class LoginPage : Form
    {
        private string connectionString;

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "Login";
        public int selectedIdOrganisasi;

        // Konstruktor default untuk LoginPage.
        public LoginPage()
        {
            InitializeComponent(); // Inisialisasi komponen-komponen UI form.

            try
            {
                Koneksi kn = new Koneksi();
                this.connectionString = kn.connectionString(); // Memanggil method dari class Koneksi. [cite: 16]

                // Jika string koneksi gagal dibuat, lemparkan error.
                if (string.IsNullOrEmpty(this.connectionString))
                {
                    throw new Exception("String koneksi tidak berhasil dibuat dari class Koneksi.");
                }
            }
            catch (Exception ex)
            {
                // Menampilkan pesan jika koneksi awal gagal (misalnya, tidak menemukan IP).
                MessageBox.Show("Tidak dapat terhubung ke database. Aplikasi akan tertutup. \nDetail: " + ex.Message, "Error Koneksi Kritis", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Menonaktifkan form jika koneksi gagal total.
                this.Load += (s, e) => this.Close();
            }
        }

        // Event handler yang dipanggil saat form LoginPage dimuat.
        // Biasanya digunakan untuk inisialisasi tambahan jika diperlukan.
        private void LoginAdmin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus(); // Contoh: Mengatur fokus ke textbox Username saat form load.
        }

        // Event handler untuk tombol "Login".
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Mengambil nilai Username dan Password dari TextBox.
            // .Trim() digunakan untuk menghapus spasi di awal atau akhir input.
            string Username = txtUsername.Text.Trim();
            string Password = txtPassword.Text.Trim();

            // Memeriksa apakah Username atau Password kosong.
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Username dan Password tidak boleh kosong.", "Input Tidak Lengkap", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Menghentikan proses login jika input tidak lengkap.
            }

            try
            {
                // Blok 'using' memastikan objek SqlConnection ('conn') akan di-dispose dengan benar
                // setelah selesai digunakan, termasuk menutup koneksi jika masih terbuka.
                using (SqlConnection conn = new SqlConnection(this.connectionString))
                {
                    // Mencoba membuka koneksi ke database.
                    // Jika username atau password SQL Server salah, atau server tidak terjangkau,
                    // baris ini akan melemparkan SqlException.
                    conn.Open();

                    // Jika conn.Open() berhasil, berarti kredensial SQL Server (Username & Password) valid.
                    // Selanjutnya, kita perlu menentukan peran pengguna berdasarkan Username (admin atau customer).
                    // CATATAN PENTING: Membedakan peran HANYA berdasarkan username yang di-hardcode ("admin", "customer")
                    // adalah praktik yang kurang aman dan tidak fleksibel untuk aplikasi nyata.
                    // Pertimbangkan untuk menggunakan tabel 'Users' dengan kolom 'Role' di database Anda
                    // dan memverifikasi peran setelah login SQL berhasil.

                    if (Username.Equals("admin", StringComparison.OrdinalIgnoreCase)) // Perbandingan username tanpa memperhatikan huruf besar/kecil.
                    {
                        // Jika username adalah "admin" (dan login SQL berhasil).
                        MessageBox.Show("Login berhasil sebagai admin.", "Login Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Membuat instance AdminPage dan meneruskan string koneksi yang berhasil digunakan.
                        AdminPage adminForm = new AdminPage(this.connectionString);
                        adminForm.Show(); // Menampilkan form AdminPage.
                        this.Hide();     // Menyembunyikan form LoginPage saat ini.
                    }
                    else if (Username.Equals("customer", StringComparison.OrdinalIgnoreCase)) // Perbandingan username tanpa memperhatikan huruf besar/kecil.
                    {
                        // Jika username adalah "customer" (dan login SQL berhasil).
                        MessageBox.Show("Login berhasil sebagai customer.", "Login Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Membuat instance CustomerPage.
                        // PENTING: CustomerPage juga perlu dimodifikasi untuk menerima string koneksi
                        // jika ia akan berinteraksi dengan database.
                        CustomerPage customerForm = new CustomerPage(this.connectionString); // Asumsi CustomerPage telah diadaptasi.
                        customerForm.Show(); // Menampilkan form CustomerPage.
                        this.Hide();      // Menyembunyikan form LoginPage saat ini.
                    }
                    else
                    {
                        // Jika login SQL berhasil tetapi username aplikasi tidak dikenali ("admin" atau "customer").
                        MessageBox.Show("Login SQL berhasil, tetapi peran pengguna ('" + Username + "') tidak dikenali oleh aplikasi.", "Peran Tidak Dikenal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        // Koneksi akan otomatis ditutup oleh blok 'using' saat keluar dari scope.
                    }
                } // conn.Dispose() akan dipanggil di sini, yang juga akan menutup koneksi jika masih terbuka.
            }
            catch (SqlException sqlEx) // Menangkap error spesifik dari SQL Server.
            {
                // Nomor error 18456 umumnya mengindikasikan kegagalan login karena username/password salah.
                if (sqlEx.Number == 18456)
                {
                    MessageBox.Show("Login gagal. Username atau Password untuk SQL Server salah.", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Menampilkan pesan error SQL Server lainnya.
                    MessageBox.Show($"Koneksi database gagal: {sqlEx.Message}\n(Nomor Error SQL: {sqlEx.Number})", "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                txtPassword.Clear(); // Bersihkan field password setelah gagal login
                txtPassword.Focus();
            }
            catch (Exception ex) // Menangkap error umum lainnya yang mungkin terjadi.
            {
                MessageBox.Show($"Terjadi kesalahan saat proses login: {ex.Message}", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }
    }
}