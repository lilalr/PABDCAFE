namespace PABDCAFE
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.txtNoTelp = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.dgvKafe = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpReservasii = new System.Windows.Forms.DateTimePicker();
            this.cmbMeja = new System.Windows.Forms.ComboBox();
            this.btnback = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKafe)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(610, 85);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Tambah";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnTambah);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(610, 114);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Edit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnEdit);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(610, 143);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Hapus";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.btnHapus);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(610, 172);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Refresh";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btnRefresh);
            // 
            // txtNoTelp
            // 
            this.txtNoTelp.Location = new System.Drawing.Point(224, 115);
            this.txtNoTelp.Name = "txtNoTelp";
            this.txtNoTelp.Size = new System.Drawing.Size(332, 22);
            this.txtNoTelp.TabIndex = 4;
            this.txtNoTelp.TextChanged += new System.EventHandler(this.txtChoose_TextChanged);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(224, 79);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(332, 22);
            this.txtName.TabIndex = 6;
            // 
            // dgvKafe
            // 
            this.dgvKafe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKafe.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvKafe.Location = new System.Drawing.Point(0, 404);
            this.dgvKafe.Name = "dgvKafe";
            this.dgvKafe.RowHeadersWidth = 51;
            this.dgvKafe.RowTemplate.Height = 24;
            this.dgvKafe.Size = new System.Drawing.Size(811, 96);
            this.dgvKafe.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(100, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Nama";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(100, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 10;
            this.label2.Text = "No Telp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 158);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Pilih Meja";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(100, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 16);
            this.label4.TabIndex = 13;
            this.label4.Text = "Waktu Reservasi";
            // 
            // dtpReservasii
            // 
            this.dtpReservasii.Location = new System.Drawing.Point(224, 193);
            this.dtpReservasii.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpReservasii.Name = "dtpReservasii";
            this.dtpReservasii.Size = new System.Drawing.Size(332, 22);
            this.dtpReservasii.TabIndex = 14;
            this.dtpReservasii.Tag = "dtpReservasi";
            // 
            // cmbMeja
            // 
            this.cmbMeja.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeja.FormattingEnabled = true;
            this.cmbMeja.Location = new System.Drawing.Point(224, 155);
            this.cmbMeja.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbMeja.Name = "cmbMeja";
            this.cmbMeja.Size = new System.Drawing.Size(332, 24);
            this.cmbMeja.TabIndex = 15;
            // 
            // btnback
            // 
            this.btnback.Location = new System.Drawing.Point(610, 201);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(75, 23);
            this.btnback.TabIndex = 16;
            this.btnback.Text = "kembali";
            this.btnback.UseVisualStyleBackColor = true;
            this.btnback.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 500);
            this.Controls.Add(this.btnback);
            this.Controls.Add(this.cmbMeja);
            this.Controls.Add(this.dtpReservasii);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvKafe);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtNoTelp);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Halaman Admin";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKafe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtNoTelp;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.ComboBox cmbMeja;
        private System.Windows.Forms.DateTimePicker dtpReservasii;
        private System.Windows.Forms.DataGridView dgvKafe;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnback;
    }
}

