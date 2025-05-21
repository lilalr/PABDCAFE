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
            this.btnCustTambah = new System.Windows.Forms.Button();
            this.btnCustHapus = new System.Windows.Forms.Button();
            this.dgvCustomer = new System.Windows.Forms.DataGridView();
            this.btnLogout = new System.Windows.Forms.Button();
            this.txtCustNama = new System.Windows.Forms.TextBox();
            this.txtCustNoTelp = new System.Windows.Forms.TextBox();
            this.CustNama = new System.Windows.Forms.Label();
            this.CustTelp = new System.Windows.Forms.Label();
            this.CustWaktu = new System.Windows.Forms.Label();
            this.CustMeja = new System.Windows.Forms.Label();
            this.cmbCustMeja = new System.Windows.Forms.ComboBox();
            this.dtpCustWaktu = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCustTambah
            // 
            this.btnCustTambah.Location = new System.Drawing.Point(764, 83);
            this.btnCustTambah.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCustTambah.Name = "btnCustTambah";
            this.btnCustTambah.Size = new System.Drawing.Size(155, 44);
            this.btnCustTambah.TabIndex = 2;
            this.btnCustTambah.Text = "Tambah Reservasi";
            this.btnCustTambah.UseVisualStyleBackColor = true;
            this.btnCustTambah.Click += new System.EventHandler(this.btnCustTambah_Click);
            // 
            // btnCustHapus
            // 
            this.btnCustHapus.Location = new System.Drawing.Point(764, 150);
            this.btnCustHapus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCustHapus.Name = "btnCustHapus";
            this.btnCustHapus.Size = new System.Drawing.Size(155, 46);
            this.btnCustHapus.TabIndex = 3;
            this.btnCustHapus.Text = "Hapus Reservasi";
            this.btnCustHapus.UseVisualStyleBackColor = true;
            this.btnCustHapus.Click += new System.EventHandler(this.btnCustHapus_Click);
            // 
            // dgvCustomer
            // 
            this.dgvCustomer.BackgroundColor = System.Drawing.Color.LavenderBlush;
            this.dgvCustomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomer.Location = new System.Drawing.Point(55, 286);
            this.dgvCustomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvCustomer.Name = "dgvCustomer";
            this.dgvCustomer.RowHeadersWidth = 62;
            this.dgvCustomer.RowTemplate.Height = 28;
            this.dgvCustomer.Size = new System.Drawing.Size(864, 254);
            this.dgvCustomer.TabIndex = 4;
            this.dgvCustomer.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomer_CellClick);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(887, 565);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 34);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // txtCustNama
            // 
            this.txtCustNama.Location = new System.Drawing.Point(232, 58);
            this.txtCustNama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCustNama.Name = "txtCustNama";
            this.txtCustNama.Size = new System.Drawing.Size(465, 23);
            this.txtCustNama.TabIndex = 6;
            // 
            // txtCustNoTelp
            // 
            this.txtCustNoTelp.Location = new System.Drawing.Point(232, 104);
            this.txtCustNoTelp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCustNoTelp.Name = "txtCustNoTelp";
            this.txtCustNoTelp.Size = new System.Drawing.Size(465, 23);
            this.txtCustNoTelp.TabIndex = 7;
            // 
            // CustNama
            // 
            this.CustNama.AutoSize = true;
            this.CustNama.BackColor = System.Drawing.Color.Transparent;
            this.CustNama.Location = new System.Drawing.Point(89, 62);
            this.CustNama.Name = "CustNama";
            this.CustNama.Size = new System.Drawing.Size(109, 17);
            this.CustNama.TabIndex = 10;
            this.CustNama.Text = "Nama Customer";
            // 
            // CustTelp
            // 
            this.CustTelp.AutoSize = true;
            this.CustTelp.BackColor = System.Drawing.Color.Transparent;
            this.CustTelp.Location = new System.Drawing.Point(90, 107);
            this.CustTelp.Name = "CustTelp";
            this.CustTelp.Size = new System.Drawing.Size(106, 17);
            this.CustTelp.TabIndex = 11;
            this.CustTelp.Text = "Nomor Telepon";
            // 
            // CustWaktu
            // 
            this.CustWaktu.AutoSize = true;
            this.CustWaktu.BackColor = System.Drawing.Color.Transparent;
            this.CustWaktu.Location = new System.Drawing.Point(89, 153);
            this.CustWaktu.Name = "CustWaktu";
            this.CustWaktu.Size = new System.Drawing.Size(115, 17);
            this.CustWaktu.TabIndex = 12;
            this.CustWaktu.Text = "Waktu Reservasi";
            // 
            // CustMeja
            // 
            this.CustMeja.AutoSize = true;
            this.CustMeja.BackColor = System.Drawing.Color.Transparent;
            this.CustMeja.Location = new System.Drawing.Point(91, 198);
            this.CustMeja.Name = "CustMeja";
            this.CustMeja.Size = new System.Drawing.Size(84, 17);
            this.CustMeja.TabIndex = 13;
            this.CustMeja.Text = "Nomor Meja";
            // 
            // cmbCustMeja
            // 
            this.cmbCustMeja.FormattingEnabled = true;
            this.cmbCustMeja.Location = new System.Drawing.Point(232, 195);
            this.cmbCustMeja.Name = "cmbCustMeja";
            this.cmbCustMeja.Size = new System.Drawing.Size(200, 24);
            this.cmbCustMeja.TabIndex = 14;
            // 
            // dtpCustWaktu
            // 
            this.dtpCustWaktu.Location = new System.Drawing.Point(232, 147);
            this.dtpCustWaktu.MaxDate = new System.DateTime(2025, 12, 31, 0, 0, 0, 0);
            this.dtpCustWaktu.MinDate = new System.DateTime(2025, 5, 22, 0, 0, 0, 0);
            this.dtpCustWaktu.Name = "dtpCustWaktu";
            this.dtpCustWaktu.Size = new System.Drawing.Size(200, 23);
            this.dtpCustWaktu.TabIndex = 15;
            this.dtpCustWaktu.Value = new System.DateTime(2025, 5, 22, 1, 49, 14, 0);
            this.dtpCustWaktu.ValueChanged += new System.EventHandler(this.dtpCustWaktu_ValueChanged);
            // 
            // CustomerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoginPage.Properties.Resources.WhatsApp_Image_2025_05_19_at_00_38_34_dbdf79d4;
            this.ClientSize = new System.Drawing.Size(993, 635);
            this.Controls.Add(this.dtpCustWaktu);
            this.Controls.Add(this.cmbCustMeja);
            this.Controls.Add(this.CustMeja);
            this.Controls.Add(this.CustWaktu);
            this.Controls.Add(this.CustTelp);
            this.Controls.Add(this.CustNama);
            this.Controls.Add(this.txtCustNoTelp);
            this.Controls.Add(this.txtCustNama);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.dgvCustomer);
            this.Controls.Add(this.btnCustHapus);
            this.Controls.Add(this.btnCustTambah);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CustomerPage";
            this.Text = "CustomerPage";
            this.Load += new System.EventHandler(this.CustomerPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCustTambah;
        private System.Windows.Forms.Button btnCustHapus;
        private System.Windows.Forms.DataGridView dgvCustomer;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TextBox txtCustNama;
        private System.Windows.Forms.TextBox txtCustNoTelp;
        private System.Windows.Forms.Label CustNama;
        private System.Windows.Forms.Label CustTelp;
        private System.Windows.Forms.Label CustWaktu;
        private System.Windows.Forms.Label CustMeja;
        private System.Windows.Forms.ComboBox cmbCustMeja;
        private System.Windows.Forms.DateTimePicker dtpCustWaktu;
    }
}