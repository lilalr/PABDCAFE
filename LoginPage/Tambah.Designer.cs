namespace LoginPage
{
    partial class Tambah
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
            this.txtTrNama = new System.Windows.Forms.TextBox();
            this.btnCustAdd = new System.Windows.Forms.Button();
            this.txtTrNoTelp = new System.Windows.Forms.TextBox();
            this.txtTrWaktu = new System.Windows.Forms.TextBox();
            this.ArNama = new System.Windows.Forms.Label();
            this.ArNoTelp = new System.Windows.Forms.Label();
            this.ArWaktu = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtTrNama
            // 
            this.txtTrNama.Location = new System.Drawing.Point(134, 69);
            this.txtTrNama.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTrNama.Name = "txtTrNama";
            this.txtTrNama.Size = new System.Drawing.Size(273, 22);
            this.txtTrNama.TabIndex = 0;
            this.txtTrNama.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnCustAdd
            // 
            this.btnCustAdd.Location = new System.Drawing.Point(430, 69);
            this.btnCustAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCustAdd.Name = "btnCustAdd";
            this.btnCustAdd.Size = new System.Drawing.Size(69, 29);
            this.btnCustAdd.TabIndex = 1;
            this.btnCustAdd.Text = "add";
            this.btnCustAdd.UseVisualStyleBackColor = true;
            // 
            // txtTrNoTelp
            // 
            this.txtTrNoTelp.Location = new System.Drawing.Point(134, 112);
            this.txtTrNoTelp.Name = "txtTrNoTelp";
            this.txtTrNoTelp.Size = new System.Drawing.Size(273, 22);
            this.txtTrNoTelp.TabIndex = 2;
            // 
            // txtTrWaktu
            // 
            this.txtTrWaktu.Location = new System.Drawing.Point(134, 152);
            this.txtTrWaktu.Name = "txtTrWaktu";
            this.txtTrWaktu.Size = new System.Drawing.Size(273, 22);
            this.txtTrWaktu.TabIndex = 3;
            this.txtTrWaktu.TextChanged += new System.EventHandler(this.txtWaktuReservasi_TextChanged);
            // 
            // ArNama
            // 
            this.ArNama.AutoSize = true;
            this.ArNama.Location = new System.Drawing.Point(12, 75);
            this.ArNama.Name = "ArNama";
            this.ArNama.Size = new System.Drawing.Size(104, 16);
            this.ArNama.TabIndex = 4;
            this.ArNama.Text = "Nama Customer";
            this.ArNama.Click += new System.EventHandler(this.label1_Click);
            // 
            // ArNoTelp
            // 
            this.ArNoTelp.AutoSize = true;
            this.ArNoTelp.Location = new System.Drawing.Point(15, 118);
            this.ArNoTelp.Name = "ArNoTelp";
            this.ArNoTelp.Size = new System.Drawing.Size(102, 16);
            this.ArNoTelp.TabIndex = 5;
            this.ArNoTelp.Text = "Nomor Telepon";
            // 
            // ArWaktu
            // 
            this.ArWaktu.AutoSize = true;
            this.ArWaktu.Location = new System.Drawing.Point(18, 158);
            this.ArWaktu.Name = "ArWaktu";
            this.ArWaktu.Size = new System.Drawing.Size(110, 16);
            this.ArWaktu.TabIndex = 6;
            this.ArWaktu.Text = "Waktu Reservasi";
            // 
            // Tambah
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.ArWaktu);
            this.Controls.Add(this.ArNoTelp);
            this.Controls.Add(this.ArNama);
            this.Controls.Add(this.txtTrWaktu);
            this.Controls.Add(this.txtTrNoTelp);
            this.Controls.Add(this.btnCustAdd);
            this.Controls.Add(this.txtTrNama);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Tambah";
            this.Text = "TambahReservasi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCustAdd;
        private System.Windows.Forms.TextBox txtTrNoTelp;
        private System.Windows.Forms.TextBox txtTrWaktu;
        private System.Windows.Forms.Label ArNama;
        private System.Windows.Forms.Label ArNoTelp;
        private System.Windows.Forms.Label ArWaktu;
        private System.Windows.Forms.TextBox txtTrNama;
    }
}