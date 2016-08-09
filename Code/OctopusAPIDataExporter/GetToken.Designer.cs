namespace OctopusAPIDataExporter
{
    partial class GetToken
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetToken));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_tokenUrl = new System.Windows.Forms.TextBox();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_tokenText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Token获取地址";
            // 
            // textBox_tokenUrl
            // 
            this.textBox_tokenUrl.Location = new System.Drawing.Point(102, 10);
            this.textBox_tokenUrl.Name = "textBox_tokenUrl";
            this.textBox_tokenUrl.Size = new System.Drawing.Size(270, 21);
            this.textBox_tokenUrl.TabIndex = 1;
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(102, 39);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.Size = new System.Drawing.Size(111, 21);
            this.textBox_userName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "八爪鱼用户名";
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(254, 39);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(118, 21);
            this.textBox_password.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "密码";
            // 
            // textBox_tokenText
            // 
            this.textBox_tokenText.Location = new System.Drawing.Point(102, 66);
            this.textBox_tokenText.Multiline = true;
            this.textBox_tokenText.Name = "textBox_tokenText";
            this.textBox_tokenText.ReadOnly = true;
            this.textBox_tokenText.Size = new System.Drawing.Size(270, 164);
            this.textBox_tokenText.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Token";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(102, 236);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(270, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "获取Token";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GetToken
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_tokenText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_userName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_tokenUrl);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 300);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "GetToken";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成Token";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_tokenUrl;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_tokenText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
    }
}

