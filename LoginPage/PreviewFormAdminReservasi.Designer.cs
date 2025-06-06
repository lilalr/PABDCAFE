namespace LoginPage
{
    partial class PreviewFormAdminReservasi
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
            this.dgvPreviewAdminReservasi = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviewAdminReservasi)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPreviewAdminReservasi
            // 
            this.dgvPreviewAdminReservasi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreviewAdminReservasi.Location = new System.Drawing.Point(61, 54);
            this.dgvPreviewAdminReservasi.Name = "dgvPreviewAdminReservasi";
            this.dgvPreviewAdminReservasi.RowHeadersWidth = 51;
            this.dgvPreviewAdminReservasi.RowTemplate.Height = 24;
            this.dgvPreviewAdminReservasi.Size = new System.Drawing.Size(676, 321);
            this.dgvPreviewAdminReservasi.TabIndex = 0;
            this.dgvPreviewAdminReservasi.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPreviewAdminReservasi_CellContentClick);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(662, 390);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // PreviewFormAdminReservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgvPreviewAdminReservasi);
            this.Name = "PreviewFormAdminReservasi";
            this.Text = "PreviewDataReservasi";
            this.Load += new System.EventHandler(this.PreviewFormAdminReservasi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviewAdminReservasi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPreviewAdminReservasi;
        private System.Windows.Forms.Button btnOK;
    }
}