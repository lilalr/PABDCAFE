namespace PABDCAFE
{
    partial class PreviewDataReservasi
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.dgvPreviewReservasi = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviewReservasi)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackgroundImage = global::LoginPage.Properties.Resources.df3765d348fc540b36e7c7feb1be7268;
            this.btnOK.Location = new System.Drawing.Point(534, 344);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(67, 30);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImage = global::LoginPage.Properties.Resources.df3765d348fc540b36e7c7feb1be7268;
            this.btnCancel.Location = new System.Drawing.Point(41, 344);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(67, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // dgvPreviewReservasi
            // 
            this.dgvPreviewReservasi.BackgroundColor = System.Drawing.Color.SeaShell;
            this.dgvPreviewReservasi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPreviewReservasi.Location = new System.Drawing.Point(41, 43);
            this.dgvPreviewReservasi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvPreviewReservasi.Name = "dgvPreviewReservasi";
            this.dgvPreviewReservasi.RowHeadersWidth = 62;
            this.dgvPreviewReservasi.RowTemplate.Height = 28;
            this.dgvPreviewReservasi.Size = new System.Drawing.Size(560, 285);
            this.dgvPreviewReservasi.TabIndex = 5;
            // 
            // PreviewDataReservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LoginPage.Properties.Resources.df3765d348fc540b36e7c7feb1be7268;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(651, 394);
            this.Controls.Add(this.dgvPreviewReservasi);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "PreviewDataReservasi";
            this.Text = "PreviewDataReservasi";
            this.Load += new System.EventHandler(this.PreviewFormAdminReservasi_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPreviewReservasi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView dgvPreviewReservasi;
    }
}