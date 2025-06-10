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
    public partial class ReportViewerReservasi : Form
    {
        private readonly string connectionString;
        public ReportViewerReservasi()
        {
            InitializeComponent();
        }

        private void ReportViewerReservasi_Load(object sender, EventArgs e)
        {
            SetupReportViewer();

            this.reportViewer1.RefreshReport();
        }

        private void SetupReportViewer()
        {
            string connectionString = "Data Source=IDEAPAD5PRO\\LILA;Initial Catalog=ReservasiCafe;Integrated Security = True;";

            string query = @"
                 SELECT
                    Nama_Customer, 
                    No_Telp, 
                    Waktu_Reservasi, 
                    Nomor_Meja 
                 FROM 
                    Reservasi;";

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
