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
        private readonly Koneksi kn = new Koneksi();

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
            InitializeComponent();
            txtUsername.Focus();
        }

        // Event handler yang dipanggil saat form LoginPage dimuat.
        // Biasanya digunakan untuk inisialisasi tambahan jika diperlukan.
        

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
                // 1. Ambil koneksi dasar (Server=...;Database=...) dari class Koneksi.
                string baseConnectionString = kn.connectionString();
                if (string.IsNullOrEmpty(baseConnectionString))
                {
                    throw new Exception("Gagal mendapatkan konfigurasi dasar koneksi dari Koneksi.cs");
                }

                // 2. Gabungkan koneksi dasar dengan username & password dari input pengguna.
                string dynamicConnectionString = $"{baseConnectionString}User ID={Username};Password={Password};";

                // 3. Gunakan koneksi dinamis ini untuk mencoba login.
                // 'using' block akan memastikan koneksi ditutup otomatis.
                using (SqlConnection conn = new SqlConnection(dynamicConnectionString))
                {
                    // 'conn.Open()' sekarang menjadi kunci validasi.
                    // Jika username atau password salah, baris ini akan melempar SqlException.
                    conn.Open();

                    // Jika kode sampai di sini, artinya login BERHASIL.
                    // Lanjutkan dengan logika peran (admin/customer).
                    if (Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("Login berhasil sebagai admin.", "Login Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Teruskan koneksi yang SUDAH TERVALIDASI ke halaman berikutnya.
                        AdminPage adminForm = new AdminPage(dynamicConnectionString);
                        adminForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Asumsi semua user selain "admin" adalah customer
                        MessageBox.Show("Login berhasil sebagai customer.", "Login Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CustomerPage customerForm = new CustomerPage(dynamicConnectionString);
                        customerForm.Show();
                        this.Hide();
                    }
                }
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