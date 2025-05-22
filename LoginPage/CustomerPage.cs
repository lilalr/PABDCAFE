using System;

using System.Collections.Generic;

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



        private void CustomerPage_Load(object sender, EventArgs e)

        {

            LoadReservasi();

            LoadComboBoxMeja(cmbCustMeja);



            dtpCustWaktu.Format = DateTimePickerFormat.Custom;

            dtpCustWaktu.CustomFormat = "yyyy-MM-dd HH:mm";

            dtpCustWaktu.ShowUpDown = false;

        }



        private void LoadComboBoxMeja(ComboBox cmb)

        {

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Query untuk mendapatkan Nomor_Meja dari tabel Meja yang statusnya 'Tersedia'
                // Sesuai dengan skema database yang Anda berikan.
                using (SqlCommand cmd = new SqlCommand("SELECT Nomor_Meja FROM Meja WHERE Status_Meja = 'Tersedia' ORDER BY Nomor_Meja", conn))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<string> mejaList = new List<string>();
                    while (reader.Read())
                    {
                        mejaList.Add(reader["Nomor_Meja"].ToString());
                    }
                    reader.Close();

                    // Mengisi DataSource ComboBox
                    cmb.DataSource = mejaList;
                    if (mejaList.Count > 0)
                    {
                        cmb.SelectedIndex = -1; // Tidak ada seleksi default
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
                    conn.Close();
                }
            }
        }



        private bool ValidasiInput()

        {

            string nama = txtCustNama.Text.Trim();

            string telp = txtCustNoTelp.Text.Trim();

            string meja = cmbCustMeja.Text.Trim();

            DateTime waktu = dtpCustWaktu.Value;



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



            if (waktu.Year != 2025)

            {

                MessageBox.Show("Reservasi hanya boleh di tahun 2025.");

                return false;

            }



            if (string.IsNullOrWhiteSpace(meja))

            {

                MessageBox.Show("Silakan pilih nomor meja yang tersedia.");

                return false;

            }



            try

            {

                conn.Open();



                SqlCommand cekMeja = new SqlCommand("SELECT Status_Meja FROM Meja WHERE Nomor_Meja = @Meja", conn);

                cekMeja.Parameters.AddWithValue("@Meja", meja);

                object status = cekMeja.ExecuteScalar();



                if (status == null)

                {

                    MessageBox.Show("Nomor meja tidak ditemukan.");

                    return false;

                }



                if (status.ToString() == "Dipesan")

                {

                    MessageBox.Show("Meja tersebut sedang dipesan. Pilih meja lain.");

                    return false;

                }



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

                MessageBox.Show(result > 0 ? "Reservasi berhasil ditambahkan." : "Reservasi gagal ditambahkan.");



                LoadReservasi();
                LoadComboBoxMeja(cmbCustMeja);
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



        private void btnCustHapus_Click(object sender, EventArgs e)

        {

            if (string.IsNullOrWhiteSpace(txtCustNama.Text))

            {

                MessageBox.Show("Isi Nama terlebih dahulu.");

                return;

            }



            try

            {

                conn.Open();

                SqlCommand cmd = new SqlCommand(

                  "DELETE FROM Reservasi WHERE Nama_Customer = @Nama AND Waktu_Reservasi = @Waktu", conn);

                cmd.Parameters.AddWithValue("@Nama", txtCustNama.Text.Trim());

                cmd.Parameters.AddWithValue("@Waktu", dtpCustWaktu.Value);



                int result = cmd.ExecuteNonQuery();

                MessageBox.Show(result > 0 ? "Reservasi berhasil dihapus." : "Reservasi tidak ditemukan.");



                LoadReservasi();

                LoadComboBoxMeja(cmbCustMeja);

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



        private void ClearForm()

        {

            txtCustNama.Clear();

            txtCustNoTelp.Clear();

            cmbCustMeja.SelectedIndex = -1;

            dtpCustWaktu.Value = DateTime.Now;

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



    }

}