namespace PABDCAFE
{
    partial class AdminMeja
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
            this.btnBack = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.dgvAdminMeja = new System.Windows.Forms.DataGridView();
            this.txtNomor = new System.Windows.Forms.TextBox();
            this.txtKapasitas = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.txtWaktu = new System.Windows.Forms.TextBox();
            this.lblNomor = new System.Windows.Forms.Label();
            this.lblKapasitas = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblWaktu = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminMeja)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(691, 375);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 28);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "<back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(665, 37);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 1;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(665, 67);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(665, 97);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(75, 23);
            this.btnHapus.TabIndex = 3;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            // 
            // dgvAdminMeja
            // 
            this.dgvAdminMeja.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdminMeja.Location = new System.Drawing.Point(74, 230);
            this.dgvAdminMeja.Name = "dgvAdminMeja";
            this.dgvAdminMeja.RowHeadersWidth = 51;
            this.dgvAdminMeja.RowTemplate.Height = 24;
            this.dgvAdminMeja.Size = new System.Drawing.Size(584, 150);
            this.dgvAdminMeja.TabIndex = 4;
            // 
            // txtNomor
            // 
            this.txtNomor.Location = new System.Drawing.Point(157, 37);
            this.txtNomor.Name = "txtNomor";
            this.txtNomor.Size = new System.Drawing.Size(365, 22);
            this.txtNomor.TabIndex = 5;
            // 
            // txtKapasitas
            // 
            this.txtKapasitas.Location = new System.Drawing.Point(157, 67);
            this.txtKapasitas.Name = "txtKapasitas";
            this.txtKapasitas.Size = new System.Drawing.Size(365, 22);
            this.txtKapasitas.TabIndex = 6;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(157, 97);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(365, 22);
            this.txtStatus.TabIndex = 7;
            // 
            // txtWaktu
            // 
            this.txtWaktu.Location = new System.Drawing.Point(157, 138);
            this.txtWaktu.Name = "txtWaktu";
            this.txtWaktu.Size = new System.Drawing.Size(365, 22);
            this.txtWaktu.TabIndex = 8;
            // 
            // lblNomor
            // 
            this.lblNomor.AutoSize = true;
            this.lblNomor.Location = new System.Drawing.Point(26, 37);
            this.lblNomor.Name = "lblNomor";
            this.lblNomor.Size = new System.Drawing.Size(81, 16);
            this.lblNomor.TabIndex = 10;
            this.lblNomor.Text = "Nomor Meja";
            // 
            // lblKapasitas
            // 
            this.lblKapasitas.AutoSize = true;
            this.lblKapasitas.Location = new System.Drawing.Point(29, 67);
            this.lblKapasitas.Name = "lblKapasitas";
            this.lblKapasitas.Size = new System.Drawing.Size(100, 16);
            this.lblKapasitas.TabIndex = 11;
            this.lblKapasitas.Text = "Kapasitas Meja";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(32, 97);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(77, 16);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Status Meja";
            // 
            // lblWaktu
            // 
            this.lblWaktu.AutoSize = true;
            this.lblWaktu.Location = new System.Drawing.Point(32, 127);
            this.lblWaktu.Name = "lblWaktu";
            this.lblWaktu.Size = new System.Drawing.Size(110, 16);
            this.lblWaktu.TabIndex = 13;
            this.lblWaktu.Text = "Waktu Reservasi";
            // 
            // AdminMeja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblWaktu);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblKapasitas);
            this.Controls.Add(this.lblNomor);
            this.Controls.Add(this.txtWaktu);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtKapasitas);
            this.Controls.Add(this.txtNomor);
            this.Controls.Add(this.dgvAdminMeja);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.btnBack);
            this.Name = "AdminMeja";
            this.Text = "AdminMeja";
            this.Load += new System.EventHandler(this.AdminMeja_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminMeja)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnHapus;
        private System.Windows.Forms.DataGridView dgvAdminMeja;
        private System.Windows.Forms.TextBox txtNomor;
        private System.Windows.Forms.TextBox txtKapasitas;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.TextBox txtWaktu;
        private System.Windows.Forms.Label lblNomor;
        private System.Windows.Forms.Label lblKapasitas;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblWaktu;
    }
}