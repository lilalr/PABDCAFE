namespace PABDCAFE
{
    partial class CustomerPage
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
            this.btnCustLihat = new System.Windows.Forms.Button();
            this.btnCustTambah = new System.Windows.Forms.Button();
            this.btnCustHapus = new System.Windows.Forms.Button();
            this.dgvCustomer = new System.Windows.Forms.DataGridView();
            this.btnLogout = new System.Windows.Forms.Button();
            this.txtCustNama = new System.Windows.Forms.TextBox();
            this.txtCustNoTelp = new System.Windows.Forms.TextBox();
            this.txtCustWaktu = new System.Windows.Forms.TextBox();
            this.txtCustMeja = new System.Windows.Forms.TextBox();
            this.CustNama = new System.Windows.Forms.Label();
            this.CustTelp = new System.Windows.Forms.Label();
            this.CustWaktu = new System.Windows.Forms.Label();
            this.CustMeja = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCustLihat
            // 
            this.btnCustLihat.Location = new System.Drawing.Point(679, 96);
            this.btnCustLihat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCustLihat.Name = "btnCustLihat";
            this.btnCustLihat.Size = new System.Drawing.Size(138, 35);
            this.btnCustLihat.TabIndex = 1;
            this.btnCustLihat.Text = "Lihat Reservasi";
            this.btnCustLihat.UseVisualStyleBackColor = true;
            this.btnCustLihat.Click += new System.EventHandler(this.btnCustLihat_Click);
            // 
            // btnCustTambah
            // 
            this.btnCustTambah.Location = new System.Drawing.Point(679, 41);
            this.btnCustTambah.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCustTambah.Name = "btnCustTambah";
            this.btnCustTambah.Size = new System.Drawing.Size(138, 35);
            this.btnCustTambah.TabIndex = 2;
            this.btnCustTambah.Text = "Tambah Reservasi";
            this.btnCustTambah.UseVisualStyleBackColor = true;
            this.btnCustTambah.Click += new System.EventHandler(this.btnCustTambah_Click);
            // 
            // btnCustHapus
            // 
            this.btnCustHapus.Location = new System.Drawing.Point(679, 141);
            this.btnCustHapus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCustHapus.Name = "btnCustHapus";
            this.btnCustHapus.Size = new System.Drawing.Size(138, 37);
            this.btnCustHapus.TabIndex = 3;
            this.btnCustHapus.Text = "Hapus Reservasi";
            this.btnCustHapus.UseVisualStyleBackColor = true;
            this.btnCustHapus.Click += new System.EventHandler(this.btnCustHapus_Click);
            // 
            // dgvCustomer
            // 
            this.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomer.Location = new System.Drawing.Point(49, 229);
            this.dgvCustomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvCustomer.Name = "dgvCustomer";
            this.dgvCustomer.RowHeadersWidth = 62;
            this.dgvCustomer.RowTemplate.Height = 28;
            this.dgvCustomer.Size = new System.Drawing.Size(739, 203);
            this.dgvCustomer.TabIndex = 4;
            this.dgvCustomer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCutomer_CellContentClick);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(804, 427);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(67, 27);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // txtCustNama
            // 
            this.txtCustNama.Location = new System.Drawing.Point(208, 24);
            this.txtCustNama.Name = "txtCustNama";
            this.txtCustNama.Size = new System.Drawing.Size(403, 22);
            this.txtCustNama.TabIndex = 6;
            // 
            // txtCustNoTelp
            // 
            this.txtCustNoTelp.Location = new System.Drawing.Point(208, 66);
            this.txtCustNoTelp.Name = "txtCustNoTelp";
            this.txtCustNoTelp.Size = new System.Drawing.Size(403, 22);
            this.txtCustNoTelp.TabIndex = 7;
            // 
            // txtCustWaktu
            // 
            this.txtCustWaktu.Location = new System.Drawing.Point(208, 109);
            this.txtCustWaktu.Name = "txtCustWaktu";
            this.txtCustWaktu.Size = new System.Drawing.Size(403, 22);
            this.txtCustWaktu.TabIndex = 8;
            // 
            // txtCustMeja
            // 
            this.txtCustMeja.Location = new System.Drawing.Point(208, 156);
            this.txtCustMeja.Name = "txtCustMeja";
            this.txtCustMeja.Size = new System.Drawing.Size(403, 22);
            this.txtCustMeja.TabIndex = 9;
            // 
            // CustNama
            // 
            this.CustNama.AutoSize = true;
            this.CustNama.Location = new System.Drawing.Point(81, 30);
            this.CustNama.Name = "CustNama";
            this.CustNama.Size = new System.Drawing.Size(104, 16);
            this.CustNama.TabIndex = 10;
            this.CustNama.Text = "Nama Customer";
            // 
            // CustTelp
            // 
            this.CustTelp.AutoSize = true;
            this.CustTelp.Location = new System.Drawing.Point(83, 69);
            this.CustTelp.Name = "CustTelp";
            this.CustTelp.Size = new System.Drawing.Size(102, 16);
            this.CustTelp.TabIndex = 11;
            this.CustTelp.Text = "Nomor Telepon";
            // 
            // CustWaktu
            // 
            this.CustWaktu.AutoSize = true;
            this.CustWaktu.Location = new System.Drawing.Point(85, 114);
            this.CustWaktu.Name = "CustWaktu";
            this.CustWaktu.Size = new System.Drawing.Size(110, 16);
            this.CustWaktu.TabIndex = 12;
            this.CustWaktu.Text = "Waktu Reservasi";
            // 
            // CustMeja
            // 
            this.CustMeja.AutoSize = true;
            this.CustMeja.Location = new System.Drawing.Point(85, 162);
            this.CustMeja.Name = "CustMeja";
            this.CustMeja.Size = new System.Drawing.Size(81, 16);
            this.CustMeja.TabIndex = 13;
            this.CustMeja.Text = "Nomor Meja";
            // 
            // CustomerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 508);
            this.Controls.Add(this.CustMeja);
            this.Controls.Add(this.CustWaktu);
            this.Controls.Add(this.CustTelp);
            this.Controls.Add(this.CustNama);
            this.Controls.Add(this.txtCustMeja);
            this.Controls.Add(this.txtCustWaktu);
            this.Controls.Add(this.txtCustNoTelp);
            this.Controls.Add(this.txtCustNama);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.dgvCustomer);
            this.Controls.Add(this.btnCustHapus);
            this.Controls.Add(this.btnCustTambah);
            this.Controls.Add(this.btnCustLihat);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CustomerPage";
            this.Text = "CustomerPage";
            this.Load += new System.EventHandler(this.CustomerPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCustLihat;
        private System.Windows.Forms.Button btnCustTambah;
        private System.Windows.Forms.Button btnCustHapus;
        private System.Windows.Forms.DataGridView dgvCustomer;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TextBox txtCustNama;
        private System.Windows.Forms.TextBox txtCustNoTelp;
        private System.Windows.Forms.TextBox txtCustWaktu;
        private System.Windows.Forms.TextBox txtCustMeja;
        private System.Windows.Forms.Label CustNama;
        private System.Windows.Forms.Label CustTelp;
        private System.Windows.Forms.Label CustWaktu;
        private System.Windows.Forms.Label CustMeja;
    }
}