using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace FileCopy
{
    public partial class MainWindow : Form
    {
        private ManualResetEvent pauseEvent = new ManualResetEvent(true);
        private bool isPaused = false;
        private CancellationTokenSource cancellationTokenSource;
        private int copiedFiles = 0;
        private int errorFileCount = 0;
        private int skippedFiles = 0; // Track skipped files
        private int processedFiles = 0;
        private long totalTransferredSize = 0; // Tracks the total size of all transferred files

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            // UI: Configure the ListView to have detailed view with columns
            lstLog.View = View.Details;
            lstLog.Columns.Add("Date & Time", 130, HorizontalAlignment.Left); // DateTime column at the start
            lstLog.Columns.Add("File Name", 280, HorizontalAlignment.Left);
            lstLog.Columns.Add("Size", 70, HorizontalAlignment.Right);
            lstLog.Columns.Add("Status", 200, HorizontalAlignment.Left);
            lstLog.Columns.Add("Location", 195, HorizontalAlignment.Left); // Location column at the end

            txtCSVFileLocation.Enabled = false;
            btnCSVFileBrowse.Enabled = false;
            txtJSONFileLocation.Enabled = false;
            btnJSONFileBrowse.Enabled = false;
            cmbExistingFile.SelectedIndex = 0;

            lblFileNameSize.Text = "";
            lblFileCount.Text = "";
            btnPause.Enabled = false;
            btnStop.Enabled = false;


            // Get the current assembly
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            // Format the version string to exclude trailing zeros (build and revision numbers)
            string formattedVersion = version.Revision == 0
                ? (version.Build == 0
                    ? $"{version.Major}.{version.Minor}"
                    : $"{version.Major}.{version.Minor}.{version.Build}")
                : version.ToString();

            // Set the form's title with the version number
            this.Text = "Copy Pasta" + $" v{formattedVersion}";



            cmbBufferSize.Items.Add("Auto");
            cmbBufferSize.Items.Add("4 KB");
            cmbBufferSize.Items.Add("8 KB");
            cmbBufferSize.Items.Add("16 KB");
            cmbBufferSize.Items.Add("32 KB");
            cmbBufferSize.Items.Add("64 KB");
            cmbBufferSize.Items.Add("128 KB");
            cmbBufferSize.Items.Add("256 KB");
            cmbBufferSize.Items.Add("512 KB");
            cmbBufferSize.Items.Add("1 MB");
            cmbBufferSize.Items.Add("4 MB");
            cmbBufferSize.Items.Add("8 MB");
            cmbBufferSize.Items.Add("10 MB");
            cmbBufferSize.SelectedIndex = 0;
            lblFileErrors.Text = "Errored Files: 0";
            cmbPauseDuration.Items.AddRange(new object[]
            {
                "None",
                "100 ms",
                "500 ms",
                "1000 ms",
                "2000 ms",
                "5000 ms"
            });
            cmbPauseDuration.SelectedIndex = 0; // Default to 'None'

            // Initialize the labels
            // UI: Update file count and other UI labels
            UpdateBufferSizeLabel(0);
            UpdateSpeedLabel(0, 0);

            successfulTransfers = 0;
            UpdateSuccTransfersLabel(successfulTransfers);

            skippedFiles = 0;
            UpdateSkippedFilesLabel(skippedFiles);
            UpdateTransferredSizeLabel(0);
            UpdateSpeedLabel("0");
        }

        private async void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                string sourcePath = txtSource.Text;
                string destinationPath = txtDestination.Text;

                // UI: Check if source and destination are the same
                if (string.Equals(sourcePath, destinationPath, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("You cannot have the source location the same as the destination.", "Invalid Paths", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method early to prevent copying
                }

                // UI: Clear log list and reset counters
                lstLog.Items.Clear();
                processedFiles = 0;
                successfulTransfers = 0;
                UpdateSuccTransfersLabel(successfulTransfers);

                skippedFiles = 0;
                UpdateSkippedFilesLabel(skippedFiles);

                // Initialize the total transferred size counter
                totalTransferredSize = 0;
                UpdateTransferredSizeLabel(totalTransferredSize);

                if (Directory.Exists(sourcePath) && Directory.Exists(destinationPath))
                {
                    // UI: Add an entry indicating the analysis of source files
                    string analysingMessage = "Analysing Source Files";
                    string dateTimeNow = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
                    AddLogEntry(dateTimeNow, analysingMessage, "", "", "", isError: false);

                    // BAL: Analyze source files and get the file count
                    int fileCount = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories).Length;

                    // UI: Remove the "Analyzing Source Files" message after analysis
                    lstLog.Items.RemoveAt(0);

                    // UI: Initialize the overall progress bar
                    OverallProgress.Maximum = fileCount;
                    OverallProgress.Value = 0;

                    lblSpeed.Visible = true;
                    lblBufferSize.Visible = true; // Show the buffer size label when copying starts
                    btnPause.Enabled = true;
                    btnStop.Enabled = true;
                    btnCopy.Enabled = false;

                    // UI: Disable the group boxes during the copy job
                    grpSource.Enabled = false;
                    grpDestination.Enabled = false;
                    grpOptions.Enabled = false;

                    cancellationTokenSource = new CancellationTokenSource();
                    var token = cancellationTokenSource.Token;

                    try
                    {
                        // BAL: Start copying files asynchronously
                        await CopyFilesAsync(sourcePath, destinationPath, fileCount, token);
                    }
                    catch (OperationCanceledException)
                    {
                        // UI: Notify the user that copying was canceled
                        MessageBox.Show("Copying was canceled.", "Operation Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        // BAL: Handle any errors that occur during the copying process
                        HandleError(ex, "in btnCopy_Click.");
                    }
                    finally
                    {
                        // UI: Ensure the overall progress bar is fully filled at the end
                        OverallProgress.Invoke((MethodInvoker)(() =>
                        {
                            OverallProgress.Value = OverallProgress.Maximum;
                        }));

                        // UI: Re-enable controls after the copying job is complete
                        lblFileCount.Text = "";
                        btnCopy.Enabled = true;
                        btnPause.Enabled = false;
                        btnStop.Enabled = false;
                        TransferProgress.Value = 0;
                        UpdateSpeedLabel(0, 1);
                        UpdateFileNameSizeLabel("", 0, 0);
                        errorFileCount = 0;
                        UpdateErrorFileLabel();

                        UpdateSkippedFilesLabel(0);
                        grpSource.Enabled = true;
                        grpDestination.Enabled = true;
                        grpOptions.Enabled = true;

                        btnClose.Enabled = true;

                        // UI: Reset the overall progress bar to 0 after completion
                        OverallProgress.Invoke((MethodInvoker)(() =>
                        {
                            OverallProgress.Value = 0;
                        }));

                        // UI: Show the "Processing Complete" message box
                        MessageBox.Show("File processing is complete.", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // UI: Notify the user of invalid paths
                    MessageBox.Show("Please ensure both source and destination paths are valid.", "Invalid Paths", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnClose.Enabled = true;
                    grpSource.Enabled = true;
                    grpDestination.Enabled = true;
                    grpOptions.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                // BAL: Handle any errors that occur during the click event
                HandleError(ex, "in btnCopy_Click.");
            }
        }


        // BAL: Method to copy files asynchronously
        private async Task CopyFilesAsync(string sourcePath, string destinationPath, int totalFiles, CancellationToken token)
        {
            StreamWriter csvWriter = null;
            StreamWriter jsonWriter = null;

            try
            {
                if (chkOutputToCSV.Checked)
                {
                    // DAL: Generate CSV log file
                    string csvFilePath = GenerateCSVFileName(txtCSVFileLocation.Text);
                    csvWriter = new StreamWriter(csvFilePath);
                    await csvWriter.WriteLineAsync("DateTime,FileName,Size,Status,Source File,Destination");
                }

                if (chkOutputToJSON.Checked)
                {
                    // DAL: Generate JSON log file
                    string jsonFilePath = GenerateJSONFileName(txtJSONFileLocation.Text);
                    jsonWriter = new StreamWriter(jsonFilePath);
                }

                // BAL: Start the recursive copy process
                await RecursiveCopyAsync(sourcePath, destinationPath, token, csvWriter, jsonWriter, totalFiles);
            }
            catch (OperationCanceledException)
            {
                // UI: Notify the user that copying was stopped
                MessageBox.Show("Copying was stopped by the user.", "Operation Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // BAL: Handle any errors that occur during the file copying
                HandleError(ex, "during file copying in CopyFilesAsync.");
            }
            finally
            {
                csvWriter?.Close();
                jsonWriter?.Close();
            }
        }

        // BAL: Method to recursively copy files
        private async Task RecursiveCopyAsync(string sourceDir, string destDir, CancellationToken token, StreamWriter csvWriter, StreamWriter jsonWriter, int totalFiles)
        {
            try
            {
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                }

                foreach (string file in Directory.GetFiles(sourceDir))
                {
                    if (token.IsCancellationRequested)
                    {
                        return; // BAL: Gracefully handle cancellation
                    }

                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(destDir, fileName);
                    long fileSize = new FileInfo(file).Length;
                    string fileSizeFormatted = FormatSize(fileSize);
                    string dateTimeNow = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");

                    string selectedOption = null;
                    cmbExistingFile.Invoke((MethodInvoker)(() =>
                    {
                        selectedOption = cmbExistingFile.SelectedItem.ToString();
                    }));

                    bool wasOverwritten = false;
                    string status = "Copy Complete";

                    if (File.Exists(destFile))
                    {
                        if (selectedOption == "Overwrite the destination with the source file")
                        {
                            wasOverwritten = true;
                            status += " (Overwritten)";
                        }
                        else if (selectedOption == "Skip the file altogether")
                        {
                            status = "Skipped";
                            skippedFiles++; // Increment skipped files counter
                            UpdateSkippedFilesLabel(skippedFiles);

                            // Check file hash for skipped files if enabled
                            if (chkCheckCompareHash.Checked)
                            {
                                VerifyFileHash(file, destFile);
                            }

                            processedFiles++; // Increment processed files counter
                            UpdateFileCountLabel(processedFiles, totalFiles);
                            OverallProgress.Invoke((MethodInvoker)(() => { OverallProgress.Value = processedFiles; }));
                            AddLogEntry(dateTimeNow, fileName, fileSizeFormatted, status, file);
                            await WriteLogAsync(csvWriter, jsonWriter, dateTimeNow, fileName, fileSizeFormatted, status, file, destFile);

                            continue;
                        }
                        else if (selectedOption == "Ask per file")
                        {
                            DialogResult result = DialogResult.None;
                            cmbExistingFile.Invoke((MethodInvoker)(() =>
                            {
                                result = MessageBox.Show($"File '{fileName}' already exists in the destination. Overwrite?", "File Exists", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            }));

                            if (result == DialogResult.Yes)
                            {
                                wasOverwritten = true;
                                status += " (Overwritten)";
                            }
                            else if (result == DialogResult.No)
                            {
                                status = "Skipped";
                                skippedFiles++;
                                UpdateSkippedFilesLabel(skippedFiles);

                                // Check file hash for skipped files if enabled
                                if (chkCheckCompareHash.Checked)
                                {
                                    VerifyFileHash(file, destFile);
                                }

                                processedFiles++;
                                UpdateFileCountLabel(processedFiles, totalFiles);
                                OverallProgress.Invoke((MethodInvoker)(() => { OverallProgress.Value = processedFiles; }));
                                AddLogEntry(dateTimeNow, fileName, fileSizeFormatted, status, file);
                                await WriteLogAsync(csvWriter, jsonWriter, dateTimeNow, fileName, fileSizeFormatted, status, file, destFile);

                                continue;
                            }
                            else if (result == DialogResult.Cancel)
                            {
                                token.ThrowIfCancellationRequested();
                            }
                        }
                    }
                    else
                    {
                        status = "Copy Complete (New File Added)";
                    }

                    totalTransferredSize += fileSize;
                    UpdateTransferredSizeLabel(totalTransferredSize);

                    try
                    {
                        // BAL: Copy the file with progress tracking
                        await CopyFileWithProgressAsync(file, destFile, token, csvWriter, jsonWriter);

                        processedFiles++;
                        UpdateFileCountLabel(processedFiles, totalFiles);
                        OverallProgress.Invoke((MethodInvoker)(() => { OverallProgress.Value = processedFiles; }));
                        AddLogEntry(dateTimeNow, fileName, fileSizeFormatted, status, destFile);
                        await WriteLogAsync(csvWriter, jsonWriter, dateTimeNow, fileName, fileSizeFormatted, status, file, destFile);

                        // Check file hash for copied or overwritten files if enabled
                        if (chkCheckCompareHash.Checked)
                        {
                            VerifyFileHash(file, destFile);
                        }
                    }
                    catch (IOException ex)
                    {
                        errorFileCount++;
                        UpdateErrorFileLabel();
                        HandleError(ex, $"while copying file {fileName} in RecursiveCopyAsync.");
                    }
                }

                foreach (string directory in Directory.GetDirectories(sourceDir))
                {
                    string destSubDir = Path.Combine(destDir, Path.GetFileName(directory));
                    await RecursiveCopyAsync(directory, destSubDir, token, csvWriter, jsonWriter, totalFiles);
                }
            }
            catch (OperationCanceledException)
            {
                // BAL: Gracefully exit on cancellation without throwing an error
            }
            catch (Exception ex)
            {
                errorFileCount++;
                UpdateErrorFileLabel();
                HandleError(ex, "during recursive copying in RecursiveCopyAsync.");
            }
        }


        private void VerifyFileHash(string sourceFile, string destFile)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // BAL: Compute hash for source file
                byte[] sourceHash;
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    sourceHash = sha256.ComputeHash(sourceStream);
                }

                // BAL: Compute hash for destination file
                byte[] destHash;
                using (FileStream destStream = new FileStream(destFile, FileMode.Open, FileAccess.Read))
                {
                    destHash = sha256.ComputeHash(destStream);
                }

                // BAL: Compare hashes
                if (!sourceHash.SequenceEqual(destHash))
                {
                    MessageBox.Show($"Hash mismatch detected between source and destination for file '{Path.GetFileName(destFile)}'.", "Hash Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void VerifyFileSize(string sourceFile, string destFile)
        {
            long sourceSize = new FileInfo(sourceFile).Length;
            long destSize = new FileInfo(destFile).Length;

            if (sourceSize != destSize)
            {
                DialogResult result = MessageBox.Show(
                    $"The file '{Path.GetFileName(destFile)}' in the destination has a different size from the source file.\n\n" +
                    $"Source Size: {FormatSize(sourceSize)}\n" +
                    $"Destination Size: {FormatSize(destSize)}\n\n" +
                    "What would you like to do?\n\n" +
                    "1. Keep the file as it is (Click Cancel)\n" +
                    "2. Delete the destination file (Click No)\n" +
                    "3. Overwrite the destination file again (Click Yes)",
                    "File Size Mismatch",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button3);

                if (result == DialogResult.Yes)
                {
                    // BAL: Overwrite the destination file again
                    File.Copy(sourceFile, destFile, true);
                    MessageBox.Show("File overwritten successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.No)
                {
                    // BAL: Delete the destination file
                    File.Delete(destFile);
                    MessageBox.Show("Destination file deleted.", "File Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // If the user clicks Cancel, do nothing (leave the file as is)
            }
        }


        // UI: Update skipped files label
        private void UpdateSkippedFilesLabel(int skippedFiles)
        {
            lblSkippedFiles.Invoke(new Action(() =>
            {
                lblSkippedFiles.Text = $"Skipped Files: {skippedFiles}";
                lblSkippedFiles.Refresh();
            }));
        }

        private int successfulTransfers = 0; // Track successful transfers

        // BAL: Copy a file with progress tracking
        private async Task CopyFileWithProgressAsync(string sourceFile, string destinationFile, CancellationToken token, StreamWriter csvWriter, StreamWriter jsonWriter)
        {
            long fileLength = new FileInfo(sourceFile).Length;

            int bufferSize = CalculateOptimalBufferSize(fileLength);
            byte[] buffer = new byte[bufferSize];

            lblBufferSize.Invoke(new Action(() =>
            {
                UpdateBufferSizeLabel(bufferSize);
                lblBufferSize.Refresh();
            }));

            long totalBytes = 0;

            try
            {
                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                using (FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
                {
                    TransferProgress.Invoke((MethodInvoker)(() =>
                    {
                        TransferProgress.Maximum = (int)(fileLength / bufferSize);
                        TransferProgress.Value = 0;
                    }));

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    DateTime lastUpdateTime = DateTime.Now;

                    int bytesRead;
                    while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, bufferSize, token)) > 0)
                    {
                        while (isPaused)
                        {
                            await Task.Delay(100);  // Delay for a short time before checking again
                        }

                        token.ThrowIfCancellationRequested(); // BAL: Check if cancellation was requested
                        await destinationStream.WriteAsync(buffer, 0, bytesRead, token);
                        totalBytes += bytesRead;

                        if (cmbPauseDuration.SelectedItem != null && cmbPauseDuration.SelectedItem.ToString() != "None")
                        {
                            if (int.TryParse(cmbPauseDuration.SelectedItem.ToString().Split(' ')[0], out int pauseDuration))
                            {
                                await Task.Delay(pauseDuration, token); // Delay by the specified milliseconds
                            }
                        }

                        TransferProgress.Invoke((MethodInvoker)(() =>
                        {
                            TransferProgress.Value = (int)(totalBytes / bufferSize);
                        }));

                        UpdateFileNameSizeLabel(Path.GetFileName(sourceFile), fileLength, totalBytes);

                        if ((DateTime.Now - lastUpdateTime).TotalSeconds >= 1)
                        {
                            double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                            if (elapsedSeconds > 0)
                            {
                                UpdateSpeedLabel(totalBytes, elapsedSeconds);
                                lastUpdateTime = DateTime.Now;
                            }
                        }
                    }

                    stopwatch.Stop();
                    UpdateSpeedLabel(totalBytes, stopwatch.Elapsed.TotalSeconds);
                }

                // BAL: Increment successful transfers only if the whole file copy is successful
                successfulTransfers++;
                UpdateSuccTransfersLabel(successfulTransfers);
            }
            catch (IOException ex)
            {
                // BAL: Handle any IO exceptions during file copy
                errorFileCount++;
                UpdateErrorFileLabel();
                HandleError(ex, $"while copying file {sourceFile} in CopyFileWithProgressAsync.");
            }
        }

        // UI: Update successful transfers label
        private void UpdateSuccTransfersLabel(int successfulTransfers)
        {
            lblSuccTransfers.Invoke(new Action(() =>
            {
                lblSuccTransfers.Text = $"Successful Transfers: {successfulTransfers}";
                lblSuccTransfers.Refresh();
            }));
        }

        // DAL: Write log entries to CSV and JSON files
        private async Task WriteLogAsync(StreamWriter csvWriter, StreamWriter jsonWriter, string dateTimeNow, string fileName, string fileSizeFormatted, string status, string sourceFilePath, string destFilePath)
        {
            if (csvWriter != null)
            {
                await csvWriter.WriteLineAsync($"{dateTimeNow},{fileName},{fileSizeFormatted},{status},{sourceFilePath},{destFilePath}");
                await csvWriter.FlushAsync();
            }

            if (jsonWriter != null)
            {
                var logEntry = new
                {
                    DateTime = dateTimeNow,
                    FileName = fileName,
                    Size = fileSizeFormatted,
                    Status = status,
                    Source = sourceFilePath,
                    Destination = destFilePath
                };
                await jsonWriter.WriteLineAsync(JsonConvert.SerializeObject(logEntry, Formatting.Indented));
                await jsonWriter.FlushAsync();
            }
        }

        // UI: Add a log entry to the ListView
        private void AddLogEntry(string dateTimeNow, string fileName, string size, string status, string location, bool isError = false)
        {
            lstLog.Invoke(new Action(() =>
            {
                var item = new ListViewItem(new[] { dateTimeNow, fileName, size, status, location });
                if (isError)
                {
                    item.ForeColor = Color.Red;  // Set the text color to red for errors
                }
                lstLog.Items.Insert(0, item);  // Insert at the top of the ListView
                lstLog.Refresh();
            }));
        }

        // BAL: Calculate the optimal buffer size for copying files
        private int CalculateOptimalBufferSize(long fileSize)
        {
            const int minBufferSize = 64 * 1024; // 64 KB
            const int maxBufferSize = 10 * 1024 * 1024; // 10 MB

            long bufferSize = fileSize / 100;

            if (bufferSize < minBufferSize)
                return minBufferSize;
            if (bufferSize > maxBufferSize)
                return maxBufferSize;

            return (int)bufferSize; // Cast safely within the int range
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                if (isPaused)
                {
                    isPaused = false;
                    btnPause.Text = "Pause";
                }
                else
                {
                    isPaused = true;
                    btnPause.Text = "Resume";
                }
            }
            catch (Exception ex)
            {
                // BAL: Handle errors during pause/resume operation
                HandleError(ex, "during pause/resume operation.");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                var result = MessageBox.Show("Are you sure you want to stop the copying process?", "Confirm Stop", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    cancellationTokenSource?.Cancel();
                }
            }
            catch (Exception ex)
            {
                // BAL: Handle errors while attempting to stop the copying process
                HandleError(ex, "while attempting to stop the copying process.");
            }
        }

        // UI: Update speed label
        private void UpdateSpeedLabel(string speed)
        {
            lblSpeed.Invoke(new Action(() =>
            {
                lblSpeed.Text = $"Speed: {speed}";
                lblSpeed.Refresh();
            }));
        }

        // UI: Update speed label based on bytes transferred and elapsed time
        private void UpdateSpeedLabel(long totalBytes, double elapsedSeconds)
        {
            double speedInBytesPerSecond = totalBytes / elapsedSeconds;
            string speedFormatted;

            if (speedInBytesPerSecond >= 1024L * 1024L * 1024L * 1024L) // TB/s
            {
                speedFormatted = $"{speedInBytesPerSecond / (1024L * 1024L * 1024L * 1024L):0.##} TB/s";
            }
            else if (speedInBytesPerSecond >= 1024L * 1024L * 1024L) // GB/s
            {
                speedFormatted = $"{speedInBytesPerSecond / (1024L * 1024L * 1024L):0.##} GB/s";
            }
            else if (speedInBytesPerSecond >= 1024L * 1024L) // MB/s
            {
                speedFormatted = $"{speedInBytesPerSecond / (1024L * 1024L):0.##} MB/s";
            }
            else if (speedInBytesPerSecond >= 1024L) // KB/s
            {
                speedFormatted = $"{speedInBytesPerSecond / 1024L:0.##} KB/s";
            }
            else // B/s
            {
                speedFormatted = $"{speedInBytesPerSecond:0.##} B/s";
            }

            UpdateSpeedLabel(speedFormatted);
        }

        // UI: Update the file name and size label
        private void UpdateFileNameSizeLabel(string fileName, long totalSize, long copiedSize)
        {
            string totalSizeFormatted = FormatSize(totalSize);
            string copiedSizeFormatted = FormatSize(copiedSize);

            string displayText = string.IsNullOrEmpty(fileName)
                ? ""
                : $"{fileName} {copiedSizeFormatted}/{totalSizeFormatted}";

            lblFileNameSize.Invoke(new Action(() =>
            {
                lblFileNameSize.Text = displayText;
                lblFileNameSize.Refresh();
            }));
        }

        // BAL: Format file size into a human-readable format
        private string FormatSize(long size)
        {
            double formattedSize = size;
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;

            while (formattedSize >= 1024 && order < sizes.Length - 1)
            {
                order++;
                formattedSize = formattedSize / 1024;
            }

            return $"{formattedSize:0.##} {sizes[order]}";
        }

        private void btnSourceBrowse_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        txtSource.Text = dialog.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                // BAL: Handle errors while browsing for the source folder
                HandleError(ex, "while browsing for the source folder.");
            }
        }

        private void btnDestinationBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                using (var dialog = new FolderBrowserDialog())
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        txtDestination.Text = dialog.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                // BAL: Handle errors while browsing for the destination folder
                HandleError(ex, "while browsing for the destination folder.");
            }
        }

        // UI: Update the file count label
        private void UpdateFileCountLabel(int processedFiles, int totalFiles)
        {
            lblFileCount.Invoke(new Action(() =>
            {
                lblFileCount.Text = $"Files processed: {processedFiles}/{totalFiles}";
                lblFileCount.Refresh();
            }));
        }

        private void chkOutputToCSV_CheckedChanged_1(object sender, EventArgs e)
        {
            bool isChecked = chkOutputToCSV.Checked;
            txtCSVFileLocation.Enabled = isChecked;
            btnCSVFileBrowse.Enabled = isChecked;
        }

        private void btnCSVFileBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtCSVFileLocation.Text = dialog.SelectedPath;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you wish to close? This will stop any ongoing operation", "Confirm Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // UI: Update the buffer size label
        private void UpdateBufferSizeLabel(int bufferSize)
        {
            lblBufferSize.Invoke(new Action(() =>
            {
                lblBufferSize.Text = $"Buffer Size: {FormatSize(bufferSize)}";
                lblBufferSize.Refresh();
            }));
        }

        private void chkOutputToJSON_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkOutputToJSON.Checked;
            txtJSONFileLocation.Enabled = isChecked;
            btnJSONFileBrowse.Enabled = isChecked;
        }

        private void btnJSONFileBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtJSONFileLocation.Text = dialog.SelectedPath;
                }
            }
        }

        // DAL: Generate a CSV file name based on the current date and time
        private string GenerateCSVFileName(string directoryPath)
        {
            string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(directoryPath, $"FileCopyLog.{dateTime}.csv");
        }

        // DAL: Generate a JSON file name based on the current date and time
        private string GenerateJSONFileName(string directoryPath)
        {
            string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return Path.Combine(directoryPath, $"FileCopyLog.{dateTime}.json");
        }

        // BAL: Handle errors by logging and displaying them
        private void HandleError(Exception ex, string additionalInfo = "")
        {
            // Log the error to the console (optional)
            Console.WriteLine($"An error occurred: {ex.Message}\n{ex.StackTrace}");

            // BAL: Log the error to the ListView (you can also log to a file or any other logging mechanism)
            string dateTimeNow = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            AddLogEntry(dateTimeNow, "Error", "", $"Error: {ex.Message} {additionalInfo}", "", isError: true);

            // UI: Display the error in a MessageBox
            // MessageBox.Show($"An error occurred: {ex.Message}\n{additionalInfo}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // UI: Update the error file label
        private void UpdateErrorFileLabel()
        {
            lblFileErrors.Invoke(new Action(() =>
            {
                lblFileErrors.Text = $"Errored Files: {errorFileCount}";
                lblFileErrors.Refresh();
            }));
        }

        // UI: Update the transferred size label
        private void UpdateTransferredSizeLabel(long totalSize)
        {
            lblTransferredSize.Invoke(new Action(() =>
            {
                lblTransferredSize.Text = $"Total Processed Size: {FormatSize(totalSize)}";
                lblTransferredSize.Refresh();
            }));
        }

    }
}
