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
            this.dgvCutomer = new System.Windows.Forms.DataGridView();
            this.btnLogout = new System.Windows.Forms.Button();
            this.textCustNama = new System.Windows.Forms.TextBox();
            this.txtCustNoTelp = new System.Windows.Forms.TextBox();
            this.txtCustWaktu = new System.Windows.Forms.TextBox();
            this.txtCustNoMeja = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCutomer)).BeginInit();
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
            this.btnCustHapus.Location = new System.Drawing.Point(679, 147);
            this.btnCustHapus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCustHapus.Name = "btnCustHapus";
            this.btnCustHapus.Size = new System.Drawing.Size(138, 41);
            this.btnCustHapus.TabIndex = 3;
            this.btnCustHapus.Text = "Hapus Reservasi";
            this.btnCustHapus.UseVisualStyleBackColor = true;
            this.btnCustHapus.Click += new System.EventHandler(this.btnCustHapus_Click);
            // 
            // dgvCutomer
            // 
            this.dgvCutomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCutomer.Location = new System.Drawing.Point(49, 229);
            this.dgvCutomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvCutomer.Name = "dgvCutomer";
            this.dgvCutomer.RowHeadersWidth = 62;
            this.dgvCutomer.RowTemplate.Height = 28;
            this.dgvCutomer.Size = new System.Drawing.Size(739, 203);
            this.dgvCutomer.TabIndex = 4;
            this.dgvCutomer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCutomer_CellContentClick);
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
            // textCustNama
            // 
            this.textCustNama.Location = new System.Drawing.Point(208, 24);
            this.textCustNama.Name = "textCustNama";
            this.textCustNama.Size = new System.Drawing.Size(403, 22);
            this.textCustNama.TabIndex = 6;
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
            // txtCustNoMeja
            // 
            this.txtCustNoMeja.Location = new System.Drawing.Point(208, 156);
            this.txtCustNoMeja.Name = "txtCustNoMeja";
            this.txtCustNoMeja.Size = new System.Drawing.Size(403, 22);
            this.txtCustNoMeja.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Nama Customer";
            // 
            // CustomerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 508);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCustNoMeja);
            this.Controls.Add(this.txtCustWaktu);
            this.Controls.Add(this.txtCustNoTelp);
            this.Controls.Add(this.textCustNama);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.dgvCutomer);
            this.Controls.Add(this.btnCustHapus);
            this.Controls.Add(this.btnCustTambah);
            this.Controls.Add(this.btnCustLihat);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CustomerPage";
            this.Text = "CustomerPage";
            this.Load += new System.EventHandler(this.CustomerPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCutomer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCustLihat;
        private System.Windows.Forms.Button btnCustTambah;
        private System.Windows.Forms.Button btnCustHapus;
        private System.Windows.Forms.DataGridView dgvCutomer;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.TextBox textCustNama;
        private System.Windows.Forms.TextBox txtCustNoTelp;
        private System.Windows.Forms.TextBox txtCustWaktu;
        private System.Windows.Forms.TextBox txtCustNoMeja;
        private System.Windows.Forms.Label label1;
    }
}