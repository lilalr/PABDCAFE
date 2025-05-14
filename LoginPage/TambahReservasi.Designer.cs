namespace LoginPage
{
    partial class TambahReservasi
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnCustAdd = new System.Windows.Forms.Button();
            this.txtNoTelp = new System.Windows.Forms.TextBox();
            this.txtWaktuReservasi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 69);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(273, 22);
            this.textBox1.TabIndex = 0;
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
            // txtNoTelp
            // 
            this.txtNoTelp.Location = new System.Drawing.Point(120, 108);
            this.txtNoTelp.Name = "txtNoTelp";
            this.txtNoTelp.Size = new System.Drawing.Size(273, 22);
            this.txtNoTelp.TabIndex = 2;
            // 
            // txtWaktuReservasi
            // 
            this.txtWaktuReservasi.Location = new System.Drawing.Point(120, 153);
            this.txtWaktuReservasi.Name = "txtWaktuReservasi";
            this.txtWaktuReservasi.Size = new System.Drawing.Size(273, 22);
            this.txtWaktuReservasi.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Nama Customer";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // TambahReservasi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWaktuReservasi);
            this.Controls.Add(this.txtNoTelp);
            this.Controls.Add(this.btnCustAdd);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "TambahReservasi";
            this.Text = "Admin Reservasi";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnCustAdd;
        private System.Windows.Forms.TextBox txtNoTelp;
        private System.Windows.Forms.TextBox txtWaktuReservasi;
        private System.Windows.Forms.Label label1;
    }
}