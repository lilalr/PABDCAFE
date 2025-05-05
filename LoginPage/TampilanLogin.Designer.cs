namespace LoginPage
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
            this.InUser = new System.Windows.Forms.TextBox();
            this.InPass = new System.Windows.Forms.TextBox();
            this.Masukk = new System.Windows.Forms.Button();
            this.LabUser = new System.Windows.Forms.Label();
            this.LabPass = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InUser
            // 
            this.InUser.Location = new System.Drawing.Point(224, 182);
            this.InUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InUser.Name = "InUser";
            this.InUser.Size = new System.Drawing.Size(430, 26);
            this.InUser.TabIndex = 0;
            // 
            // InPass
            // 
            this.InPass.Location = new System.Drawing.Point(224, 301);
            this.InPass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.InPass.Name = "InPass";
            this.InPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.InPass.Size = new System.Drawing.Size(430, 26);
            this.InPass.TabIndex = 1;
            this.InPass.UseSystemPasswordChar = true;
            // 
            // Masukk
            // 
            this.Masukk.Location = new System.Drawing.Point(398, 389);
            this.Masukk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Masukk.Name = "Masukk";
            this.Masukk.Size = new System.Drawing.Size(84, 29);
            this.Masukk.TabIndex = 2;
            this.Masukk.Text = "MASUK";
            this.Masukk.UseVisualStyleBackColor = true;
            this.Masukk.Click += new System.EventHandler(this.Masukk_Click);
            // 
            // LabUser
            // 
            this.LabUser.AutoSize = true;
            this.LabUser.Location = new System.Drawing.Point(398, 159);
            this.LabUser.Name = "LabUser";
            this.LabUser.Size = new System.Drawing.Size(83, 20);
            this.LabUser.TabIndex = 3;
            this.LabUser.Text = "Username";
            // 
            // LabPass
            // 
            this.LabPass.AutoSize = true;
            this.LabPass.Location = new System.Drawing.Point(398, 278);
            this.LabPass.Name = "LabPass";
            this.LabPass.Size = new System.Drawing.Size(78, 20);
            this.LabPass.TabIndex = 4;
            this.LabPass.Text = "Password";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 562);
            this.Controls.Add(this.LabPass);
            this.Controls.Add(this.LabUser);
            this.Controls.Add(this.Masukk);
            this.Controls.Add(this.InPass);
            this.Controls.Add(this.InUser);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox InUser;
        private System.Windows.Forms.TextBox InPass;
        private System.Windows.Forms.Button Masukk;
        private System.Windows.Forms.Label LabUser;
        private System.Windows.Forms.Label LabPass;
    }
}

