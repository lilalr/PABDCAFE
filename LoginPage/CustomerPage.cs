using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class CustomerPage : Form
    {
        private readonly string connectionString = "Data Source=LAPTOP-4FJGLBGI\\NANDA;Initial Catalog=ReservasiCafe;Integrated Security=true";

        private readonly MemoryCache _cache = MemoryCache.Default;
        private readonly CacheItemPolicy _policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5)
        };
        private const string CacheKey = "ReservasiCafeData";

        public CustomerPage()
        {
            InitializeComponent();
        }

        private void CustomerPage_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void ClearForm()
        {
            txtCustNama.Clear();
            txtCustNoTelp.Clear();
            txtCustWaktu.Clear();
            txtCustMeja.Clear();
            dgvCustomer.ClearSelection();
            txtCustNama.Focus(); // perbaikan dari CustNama
        }

        private void LoadData()
        {
            DataTable dt;
            if (_cache.Contains(CacheKey))
            {
                dt = _cache.Get(CacheKey) as DataTable;
            }
            else
            {
                dt = new DataTable();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var query = "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM dbo.ReservasiCafe";
                    var da = new SqlDataAdapter(query, conn);
                    da.Fill(dt);
                }
                _cache.Add(CacheKey, dt, _policy);
            }

            dgvCustomer.AutoGenerateColumns = true;
            dgvCustomer.DataSource = dt;
        }

        private void btnCustTambah_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustNama.Text) ||
                string.IsNullOrWhiteSpace(txtCustNoTelp.Text) ||
                string.IsNullOrWhiteSpace(txtCustWaktu.Text) ||
                string.IsNullOrWhiteSpace(txtCustMeja.Text))
            {
                MessageBox.Show("Harap isi semua data!", "Peringatan");
                return;
            }

            if (!DateTime.TryParse(txtCustWaktu.Text.Trim(), out DateTime waktuReservasi))
            {
                MessageBox.Show("Format waktu tidak valid. Gunakan format: yyyy-MM-dd HH:mm", "Kesalahan");
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("AddReservasiCafe", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Nama_Customer", txtCustNama.Text.Trim());
                        cmd.Parameters.AddWithValue("@No_telp", txtCustNoTelp.Text.Trim());
                        cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                        cmd.Parameters.AddWithValue("@Nomor_Meja", txtCustMeja.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                _cache.Remove(CacheKey);
                MessageBox.Show("Data berhasil ditambahkan!", "Sukses");
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan");
            }
        }

        private void btnCustHapus_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                var ID_Reservasi = dgvCustomer.SelectedRows[0].Cells["ID_Reservasi"].Value.ToString();
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("DeleteReservasiCafe", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_Reservasi", ID_Reservasi);
                        cmd.ExecuteNonQuery();
                    }
                }

                _cache.Remove(CacheKey);
                MessageBox.Show("Data berhasil dihapus!", "Sukses");
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan");
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin logout?", "Konfirmasi Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginPage login = new LoginPage();
                login.Show();
                this.Close();
            }
        }

        private void btnCustLihat_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvCutomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kosong: Bisa diisi fitur klik untuk edit jika ingin
        }
    }
}
