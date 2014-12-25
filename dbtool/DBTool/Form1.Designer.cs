namespace net.ELF.DBTool
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
            this.PDFilePath = new System.Windows.Forms.TextBox();
            this.CodeFilePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DBConnectStr = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.DefaultNamespace = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DefaultBaseObject = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.DefaultTreeBaseName = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PDFilePath
            // 
            this.PDFilePath.Location = new System.Drawing.Point(95, 12);
            this.PDFilePath.Name = "PDFilePath";
            this.PDFilePath.Size = new System.Drawing.Size(251, 21);
            this.PDFilePath.TabIndex = 0;
            this.PDFilePath.Text = "D:\\elf\\trunk\\docs\\开发\\DB\\elfOOM.oom";
            // 
            // CodeFilePath
            // 
            this.CodeFilePath.Location = new System.Drawing.Point(95, 39);
            this.CodeFilePath.Name = "CodeFilePath";
            this.CodeFilePath.Size = new System.Drawing.Size(251, 21);
            this.CodeFilePath.TabIndex = 1;
            this.CodeFilePath.Text = "D:\\elf\\trunk\\code\\DB\\Base";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "PD文件";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "DBBase文件夹";
            // 
            // DBConnectStr
            // 
            this.DBConnectStr.Location = new System.Drawing.Point(12, 94);
            this.DBConnectStr.Name = "DBConnectStr";
            this.DBConnectStr.Size = new System.Drawing.Size(347, 21);
            this.DBConnectStr.TabIndex = 4;
            this.DBConnectStr.Text = "data source=127.0.0.1;user id=sa;password=1;Initial Catalog=elf";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "数据库连接字符串";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(242, 219);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "代码-数据库";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(149, 219);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "PD-代码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(14, 219);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 9;
            this.button3.Text = "所有sql";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "默认命名空间";
            // 
            // DefaultNamespace
            // 
            this.DefaultNamespace.Location = new System.Drawing.Point(127, 126);
            this.DefaultNamespace.Name = "DefaultNamespace";
            this.DefaultNamespace.Size = new System.Drawing.Size(152, 21);
            this.DefaultNamespace.TabIndex = 11;
            this.DefaultNamespace.Text = "net.ELF";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "DB默认继承类名";
            // 
            // DefaultBaseObject
            // 
            this.DefaultBaseObject.Location = new System.Drawing.Point(127, 154);
            this.DefaultBaseObject.Name = "DefaultBaseObject";
            this.DefaultBaseObject.Size = new System.Drawing.Size(232, 21);
            this.DefaultBaseObject.TabIndex = 13;
            this.DefaultBaseObject.Text = "DBTableUserObject,My_DbTableObject";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 186);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "默认继承树形类名";
            // 
            // DefaultTreeBaseName
            // 
            this.DefaultTreeBaseName.Location = new System.Drawing.Point(127, 181);
            this.DefaultTreeBaseName.Name = "DefaultTreeBaseName";
            this.DefaultTreeBaseName.Size = new System.Drawing.Size(152, 21);
            this.DefaultTreeBaseName.TabIndex = 15;
            this.DefaultTreeBaseName.Text = "TreeBase";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(149, 248);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(197, 47);
            this.button4.TabIndex = 16;
            this.button4.Text = "PD-代码-数据库";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 307);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.DefaultTreeBaseName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DefaultBaseObject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.DefaultNamespace);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DBConnectStr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CodeFilePath);
            this.Controls.Add(this.PDFilePath);
            this.Name = "Form1";
            this.Text = "DBToolMain";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PDFilePath;
        private System.Windows.Forms.TextBox CodeFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DBConnectStr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox DefaultNamespace;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox DefaultBaseObject;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox DefaultTreeBaseName;
        private System.Windows.Forms.Button button4;
    }
}

