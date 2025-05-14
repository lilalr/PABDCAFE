namespace PABDCAFE
{
    partial class TambahPage
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
            this.txtArNama = new System.Windows.Forms.TextBox();
            this.btnArAdd = new System.Windows.Forms.Button();
            this.txtArNoTelp = new System.Windows.Forms.TextBox();
            this.txtArWaktu = new System.Windows.Forms.TextBox();
            this.ArNama = new System.Windows.Forms.Label();
            this.ArNoTelp = new System.Windows.Forms.Label();
            this.ArWaktu = new System.Windows.Forms.Label();
            this.dataGridViewTr = new System.Windows.Forms.DataGridView();
            this.txtArMeja = new System.Windows.Forms.TextBox();
            this.ArMeja = new System.Windows.Forms.Label();
            this.btnArHapus = new System.Windows.Forms.Button();
            this.btnArEdit = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTr)).BeginInit();
            this.SuspendLayout();
            // 
            // txtArNama
            // 
            this.txtArNama.Location = new System.Drawing.Point(183, 22);
            this.txtArNama.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtArNama.Name = "txtArNama";
            this.txtArNama.Size = new System.Drawing.Size(273, 22);
            this.txtArNama.TabIndex = 0;
            this.txtArNama.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnArAdd
            // 
            this.btnArAdd.Location = new System.Drawing.Point(547, 28);
            this.btnArAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnArAdd.Name = "btnArAdd";
            this.btnArAdd.Size = new System.Drawing.Size(135, 29);
            this.btnArAdd.TabIndex = 1;
            this.btnArAdd.Text = "Tambah Reservasi";
            this.btnArAdd.UseVisualStyleBackColor = true;
            this.btnArAdd.Click += new System.EventHandler(this.btnArAdd_Click);
            // 
            // txtArNoTelp
            // 
            this.txtArNoTelp.Location = new System.Drawing.Point(183, 49);
            this.txtArNoTelp.Name = "txtArNoTelp";
            this.txtArNoTelp.Size = new System.Drawing.Size(273, 22);
            this.txtArNoTelp.TabIndex = 2;
            // 
            // txtArWaktu
            // 
            this.txtArWaktu.Location = new System.Drawing.Point(183, 77);
            this.txtArWaktu.Name = "txtArWaktu";
            this.txtArWaktu.Size = new System.Drawing.Size(273, 22);
            this.txtArWaktu.TabIndex = 3;
            this.txtArWaktu.TextChanged += new System.EventHandler(this.txtWaktuReservasi_TextChanged);
            // 
            // ArNama
            // 
            this.ArNama.AutoSize = true;
            this.ArNama.Location = new System.Drawing.Point(62, 28);
            this.ArNama.Name = "ArNama";
            this.ArNama.Size = new System.Drawing.Size(104, 16);
            this.ArNama.TabIndex = 4;
            this.ArNama.Text = "Nama Customer";
            this.ArNama.Click += new System.EventHandler(this.label1_Click);
            // 
            // ArNoTelp
            // 
            this.ArNoTelp.AutoSize = true;
            this.ArNoTelp.Location = new System.Drawing.Point(64, 55);
            this.ArNoTelp.Name = "ArNoTelp";
            this.ArNoTelp.Size = new System.Drawing.Size(102, 16);
            this.ArNoTelp.TabIndex = 5;
            this.ArNoTelp.Text = "Nomor Telepon";
            // 
            // ArWaktu
            // 
            this.ArWaktu.AutoSize = true;
            this.ArWaktu.Location = new System.Drawing.Point(64, 83);
            this.ArWaktu.Name = "ArWaktu";
            this.ArWaktu.Size = new System.Drawing.Size(110, 16);
            this.ArWaktu.TabIndex = 6;
            this.ArWaktu.Text = "Waktu Reservasi";
            // 
            // dataGridViewTr
            // 
            this.dataGridViewTr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTr.Location = new System.Drawing.Point(47, 189);
            this.dataGridViewTr.Name = "dataGridViewTr";
            this.dataGridViewTr.RowHeadersWidth = 51;
            this.dataGridViewTr.RowTemplate.Height = 24;
            this.dataGridViewTr.Size = new System.Drawing.Size(569, 150);
            this.dataGridViewTr.TabIndex = 7;
            // 
            // txtArMeja
            // 
            this.txtArMeja.Location = new System.Drawing.Point(183, 106);
            this.txtArMeja.Name = "txtArMeja";
            this.txtArMeja.Size = new System.Drawing.Size(273, 22);
            this.txtArMeja.TabIndex = 8;
            // 
            // ArMeja
            // 
            this.ArMeja.AutoSize = true;
            this.ArMeja.Location = new System.Drawing.Point(65, 111);
            this.ArMeja.Name = "ArMeja";
            this.ArMeja.Size = new System.Drawing.Size(81, 16);
            this.ArMeja.TabIndex = 9;
            this.ArMeja.Text = "Nomor Meja";
            // 
            // btnArHapus
            // 
            this.btnArHapus.Location = new System.Drawing.Point(547, 62);
            this.btnArHapus.Name = "btnArHapus";
            this.btnArHapus.Size = new System.Drawing.Size(135, 29);
            this.btnArHapus.TabIndex = 10;
            this.btnArHapus.Text = "Hapus Reservasi";
            this.btnArHapus.UseVisualStyleBackColor = true;
            this.btnArHapus.Click += new System.EventHandler(this.btnArHapus_Click);
            // 
            // btnArEdit
            // 
            this.btnArEdit.Location = new System.Drawing.Point(547, 97);
            this.btnArEdit.Name = "btnArEdit";
            this.btnArEdit.Size = new System.Drawing.Size(135, 29);
            this.btnArEdit.TabIndex = 11;
            this.btnArEdit.Text = "Edit Reservasi";
            this.btnArEdit.UseVisualStyleBackColor = true;
            // 
            // AdminReservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.btnArEdit);
            this.Controls.Add(this.btnArHapus);
            this.Controls.Add(this.ArMeja);
            this.Controls.Add(this.txtArMeja);
            this.Controls.Add(this.dataGridViewTr);
            this.Controls.Add(this.ArWaktu);
            this.Controls.Add(this.ArNoTelp);
            this.Controls.Add(this.ArNama);
            this.Controls.Add(this.txtArWaktu);
            this.Controls.Add(this.txtArNoTelp);
            this.Controls.Add(this.btnArAdd);
            this.Controls.Add(this.txtArNama);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AdminReservasi";
            this.Text = "AdminReservasi";
            this.Load += new System.EventHandler(this.Tambah_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnArAdd;
        private System.Windows.Forms.TextBox txtArNoTelp;
        private System.Windows.Forms.TextBox txtArWaktu;
        private System.Windows.Forms.Label ArNama;
        private System.Windows.Forms.Label ArNoTelp;
        private System.Windows.Forms.Label ArWaktu;
        private System.Windows.Forms.TextBox txtArNama;
        private System.Windows.Forms.DataGridView dataGridViewTr;
        private System.Windows.Forms.TextBox txtArMeja;
        private System.Windows.Forms.Label ArMeja;
        private System.Windows.Forms.Button btnArHapus;
        private System.Windows.Forms.Button btnArEdit;
    }
}