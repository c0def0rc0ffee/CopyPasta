namespace FileCopy
{
    partial class MainWindow
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
            grpSource = new GroupBox();
            btnSourceBrowse = new Button();
            txtSource = new TextBox();
            grpDestination = new GroupBox();
            btnDestinationBrowse = new Button();
            txtDestination = new TextBox();
            btnCopy = new Button();
            TransferProgress = new ProgressBar();
            lblSpeed = new Label();
            lblFileNameSize = new Label();
            grpOptions = new GroupBox();
            chkCheckCompareHash = new CheckBox();
            chkVerifyFileSize = new CheckBox();
            cmbPauseDuration = new ComboBox();
            label3 = new Label();
            cmbBufferSize = new ComboBox();
            label2 = new Label();
            btnJSONFileBrowse = new Button();
            txtJSONFileLocation = new TextBox();
            chkOutputToJSON = new CheckBox();
            btnCSVFileBrowse = new Button();
            cmbExistingFile = new ComboBox();
            label1 = new Label();
            txtCSVFileLocation = new TextBox();
            chkOutputToCSV = new CheckBox();
            btnClose = new Button();
            btnPause = new Button();
            btnStop = new Button();
            lstLog = new ListView();
            lblBufferSize = new Label();
            lblFileCount = new Label();
            lblFileErrors = new Label();
            lblSuccTransfers = new Label();
            lblSkippedFiles = new Label();
            OverallProgress = new ProgressBar();
            lblTransferredSize = new Label();
            grpSource.SuspendLayout();
            grpDestination.SuspendLayout();
            grpOptions.SuspendLayout();
            SuspendLayout();
            // 
            // grpSource
            // 
            grpSource.Controls.Add(btnSourceBrowse);
            grpSource.Controls.Add(txtSource);
            grpSource.Location = new Point(12, 12);
            grpSource.Name = "grpSource";
            grpSource.Size = new Size(887, 61);
            grpSource.TabIndex = 0;
            grpSource.TabStop = false;
            grpSource.Text = "From";
            // 
            // btnSourceBrowse
            // 
            btnSourceBrowse.Location = new Point(848, 22);
            btnSourceBrowse.Name = "btnSourceBrowse";
            btnSourceBrowse.Size = new Size(33, 23);
            btnSourceBrowse.TabIndex = 1;
            btnSourceBrowse.Text = "...";
            btnSourceBrowse.UseVisualStyleBackColor = true;
            btnSourceBrowse.Click += btnSourceBrowse_Click_1;
            // 
            // txtSource
            // 
            txtSource.Location = new Point(6, 22);
            txtSource.Name = "txtSource";
            txtSource.Size = new Size(836, 23);
            txtSource.TabIndex = 0;
            // 
            // grpDestination
            // 
            grpDestination.Controls.Add(btnDestinationBrowse);
            grpDestination.Controls.Add(txtDestination);
            grpDestination.Location = new Point(12, 79);
            grpDestination.Name = "grpDestination";
            grpDestination.Size = new Size(887, 60);
            grpDestination.TabIndex = 1;
            grpDestination.TabStop = false;
            grpDestination.Text = "To";
            // 
            // btnDestinationBrowse
            // 
            btnDestinationBrowse.Location = new Point(848, 22);
            btnDestinationBrowse.Name = "btnDestinationBrowse";
            btnDestinationBrowse.Size = new Size(33, 23);
            btnDestinationBrowse.TabIndex = 1;
            btnDestinationBrowse.Text = "...";
            btnDestinationBrowse.UseVisualStyleBackColor = true;
            btnDestinationBrowse.Click += btnDestinationBrowse_Click;
            // 
            // txtDestination
            // 
            txtDestination.Location = new Point(6, 22);
            txtDestination.Name = "txtDestination";
            txtDestination.Size = new Size(836, 23);
            txtDestination.TabIndex = 0;
            // 
            // btnCopy
            // 
            btnCopy.Location = new Point(778, 145);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(121, 137);
            btnCopy.TabIndex = 2;
            btnCopy.Text = "Copy";
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // TransferProgress
            // 
            TransferProgress.Location = new Point(12, 470);
            TransferProgress.Name = "TransferProgress";
            TransferProgress.Size = new Size(887, 23);
            TransferProgress.TabIndex = 3;
            // 
            // lblSpeed
            // 
            lblSpeed.Location = new Point(12, 533);
            lblSpeed.Name = "lblSpeed";
            lblSpeed.Size = new Size(150, 15);
            lblSpeed.TabIndex = 4;
            lblSpeed.Text = "Speed: 999.99 MBS";
            lblSpeed.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFileNameSize
            // 
            lblFileNameSize.AutoSize = true;
            lblFileNameSize.Location = new Point(18, 474);
            lblFileNameSize.Name = "lblFileNameSize";
            lblFileNameSize.Size = new Size(90, 15);
            lblFileNameSize.TabIndex = 6;
            lblFileNameSize.Text = "lblFileNameSize";
            // 
            // grpOptions
            // 
            grpOptions.Controls.Add(chkCheckCompareHash);
            grpOptions.Controls.Add(chkVerifyFileSize);
            grpOptions.Controls.Add(cmbPauseDuration);
            grpOptions.Controls.Add(label3);
            grpOptions.Controls.Add(cmbBufferSize);
            grpOptions.Controls.Add(label2);
            grpOptions.Controls.Add(btnJSONFileBrowse);
            grpOptions.Controls.Add(txtJSONFileLocation);
            grpOptions.Controls.Add(chkOutputToJSON);
            grpOptions.Controls.Add(btnCSVFileBrowse);
            grpOptions.Controls.Add(cmbExistingFile);
            grpOptions.Controls.Add(label1);
            grpOptions.Controls.Add(txtCSVFileLocation);
            grpOptions.Controls.Add(chkOutputToCSV);
            grpOptions.Location = new Point(12, 145);
            grpOptions.Name = "grpOptions";
            grpOptions.Size = new Size(760, 137);
            grpOptions.TabIndex = 7;
            grpOptions.TabStop = false;
            grpOptions.Text = "Options";
            // 
            // chkCheckCompareHash
            // 
            chkCheckCompareHash.AutoSize = true;
            chkCheckCompareHash.Location = new Point(409, 109);
            chkCheckCompareHash.Name = "chkCheckCompareHash";
            chkCheckCompareHash.Size = new Size(141, 19);
            chkCheckCompareHash.TabIndex = 14;
            chkCheckCompareHash.Text = "Check Compare Hash";
            chkCheckCompareHash.UseVisualStyleBackColor = true;
            // 
            // chkVerifyFileSize
            // 
            chkVerifyFileSize.AutoSize = true;
            chkVerifyFileSize.Location = new Point(304, 109);
            chkVerifyFileSize.Name = "chkVerifyFileSize";
            chkVerifyFileSize.Size = new Size(99, 19);
            chkVerifyFileSize.TabIndex = 13;
            chkVerifyFileSize.Text = "Verify File Size";
            chkVerifyFileSize.UseVisualStyleBackColor = true;
            // 
            // cmbPauseDuration
            // 
            cmbPauseDuration.FormattingEnabled = true;
            cmbPauseDuration.Location = new Point(211, 107);
            cmbPauseDuration.Name = "cmbPauseDuration";
            cmbPauseDuration.Size = new Size(69, 23);
            cmbPauseDuration.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(149, 110);
            label3.Name = "label3";
            label3.Size = new Size(65, 15);
            label3.TabIndex = 11;
            label3.Text = "Pause (ms)";
            // 
            // cmbBufferSize
            // 
            cmbBufferSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBufferSize.FormattingEnabled = true;
            cmbBufferSize.Location = new Point(70, 107);
            cmbBufferSize.Name = "cmbBufferSize";
            cmbBufferSize.Size = new Size(62, 23);
            cmbBufferSize.TabIndex = 9;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 110);
            label2.Name = "label2";
            label2.Size = new Size(62, 15);
            label2.TabIndex = 8;
            label2.Text = "Buffer Size";
            // 
            // btnJSONFileBrowse
            // 
            btnJSONFileBrowse.Location = new Point(721, 49);
            btnJSONFileBrowse.Name = "btnJSONFileBrowse";
            btnJSONFileBrowse.Size = new Size(33, 23);
            btnJSONFileBrowse.TabIndex = 7;
            btnJSONFileBrowse.Text = "...";
            btnJSONFileBrowse.UseVisualStyleBackColor = true;
            btnJSONFileBrowse.Click += btnJSONFileBrowse_Click;
            // 
            // txtJSONFileLocation
            // 
            txtJSONFileLocation.Location = new Point(184, 47);
            txtJSONFileLocation.Name = "txtJSONFileLocation";
            txtJSONFileLocation.Size = new Size(531, 23);
            txtJSONFileLocation.TabIndex = 6;
            // 
            // chkOutputToJSON
            // 
            chkOutputToJSON.AutoSize = true;
            chkOutputToJSON.Location = new Point(6, 49);
            chkOutputToJSON.Name = "chkOutputToJSON";
            chkOutputToJSON.Size = new Size(177, 19);
            chkOutputToJSON.TabIndex = 5;
            chkOutputToJSON.Text = "Output the log to a JSON file";
            chkOutputToJSON.UseVisualStyleBackColor = true;
            chkOutputToJSON.CheckedChanged += chkOutputToJSON_CheckedChanged;
            // 
            // btnCSVFileBrowse
            // 
            btnCSVFileBrowse.Location = new Point(721, 22);
            btnCSVFileBrowse.Name = "btnCSVFileBrowse";
            btnCSVFileBrowse.Size = new Size(33, 23);
            btnCSVFileBrowse.TabIndex = 4;
            btnCSVFileBrowse.Text = "...";
            btnCSVFileBrowse.UseVisualStyleBackColor = true;
            btnCSVFileBrowse.Click += btnCSVFileBrowse_Click;
            // 
            // cmbExistingFile
            // 
            cmbExistingFile.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbExistingFile.FormattingEnabled = true;
            cmbExistingFile.Items.AddRange(new object[] { "Overwrite the destination with the source file", "Skip the file altogether", "Ask per file" });
            cmbExistingFile.Location = new Point(246, 76);
            cmbExistingFile.Name = "cmbExistingFile";
            cmbExistingFile.Size = new Size(469, 23);
            cmbExistingFile.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 79);
            label1.Name = "label1";
            label1.Size = new Size(234, 15);
            label1.TabIndex = 2;
            label1.Text = "If an existing file is found in the destination";
            // 
            // txtCSVFileLocation
            // 
            txtCSVFileLocation.Location = new Point(184, 20);
            txtCSVFileLocation.Name = "txtCSVFileLocation";
            txtCSVFileLocation.Size = new Size(531, 23);
            txtCSVFileLocation.TabIndex = 1;
            // 
            // chkOutputToCSV
            // 
            chkOutputToCSV.AutoSize = true;
            chkOutputToCSV.Location = new Point(6, 22);
            chkOutputToCSV.Name = "chkOutputToCSV";
            chkOutputToCSV.Size = new Size(172, 19);
            chkOutputToCSV.TabIndex = 0;
            chkOutputToCSV.Text = "Output the log to a CSV File";
            chkOutputToCSV.UseVisualStyleBackColor = true;
            chkOutputToCSV.CheckedChanged += chkOutputToCSV_CheckedChanged_1;
            // 
            // btnClose
            // 
            btnClose.Location = new Point(763, 575);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(136, 39);
            btnClose.TabIndex = 8;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(477, 533);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(136, 39);
            btnPause.TabIndex = 9;
            btnPause.Text = "Pause";
            btnPause.UseVisualStyleBackColor = true;
            btnPause.Click += btnPause_Click;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(763, 533);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(136, 39);
            btnStop.TabIndex = 11;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // lstLog
            // 
            lstLog.Location = new Point(12, 288);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(887, 176);
            lstLog.TabIndex = 12;
            lstLog.UseCompatibleStateImageBehavior = false;
            // 
            // lblBufferSize
            // 
            lblBufferSize.AutoSize = true;
            lblBufferSize.Location = new Point(12, 557);
            lblBufferSize.Name = "lblBufferSize";
            lblBufferSize.Size = new Size(72, 15);
            lblBufferSize.TabIndex = 13;
            lblBufferSize.Text = "lblBufferSize";
            // 
            // lblFileCount
            // 
            lblFileCount.AutoSize = true;
            lblFileCount.Location = new Point(18, 503);
            lblFileCount.Name = "lblFileCount";
            lblFileCount.Size = new Size(71, 15);
            lblFileCount.TabIndex = 14;
            lblFileCount.Text = "lblFileCount";
            // 
            // lblFileErrors
            // 
            lblFileErrors.AutoSize = true;
            lblFileErrors.Location = new Point(223, 582);
            lblFileErrors.Name = "lblFileErrors";
            lblFileErrors.Size = new Size(68, 15);
            lblFileErrors.TabIndex = 15;
            lblFileErrors.Text = "lblFileErrors";
            // 
            // lblSuccTransfers
            // 
            lblSuccTransfers.AutoSize = true;
            lblSuccTransfers.Location = new Point(223, 533);
            lblSuccTransfers.Name = "lblSuccTransfers";
            lblSuccTransfers.Size = new Size(91, 15);
            lblSuccTransfers.TabIndex = 16;
            lblSuccTransfers.Text = "lblSuccTransfers";
            // 
            // lblSkippedFiles
            // 
            lblSkippedFiles.AutoSize = true;
            lblSkippedFiles.Location = new Point(223, 557);
            lblSkippedFiles.Name = "lblSkippedFiles";
            lblSkippedFiles.Size = new Size(85, 15);
            lblSkippedFiles.TabIndex = 17;
            lblSkippedFiles.Text = "lblSkippedFiles";
            // 
            // OverallProgress
            // 
            OverallProgress.Location = new Point(12, 499);
            OverallProgress.Name = "OverallProgress";
            OverallProgress.Size = new Size(887, 23);
            OverallProgress.TabIndex = 18;
            // 
            // lblTransferredSize
            // 
            lblTransferredSize.AutoSize = true;
            lblTransferredSize.Location = new Point(12, 582);
            lblTransferredSize.Name = "lblTransferredSize";
            lblTransferredSize.Size = new Size(98, 15);
            lblTransferredSize.TabIndex = 20;
            lblTransferredSize.Text = "lblTransferredSize";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(911, 624);
            Controls.Add(lblTransferredSize);
            Controls.Add(lblFileCount);
            Controls.Add(OverallProgress);
            Controls.Add(lblSkippedFiles);
            Controls.Add(lblSuccTransfers);
            Controls.Add(lblFileErrors);
            Controls.Add(lblBufferSize);
            Controls.Add(lstLog);
            Controls.Add(btnStop);
            Controls.Add(btnPause);
            Controls.Add(btnClose);
            Controls.Add(grpOptions);
            Controls.Add(lblFileNameSize);
            Controls.Add(lblSpeed);
            Controls.Add(TransferProgress);
            Controls.Add(btnCopy);
            Controls.Add(grpDestination);
            Controls.Add(grpSource);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainWindow";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Copy Pasta";
            Load += MainWindow_Load;
            grpSource.ResumeLayout(false);
            grpSource.PerformLayout();
            grpDestination.ResumeLayout(false);
            grpDestination.PerformLayout();
            grpOptions.ResumeLayout(false);
            grpOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox grpSource;
        private GroupBox grpDestination;
        private Button btnCopy;
        private ProgressBar TransferProgress;
        private Label lblSpeed;
        private Button btnSourceBrowse;
        private TextBox txtSource;
        private Button btnDestinationBrowse;
        private TextBox txtDestination;
        private Label lblFileNameSize;
        private GroupBox grpOptions;
        private ComboBox cmbExistingFile;
        private Label label1;
        private TextBox txtCSVFileLocation;
        private CheckBox chkOutputToCSV;
        private Button btnCSVFileBrowse;
        private Button btnClose;
        private Button btnJSONFileBrowse;
        private TextBox txtJSONFileLocation;
        private CheckBox chkOutputToJSON;
        private Button btnPause;
        private Button btnStop;
        private ListView lstLog;
        private Label label2;
        private ComboBox cmbBufferSize;
        private Label lblBufferSize;
        private Label lblFileCount;
        private Label label3;
        private ComboBox cmbPauseDuration;
        private Label lblFileErrors;
        private Label lblSuccTransfers;
        private Label lblSkippedFiles;
        private ProgressBar OverallProgress;
        private Label lblTransferredSize;
        private CheckBox chkVerifyFileSize;
        private CheckBox chkCheckCompareHash;
    }
}
