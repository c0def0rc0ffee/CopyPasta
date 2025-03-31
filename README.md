#  CopyPasta

**CopyPasta** – This app lets you manage file transfers effortlessly with detailed logging, pausing and resuming options, and duplicate handling. Every feature has been carefully handwritten **on the form** for supreme coding efficiency. 
##  Features

- **Pause and Resume**: Need a quick break? Pause and resume transfers anytime.
- **Detailed Logging**: Track file operations in CSV or JSON logs, complete with timestamps.
- **Buffer Size Control**: Auto for each file or adjust it to suit your needs.
- **File Integrity Verification**: Check copied files with hash and size comparisons for peace of mind.

1. **Install Required Packages**:
   - [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json): `dotnet add package Newtonsoft.Json`

##  How to Use CopyPasta

1. **Select Your Pasta Ingredients**:
   - Choose your *Source Folder* (where files are copied from) and *Destination Folder* (where files are copied to).

2. **Configure Sauce Options**:
   - **Duplicate Handling**: Choose how to handle existing files (overwrite, skip, or ask for each file).
   - **Logging**: Enable CSV or JSON logging and set the output paths.
   - **Buffer Size**: Choose a buffer size to balance transfer speed and memory use.
   - **Pause Duration**: Add a custom interval between file transfers if needed. I found this to be useful copying large volumes of small files.

3. **Start Copying**:
   - Click **Copy** to start. Watch the progress bar as CopyPasta transfers your files with flavour!
   - **Pause/Resume** anytime, or **Stop** if you need to cancel.

4. **Check the Log Files**:
   - Find the logs in the specified paths after copying is complete. Detailed logs help you keep track of every step.

