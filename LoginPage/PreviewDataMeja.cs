using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class PreviewDataMeja : Form
    {
        private readonly string connectionString;
        private SqlConnection conn;
        public DataTable Data { get; private set; }
        public bool ImportConfirmed { get; private set; } = false;

        public PreviewDataMeja(DataTable data, string connStr)
        {
            InitializeComponent();
            this.Data = data;
            this.connectionString = connStr;
            this.conn = new SqlConnection(this.connectionString);
            dgvPreview.DataSource = data;
        }

        private void PreviewDataMeja_Load(object sender, EventArgs e)
        {
            dgvPreview.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public bool ValidateRow(DataRow row, int lineNumber, out string errorMessage)
        {
            errorMessage = "";
            if (!row.Table.Columns.Contains("Nomor_Meja") || !row.Table.Columns.Contains("Kapasitas"))
            {
                errorMessage = $"Baris {lineNumber}: Kolom 'Nomor_Meja' atau 'Kapasitas' tidak ditemukan.";
                return false;
            }

            string nomorMeja = row["Nomor_Meja"].ToString().Trim();
            string kapasitasStr = row["Kapasitas"].ToString().Trim();

            if (!Regex.IsMatch(nomorMeja, @"^\d{2}$"))
            {
                errorMessage = $"Baris {lineNumber}: Nomor Meja '{nomorMeja}' tidak valid. Harus 2 digit angka.";
                return false;
            }

            if (!int.TryParse(kapasitasStr, out int kapasitas) || kapasitas < 1 || kapasitas > 99)
            {
                errorMessage = $"Baris {lineNumber}: Kapasitas '{kapasitasStr}' tidak valid. Harus angka antara 1-99.";
                return false;
            }

            return true;
        }


        private void ImportDataToDatabase()
        {
            try
            {
                DataTable dt = (DataTable)dgvPreview.DataSource;
                List<string> errorList = new List<string>();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                            if (!ValidateRow(row, i + 2, out string errorMsg))
                            {
                                errorList.Add(errorMsg);
                                continue;
                            }

                            using (SqlCommand cmd = new SqlCommand("TambahMeja", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Nomor_Meja", row["Nomor_Meja"]);
                                cmd.Parameters.AddWithValue("@Kapasitas", row["Kapasitas"]);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        if (errorList.Count > 0)
                        {
                            transaction.Rollback();
                            MessageBox.Show("Import dibatalkan karena kesalahan:\n" + string.Join("\n", errorList),
                                "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        transaction.Commit();
                        MessageBox.Show("Data berhasil diimpor!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Gagal mengimpor data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kesalahan saat membaca data: " + ex.Message, "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Yakin ingin mengimpor data ini ke database?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ImportConfirmed = true;
                ImportDataToDatabase();
            }
        }
        
        private void dgvPreview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            ImportConfirmed = false;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
