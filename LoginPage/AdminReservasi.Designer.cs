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
            this.txtArNama = new System.Windows.Forms.TextBox();
            this.btnTambah = new System.Windows.Forms.Button();
            this.txtTelepon = new System.Windows.Forms.TextBox();
            this.txtArWaktu = new System.Windows.Forms.TextBox();
            this.ArNama = new System.Windows.Forms.Label();
            this.lblTelepon = new System.Windows.Forms.Label();
            this.lblWaktu = new System.Windows.Forms.Label();
            this.dgvAdminReservasi = new System.Windows.Forms.DataGridView();
            this.txtMeja = new System.Windows.Forms.TextBox();
            this.lblMeja = new System.Windows.Forms.Label();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblNama = new System.Windows.Forms.Label();
            this.txtWaktu = new System.Windows.Forms.TextBox();
            this.txtNama = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminReservasi)).BeginInit();
            this.SuspendLayout();
            // 
            // txtArNama
            // 
            this.txtArNama.Location = new System.Drawing.Point(0, 0);
            this.txtArNama.Name = "txtArNama";
            this.txtArNama.Size = new System.Drawing.Size(100, 22);
            this.txtArNama.TabIndex = 14;
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(547, 28);
            this.btnTambah.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(135, 29);
            this.btnTambah.TabIndex = 1;
            this.btnTambah.Text = "Tambah Reservasi";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnArAdd_Click);
            // 
            // txtTelepon
            // 
            this.txtTelepon.Location = new System.Drawing.Point(183, 49);
            this.txtTelepon.Name = "txtTelepon";
            this.txtTelepon.Size = new System.Drawing.Size(273, 22);
            this.txtTelepon.TabIndex = 2;
            // 
            // txtArWaktu
            // 
            this.txtArWaktu.Location = new System.Drawing.Point(0, 0);
            this.txtArWaktu.Name = "txtArWaktu";
            this.txtArWaktu.Size = new System.Drawing.Size(100, 22);
            this.txtArWaktu.TabIndex = 13;
            // 
            // ArNama
            // 
            this.ArNama.Location = new System.Drawing.Point(0, 0);
            this.ArNama.Name = "ArNama";
            this.ArNama.Size = new System.Drawing.Size(100, 23);
            this.ArNama.TabIndex = 12;
            // 
            // lblTelepon
            // 
            this.lblTelepon.AutoSize = true;
            this.lblTelepon.Location = new System.Drawing.Point(64, 55);
            this.lblTelepon.Name = "lblTelepon";
            this.lblTelepon.Size = new System.Drawing.Size(102, 16);
            this.lblTelepon.TabIndex = 5;
            this.lblTelepon.Text = "Nomor Telepon";
            // 
            // lblWaktu
            // 
            this.lblWaktu.AutoSize = true;
            this.lblWaktu.Location = new System.Drawing.Point(64, 83);
            this.lblWaktu.Name = "lblWaktu";
            this.lblWaktu.Size = new System.Drawing.Size(110, 16);
            this.lblWaktu.TabIndex = 6;
            this.lblWaktu.Text = "Waktu Reservasi";
            // 
            // dgvAdminReservasi
            // 
            this.dgvAdminReservasi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdminReservasi.Location = new System.Drawing.Point(47, 189);
            this.dgvAdminReservasi.Name = "dgvAdminReservasi";
            this.dgvAdminReservasi.RowHeadersWidth = 51;
            this.dgvAdminReservasi.RowTemplate.Height = 24;
            this.dgvAdminReservasi.Size = new System.Drawing.Size(569, 150);
            this.dgvAdminReservasi.TabIndex = 7;
            // 
            // txtMeja
            // 
            this.txtMeja.Location = new System.Drawing.Point(183, 106);
            this.txtMeja.Name = "txtMeja";
            this.txtMeja.Size = new System.Drawing.Size(273, 22);
            this.txtMeja.TabIndex = 8;
            // 
            // lblMeja
            // 
            this.lblMeja.AutoSize = true;
            this.lblMeja.Location = new System.Drawing.Point(65, 111);
            this.lblMeja.Name = "lblMeja";
            this.lblMeja.Size = new System.Drawing.Size(81, 16);
            this.lblMeja.TabIndex = 9;
            this.lblMeja.Text = "Nomor Meja";
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(547, 62);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(135, 29);
            this.btnHapus.TabIndex = 10;
            this.btnHapus.Text = "Hapus Reservasi";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnArHapus_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(547, 97);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(135, 29);
            this.btnEdit.TabIndex = 11;
            this.btnEdit.Text = "Edit Reservasi";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(624, 313);
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
            this.lblNama.Location = new System.Drawing.Point(67, 27);
            this.lblNama.Name = "lblNama";
            this.lblNama.Size = new System.Drawing.Size(104, 16);
            this.lblNama.TabIndex = 16;
            this.lblNama.Text = "Nama Customer";
            // 
            // txtWaktu
            // 
            this.txtWaktu.Location = new System.Drawing.Point(183, 78);
            this.txtWaktu.Name = "txtWaktu";
            this.txtWaktu.Size = new System.Drawing.Size(273, 22);
            this.txtWaktu.TabIndex = 17;
            // 
            // txtNama
            // 
            this.txtNama.Location = new System.Drawing.Point(183, 21);
            this.txtNama.Name = "txtNama";
            this.txtNama.Size = new System.Drawing.Size(273, 22);
            this.txtNama.TabIndex = 18;
            // 
            // AdminReservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.txtNama);
            this.Controls.Add(this.txtWaktu);
            this.Controls.Add(this.lblNama);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.lblMeja);
            this.Controls.Add(this.txtMeja);
            this.Controls.Add(this.dgvAdminReservasi);
            this.Controls.Add(this.lblWaktu);
            this.Controls.Add(this.lblTelepon);
            this.Controls.Add(this.ArNama);
            this.Controls.Add(this.txtArWaktu);
            this.Controls.Add(this.txtTelepon);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.txtArNama);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AdminReservasi";
            this.Text = "AdminReservasi";
            this.Load += new System.EventHandler(this.Tambah_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminReservasi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.TextBox txtTelepon;
        private System.Windows.Forms.TextBox txtArWaktu;
        private System.Windows.Forms.Label ArNama;
        private System.Windows.Forms.Label lblTelepon;
        private System.Windows.Forms.Label lblWaktu;
        private System.Windows.Forms.TextBox txtArNama;
        private System.Windows.Forms.DataGridView dgvAdminReservasi;
        private System.Windows.Forms.TextBox txtMeja;
        private System.Windows.Forms.Label lblMeja;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblNama;
        private System.Windows.Forms.TextBox txtWaktu;
        private System.Windows.Forms.TextBox txtNama;
    }
}