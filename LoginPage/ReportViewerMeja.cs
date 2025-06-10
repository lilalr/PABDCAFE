using LoginPage;
using Microsoft.Reporting.WinForms;
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PABDCAFE
{
    public partial class ReportViewerMeja : Form
    {
        private readonly string connectionString;
        //private SqlConnection conn;
        // Deklarasi komponen UI
        //private DataGridView dgvTampilanReservasi;
        //private Button btnRefresh;
        //private ReportViewerMeja ReportViewerMeja;

        public ReportViewerMeja()
        {
            // Konstruktor ini akan menginisialisasi Form dan semua komponennya
            InitializeComponent();
        }

        private void ReportViewerMeja_Load(object sender, EventArgs e)
        {
            SetupReportViewer();

            this.reportViewer1.RefreshReport();
        }

        private void SetupReportViewer()
        {
            string connectionString = "Data Source=LAPTOP-4FJGLBGI\\NANDA;Initial Catalog=ReservasiCafe;Integrated Security = True;";

            string query = @"SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja;";

            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Menggunakan SqlDataAdapter untuk mengisi DataTable
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(rds);

            this.reportViewer1.LocalReport.ReportPath = @"C:\Users\User\source\repos\PABDCAFErgbr4tg\LoginPage\MejaReport1.rdlc";
            this.reportViewer1.RefreshReport();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            try
            {
                // LANGKAH 1: AMBIL KEMBALI DATA DARI DATABASE
                // Kita membuat DataTable baru di sini agar fungsi ini independen.
                DataTable dt = new DataTable();
                string connectionString = "Data Source=LAPTOP-4FJGLBGI\\NANDA;Initial Catalog=ReservasiCafe;Integrated Security=True;";
                string query = "SELECT Nomor_Meja, Kapasitas, Status_Meja FROM Meja;";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(dt);
                }

                // Jika tidak ada data, hentikan proses.
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diekspor.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // LANGKAH 2: BUAT STRING FORMAT CSV SECARA MANUAL
                StringBuilder sb = new StringBuilder();

                // Buat Header (Nama Kolom)
                string[] columnNames = new string[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columnNames[i] = dt.Columns[i].ColumnName;
                }
                sb.AppendLine(string.Join(",", columnNames));

                // Tambahkan setiap baris data
                foreach (DataRow row in dt.Rows)
                {
                    string[] fields = new string[dt.Columns.Count];
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        // Ambil nilai dan pastikan aman untuk CSV (menangani koma atau kutip dalam data)
                        string field = row[i].ToString();
                        if (field.Contains(",") || field.Contains("\""))
                        {
                            // Jika ada koma atau kutip, apit dengan kutip ganda
                            field = $"\"{field.Replace("\"", "\"\"")}\"";
                        }
                        fields[i] = field;
                    }
                    sb.AppendLine(string.Join(",", fields));
                }

                // LANGKAH 3: SIMPAN STRING KE FILE
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = $"LaporanMeja_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                saveFileDialog.Filter = "CSV File (*.csv)|*.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Simpan teks yang sudah kita buat ke dalam file
                    File.WriteAllText(saveFileDialog.FileName, sb.ToString());

                    MessageBox.Show("Laporan berhasil diekspor ke CSV!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengekspor: " + ex.Message, "Error Ekspor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
    
}
