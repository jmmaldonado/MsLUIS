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
            this.botTestJSON2 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.treeView = new System.Windows.Forms.TreeView();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // botSelectFolders
            // 
            this.botSelectFolders.Location = new System.Drawing.Point(463, 523);
            this.botSelectFolders.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.botSelectFolders.Name = "botSelectFolders";
            this.botSelectFolders.Size = new System.Drawing.Size(27, 30);
            this.botSelectFolders.TabIndex = 0;
            this.botSelectFolders.Text = "S";
            this.botSelectFolders.UseVisualStyleBackColor = true;
            this.botSelectFolders.Click += new System.EventHandler(this.botSelectFolders_Click);
            // 
            // listFiles
            // 
            this.listFiles.FormattingEnabled = true;
            this.listFiles.HorizontalScrollbar = true;
            this.listFiles.ItemHeight = 20;
            this.listFiles.Location = new System.Drawing.Point(18, 563);
            this.listFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listFiles.Name = "listFiles";
            this.listFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listFiles.Size = new System.Drawing.Size(470, 144);
            this.listFiles.TabIndex = 1;
            // 
            // botGo
            // 
            this.botGo.Location = new System.Drawing.Point(18, 718);
            this.botGo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.botGo.Name = "botGo";
            this.botGo.Size = new System.Drawing.Size(234, 35);
            this.botGo.TabIndex = 2;
            this.botGo.Text = "Process selected text files";
            this.botGo.UseVisualStyleBackColor = true;
            this.botGo.Click += new System.EventHandler(this.botGo_Click);
            // 
            // botAuto
            // 
            this.botAuto.Location = new System.Drawing.Point(256, 718);
            this.botAuto.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.botAuto.Name = "botAuto";
            this.botAuto.Size = new System.Drawing.Size(234, 35);
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
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(74, 4);
            // 
            // textSelectedFolder
            // 
            this.textSelectedFolder.Location = new System.Drawing.Point(18, 523);
            this.textSelectedFolder.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textSelectedFolder.Name = "textSelectedFolder";
            this.textSelectedFolder.Size = new System.Drawing.Size(437, 26);
            this.textSelectedFolder.TabIndex = 5;
            // 
            // textLog
            // 
            this.textLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textLog.Location = new System.Drawing.Point(519, 18);
            this.textLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textLog.Multiline = true;
            this.textLog.Name = "textLog";
            this.textLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textLog.Size = new System.Drawing.Size(784, 778);
            this.textLog.TabIndex = 6;
            // 
            // botTestReadFromBlob
            // 
            this.botTestReadFromBlob.Location = new System.Drawing.Point(18, 763);
            this.botTestReadFromBlob.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.botTestReadFromBlob.Name = "botTestReadFromBlob";
            this.botTestReadFromBlob.Size = new System.Drawing.Size(234, 35);
            this.botTestReadFromBlob.TabIndex = 7;
            this.botTestReadFromBlob.Text = "Process all indexed files";
            this.botTestReadFromBlob.UseVisualStyleBackColor = true;
            this.botTestReadFromBlob.Click += new System.EventHandler(this.botTestReadFromBlob_Click);
            // 
            // botTestJSON2
            // 
            this.botTestJSON2.AllowDrop = true;
            this.botTestJSON2.Location = new System.Drawing.Point(256, 762);
            this.botTestJSON2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.botTestJSON2.Name = "botTestJSON2";
            this.botTestJSON2.Size = new System.Drawing.Size(234, 35);
            this.botTestJSON2.TabIndex = 9;
            this.botTestJSON2.Text = "JSON to SB";
            this.botTestJSON2.UseVisualStyleBackColor = true;
            this.botTestJSON2.Click += new System.EventHandler(this.botTestJSON2_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 824);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1323, 31);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(998, 26);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "statusLabel";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusProgress
            // 
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Size = new System.Drawing.Size(300, 25);
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(18, 18);
            this.treeView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(470, 493);
            this.treeView.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1323, 855);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.botTestJSON2);
            this.Controls.Add(this.botTestReadFromBlob);
            this.Controls.Add(this.textLog);
            this.Controls.Add(this.textSelectedFolder);
            this.Controls.Add(this.botAuto);
            this.Controls.Add(this.botGo);
            this.Controls.Add(this.listFiles);
            this.Controls.Add(this.botSelectFolders);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.Button botTestJSON2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;
        private System.Windows.Forms.TreeView treeView;
    }
}

