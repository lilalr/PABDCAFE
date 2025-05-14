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
            this.btnReservasi = new System.Windows.Forms.Button();
            this.btnMeja = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnReservasi
            // 
            this.btnReservasi.Location = new System.Drawing.Point(12, 69);
            this.btnReservasi.Name = "btnReservasi";
            this.btnReservasi.Size = new System.Drawing.Size(152, 42);
            this.btnReservasi.TabIndex = 0;
            this.btnReservasi.Text = "Reservasi";
            this.btnReservasi.UseVisualStyleBackColor = true;
            this.btnReservasi.Click += new System.EventHandler(this.btnReservasi_Click);
            // 
            // btnMeja
            // 
            this.btnMeja.Location = new System.Drawing.Point(12, 108);
            this.btnMeja.Name = "btnMeja";
            this.btnMeja.Size = new System.Drawing.Size(152, 42);
            this.btnMeja.TabIndex = 1;
            this.btnMeja.Text = "Meja";
            this.btnMeja.UseVisualStyleBackColor = true;
            this.btnMeja.Click += new System.EventHandler(this.btnMeja_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(684, 364);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(75, 40);
            this.btnLogout.TabIndex = 3;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // AdminPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnMeja);
            this.Controls.Add(this.btnReservasi);
            this.Name = "AdminPage";
            this.Text = "AdminPage";
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReservasi;
        private System.Windows.Forms.Button btnMeja;
        private System.Windows.Forms.Button btnLogout;
    }
}