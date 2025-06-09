using LoginPage;
using Microsoft.Reporting.WinForms;
using System;
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

    }
}
