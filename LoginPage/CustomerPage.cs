using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class CustomerPage : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-4FJGLBGI\\NANDA; Initial Catalog=ReservasiCafe; Integrated Security=True;");

        public CustomerPage()
        {
            InitializeComponent();
        }

        private bool ValidasiInput()
        {
            string nama = txtCustNama.Text.Trim();
            string telp = txtCustNoTelp.Text.Trim();
            string meja = cmbCustMeja.Text.Trim();
            string waktuStr = dtpCustWaktu.Text.Trim();

            if (string.IsNullOrWhiteSpace(nama) || nama.Length < 3)
            {
                MessageBox.Show("Nama customer tidak boleh kosong dan minimal 3 karakter.");
                return false;
            }

            if (!Regex.IsMatch(telp, @"^\+62\d{8,12}$") && !Regex.IsMatch(telp, @"^\d{10,15}$"))
            {
                MessageBox.Show("Nomor telepon tidak valid. Gunakan +62xxxxxxxxxx atau 08123456789.");
                return false;
            }

            if (!DateTime.TryParse(waktuStr, out DateTime waktu))
            {
                MessageBox.Show("Format waktu tidak valid. Gunakan format yyyy-MM-dd HH:mm.");
                return false;
            }

            if (waktu.Year != 2025)
            {
                MessageBox.Show("Reservasi hanya boleh di tahun 2025.");
                return false;
            }

            try
            {
                conn.Open();

                // Cek apakah sudah ada reservasi di meja itu pada waktu yang sama
                SqlCommand cekJadwal = new SqlCommand(
                    "SELECT COUNT(*) FROM Reservasi WHERE Nomor_Meja = @Meja AND Waktu_Reservasi = @Waktu", conn);
                cekJadwal.Parameters.AddWithValue("@Meja", meja);
                cekJadwal.Parameters.AddWithValue("@Waktu", waktu);
                int count = (int)cekJadwal.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Waktu dan meja sudah dibooking. Silakan pilih waktu atau meja lain.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kesalahan validasi: " + ex.Message);
                return false;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnCustTambah_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput()) return;

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Reservasi (Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja) " +
                    "VALUES (@Nama, @Telp, @Waktu, @Meja)", conn);
                cmd.Parameters.AddWithValue("@Nama", txtCustNama.Text.Trim());
                cmd.Parameters.AddWithValue("@Telp", txtCustNoTelp.Text.Trim());
                cmd.Parameters.AddWithValue("@Waktu", dtpCustWaktu.Value);
                cmd.Parameters.AddWithValue("@Meja", cmbCustMeja.Text.Trim());

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    // Update status meja menjadi "Dipesan"
                    SqlCommand updateMeja = new SqlCommand("UPDATE Meja SET Status_Meja = 'Dipesan' WHERE Nomor_Meja = @Meja", conn);
                    updateMeja.Parameters.AddWithValue("@Meja", cmbCustMeja.Text.Trim());
                    updateMeja.ExecuteNonQuery();

                    MessageBox.Show("Reservasi berhasil ditambahkan.");
                }
                else
                {
                    MessageBox.Show("Reservasi gagal ditambahkan.");
                }

                LoadReservasi();
                LoadMejaTersedia();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void btnCustHapus_Click(object sender, EventArgs e)
        {
            string nama = txtCustNama.Text.Trim();
            string waktuStr = dtpCustWaktu.Text.Trim();
            string meja = cmbCustMeja.Text.Trim();

            if (string.IsNullOrWhiteSpace(nama) || string.IsNullOrWhiteSpace(waktuStr))
            {
                MessageBox.Show("Isi Nama dan Waktu Reservasi terlebih dahulu.");
                return;
            }

            if (!DateTime.TryParse(waktuStr, out DateTime waktu))
            {
                MessageBox.Show("Format waktu tidak valid.");
                return;
            }

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Reservasi WHERE Nama_Customer = @Nama AND Waktu_Reservasi = @Waktu", conn);
                cmd.Parameters.AddWithValue("@Nama", nama);
                cmd.Parameters.AddWithValue("@Waktu", waktu);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    // Kembalikan status meja menjadi "Tersedia"
                    SqlCommand updateMeja = new SqlCommand("UPDATE Meja SET Status_Meja = 'Tersedia' WHERE Nomor_Meja = @Meja", conn);
                    updateMeja.Parameters.AddWithValue("@Meja", meja);
                    updateMeja.ExecuteNonQuery();

                    MessageBox.Show("Reservasi berhasil dihapus.");
                }
                else
                {
                    MessageBox.Show("Reservasi tidak ditemukan.");
                }

                LoadReservasi();
                LoadMejaTersedia();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menghapus: " + ex.Message);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        private void CustomerPage_Load(object sender, EventArgs e)
        {
            LoadReservasi();
            LoadMejaTersedia();
        }

        private void LoadReservasi()
        {
            try
            {
                using (SqlConnection localConn = new SqlConnection(conn.ConnectionString))
                {
                    string query = "SELECT * FROM Reservasi";
                    SqlDataAdapter da = new SqlDataAdapter(query, localConn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvCustomer.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menampilkan data: " + ex.Message);
            }
        }

        private void LoadMejaTersedia()
        {
            try
            {
                cmbCustMeja.Items.Clear();

                using (SqlConnection localConn = new SqlConnection(conn.ConnectionString))
                {
                    localConn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja WHERE Status_Meja = 'Tersedia'", localConn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbCustMeja.Items.Add(reader["Nomor_Meja"].ToString());
                    }
                }

                if (cmbCustMeja.Items.Count > 0)
                    cmbCustMeja.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat meja tersedia: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtCustNama.Clear();
            txtCustNoTelp.Clear();
            dtpCustWaktu.Value = DateTime.Now;
            cmbCustMeja.SelectedIndex = cmbCustMeja.Items.Count > 0 ? 0 : -1;
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

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomer.Rows.Count > e.RowIndex)
            {
                txtCustNama.Text = dgvCustomer.Rows[e.RowIndex].Cells["Nama_Customer"].Value?.ToString();
                txtCustNoTelp.Text = dgvCustomer.Rows[e.RowIndex].Cells["No_Telp"].Value?.ToString();
                dtpCustWaktu.Value = Convert.ToDateTime(dgvCustomer.Rows[e.RowIndex].Cells["Waktu_Reservasi"].Value);
                cmbCustMeja.Text = dgvCustomer.Rows[e.RowIndex].Cells["Nomor_Meja"].Value?.ToString();
            }
        }

        private void dtpCustWaktu_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
