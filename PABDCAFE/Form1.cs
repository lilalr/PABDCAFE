using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class Form1 : Form
    {
        private DataTable dtReservasi = new DataTable();

        public Form1()
        {
            InitializeComponent();
            SetupTable();
            dgvKafe.SelectionChanged += dgvKafe_SelectionChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvKafe.DataSource = dtReservasi;
            ClearForm();
        }

        private void SetupTable()
        {
            dtReservasi.Columns.Add("ID", typeof(int));
            dtReservasi.Columns.Add("Nama");
            dtReservasi.Columns.Add("NoTelp");
            dtReservasi.Columns.Add("Meja");
            dtReservasi.Columns.Add("Waktu", typeof(DateTime));
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtNoTelp.Clear();
            cmbMeja.Items.Clear();
            cmbMeja.Items.AddRange(new object[] { "01", "02", "03", "04", "05" });
            cmbMeja.SelectedIndex = -1;
            dtpReservasii.Value = DateTime.Now;
            txtName.Focus();
        }

        private void btnTambah(object sender, EventArgs e)
        {
            // Validasi sederhana
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtNoTelp.Text) ||
                cmbMeja.SelectedIndex < 0)
            {
                MessageBox.Show("Lengkapi semua field!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Cek unik meja
            var mejaDipilih = cmbMeja.SelectedItem.ToString();
            bool sudahDipakai = dtReservasi.AsEnumerable()
                .Any(r => r.Field<string>("Meja") == mejaDipilih);
            if (sudahDipakai)
            {
                MessageBox.Show($"Meja {mejaDipilih} sudah dipakai!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tambah
            int id = dtReservasi.Rows.Count + 1;
            dtReservasi.Rows.Add(
                id,
                txtName.Text.Trim(),
                txtNoTelp.Text.Trim(),
                mejaDipilih,
                dtpReservasii.Value
            );
            MessageBox.Show("Data berhasil ditambahkan!", "Sukses",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
        }

        private void btnEdit(object sender, EventArgs e)
        {
            if (dgvKafe.CurrentRow == null)
            {
                MessageBox.Show("Pilih baris untuk diedit!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mejaDipilih = cmbMeja.SelectedItem.ToString();
            int idx = dgvKafe.CurrentRow.Index;

            // Cek unik meja: boleh sama dengan baris sendiri, tapi tidak sama dengan baris lain
            bool sudahDipakai = dtReservasi.AsEnumerable()
                .Where((r, i) => i != idx)
                .Any(r => r.Field<string>("Meja") == mejaDipilih);
            if (sudahDipakai)
            {
                MessageBox.Show($"Meja {mejaDipilih} sudah dipakai!", "Peringatan",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Perbarui
            dtReservasi.Rows[idx]["Nama"] = txtName.Text.Trim();
            dtReservasi.Rows[idx]["NoTelp"] = txtNoTelp.Text.Trim();
            dtReservasi.Rows[idx]["Meja"] = mejaDipilih;
            dtReservasi.Rows[idx]["Waktu"] = dtpReservasii.Value;

            MessageBox.Show("Data berhasil diperbarui!", "Sukses",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            ClearForm();
        }


        

        private void dgvKafe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvKafe.CurrentRow == null) return;
            txtName.Text = dgvKafe.CurrentRow.Cells["Nama"].Value?.ToString();
            txtNoTelp.Text = dgvKafe.CurrentRow.Cells["NoTelp"].Value?.ToString();
            cmbMeja.SelectedItem = dgvKafe.CurrentRow.Cells["Meja"].Value;
            dtpReservasii.Value = (DateTime)dgvKafe.CurrentRow.Cells["Waktu"].Value;
        }

        private void btnHapus(object sender, EventArgs e)
        {
            if (dgvKafe.CurrentRow == null) return;

            var res = MessageBox.Show(
                "Yakin menghapus data ini?",
                "Konfirmasi Hapus",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                dtReservasi.Rows.RemoveAt(dgvKafe.CurrentRow.Index);
                MessageBox.Show("Data berhasil dihapus!", "Sukses",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide(); // 
            Form1 formUtama = new Form1(); // 
            formUtama.Show(); 
        }


        private void btnRefresh(object sender, EventArgs e)
        {
            dgvKafe.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtChoose_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        }
    }
