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
            ((System.ComponentModel.ISupportInitialize)(this.dgvCutomer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCustLihat
            // 
            this.btnCustLihat.Location = new System.Drawing.Point(12, 12);
            this.btnCustLihat.Name = "btnCustLihat";
            this.btnCustLihat.Size = new System.Drawing.Size(245, 63);
            this.btnCustLihat.TabIndex = 1;
            this.btnCustLihat.Text = "Lihat Reservasi";
            this.btnCustLihat.UseVisualStyleBackColor = true;
            this.btnCustLihat.Click += new System.EventHandler(this.btnCustLihat_Click);
            // 
            // btnCustTambah
            // 
            this.btnCustTambah.Location = new System.Drawing.Point(273, 12);
            this.btnCustTambah.Name = "btnCustTambah";
            this.btnCustTambah.Size = new System.Drawing.Size(245, 63);
            this.btnCustTambah.TabIndex = 2;
            this.btnCustTambah.Text = "Tambah Reservasi";
            this.btnCustTambah.UseVisualStyleBackColor = true;
            this.btnCustTambah.Click += new System.EventHandler(this.btnCustTambah_Click);
            // 
            // btnCustHapus
            // 
            this.btnCustHapus.Location = new System.Drawing.Point(533, 12);
            this.btnCustHapus.Name = "btnCustHapus";
            this.btnCustHapus.Size = new System.Drawing.Size(245, 63);
            this.btnCustHapus.TabIndex = 3;
            this.btnCustHapus.Text = "Hapus Reservasi";
            this.btnCustHapus.UseVisualStyleBackColor = true;
            this.btnCustHapus.Click += new System.EventHandler(this.btnCustHapus_Click);
            // 
            // dgvCutomer
            // 
            this.dgvCutomer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCutomer.Location = new System.Drawing.Point(17, 100);
            this.dgvCutomer.Name = "dgvCutomer";
            this.dgvCutomer.RowHeadersWidth = 62;
            this.dgvCutomer.RowTemplate.Height = 28;
            this.dgvCutomer.Size = new System.Drawing.Size(661, 309);
            this.dgvCutomer.TabIndex = 4;
            this.dgvCutomer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCutomer_CellContentClick);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(703, 375);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 34);
            this.btnLogout.TabIndex = 5;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // CustomerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.dgvCutomer);
            this.Controls.Add(this.btnCustHapus);
            this.Controls.Add(this.btnCustTambah);
            this.Controls.Add(this.btnCustLihat);
            this.Name = "CustomerPage";
            this.Text = "CustomerPage";
            this.Load += new System.EventHandler(this.CustomerPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCutomer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnCustLihat;
        private System.Windows.Forms.Button btnCustTambah;
        private System.Windows.Forms.Button btnCustHapus;
        private System.Windows.Forms.DataGridView dgvCutomer;
        private System.Windows.Forms.Button btnLogout;
    }
}