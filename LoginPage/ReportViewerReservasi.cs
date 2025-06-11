using System;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace PABDCAFE
{
    public partial class ReportViewerReservasi : Form
    {
        public ReportViewerReservasi()
        {
            InitializeComponent();
        }

        private void ReportViewerReservasi_Load(object sender, EventArgs e)
        {
            SetupReportViewer();

            this.reportViewer1.RefreshReport();
            this.reportViewer1.RefreshReport();
        }

        private void SetupReportViewer()
        {
            //string connectionString = "Data Source=IDEAPAD5PRO\\LILA;Initial Catalog=ReservasiCafe;Integrated Security = True;";  //Lila
            string connectionString = "Data Source=LAPTOP-4FJGLBGI\\NANDA;Initial Catalog=ReservasiCafe;Integrated Security = True;";  //Mesi

            string query = @"
                 SELECT
                    Reservasi.ID_Reservasi, 
                    Reservasi.Nama_Customer, 
                    Reservasi.No_Telp, 
                    Reservasi.Waktu_Reservasi, 
                    Meja.Nomor_Meja
                FROM   
                    Reservasi 
                INNER JOIN
                    Meja ON Reservasi.Nomor_Meja = Meja.Nomor_Meja;";

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

            //this.reportViewer1.LocalReport.ReportPath = @"C:\Users\lilaa\source\repos\PROJECT PABD\LoginPage\ReservasiReport.rdlc";  //Lila
            this.reportViewer1.LocalReport.ReportPath = @"C:\Users\User\source\repos\PABDCAFErgbr4tg\LoginPage\ReservasiReport.rdlc";  //Mesi
            this.reportViewer1.RefreshReport();
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            try
            {
                // LANGKAH 1: MENGAMBIL DATA RESERVASI DARI DATABASE
                DataTable dt = new DataTable();
                string connectionString = "Data Source=LAPTOP-4FJGLBGI\\NANDA;Initial Catalog=ReservasiCafe;Integrated Security=True;";
                // Query disesuaikan untuk mengambil data dari tabel Reservasi
                string query = "SELECT ID_Reservasi, Nama_Customer, No_Telp, Waktu_Reservasi, Nomor_Meja FROM Reservasi ORDER BY Waktu_Reservasi DESC";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    da.Fill(dt);
                }

                // Jika tidak ada data reservasi, tampilkan pesan dan hentikan proses
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data reservasi untuk diekspor.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // LANGKAH 2: MEMBUAT STRING FORMAT CSV DARI DATA RESERVASI
                StringBuilder sb = new StringBuilder();

                // Membuat Header/Judul Kolom
                string[] columnNames = dt.Columns.Cast<DataColumn>()
                                         .Select(column => column.ColumnName)
                                         .ToArray();
                sb.AppendLine(string.Join(",", columnNames));

                // Menambahkan setiap baris data reservasi
                foreach (DataRow row in dt.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => {
                        string value = field.ToString();
                        // Menangani jika data mengandung koma atau kutip agar format CSV tidak rusak
                        if (value.Contains(",") || value.Contains("\""))
                        {
                            value = $"\"{value.Replace("\"", "\"\"")}\"";
                        }
                        return value;
                    }).ToArray();
                    sb.AppendLine(string.Join(",", fields));
                }

                // LANGKAH 3: MENYIMPAN STRING KE FILE CSV
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                // Nama file default disesuaikan untuk laporan reservasi
                saveFileDialog.FileName = $"LaporanReservasi_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
                saveFileDialog.Filter = "CSV File (*.csv)|*.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Menulis semua teks yang telah dibuat ke dalam file yang dipilih
                    File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                    MessageBox.Show("Laporan reservasi berhasil diekspor ke CSV!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat mengekspor: " + ex.Message, "Error Ekspor", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
