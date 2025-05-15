using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class AdminReservasi : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-4FJGLBGI\\NANDA; Initial Catalog=ReservasiCafe; Integrated Security=True;");
        public AdminReservasi()
        {
            InitializeComponent();
        }

        private bool ValidasiInput()
        {
            // Validasi nama
            if (string.IsNullOrWhiteSpace(txtArNama.Text))
            {
                MessageBox.Show("Nama tidak boleh kosong.");
                return false;
            }

            // Validasi nomor telepon: hanya angka atau +62
            string telp = txtArNoTelp.Text.Trim();
            if (string.IsNullOrWhiteSpace(telp) ||
                !(Regex.IsMatch(telp, @"^\+62\d{8,12}$") || Regex.IsMatch(telp, @"^\d{10,15}$")))
            {
                MessageBox.Show("Nomor telepon tidak valid. Gunakan format +62xxxxxxxxxx atau 08123456789.");
                return false;
            }

            // Validasi waktu reservasi
            if (!DateTime.TryParse(txtArWaktu.Text, out DateTime waktu))
            {
                MessageBox.Show("Format waktu tidak valid. Gunakan format yyyy-MM-dd HH:mm.");
                return false;
            }
            if (waktu.Year != 2025)
            {
                MessageBox.Show("Reservasi hanya untuk tahun 2025.");
                return false;
            }

            // Validasi nomor meja
            if (string.IsNullOrWhiteSpace(txtArMeja.Text) || txtArMeja.Text.Length > 2)
            {
                MessageBox.Show("Nomor meja tidak valid (maks 2 digit).");
                return false;
            }

            return true;
        }

        private void btnArAdd_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput()) return;

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Reservasi (Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja) " +
                    "VALUES (@Nama, @Telp, @Waktu, @Meja)", conn);
                cmd.Parameters.AddWithValue("@Nama", txtArNama.Text.Trim());
                cmd.Parameters.AddWithValue("@Telp", txtArNoTelp.Text.Trim());
                cmd.Parameters.AddWithValue("@Waktu", DateTime.Parse(txtArWaktu.Text.Trim()));
                cmd.Parameters.AddWithValue("@Meja", txtArMeja.Text.Trim());

                int result = cmd.ExecuteNonQuery();
                MessageBox.Show(result > 0 ? "Reservasi berhasil ditambahkan." : "Reservasi gagal ditambahkan.");

                LoadReservasi();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Tambah_Load(object sender, EventArgs e)
        {
            LoadReservasi();
        }

        private void LoadReservasi()
        {
            try
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Reservasi", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewTr.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void btnCustHapus_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArNama.Text) || string.IsNullOrWhiteSpace(txtArWaktu.Text))
            {
                MessageBox.Show("Masukkan nama dan waktu reservasi untuk menghapus.");
                return;
            }

            if (!DateTime.TryParse(txtArWaktu.Text.Trim(), out DateTime waktu))
            {
                MessageBox.Show("Format waktu tidak valid.");
                return;
            }

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Reservasi WHERE Nama_Customer = @Nama AND Waktu_Reservasi = @Waktu", conn);
                cmd.Parameters.AddWithValue("@Nama", txtArNama.Text.Trim());
                cmd.Parameters.AddWithValue("@Waktu", waktu);

                int result = cmd.ExecuteNonQuery();
                MessageBox.Show(result > 0 ? "Reservasi berhasil dihapus." : "Reservasi tidak ditemukan.");

                LoadReservasi();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menghapus: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void ClearForm()
        {
            txtArNama.Text = "";
            txtArNoTelp.Text = "";
            txtArWaktu.Text = "";
            txtArMeja.Text = "";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Kembali ke LoginPage
                LoginPage login = new LoginPage();
                login.Show();
                this.Close();
            }
            else
            {
                // cancel
            }
        }

        private void btnCustLihat_Click(object sender, EventArgs e)
        {
            // panggil fungsi utama
        }
        private void btnArHapus_Click(object sender, EventArgs e)
        {
            // panggil fungsi utama
        }
        private void dgvCutomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // isi textbox ketika klik row di datagrid (jika perlu)
            if (e.RowIndex >= 0 && dataGridViewTr.Rows.Count > e.RowIndex)
            {
                txtArNama.Text = dataGridViewTr.Rows[e.RowIndex].Cells["Nama_Customer"].Value?.ToString();
                txtArNoTelp.Text = dataGridViewTr.Rows[e.RowIndex].Cells["No_Telp"].Value?.ToString();
                txtArWaktu.Text = dataGridViewTr.Rows[e.RowIndex].Cells["Waktu_Reservasi"].Value?.ToString();
                txtArMeja.Text = dataGridViewTr.Rows[e.RowIndex].Cells["Nomor_Meja"].Value?.ToString();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminPage ap = new AdminPage();
            ap.Show();
            this.Close();
        }
    }
}
