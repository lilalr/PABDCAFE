using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO; // Diperlukan untuk System.IO.File

namespace PABDCAFE
{
    public partial class AdminReservasi : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-4FJGLBGI\\NANDA; Initial Catalog=ReservasiCafe; Integrated Security=True;");


        public AdminReservasi()
        {
            InitializeComponent(); // Pastikan dtpWaktuReservasi sudah diinisialisasi di sini
            LoadData();
            SetupDateTimePicker();
        }

        private void SetupDateTimePicker()
        {
            // Pastikan dtpWaktuReservasi tidak null jika Anda menambahkannya melalui designer
            Control[] controls = this.Controls.Find("dtpWaktuReservasi", true);
            if (controls.Length > 0 && controls[0] is DateTimePicker dtp)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "yyyy-MM-dd HH:mm"; // Format tanggal dan waktu
                dtp.Value = DateTime.Now; // Default ke waktu sekarang
            }
            // Jika Anda memiliki beberapa DateTimePicker atau namanya berbeda, sesuaikan pencariannya.
        }


        void LoadData()
        {
            try
            {
                using (var da = new SqlDataAdapter(
                    "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi", conn))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    dgvAdminReservasi.DataSource = dt;

                    // Atur header dan format
                    if (dgvAdminReservasi.Columns.Contains("ID_Reservasi"))
                        dgvAdminReservasi.Columns["ID_Reservasi"].HeaderText = "ID";
                    if (dgvAdminReservasi.Columns.Contains("Nama_Customer"))
                        dgvAdminReservasi.Columns["Nama_Customer"].HeaderText = "Nama Customer";
                    if (dgvAdminReservasi.Columns.Contains("No_Telp"))
                        dgvAdminReservasi.Columns["No_Telp"].HeaderText = "No. Telepon";
                    if (dgvAdminReservasi.Columns.Contains("Waktu_Reservasi"))
                    {
                        dgvAdminReservasi.Columns["Waktu_Reservasi"].HeaderText = "Waktu Reservasi";
                        dgvAdminReservasi.Columns["Waktu_Reservasi"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
                    }
                    if (dgvAdminReservasi.Columns.Contains("Nomor_Meja"))
                        dgvAdminReservasi.Columns["Nomor_Meja"].HeaderText = "Nomor Meja";

                    dgvAdminReservasi.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data: " + ex.Message, "Kesalahan Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void AdminReservasi_Load(object sender, EventArgs e)
        {
            // throw new NotImplementedException(); // Hapus ini
            // LoadData() sudah dipanggil di constructor.
            // SetupDateTimePicker() juga sudah dipanggil di constructor.
        }

        void ClearForm()
        {
            txtNama.Clear();
            txtTelepon.Clear();
            // txtWaktu.Clear(); // Dihapus karena txtWaktu diganti dtpWaktuReservasi
            Control[] dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
            if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
            {
                dtp.Value = DateTime.Now.Date.AddHours(DateTime.Now.Hour); // Atur ke tanggal hari ini, jam sekarang, menit 0
            }
            txtMeja.Clear();
            dgvAdminReservasi.ClearSelection();
            txtNama.Focus();
        }


        bool ValidasiInput(out string err)
        {
            err = string.Empty;
            if (string.IsNullOrWhiteSpace(txtNama.Text))
                err += "Nama customer tidak boleh kosong.\n";

            if (!Regex.IsMatch(txtTelepon.Text.Trim(), "^(\\+62\\d{8,12}|0\\d{9,14})$"))
                err += "Format Nomor telepon tidak valid (Contoh: +6281234567890 atau 081234567890).\n";

            var ctrls = this.Controls.Find("dtpWaktuReservasi", true);
            if (ctrls.Length > 0 && ctrls[0] is DateTimePicker dtp)
            {
                // Contoh validasi tahun
                if (dtp.Value.Year != DateTime.Now.Year)
                    err += $"Waktu reservasi harus tahun {DateTime.Now.Year}.\n";
            }
            else
                err += "Kontrol DateTimePicker untuk waktu reservasi tidak ditemukan.\n";

            if (string.IsNullOrWhiteSpace(txtMeja.Text) || !Regex.IsMatch(txtMeja.Text.Trim(), "^\\d{2}$"))
                err += "Nomor meja harus terdiri dari 2 digit angka.\n";

            return string.IsNullOrEmpty(err);
        }

        // Menggunakan btnEdit_Click sebagai handler utama untuk edit
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvAdminReservasi.CurrentRow == null || dgvAdminReservasi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih data reservasi yang ingin diedit dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidasiInput(out string errMsg))
            {
                MessageBox.Show(errMsg, "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int idReservasi = Convert.ToInt32(dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value);
                DateTime waktuReservasi = DateTime.Now; // Default
                Control[] dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
                if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
                {
                    waktuReservasi = dtp.Value;
                }
                else
                {
                    MessageBox.Show("Error: Kontrol DateTimePicker tidak ditemukan.", "Error Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = new SqlCommand("EditReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi); // Menggunakan nilai dari DateTimePicker
                    cmd.Parameters.AddWithValue("@Nomor_Meja_Baru", txtMeja.Text.Trim()); // Pastikan SP Anda menggunakan @Nomor_Meja_Baru atau @Nomor_Meja

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil diperbarui!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak berhasil diperbarui atau tidak ada perubahan data.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("SQL Error: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
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
                DateTime waktuReservasi = DateTime.Now; // Default
                Control[] dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
                if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
                {
                    waktuReservasi = dtp.Value;
                }
                else
                {
                    MessageBox.Show("Error: Kontrol DateTimePicker tidak ditemukan.", "Error Kontrol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi); // Menggunakan nilai dari DateTimePicker
                    cmd.Parameters.AddWithValue("@Nomor_Meja", txtMeja.Text.Trim());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Reservasi berhasil ditambahkan!");
                    LoadData();
                    ClearForm();
                }
            }
            catch (SqlException sqlEx) // Lebih spesifik untuk error dari SP (misal Nomor Meja tidak tersedia, dll)
            {
                MessageBox.Show("Gagal menambahkan reservasi: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menambahkan data: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (dgvAdminReservasi.CurrentRow == null || dgvAdminReservasi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Pilih data reservasi yang ingin dihapus dari tabel.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Penghapusan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            try
            {
                int idReservasi = Convert.ToInt32(dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value);

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                using (SqlCommand cmd = new SqlCommand("HapusReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservasi berhasil dihapus.");
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak ditemukan atau gagal dihapus.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Gagal menghapus data: " + sqlEx.Message, "Kesalahan Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal menghapus data: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }



        // Event handler CellContentClick bisa dihapus jika Anda menggunakan CellClick
        private void dgvAdminReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvAdminReservasi.Rows.Count)
                return;

            var row = dgvAdminReservasi.Rows[e.RowIndex];
            if (row.IsNewRow)
                return;

            txtNama.Text = row.Cells["Nama_Customer"].Value?.ToString() ?? string.Empty;
            txtTelepon.Text = row.Cells["No_Telp"].Value?.ToString() ?? string.Empty;
            txtMeja.Text = row.Cells["Nomor_Meja"].Value?.ToString() ?? string.Empty;

            var ctrls = this.Controls.Find("dtpWaktuReservasi", true);
            if (ctrls.Length > 0 && ctrls[0] is DateTimePicker dtp)
            {
                // Pastikan Min/Max mencakup nilai
                dtp.MinDate = DateTimePicker.MinimumDateTime;
                dtp.MaxDate = DateTimePicker.MaximumDateTime;

                if (row.Cells["Waktu_Reservasi"].Value != null && row.Cells["Waktu_Reservasi"].Value != DBNull.Value)
                {
                    var nilai = Convert.ToDateTime(row.Cells["Waktu_Reservasi"].Value);
                    dtp.Value = (nilai < dtp.MinDate) ? dtp.MinDate : (nilai > dtp.MaxDate ? dtp.MaxDate : nilai);
                }
                else
                {
                    dtp.Value = DateTime.Now;
                }
            }
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"; // Filter lebih baik
            openFileDialog.Title = "Pilih File CSV untuk Impor";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                int successCount = 0;
                int failCount = 0;
                string errorMessages = "";

                try
                {
                    string[] lines = File.ReadAllLines(filePath);
                    if (lines.Length <= 1 && (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0]))) // Mengabaikan header atau file kosong
                    {
                        MessageBox.Show("File CSV kosong atau hanya berisi header.", "Informasi Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (conn.State == ConnectionState.Closed) conn.Open();

                    // Mulai dari baris kedua jika baris pertama adalah header
                    // bool skipHeader = true; // Atur ke true jika CSV punya header
                    for (int i = 0; i < lines.Length; i++) // Ubah i = 1 jika ada header
                    {
                        // if (skipHeader && i == 0) continue; 

                        string line = lines[i];
                        if (string.IsNullOrWhiteSpace(line)) continue; // Lewati baris kosong

                        string[] data = line.Split(',');
                        if (data.Length >= 4) // Pastikan setidaknya ada 4 kolom
                        {
                            try
                            {
                                // Validasi dan parsing data dari CSV
                                string namaCustomer = data[0].Trim();
                                string noTelp = data[1].Trim();
                                if (!DateTime.TryParse(data[2].Trim(), out DateTime waktuReservasi))
                                {
                                    failCount++;
                                    errorMessages += $"Baris {i + 1}: Format waktu '{data[2]}' tidak valid.\n";
                                    continue;
                                }
                                string nomorMeja = data[3].Trim();

                                // Validasi sederhana tambahan (opsional, bisa disamakan dengan ValidasiInput)
                                if (string.IsNullOrWhiteSpace(namaCustomer) ||
                                    !Regex.IsMatch(noTelp, @"^(\+62\d{8,12}|0\d{9,14})$") ||
                                    waktuReservasi.Year != 2025 || // Sesuaikan validasi tahun jika perlu
                                    !Regex.IsMatch(nomorMeja, @"^\d{2}$"))
                                {
                                    failCount++;
                                    errorMessages += $"Baris {i + 1}: Data tidak valid ({namaCustomer}, {noTelp}, {waktuReservasi}, {nomorMeja}).\n";
                                    continue;
                                }


                                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@Nama_Customer", namaCustomer);
                                    cmd.Parameters.AddWithValue("@No_Telp", noTelp);
                                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                                    cmd.Parameters.AddWithValue("@Nomor_Meja", nomorMeja);
                                    cmd.ExecuteNonQuery();
                                    successCount++;
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                failCount++;
                                errorMessages += $"Baris {i + 1} (SQL): {sqlEx.Message}\n";
                            }
                            catch (FormatException formatEx)
                            {
                                failCount++;
                                errorMessages += $"Baris {i + 1} (Format): {formatEx.Message}\n";
                            }
                            catch (Exception ex)
                            {
                                failCount++;
                                errorMessages += $"Baris {i + 1} (Umum): {ex.Message}\n";
                            }
                        }
                        else
                        {
                            failCount++;
                            errorMessages += $"Baris {i + 1}: Jumlah kolom tidak sesuai (Harus 4, ditemukan {data.Length}).\n";
                        }
                    }

                    MessageBox.Show($"Impor selesai.\nBerhasil: {successCount} data.\nGagal: {failCount} data." +
                                    (failCount > 0 ? "\n\nDetail Kegagalan:\n" + errorMessages : ""),
                                    "Hasil Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("Gagal membaca file: " + ioEx.Message, "Kesalahan File IO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal impor: " + ex.Message, "Kesalahan Impor Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        private void dtpWaktuReservasi_ValueChanged(object sender, EventArgs e)
        {
            var ctrls = this.Controls.Find("dtpWaktuReservasi", true);
            if (ctrls.Length > 0 && ctrls[0] is DateTimePicker dtp)
            {
                // Atur format custom
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "yyyy-MM-dd HH:mm";
                // Pastikan rentang tanggal cukup luas
                dtp.MinDate = DateTimePicker.MinimumDateTime;
                dtp.MaxDate = DateTimePicker.MaximumDateTime;
                dtp.Value = DateTime.Now;
            }
        }
    }
}
