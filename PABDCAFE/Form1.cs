using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class Form1 : Form
    {
        private string connectionString =
            @"Data Source=IDEAPAD5PRO\LILA;
              Initial Catalog=ReservasiCafe;
              Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadComboBoxMeja();
            LoadData();
        }

        private void LoadComboBoxMeja()
        {
            cmbMeja.Items.Clear();
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(
                "SELECT Number_Table FROM Meja WHERE Status_Meja = 'Tersedia'",
                conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
                cmbMeja.Items.Add(rdr.GetString(0));

            // Pastikan hanya bisa pilih daftar
            cmbMeja.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMeja.SelectedIndex = -1;
        }

        private void LoadData()
        {
            using var conn = new SqlConnection(connectionString);
            using var da = new SqlDataAdapter(
                "SELECT ID_Reservasi, Nama_Customer, No_telp, Number_Table, Waktu_Reservasi FROM Reservasi",
                conn);
            var dt = new DataTable();
            da.Fill(dt);
            dgvKafe.DataSource = dt;

            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtNoTelp.Clear();
            cmbMeja.SelectedIndex = -1;
            dtpReservasii.Value = DateTime.Now;
            txtName.Focus();
        }

        private void btnTambah(object sender, EventArgs e)
        {
            // Validasi
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtNoTelp.Text))
            {
                MessageBox.Show("Harap isi Nama dan No Telp!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbMeja.SelectedIndex < 0)
            {
                MessageBox.Show("Harap pilih meja!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // INSERT ke DB
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(@"
                INSERT INTO Reservasi
                   (Nama_Customer, No_telp, Waktu_Reservasi, Number_Table)
                VALUES
                   (@Nama, @Telp, @Waktu, @Meja)", conn);
            cmd.Parameters.AddWithValue("@Nama", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Telp", txtNoTelp.Text.Trim());
            cmd.Parameters.AddWithValue("@Waktu", dtpReservasii.Value);
            cmd.Parameters.AddWithValue("@Meja", cmbMeja.SelectedItem.ToString());
            conn.Open();
            cmd.ExecuteNonQuery();

            MessageBox.Show("Data berhasil ditambahkan!", "Sukses",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadData();
        }

        private void btnEdit(object sender, EventArgs e)
        {
            if (dgvKafe.CurrentRow == null)
            {
                MessageBox.Show("Pilih baris untuk diedit!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvKafe.CurrentRow.Cells["ID_Reservasi"].Value;

            // Validasi sama seperti tambah
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtNoTelp.Text) ||
                cmbMeja.SelectedIndex < 0)
            {
                MessageBox.Show("Harap lengkapi data!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // UPDATE DB
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(@"
                UPDATE Reservasi
                SET
                  Nama_Customer   = @Nama,
                  No_telp         = @Telp,
                  Waktu_Reservasi = @Waktu,
                  Number_Table    = @Meja
                WHERE ID_Reservasi = @ID", conn);
            cmd.Parameters.AddWithValue("@Nama", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Telp", txtNoTelp.Text.Trim());
            cmd.Parameters.AddWithValue("@Waktu", dtpReservasii.Value);
            cmd.Parameters.AddWithValue("@Meja", cmbMeja.SelectedItem.ToString());
            cmd.Parameters.AddWithValue("@ID", id);
            conn.Open();
            cmd.ExecuteNonQuery();

            MessageBox.Show("Data berhasil diperbarui!", "Sukses",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadData();
        }

        private void btnHapus(object sender, EventArgs e)
        {
            if (dgvKafe.CurrentRow == null)
            {
                MessageBox.Show("Pilih baris untuk dihapus!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resp = MessageBox.Show(
                "Yakin ingin menghapus data?",
                "Konfirmasi",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (resp != DialogResult.Yes) return;

            int id = (int)dgvKafe.CurrentRow.Cells["ID_Reservasi"].Value;
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(
                "DELETE FROM Reservasi WHERE ID_Reservasi = @ID", conn);
            cmd.Parameters.AddWithValue("@ID", id);
            conn.Open();
            cmd.ExecuteNonQuery();

            MessageBox.Show("Data berhasil dihapus!", "Sukses",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadData();
        }

        private void btnRefresh(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show(
                $"Baris: {dgvKafe.RowCount}, Kolom: {dgvKafe.ColumnCount}",
                "Info Grid", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
