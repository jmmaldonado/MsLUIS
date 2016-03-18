namespace BBVAIndexerWinForm {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.fbd = new System.Windows.Forms.FolderBrowserDialog();
            this.botSelectFolders = new System.Windows.Forms.Button();
            this.listFiles = new System.Windows.Forms.ListBox();
            this.botGo = new System.Windows.Forms.Button();
            this.botAuto = new System.Windows.Forms.Button();
            this.timerAutoProcess = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textSelectedFolder = new System.Windows.Forms.TextBox();
            this.textLog = new System.Windows.Forms.TextBox();
            this.botTestReadFromBlob = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.botTestJSON2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // botSelectFolders
            // 
            this.botSelectFolders.Location = new System.Drawing.Point(304, 12);
            this.botSelectFolders.Name = "botSelectFolders";
            this.botSelectFolders.Size = new System.Drawing.Size(23, 20);
            this.botSelectFolders.TabIndex = 0;
            this.botSelectFolders.Text = "S";
            this.botSelectFolders.UseVisualStyleBackColor = true;
            this.botSelectFolders.Click += new System.EventHandler(this.botSelectFolders_Click);
            // 
            // listFiles
            // 
            this.listFiles.FormattingEnabled = true;
            this.listFiles.HorizontalScrollbar = true;
            this.listFiles.Location = new System.Drawing.Point(12, 41);
            this.listFiles.Name = "listFiles";
            this.listFiles.Size = new System.Drawing.Size(315, 420);
            this.listFiles.TabIndex = 1;
            // 
            // botGo
            // 
            this.botGo.Location = new System.Drawing.Point(12, 467);
            this.botGo.Name = "botGo";
            this.botGo.Size = new System.Drawing.Size(156, 23);
            this.botGo.TabIndex = 2;
            this.botGo.Text = "Process selected";
            this.botGo.UseVisualStyleBackColor = true;
            this.botGo.Click += new System.EventHandler(this.botGo_Click);
            // 
            // botAuto
            // 
            this.botAuto.Location = new System.Drawing.Point(171, 467);
            this.botAuto.Name = "botAuto";
            this.botAuto.Size = new System.Drawing.Size(156, 23);
            this.botAuto.TabIndex = 3;
            this.botAuto.Text = "TEST";
            this.botAuto.UseVisualStyleBackColor = true;
            this.botAuto.Click += new System.EventHandler(this.botAuto_Click);
            // 
            // timerAutoProcess
            // 
            this.timerAutoProcess.Interval = 1000;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // textSelectedFolder
            // 
            this.textSelectedFolder.Location = new System.Drawing.Point(12, 12);
            this.textSelectedFolder.Name = "textSelectedFolder";
            this.textSelectedFolder.Size = new System.Drawing.Size(286, 20);
            this.textSelectedFolder.TabIndex = 5;
            // 
            // textLog
            // 
            this.textLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLog.Location = new System.Drawing.Point(346, 41);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(524, 478);
            this.textLog.TabIndex = 6;
            // 
            // botTestReadFromBlob
            // 
            this.botTestReadFromBlob.Location = new System.Drawing.Point(12, 496);
            this.botTestReadFromBlob.Name = "botTestReadFromBlob";
            this.botTestReadFromBlob.Size = new System.Drawing.Size(156, 23);
            this.botTestReadFromBlob.TabIndex = 7;
            this.botTestReadFromBlob.Text = "Test read from blob";
            this.botTestReadFromBlob.UseVisualStyleBackColor = true;
            this.botTestReadFromBlob.Click += new System.EventHandler(this.botTestReadFromBlob_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(346, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(524, 20);
            this.progressBar.TabIndex = 8;
            // 
            // botTestJSON2
            // 
            this.botTestJSON2.AllowDrop = true;
            this.botTestJSON2.Location = new System.Drawing.Point(171, 495);
            this.botTestJSON2.Name = "botTestJSON2";
            this.botTestJSON2.Size = new System.Drawing.Size(156, 23);
            this.botTestJSON2.TabIndex = 9;
            this.botTestJSON2.Text = "JSON to SB";
            this.botTestJSON2.UseVisualStyleBackColor = true;
            this.botTestJSON2.Click += new System.EventHandler(this.botTestJSON2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 556);
            this.Controls.Add(this.botTestJSON2);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.botTestReadFromBlob);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.textSelectedFolder);
            this.Controls.Add(this.botAuto);
            this.Controls.Add(this.botGo);
            this.Controls.Add(this.listFiles);
            this.Controls.Add(this.botSelectFolders);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog fbd;
        private System.Windows.Forms.Button botSelectFolders;
        private System.Windows.Forms.ListBox listFiles;
        private System.Windows.Forms.Button botGo;
        private System.Windows.Forms.Button botAuto;
        private System.Windows.Forms.Timer timerAutoProcess;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textSelectedFolder;
        private System.Windows.Forms.TextBox textLog;
        private System.Windows.Forms.Button botTestReadFromBlob;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button botTestJSON2;
    }
}

