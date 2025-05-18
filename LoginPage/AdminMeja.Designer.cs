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
            this.lblNomor = new System.Windows.Forms.Label();
            this.lblKapasitas = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdminMeja)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(782, 489);
            this.btnBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(84, 35);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "<back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(736, 63);
            this.btnTambah.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(110, 38);
            this.btnTambah.TabIndex = 1;
            this.btnTambah.Text = "Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(736, 109);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(110, 38);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Location = new System.Drawing.Point(736, 155);
            this.btnHapus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(110, 38);
            this.btnHapus.TabIndex = 3;
            this.btnHapus.Text = "Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // dgvAdminMeja
            // 
            this.dgvAdminMeja.BackgroundColor = System.Drawing.Color.LavenderBlush;
            this.dgvAdminMeja.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdminMeja.Location = new System.Drawing.Point(58, 246);
            this.dgvAdminMeja.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvAdminMeja.Name = "dgvAdminMeja";
            this.dgvAdminMeja.RowHeadersWidth = 51;
            this.dgvAdminMeja.RowTemplate.Height = 24;
            this.dgvAdminMeja.Size = new System.Drawing.Size(788, 224);
            this.dgvAdminMeja.TabIndex = 4;
            this.dgvAdminMeja.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAdminMeja_CellContentClick);
            // 
            // txtNomor
            // 
            this.txtNomor.Location = new System.Drawing.Point(223, 69);
            this.txtNomor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNomor.Name = "txtNomor";
            this.txtNomor.Size = new System.Drawing.Size(466, 26);
            this.txtNomor.TabIndex = 5;
            // 
            // txtKapasitas
            // 
            this.txtKapasitas.Location = new System.Drawing.Point(223, 116);
            this.txtKapasitas.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtKapasitas.Name = "txtKapasitas";
            this.txtKapasitas.Size = new System.Drawing.Size(466, 26);
            this.txtKapasitas.TabIndex = 6;
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(223, 161);
            this.txtStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(466, 26);
            this.txtStatus.TabIndex = 7;
            // 
            // lblNomor
            // 
            this.lblNomor.AutoSize = true;
            this.lblNomor.BackColor = System.Drawing.Color.Transparent;
            this.lblNomor.Location = new System.Drawing.Point(63, 71);
            this.lblNomor.Name = "lblNomor";
            this.lblNomor.Size = new System.Drawing.Size(94, 20);
            this.lblNomor.TabIndex = 10;
            this.lblNomor.Text = "Nomor Meja";
            // 
            // lblKapasitas
            // 
            this.lblKapasitas.AutoSize = true;
            this.lblKapasitas.BackColor = System.Drawing.Color.Transparent;
            this.lblKapasitas.Location = new System.Drawing.Point(63, 118);
            this.lblKapasitas.Name = "lblKapasitas";
            this.lblKapasitas.Size = new System.Drawing.Size(117, 20);
            this.lblKapasitas.TabIndex = 11;
            this.lblKapasitas.Text = "Kapasitas Meja";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Location = new System.Drawing.Point(62, 162);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(94, 20);
            this.lblStatus.TabIndex = 12;
            this.lblStatus.Text = "Status Meja";
            // 
            // AdminMeja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoginPage.Properties.Resources.WhatsApp_Image_2025_05_19_at_00_38_34_dbdf79d4;
            this.ClientSize = new System.Drawing.Size(900, 562);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblKapasitas);
            this.Controls.Add(this.lblNomor);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtKapasitas);
            this.Controls.Add(this.txtNomor);
            this.Controls.Add(this.dgvAdminMeja);
            this.Controls.Add(this.btnHapus);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.btnBack);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
        private System.Windows.Forms.Label lblNomor;
        private System.Windows.Forms.Label lblKapasitas;
        private System.Windows.Forms.Label lblStatus;
    }
}