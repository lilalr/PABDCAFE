namespace PABDCAFE
{
    partial class AdminPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminPage));
            this.btnReservasi = new System.Windows.Forms.Button();
            this.btnMeja = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnGrafik = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReservasi
            // 
            this.btnReservasi.BackColor = System.Drawing.Color.SeaShell;
            this.btnReservasi.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnReservasi.Location = new System.Drawing.Point(169, 100);
            this.btnReservasi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReservasi.Name = "btnReservasi";
            this.btnReservasi.Size = new System.Drawing.Size(361, 34);
            this.btnReservasi.TabIndex = 0;
            this.btnReservasi.Text = "Reservasi";
            this.btnReservasi.UseVisualStyleBackColor = false;
            this.btnReservasi.Click += new System.EventHandler(this.btnReservasi_Click);
            // 
            // btnMeja
            // 
            this.btnMeja.BackColor = System.Drawing.Color.SeaShell;
            this.btnMeja.Location = new System.Drawing.Point(169, 161);
            this.btnMeja.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMeja.Name = "btnMeja";
            this.btnMeja.Size = new System.Drawing.Size(361, 34);
            this.btnMeja.TabIndex = 1;
            this.btnMeja.Text = "Meja";
            this.btnMeja.UseVisualStyleBackColor = false;
            this.btnMeja.Click += new System.EventHandler(this.btnMeja_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.BackgroundImage = global::LoginPage.Properties.Resources.df3765d348fc540b36e7c7feb1be7268;
            this.btnLogout.Location = new System.Drawing.Point(617, 304);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(67, 32);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnGrafik
            // 
            this.btnGrafik.BackColor = System.Drawing.Color.SeaShell;
            this.btnGrafik.Location = new System.Drawing.Point(169, 218);
            this.btnGrafik.Name = "btnGrafik";
            this.btnGrafik.Size = new System.Drawing.Size(361, 34);
            this.btnGrafik.TabIndex = 4;
            this.btnGrafik.Text = "Lihat Grafik";
            this.btnGrafik.UseVisualStyleBackColor = false;
            this.btnGrafik.Click += new System.EventHandler(this.btnGrafik_Click);
            // 
            // AdminPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(711, 358);
            this.Controls.Add(this.btnGrafik);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnMeja);
            this.Controls.Add(this.btnReservasi);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AdminPage";
            this.Text = "AdminPage";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReservasi;
        private System.Windows.Forms.Button btnMeja;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnGrafik;
    }
}