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
            LoadData();
        }

        void LoadData()
        {
            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Reservasi", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvAdminReservasi.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message);
            }
        }

        void ClearForm()
        {
            txtNama.Clear();
            txtTelepon.Clear();
            txtWaktu.Clear();
            txtMeja.Clear();
        }

        bool ValidasiInput(out string err)
        {
            err = "";
            if (string.IsNullOrWhiteSpace(txtNama.Text))
                err += "Nama customer tidak boleh kosong.\n";

            if (!Regex.IsMatch(txtTelepon.Text.Trim(), @"^(\+62\d{8,12}|\d{10,15})$"))
                err += "Nomor telepon tidak valid.\n";

            if (!DateTime.TryParse(txtWaktu.Text.Trim(), out DateTime waktu))
                err += "Format waktu tidak valid.\n";
            else if (waktu.Year != 2025)
                err += "Waktu reservasi harus tahun 2025.\n";

            if (string.IsNullOrWhiteSpace(txtMeja.Text) || txtMeja.Text.Length != 2)
                err += "Nomor meja harus 2 karakter.\n";

            return err == "";
        }

        private void btnArEdit_Click(object sender, EventArgs e)
        {
            if (dgvAdminReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data reservasi yang ingin diedit.");
                return;
            }

            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idReservasi = Convert.ToInt32(dgvAdminReservasi.SelectedRows[0].Cells["ID_Reservasi"].Value);

                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Reservasi SET Nama_Customer=@Nama, No_Telp=@Telp, Waktu_Reservasi=@Waktu, Nomor_Meja=@Meja WHERE ID_Reservasi=@ID", conn);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text.Trim());
                cmd.Parameters.AddWithValue("@Telp", txtTelepon.Text.Trim());
                cmd.Parameters.AddWithValue("@Waktu", DateTime.Parse(txtWaktu.Text.Trim()));
                cmd.Parameters.AddWithValue("@Meja", txtMeja.Text.Trim());
                cmd.Parameters.AddWithValue("@ID", idReservasi);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Reservasi berhasil diperbarui!");
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memperbarui data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminPage ap = new AdminPage();
            ap.Show();
            this.Close();
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Reservasi (Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja) VALUES (@Nama, @Telp, @Waktu, @Meja)", conn);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text.Trim());
                cmd.Parameters.AddWithValue("@Telp", txtTelepon.Text.Trim());
                cmd.Parameters.AddWithValue("@Waktu", DateTime.Parse(txtWaktu.Text.Trim()));
                cmd.Parameters.AddWithValue("@Meja", txtMeja.Text.Trim());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Reservasi berhasil ditambahkan!");
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menambahkan data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvAdminReservasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih data yang ingin dihapus.");
                return;
            }

            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            try
            {
                int idReservasi = Convert.ToInt32(dgvAdminReservasi.SelectedRows[0].Cells["ID_Reservasi"].Value);

                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Reservasi WHERE ID_Reservasi=@ID", conn);
                cmd.Parameters.AddWithValue("@ID", idReservasi);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Reservasi berhasil dihapus!");
                LoadData();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus data: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        private void dgvAdminReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtNama.Text = dgvAdminReservasi.Rows[e.RowIndex].Cells["Nama_Customer"].Value?.ToString();
                txtTelepon.Text = dgvAdminReservasi.Rows[e.RowIndex].Cells["No_Telp"].Value?.ToString();
                txtWaktu.Text = dgvAdminReservasi.Rows[e.RowIndex].Cells["Waktu_Reservasi"].Value?.ToString();
                txtMeja.Text = dgvAdminReservasi.Rows[e.RowIndex].Cells["Nomor_Meja"].Value?.ToString();
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }

        // (-) BANYAKKKK, GATAU DEH NGANTUKKK
    }
}
