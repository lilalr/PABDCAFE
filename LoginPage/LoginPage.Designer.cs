namespace LoginPage
{
    partial class LoginPage
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
            this.btnPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnUsername = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPassword
            // 
            this.btnPassword.AutoSize = true;
            this.btnPassword.Location = new System.Drawing.Point(390, 242);
            this.btnPassword.Name = "btnPassword";
            this.btnPassword.Size = new System.Drawing.Size(78, 20);
            this.btnPassword.TabIndex = 1;
            this.btnPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(336, 171);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(180, 26);
            this.txtUsername.TabIndex = 2;
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(336, 279);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(180, 26);
            this.txtPass.TabIndex = 3;
            // 
            // btnUsername
            // 
            this.btnUsername.AutoSize = true;
            this.btnUsername.Location = new System.Drawing.Point(385, 137);
            this.btnUsername.Name = "btnUsername";
            this.btnUsername.Size = new System.Drawing.Size(83, 20);
            this.btnUsername.TabIndex = 0;
            this.btnUsername.Text = "Username";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(389, 350);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(79, 38);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(714, 469);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 32);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "< back";
            this.btnBack.UseVisualStyleBackColor = true;
            // 
            // LoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 539);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.btnPassword);
            this.Controls.Add(this.btnUsername);
            this.Name = "LoginPage";
            this.Text = "LoginPage";
            this.Load += new System.EventHandler(this.LoginPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label btnPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label btnUsername;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnBack;
    }
}