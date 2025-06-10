using System;
using System.Data;
using System.Data.SqlClient;
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
            string connectionString = "Data Source=IDEAPAD5PRO\\LILA;Initial Catalog=ReservasiCafe;Integrated Security = True;";

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

            this.reportViewer1.LocalReport.ReportPath = @"C:\Users\lilaa\source\repos\PROJECT PABD\LoginPage\ReservasiReport.rdlc";
            this.reportViewer1.RefreshReport();
        }
    }
}
