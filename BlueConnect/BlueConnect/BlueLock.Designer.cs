namespace BlueConnect
{
    partial class BlueLock
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
            this.oFileLock = new System.Windows.Forms.OpenFileDialog();
            this.btnOFD = new System.Windows.Forms.Button();
            this.txtSelectedFiles = new System.Windows.Forms.TextBox();
            this.btnLock = new System.Windows.Forms.Button();
            this.btnBlueConnect = new System.Windows.Forms.Button();
            this.txtOut = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // oFileLock
            // 
            this.oFileLock.Multiselect = true;
            // 
            // btnOFD
            // 
            this.btnOFD.Location = new System.Drawing.Point(485, 170);
            this.btnOFD.Name = "btnOFD";
            this.btnOFD.Size = new System.Drawing.Size(122, 38);
            this.btnOFD.TabIndex = 0;
            this.btnOFD.Text = "Select Files to Lock";
            this.btnOFD.UseVisualStyleBackColor = true;
            this.btnOFD.Click += new System.EventHandler(this.btnOFD_Click);
            // 
            // txtSelectedFiles
            // 
            this.txtSelectedFiles.Location = new System.Drawing.Point(36, 159);
            this.txtSelectedFiles.Multiline = true;
            this.txtSelectedFiles.Name = "txtSelectedFiles";
            this.txtSelectedFiles.ReadOnly = true;
            this.txtSelectedFiles.Size = new System.Drawing.Size(395, 121);
            this.txtSelectedFiles.TabIndex = 1;
            // 
            // btnLock
            // 
            this.btnLock.Location = new System.Drawing.Point(36, 327);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(161, 45);
            this.btnLock.TabIndex = 2;
            this.btnLock.Text = "Lock";
            this.btnLock.UseVisualStyleBackColor = true;
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // btnBlueConnect
            // 
            this.btnBlueConnect.Location = new System.Drawing.Point(485, 27);
            this.btnBlueConnect.Name = "btnBlueConnect";
            this.btnBlueConnect.Size = new System.Drawing.Size(122, 38);
            this.btnBlueConnect.TabIndex = 3;
            this.btnBlueConnect.Text = "Connect";
            this.btnBlueConnect.UseVisualStyleBackColor = true;
            this.btnBlueConnect.Click += new System.EventHandler(this.btnBlueConnect_Click);
            // 
            // txtOut
            // 
            this.txtOut.Location = new System.Drawing.Point(36, 12);
            this.txtOut.Multiline = true;
            this.txtOut.Name = "txtOut";
            this.txtOut.ReadOnly = true;
            this.txtOut.Size = new System.Drawing.Size(395, 113);
            this.txtOut.TabIndex = 4;
            // 
            // BlueLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 549);
            this.Controls.Add(this.txtOut);
            this.Controls.Add(this.btnBlueConnect);
            this.Controls.Add(this.btnLock);
            this.Controls.Add(this.txtSelectedFiles);
            this.Controls.Add(this.btnOFD);
            this.Name = "BlueLock";
            this.Text = "BlueLock";
            this.Load += new System.EventHandler(this.BlueLock_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog oFileLock;
        private System.Windows.Forms.Button btnOFD;
        private System.Windows.Forms.TextBox txtSelectedFiles;
        private System.Windows.Forms.Button btnLock;
        private System.Windows.Forms.Button btnBlueConnect;
        private System.Windows.Forms.TextBox txtOut;
    }
}