namespace Snake_Royale_Server
{
    partial class ServerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtServerInfo = new RichTextBox();
            label1 = new Label();
            startBtn = new Button();
            txtIP = new TextBox();
            txtPort = new TextBox();
            label2 = new Label();
            label3 = new Label();
            closeBtn = new Button();
            SuspendLayout();
            // 
            // txtServerInfo
            // 
            txtServerInfo.Location = new Point(12, 46);
            txtServerInfo.Name = "txtServerInfo";
            txtServerInfo.Size = new Size(557, 336);
            txtServerInfo.TabIndex = 0;
            txtServerInfo.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 11);
            label1.Name = "label1";
            label1.Size = new Size(130, 32);
            label1.TabIndex = 1;
            label1.Text = "Server Info";
            // 
            // startBtn
            // 
            startBtn.Location = new Point(575, 125);
            startBtn.Name = "startBtn";
            startBtn.Size = new Size(105, 45);
            startBtn.TabIndex = 2;
            startBtn.Text = "Start";
            startBtn.UseVisualStyleBackColor = true;
            startBtn.Click += startBtn_Click;
            // 
            // txtIP
            // 
            txtIP.Location = new Point(575, 46);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(213, 23);
            txtIP.TabIndex = 3;
            txtIP.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(575, 96);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(213, 23);
            txtPort.TabIndex = 4;
            txtPort.Text = "13000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(575, 25);
            label2.Name = "label2";
            label2.Size = new Size(17, 15);
            label2.TabIndex = 5;
            label2.Text = "IP";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(575, 78);
            label3.Name = "label3";
            label3.Size = new Size(29, 15);
            label3.TabIndex = 6;
            label3.Text = "Port";
            // 
            // closeBtn
            // 
            closeBtn.Enabled = false;
            closeBtn.Location = new Point(686, 125);
            closeBtn.Name = "closeBtn";
            closeBtn.Size = new Size(102, 45);
            closeBtn.TabIndex = 7;
            closeBtn.Text = "Close";
            closeBtn.UseVisualStyleBackColor = true;
            closeBtn.Click += closeBtn_Click;
            // 
            // ServerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(closeBtn);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtPort);
            Controls.Add(txtIP);
            Controls.Add(startBtn);
            Controls.Add(label1);
            Controls.Add(txtServerInfo);
            Name = "ServerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Snake Royale Server";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox txtServerInfo;
        private Label label1;
        private Button startBtn;
        private TextBox txtIP;
        private TextBox txtPort;
        private Label label2;
        private Label label3;
        private Button closeBtn;
    }
}
