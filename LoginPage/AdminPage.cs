using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABDCAFE
{
    // Form AdminPage berfungsi sebagai halaman utama untuk admin
    // Dari sini, admin dapat mengakses fungsionalitas manajemen reservasi dan meja.
    public partial class AdminPage : Form
    {
        // Variabel privat untuk menyimpan string koneksi database yang diterima dari LoginPage.
        // Kata kunci 'readonly' memastikan string koneksi hanya bisa diatur sekali saat konstruktor dipanggil.
        private readonly string pageConnectionString;

        // Konstruktor default ini sebaiknya tidak digunakan jika string koneksi selalu diharapkan.
        // Jika Anda membuka form ini dari designer atau tanpa string koneksi, ini akan dipanggil.
        // Pertimbangkan untuk menghapusnya atau membuatnya private jika tidak diperlukan.
        public AdminPage()
        {
            InitializeComponent();
            // Peringatan jika form ini dibuat tanpa string koneksi yang diperlukan.
            MessageBox.Show("AdminPage dibuat tanpa string koneksi. Fitur database mungkin tidak berfungsi.", "Peringatan Konstruktor", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            // Anda bisa menonaktifkan tombol atau fitur lain di sini jika pageConnectionString null.
            // Misalnya, Anda bisa mengatur this.pageConnectionString ke null atau string kosong
            // dan menambahkan pengecekan sebelum membuka form lain.
        }

        // Konstruktor yang dimodifikasi untuk menerima string koneksi dari LoginPage.
        public AdminPage(string connStr)
        {
            InitializeComponent(); // Inisialisasi komponen-komponen form.

            // Memeriksa apakah string koneksi yang diterima valid.
            if (string.IsNullOrWhiteSpace(connStr))
            {
                MessageBox.Show("String koneksi tidak valid diterima oleh AdminPage.", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Melemparkan exception untuk menghentikan eksekusi lebih lanjut jika koneksi tidak valid.
                throw new ArgumentNullException(nameof(connStr), "String koneksi tidak boleh null atau kosong.");
            }
            this.pageConnectionString = connStr; // Menyimpan string koneksi yang diterima ke field kelas.
        }

        // Event handler yang dipanggil saat form AdminPage dimuat.
        private void AdminForm_Load(object sender, EventArgs e)
        {
            // Logika tambahan dapat ditempatkan di sini jika ada tindakan
            // yang perlu dilakukan saat form pertama kali dimuat,
            // seperti mengatur tampilan awal atau memuat data khusus untuk AdminPage.
            // Saat ini, semua inisialisasi utama dilakukan di konstruktor.
        }

        // Event handler untuk tombol "Reservasi".
        // Membuka form AdminReservasi.
        private void btnReservasi_Click(object sender, EventArgs e)
        {
            // Memeriksa apakah string koneksi telah diinisialisasi.
            if (string.IsNullOrWhiteSpace(this.pageConnectionString))
            {
                MessageBox.Show("String koneksi tidak tersedia. Tidak dapat membuka halaman reservasi.", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Menghentikan eksekusi jika tidak ada string koneksi.
            }
            // Membuat instance baru dari AdminReservasi, meneruskan string koneksi.
            AdminReservasi ar = new AdminReservasi(this.pageConnectionString);
            ar.Show(); // Menampilkan form AdminReservasi.
            this.Close(); // Menutup form AdminPage saat ini setelah membuka form baru.
        }

        // Event handler untuk tombol "Meja".
        // Membuka form AdminMeja.
        private void btnMeja_Click(object sender, EventArgs e)
        {
            // Memeriksa apakah string koneksi telah diinisialisasi.
            if (string.IsNullOrWhiteSpace(this.pageConnectionString))
            {
                MessageBox.Show("String koneksi tidak tersedia. Tidak dapat membuka halaman meja.", "Kesalahan Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Menghentikan eksekusi jika tidak ada string koneksi.
            }
            // Membuat instance baru dari AdminMeja, meneruskan string koneksi.
            AdminMeja am = new AdminMeja(this.pageConnectionString);
            am.Show(); // Menampilkan form AdminMeja.
            this.Close(); // Menutup form AdminPage saat ini setelah membuka form baru.
        }

        // Event handler untuk tombol "Logout".
        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Menampilkan dialog konfirmasi sebelum logout.
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Jika pengguna mengklik "Yes" pada dialog konfirmasi.
            if (result == DialogResult.Yes)
            {
                // Kembali ke LoginPage.
                LoginPage login = new LoginPage(); // Membuat instance baru dari LoginPage.
                login.Show(); // Menampilkan LoginPage.
                this.Close(); // Menutup form AdminPage saat ini.
            }
            else
            {
                // Jika pengguna mengklik "No", tidak ada tindakan yang diambil (proses logout dibatalkan).
            }
        }
    }
}