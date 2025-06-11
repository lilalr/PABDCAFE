using System;

namespace PABDCAFE
{
    partial class AdminReservasi
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnTambah = new System.Windows.Forms.Button();
            this.txtTelepon = new System.Windows.Forms.TextBox();
            this.lblTelepon = new System.Windows.Forms.Label();
            this.lblWaktu = new System.Windows.Forms.Label();
            this.lblMeja = new System.Windows.Forms.Label();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblNama = new System.Windows.Forms.Label();
            this.txtNama = new System.Windows.Forms.TextBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.dtpWaktuReservasi = new System.Windows.Forms.DateTimePicker();
            this.cbxNomorMeja = new System.Windows.Forms.ComboBox();
            this.dgvAdminReservasi = new System.Windows.Forms.DataGridView();
            this.btnAnalisis = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminReservasi)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(79, 195);
            this.btnTambah.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(92, 29);
            this.btnTambah.TabIndex = 1;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // txtTelepon
            // 
            this.txtTelepon.Location = new System.Drawing.Point(219, 77);
            this.txtTelepon.Name = "txtTelepon";
            this.txtTelepon.Size = new System.Drawing.Size(374, 22);
            this.txtTelepon.TabIndex = 2;
            // 
            // lblTelepon
            // 
            this.lblTelepon.AutoSize = true;
            this.lblTelepon.BackColor = System.Drawing.Color.Transparent;
            this.lblTelepon.Location = new System.Drawing.Point(68, 78);
            this.lblTelepon.Name = "lblTelepon";
            this.lblTelepon.Size = new System.Drawing.Size(102, 16);
            this.lblTelepon.TabIndex = 5;
            this.lblTelepon.Text = "Nomor Telepon";
            // 
            // lblWaktu
            // 
            this.lblWaktu.AutoSize = true;
            this.lblWaktu.BackColor = System.Drawing.Color.Transparent;
            this.lblWaktu.Location = new System.Drawing.Point(68, 115);
            this.lblWaktu.Name = "lblWaktu";
            this.lblWaktu.Size = new System.Drawing.Size(110, 16);
            this.lblWaktu.TabIndex = 6;
            this.lblWaktu.Text = "Waktu Reservasi";
            // 
            // lblMeja
            // 
            this.lblMeja.AutoSize = true;
            this.lblMeja.BackColor = System.Drawing.Color.Transparent;
            this.lblMeja.Location = new System.Drawing.Point(68, 150);
            this.lblMeja.Name = "lblMeja";
            this.lblMeja.Size = new System.Drawing.Size(81, 16);
            this.lblMeja.TabIndex = 9;
            this.lblMeja.Text = "Nomor Meja";
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(325, 195);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(92, 29);
            this.btnHapus.TabIndex = 10;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(204, 195);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(92, 29);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(772, 453);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 26);
            this.btnBack.TabIndex = 15;
            this.btnBack.Text = "<back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblNama
            // 
            this.lblNama.AutoSize = true;
            this.lblNama.BackColor = System.Drawing.Color.Transparent;
            this.lblNama.Location = new System.Drawing.Point(68, 46);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(104, 16);
            this.lblNama.TabIndex = 16;
            this.lblNama.Text = "Nama Customer";
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(219, 44);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(374, 22);
            this.txtNama.TabIndex = 18;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(455, 195);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(92, 29);
            this.btnImport.TabIndex = 19;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // dtpWaktuReservasi
            // 
            this.dtpWaktuReservasi.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtpWaktuReservasi.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpWaktuReservasi.Location = new System.Drawing.Point(219, 110);
            this.dtpWaktuReservasi.Name = "dtpWaktuReservasi";
            this.dtpWaktuReservasi.ShowUpDown = true;
            this.dtpWaktuReservasi.Size = new System.Drawing.Size(200, 22);
            this.dtpWaktuReservasi.TabIndex = 22;
            this.dtpWaktuReservasi.ValueChanged += new System.EventHandler(this.dtpWaktuReservasi_ValueChanged);
            // 
            // cbxNomorMeja
            // 
            this.cbxNomorMeja.FormattingEnabled = true;
            this.cbxNomorMeja.Location = new System.Drawing.Point(219, 150);
            this.cbxNomorMeja.Name = "cbxNomorMeja";
            this.cbxNomorMeja.Size = new System.Drawing.Size(121, 24);
            this.cbxNomorMeja.TabIndex = 21;
            // 
            // dgvAdminReservasi
            // 
            this.dgvAdminReservasi.BackgroundColor = System.Drawing.Color.LavenderBlush;
            this.dgvAdminReservasi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdminReservasi.Location = new System.Drawing.Point(56, 246);
            this.dgvAdminReservasi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvAdminReservasi.Name = "dgvAdminReservasi";
            this.dgvAdminReservasi.RowHeadersWidth = 62;
            this.dgvAdminReservasi.RowTemplate.Height = 28;
            this.dgvAdminReservasi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAdminReservasi.Size = new System.Drawing.Size(772, 193);
            this.dgvAdminReservasi.TabIndex = 23;
            this.dgvAdminReservasi.SelectionChanged += new System.EventHandler(this.dgvAdminReservasi_SelectionChanged);
            // 
            // btnAnalisis
            // 
            this.btnAnalisis.Location = new System.Drawing.Point(708, 195);
            this.btnAnalisis.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAnalisis.Name = "btnAnalisis";
            this.btnAnalisis.Size = new System.Drawing.Size(92, 29);
            this.btnAnalisis.TabIndex = 25;
            this.btnAnalisis.Text = "Analisis";
            this.btnAnalisis.UseVisualStyleBackColor = true;
            this.btnAnalisis.Click += new System.EventHandler(this.btnAnalisis_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(581, 195);
            this.btnReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(92, 29);
            this.btnReport.TabIndex = 26;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // AdminReservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoginPage.Properties.Resources.WhatsApp_Image_2025_05_19_at_00_38_34_dbdf79d4;
            this.ClientSize = new System.Drawing.Size(883, 501);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.btnAnalisis);
            this.Controls.Add(this.dgvAdminReservasi);
            this.Controls.Add(this.cbxNomorMeja);
            this.Controls.Add(this.dtpWaktuReservasi);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.lblNama);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.lblMeja);
            this.Controls.Add(this.lblWaktu);
            this.Controls.Add(this.lblTelepon);
            this.Controls.Add(this.txtTelepon);
            this.Controls.Add(this.btnTambah);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AdminReservasi";
            this.Text = "AdminReservasi";
            this.Load += new System.EventHandler(this.AdminReservasi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminReservasi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }





        #endregion
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.TextBox txtTelepon;
        private System.Windows.Forms.Label lblTelepon;
        private System.Windows.Forms.Label lblWaktu;
        private System.Windows.Forms.Label lblMeja;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblNama;
        private System.Windows.Forms.TextBox txtNama;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.DateTimePicker dtpWaktuReservasi;
        private System.Windows.Forms.ComboBox cbxNomorMeja;
        private System.Windows.Forms.DataGridView dgvAdminReservasi;
        private System.Windows.Forms.Button btnAnalisis;
        private System.Windows.Forms.Button btnReport;
    }
}