using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO; // Diperlukan untuk System.IO.File
using System.Collections.Generic; // Diperlukan untuk List

namespace PABDCAFE
{
    public partial class AdminReservasi : Form
    {
        // Pastikan connection string Anda benar dan database bisa diakses
        SqlConnection conn = new SqlConnection("Data Source=LAPTOP-4FJGLBGI\\NANDA; Initial Catalog=ReservasiCafe; Integrated Security=True;");

        public AdminReservasi()
        {
            InitializeComponent();
            SetupDateTimePicker();
            SetupNomorMejaComboBox(); // Metode untuk mengatur ComboBox Nomor Meja
            LoadData(); // Memuat data awal ke DataGridView
        }

        private void SetupDateTimePicker()
        {
            Control[] controls = this.Controls.Find("dtpWaktuReservasi", true);
            if (controls.Length > 0 && controls[0] is DateTimePicker dtp)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "yyyy-MM-dd HH:mm"; // Format tanggal dan waktu
                dtp.Value = DateTime.Now; // Nilai default adalah waktu saat ini
                // Pastikan properti ShowUpDown di desainer adalah False agar muncul kalender pop-up
            }
        }

        private void SetupNomorMejaComboBox()
        {
            Control[] controls = this.Controls.Find("cbxNomorMeja", true);
            if (controls.Length > 0 && controls[0] is ComboBox cbx)
            {
                cbx.DropDownStyle = ComboBoxStyle.DropDownList; // Membuat ComboBox tidak bisa diketik
                LoadAvailableMeja(cbx); // Memuat daftar meja yang tersedia
            }
            else
            {
                MessageBox.Show("Error: Kontrol ComboBox dengan nama 'cbxNomorMeja' tidak ditemukan di form Anda. Harap tambahkan.", "Kontrol Hilang", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAvailableMeja(ComboBox cbx)
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
                    cbx.DataSource = mejaList;
                    if (mejaList.Count > 0)
                    {
                        cbx.SelectedIndex = -1; // Tidak ada seleksi default
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

                    // Atur header dan format kolom DataGridView
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
            // LoadData() dan SetupDateTimePicker() sudah dipanggil di konstruktor.
            // SetupNomorMejaComboBox() juga sudah dipanggil di konstruktor.
            // Tidak ada kode tambahan yang diperlukan di sini kecuali ada inisialisasi lain yang spesifik untuk event Load.
        }

        void ClearForm()
        {
            txtNama.Clear();
            txtTelepon.Clear();
            Control[] dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
            if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
            {
                dtp.Value = DateTime.Now; // Setel kembali ke waktu saat ini
            }

            // Hapus seleksi ComboBox dan muat ulang daftar meja yang tersedia
            Control[] cbxControls = this.Controls.Find("cbxNomorMeja", true);
            if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
            {
                cbx.SelectedIndex = -1; // Hapus seleksi
                LoadAvailableMeja(cbx); // Muat ulang daftar meja untuk menyegarkan (misal setelah ada perubahan status meja)
            }

            dgvAdminReservasi.ClearSelection();
            txtNama.Focus();
        }

        bool ValidasiInput(out string err)
        {
            err = string.Empty;
            if (string.IsNullOrWhiteSpace(txtNama.Text))
                err += "Nama customer tidak boleh kosong.\n";

            // Validasi format nomor telepon Indonesia (mulai dengan +62 atau 0)
            // Regex ini sesuai dengan constraint CHECK di database Anda
            if (!Regex.IsMatch(txtTelepon.Text.Trim(), @"^(\+62\d{8,12}|0\d{9,14})$"))
                err += "Format nomor telepon tidak valid (Contoh: +6281234567890 atau 081234567890).\n";

            var dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
            if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
            {
                // Validasi waktu reservasi tidak boleh di masa lalu
                if (dtp.Value < DateTime.Now.AddMinutes(-1)) // Beri sedikit toleransi (1 menit)
                {
                    err += "Waktu reservasi tidak boleh di masa lalu.\n";
                }
                // Validasi tahun harus 2025, sesuai dengan constraint database Anda
                if (dtp.Value.Year != 2025)
                {
                    err += "Waktu reservasi hanya diperbolehkan untuk tahun 2025.\n";
                }
            }
            else
            {
                err += "Kontrol DateTimePicker untuk waktu reservasi tidak ditemukan.\n";
            }

            var cbxControls = this.Controls.Find("cbxNomorMeja", true);
            if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
            {
                if (cbx.SelectedItem == null || string.IsNullOrWhiteSpace(cbx.SelectedItem.ToString()))
                    err += "Nomor meja harus dipilih.\n";
            }
            else
            {
                err += "Kontrol ComboBox untuk nomor meja tidak ditemukan.\n";
            }

            return string.IsNullOrEmpty(err);
        }

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
                    MessageBox.Show("Error: Kontrol DateTimePicker tidak ditemukan.", "Kontrol Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string selectedMeja = string.Empty;
                Control[] cbxControls = this.Controls.Find("cbxNomorMeja", true);
                if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
                {
                    if (cbx.SelectedItem != null)
                    {
                        selectedMeja = cbx.SelectedItem.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Harap pilih nomor meja.", "Kesalahan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error: Kontrol ComboBox untuk nomor meja tidak ditemukan.", "Kontrol Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Menggunakan Stored Procedure untuk EditReservasi
                using (SqlCommand cmd = new SqlCommand("EditReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID_Reservasi", idReservasi);
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja_Baru", selectedMeja); // Gunakan selectedMeja

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data reservasi berhasil diperbarui!", "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Data tidak diperbarui atau tidak ada perubahan data.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Pesan error dari RAISERROR di Stored Procedure akan ditampilkan di sini
                MessageBox.Show("Kesalahan SQL: " + sqlEx.Message, "Kesalahan SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan: " + ex.Message, "Kesalahan Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Error: Kontrol DateTimePicker tidak ditemukan.", "Kontrol Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string selectedMeja = string.Empty;
                Control[] cbxControls = this.Controls.Find("cbxNomorMeja", true);
                if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
                {
                    if (cbx.SelectedItem != null)
                    {
                        selectedMeja = cbx.SelectedItem.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Harap pilih nomor meja.", "Kesalahan Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Error: Kontrol ComboBox untuk nomor meja tidak ditemukan.", "Kontrol Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Menggunakan Stored Procedure untuk TambahReservasi
                using (SqlCommand cmd = new SqlCommand("TambahReservasi", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nama_Customer", txtNama.Text.Trim());
                    cmd.Parameters.AddWithValue("@No_Telp", txtTelepon.Text.Trim());
                    cmd.Parameters.AddWithValue("@Waktu_Reservasi", waktuReservasi);
                    cmd.Parameters.AddWithValue("@Nomor_Meja", selectedMeja); // Gunakan selectedMeja

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Reservasi berhasil ditambahkan!");
                    LoadData();
                    ClearForm();
                }
            }
            catch (SqlException sqlEx)
            {
                // Pesan error dari RAISERROR di Stored Procedure akan ditampilkan di sini
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

            DialogResult result = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;

            try
            {
                int idReservasi = Convert.ToInt32(dgvAdminReservasi.CurrentRow.Cells["ID_Reservasi"].Value);

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                // Menggunakan Stored Procedure untuk HapusReservasi
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
                // Pesan error dari RAISERROR di Stored Procedure akan ditampilkan di sini
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

        private void dgvAdminReservasi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvAdminReservasi.Rows.Count)
                return;

            var row = dgvAdminReservasi.Rows[e.RowIndex];
            if (row.IsNewRow)
                return;

            txtNama.Text = row.Cells["Nama_Customer"].Value?.ToString() ?? string.Empty;
            txtTelepon.Text = row.Cells["No_Telp"].Value?.ToString() ?? string.Empty;

            var dtpControls = this.Controls.Find("dtpWaktuReservasi", true);
            if (dtpControls.Length > 0 && dtpControls[0] is DateTimePicker dtp)
            {
                // Set MinDate dan MaxDate agar tidak ada masalah dengan nilai yang valid
                // Rentang ini mencakup rentang DATETIME di SQL Server
                dtp.MinDate = new DateTime(1753, 1, 1);
                dtp.MaxDate = new DateTime(9998, 12, 31);

                if (row.Cells["Waktu_Reservasi"].Value != null && row.Cells["Waktu_Reservasi"].Value != DBNull.Value)
                {
                    var nilai = Convert.ToDateTime(row.Cells["Waktu_Reservasi"].Value);
                    // Pastikan nilai berada dalam rentang yang valid untuk DateTimePicker
                    dtp.Value = (nilai < dtp.MinDate) ? dtp.MinDate : (nilai > dtp.MaxDate ? dtp.MaxDate : nilai);
                }
                else
                {
                    dtp.Value = DateTime.Now; // Jika kosong, setel ke waktu saat ini
                }
            }

            // Mengisi ComboBox dengan nomor meja yang dipilih dari grid
            var cbxControls = this.Controls.Find("cbxNomorMeja", true);
            if (cbxControls.Length > 0 && cbxControls[0] is ComboBox cbx)
            {
                string selectedMejaFromGrid = row.Cells["Nomor_Meja"].Value?.ToString() ?? string.Empty;
                // Pastikan item yang akan dipilih ada di daftar ComboBox
                if (cbx.Items.Contains(selectedMejaFromGrid))
                {
                    cbx.SelectedItem = selectedMejaFromGrid; // Memilih item di ComboBox
                }
                else
                {
                    // Jika meja dari grid tidak ada di daftar ComboBox (misal statusnya sudah 'Dipesan' oleh reservasi lain)
                    // Anda bisa memilih untuk tidak memilih apa-apa atau menambahkan meja tersebut ke daftar sementara
                    cbx.SelectedIndex = -1;
                    // Opsional: Tambahkan item ke ComboBox jika tidak ada, agar bisa ditampilkan
                    // if (!string.IsNullOrEmpty(selectedMejaFromGrid) && !cbx.Items.Contains(selectedMejaFromGrid))
                    // {
                    //     cbx.Items.Add(selectedMejaFromGrid);
                    //     cbx.SelectedItem = selectedMejaFromGrid;
                    // }
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
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
                    if (lines.Length <= 1 && (lines.Length == 0 || string.IsNullOrWhiteSpace(lines[0])))
                    {
                        MessageBox.Show("File CSV kosong atau hanya berisi header.", "Informasi Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (conn.State == ConnectionState.Closed) conn.Open();

                    // Loop dimulai dari baris ke-0 jika tidak ada header, atau baris ke-1 jika ada header
                    // Diasumsikan CSV tidak memiliki header. Jika ada header, Anda bisa ubah 'i = 0' menjadi 'i = 1'.
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        string[] data = line.Split(',');
                        if (data.Length >= 4) // Memastikan ada setidaknya 4 kolom
                        {
                            try
                            {
                                string namaCustomer = data[0].Trim();
                                string noTelp = data[1].Trim();
                                if (!DateTime.TryParse(data[2].Trim(), out DateTime waktuReservasi))
                                {
                                    failCount++;
                                    errorMessages += $"Baris {i + 1}: Format waktu '{data[2]}' tidak valid. Format harus YYYY-MM-DD HH:MM.\n";
                                    continue;
                                }
                                string nomorMeja = data[3].Trim();

                                // Validasi data CSV sebelum dikirim ke database
                                if (string.IsNullOrWhiteSpace(namaCustomer) ||
                                    !Regex.IsMatch(noTelp, @"^(\+62\d{8,12}|0\d{9,14})$") ||
                                    waktuReservasi < DateTime.Now.AddMinutes(-1) || // Waktu tidak boleh di masa lalu saat impor
                                    waktuReservasi.Year != 2025 || // Validasi tahun harus 2025 untuk impor juga
                                    string.IsNullOrWhiteSpace(nomorMeja) ||
                                    nomorMeja.Length != 2 || // Nomor meja harus 2 karakter
                                    !int.TryParse(nomorMeja, out _) // Nomor meja harus angka
                                    )
                                {
                                    failCount++;
                                    errorMessages += $"Baris {i + 1}: Data tidak valid (Nama: '{namaCustomer}', Telp: '{noTelp}', Waktu: '{waktuReservasi}', Meja: '{nomorMeja}').\n";
                                    continue;
                                }

                                // Menggunakan Stored Procedure TambahReservasi untuk setiap baris
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
                                errorMessages += $"Baris {i + 1} (SQL Error): {sqlEx.Message}\n";
                            }
                            catch (FormatException formatEx)
                            {
                                failCount++;
                                errorMessages += $"Baris {i + 1} (Format Error): {formatEx.Message}\n";
                            }
                            catch (Exception ex)
                            {
                                failCount++;
                                errorMessages += $"Baris {i + 1} (General Error): {ex.Message}\n";
                            }
                        }
                        else
                        {
                            failCount++;
                            errorMessages += $"Baris {i + 1}: Jumlah kolom tidak sesuai (Diharapkan minimal 4, ditemukan {data.Length}).\n";
                        }
                    }

                    MessageBox.Show($"Impor selesai.\nBerhasil: {successCount} data.\nGagal: {failCount} data." +
                                    (failCount > 0 ? "\n\nDetail Kegagalan:\n" + errorMessages : ""),
                                    "Hasil Impor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Muat ulang data setelah impor
                    ClearForm(); // Bersihkan form
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("Gagal membaca file: " + ioEx.Message, "Kesalahan I/O File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal melakukan impor: " + ex.Message, "Kesalahan Impor Umum", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) conn.Close();
                }
            }
        }

        // Event handler dtpWaktuReservasi_ValueChanged_1 telah dihapus
        // karena menyebabkan DateTimePicker mereset nilai secara terus-menerus.
        // Pengaturan awal sudah cukup di SetupDateTimePicker().
    }
}
