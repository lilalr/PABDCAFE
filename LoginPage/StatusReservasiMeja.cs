using PABDCAFE;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace LoginPage
{
    public partial class StatusReservasiMeja : Form
    {
        private readonly string connectionString;

        public StatusReservasiMeja(string connStr)
        {
            InitializeComponent();
            this.connectionString = connStr;
        }

        private void StatusReservasiMeja_Load(object sender, EventArgs e)
        {
            cmbJenis.Items.AddRange(new string[] { "Dipesan", "Tersedia" });
            cmbJenis.SelectedIndex = 0;

            LoadChartData("Dipesan");

            cmbJenis.SelectedIndexChanged += cmbJenis_SelectedIndexChanged;
        }

        private void cmbJenis_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = cmbJenis.SelectedItem.ToString();
            LoadChartData(selected);
        }

        private void LoadChartData(string filter)
        {
            // utk membersihkan chart
            chartReservasiMeja.Series.Clear();
            chartReservasiMeja.Titles.Clear();
            chartReservasiMeja.Legends.Clear();
            chartReservasiMeja.ChartAreas.Clear();

            ChartArea ca = new ChartArea("StatusArea");
            ca.AxisX.Title = "Status";
            ca.AxisY.Title = "Jumlah Meja";
            ca.AxisX.LabelStyle.Angle = -45;
            ca.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            chartReservasiMeja.ChartAreas.Add(ca);

            string query = @"
            SELECT
                Status_Meja,
                COUNT (*) AS Jumlah
            FROM Meja
            GROUP BY Status_Meja";

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(this.connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }

            if (filter == "Dipesan")
            {
                Series sDipesan = new Series("Dipesan")
                {
                    ChartType = SeriesChartType.Column,
                    Color = System.Drawing.Color.Firebrick,
                    IsValueShownAsLabel = true
                };

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Status_Meja"].ToString() == "Dipesan")
                    {
                        // Ambil dari kolom "Jumlah", bukan "Pemasukan"
                        int jumlah = Convert.ToInt32(row["Jumlah"]);
                        // Sumbu X adalah status itu sendiri, bukan "NamaOrganisasi"
                        sDipesan.Points.AddXY("Dipesan", jumlah);
                    }
                }
                chartReservasiMeja.Series.Add(sDipesan);
            }

            if (filter == "Tersedia")
            {
                Series sTersedia = new Series("Tersedia")
                {
                    ChartType = SeriesChartType.Column,
                    Color = System.Drawing.Color.ForestGreen,
                    IsValueShownAsLabel = true
                };

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Status_Meja"].ToString() == "Tersedia")
                    {
                        // Ambil dari kolom "Jumlah"
                        int jumlah = Convert.ToInt32(row["Jumlah"]);
                        // Sumbu X adalah status itu sendiri
                        sTersedia.Points.AddXY("Tersedia", jumlah);
                    }
                }
                chartReservasiMeja.Series.Add(sTersedia);
            }

            chartReservasiMeja.Titles.Add("Grafik Ketersediaan Meja");
            chartReservasiMeja.Legends.Add(new Legend("Legenda"));
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            AdminPage ap = new AdminPage(this.connectionString);
            ap.Show();
            this.Close();
        }
    }
}
