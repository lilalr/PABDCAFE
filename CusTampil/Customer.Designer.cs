namespace CusTampil
{
    partial class Customer
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtCus1 = new System.Windows.Forms.TextBox();
            this.txtCus2 = new System.Windows.Forms.TextBox();
            this.txtCus4 = new System.Windows.Forms.TextBox();
            this.txtCus3 = new System.Windows.Forms.TextBox();
            this.dgvCus = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCus)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nama";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "No Telp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pilih Meja";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "Waktu Reservasi";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(628, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 43);
            this.button1.TabIndex = 4;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnSubmit);
            // 
            // txtCus1
            // 
            this.txtCus1.Location = new System.Drawing.Point(224, 103);
            this.txtCus1.Name = "txtCus1";
            this.txtCus1.Size = new System.Drawing.Size(320, 26);
            this.txtCus1.TabIndex = 7;
            // 
            // txtCus2
            // 
            this.txtCus2.Location = new System.Drawing.Point(224, 144);
            this.txtCus2.Name = "txtCus2";
            this.txtCus2.Size = new System.Drawing.Size(320, 26);
            this.txtCus2.TabIndex = 8;
            // 
            // txtCus4
            // 
            this.txtCus4.Location = new System.Drawing.Point(224, 225);
            this.txtCus4.Name = "txtCus4";
            this.txtCus4.Size = new System.Drawing.Size(320, 26);
            this.txtCus4.TabIndex = 9;
            // 
            // txtCus3
            // 
            this.txtCus3.Location = new System.Drawing.Point(224, 184);
            this.txtCus3.Name = "txtCus3";
            this.txtCus3.Size = new System.Drawing.Size(320, 26);
            this.txtCus3.TabIndex = 10;
            // 
            // dgvCus
            // 
            this.dgvCus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCus.Location = new System.Drawing.Point(66, 319);
            this.dgvCus.Name = "dgvCus";
            this.dgvCus.RowHeadersWidth = 62;
            this.dgvCus.RowTemplate.Height = 28;
            this.dgvCus.Size = new System.Drawing.Size(597, 180);
            this.dgvCus.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(628, 209);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 42);
            this.button2.TabIndex = 12;
            this.button2.Text = "Hapus";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.btnHapus);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(752, 633);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dgvCus);
            this.Controls.Add(this.txtCus3);
            this.Controls.Add(this.txtCus4);
            this.Controls.Add(this.txtCus2);
            this.Controls.Add(this.txtCus1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Halaman Customer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtCus1;
        private System.Windows.Forms.TextBox txtCus2;
        private System.Windows.Forms.TextBox txtCus4;
        private System.Windows.Forms.TextBox txtCus3;
        private System.Windows.Forms.DataGridView dgvCus;
        private System.Windows.Forms.Button button2;
    }
}

