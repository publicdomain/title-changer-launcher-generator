// <copyright file="MainForm.cs" company="PublicDomain.com">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>

namespace TitleChangerLauncherGenerator
{
    // Directives
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Microsoft.CSharp;

    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The path of the target file.
        /// </summary>
        private string targetFilePath = string.Empty;

        /// <summary>
        /// The launcher code.
        /// </summary>
        private string launcherCode = @"
            // Directives
            using System;
            using System.Diagnostics;
            using System.Runtime.InteropServices;
            using System.Text;
            using System.Windows.Forms;

            /// <summary>
            /// Title changer.
            /// </summary>
            public class TitleChanger
            {
                /// <summary>
                /// The title timer.
                /// </summary>
                static System.Windows.Forms.Timer titleTimer = new System.Windows.Forms.Timer();

                /// <summary>
                /// The exit flag.
                /// </summary>
                static bool exitFlag = false;

                /// <summary>
                /// The process.
                /// </summary>
                static Process process = null;

                /// <summary>
                /// The tick counter.
                /// </summary>
                static int tickCounter = 0;

                /// <summary>
                /// The name of the target file.
                /// </summary>
                static readonly string targetFileName = Encoding.UTF8.GetString(Convert.FromBase64String(""[|>ENCODED-FILE-NAME<|]""));

                /// <summary>
                /// The new title.
                /// </summary>
                static readonly string newTitle = Encoding.UTF8.GetString(Convert.FromBase64String(""[|>ENCODED-NEW-TITLE<|]""));

                /// <summary>
                /// Sets the window text.
                /// </summary>
                /// <returns><c>true</c>, if window text was set, <c>false</c> otherwise.</returns>
                /// <param name=""hwnd"">The Hwnd.</param>
                /// <param name=""longPointerToString"">Long pointer to string.</param>
                [DllImport(""user32.dll"", EntryPoint = ""SetWindowText"")]
                public static extern bool SetWindowText(IntPtr hwnd, String longPointerToString);

                /// <summary>
                /// Handles the title timer tick event.
                /// </summary>
                /// <param name=""sender"">Sender object.</param>
                /// <param name=""eventArgs"">Event arguments.</param>
                private static void TitleTimerTick(object sender, EventArgs eventArgs)
                {
                    // Counter erratic ticks
                    if (exitFlag)
                    {
                        // Exit function
                        return;
                    }

                    // Rise the tick counter
                    tickCounter++;

                    // Try to get the window handle.
                    if (process.MainWindowHandle != IntPtr.Zero)
                    {
                        // Set title text
                        if (SetWindowText(process.MainWindowHandle, newTitle))
                        {
                            // Close the program on sucess
                            StopTimerAndExit();
                        }
                    }

                    // Check for 2 minutes worth of ticks
                    if (tickCounter == 12000)
                    {
                        // Halt program flow
                        StopTimerAndExit();
                    }
                }

                /// <summary>
                /// Stops the timer and exit.
                /// </summary>
                private static void StopTimerAndExit()
                {
                    // Stop title timer
                    titleTimer.Stop();

                    // Set flag to terminate main program loop
                    exitFlag = true;
                }

                /// <summary>
                /// The entry point of the program, where the program control starts and ends.
                /// </summary>
                /// <returns>The exit code that is given to the operating system after the program ends.</returns>
                public static int Main()
                {
                    // Wrap to advise on program start error
                    try
                    {
                        // Set process by running target program
                        process = Process.Start(targetFileName);
                    }
                    catch (Exception ex)
                    {
                        // Advise user
                        MessageBox.Show(""Could not run program "" + targetFileName + ""."" + Environment.NewLine + Environment.NewLine + ""Reason:"" + Environment.NewLine + ex.Message, ""Process start error"", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Halt program
                        return 0;
                    }

                    // Add title timer event handler
                    titleTimer.Tick += new EventHandler(TitleTimerTick);

                    // Set interval to 10 milliseconds for 100 ticks per second
                    titleTimer.Interval = 10;

                    // Start title timer
                    titleTimer.Start();

                    // Main program loop
                    while (exitFlag == false)
                    {
                        // Process events
                        Application.DoEvents();
                    }

                    // Exit program
                    return 0;
                }
            }";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TitleChangerLauncherGenerator.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the generate/revert button click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnGenerateRevertButtonClick(object sender, EventArgs e)
        {
            // Check there's a target file path
            if (this.targetFilePath.Length == 0)
            {
                // Advise user
                MessageBox.Show("Please set target program's executable file!", "Target program missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // Halt flow
                return;
            }

            // Set target file name without extension
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.targetFilePath);

            // Check if the user picked an "-original" or "-launcher" file
            if (fileNameWithoutExtension.EndsWith("-original", StringComparison.InvariantCulture) || fileNameWithoutExtension.EndsWith("-launcher", StringComparison.InvariantCulture))
            {
                // Fix target file path
                this.targetFilePath = Path.Combine(Path.GetDirectoryName(this.targetFilePath), $"{fileNameWithoutExtension.Remove(fileNameWithoutExtension.Length - 9)}{Path.GetExtension(this.targetFilePath)}");

                // Amend file name without extension to be used below
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.targetFilePath);
            }

            // Set file name with appended "-original"
            string fileNameWithOriginal = $"{fileNameWithoutExtension}-original{Path.GetExtension(this.targetFilePath)}";

            // Set file path with inserted "-original"
            string filePathWithOriginal = Path.Combine(Path.GetDirectoryName(this.targetFilePath), fileNameWithOriginal);

            // Set file name with appended"-launcher"
            string fileNameWithLauncher = $"{fileNameWithoutExtension}-launcher{Path.GetExtension(this.targetFilePath)}";

            // Set file path with inserted "-launcher"
            string filePathWithLauncher = Path.Combine(Path.GetDirectoryName(this.targetFilePath), fileNameWithLauncher);

            // Wrap to advise on previous state restore error
            try
            {
                // Check for an "-original" file
                if (File.Exists(filePathWithOriginal))
                {
                    // Delete target
                    File.Delete(this.targetFilePath);

                    // Rename original back to target
                    File.Move(filePathWithOriginal, this.targetFilePath);
                }

                // Check for a "-launcher" file
                if (File.Exists(filePathWithLauncher))
                {
                    // Delete launcher
                    File.Delete(filePathWithLauncher);
                }
            }
            catch (Exception ex)
            {
                // Update status
                this.mainToolStripStatusLabel.Text = "Restore error. Please retry.";

                // Advise user
                MessageBox.Show($"Could not restore {fileNameWithoutExtension}.{Environment.NewLine}{Environment.NewLine}Reason:{Environment.NewLine}{ex.Message}", "Restore error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Check if must generate launcher
            if (this.generateRadioButton.Checked)
            {
                // Wrap to advise on compilation error
                try
                {
                    // Declare code provider
                    CSharpCodeProvider codeProvider = new CSharpCodeProvider();

                    // Declare compiler parameneters
                    CompilerParameters compilerParameters = new CompilerParameters();

                    // Update status
                    this.mainToolStripStatusLabel.Text = "Generating...";

                    // Add references
                    compilerParameters.ReferencedAssemblies.Add("System.dll");
                    compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");

                    // External file generation
                    compilerParameters.GenerateInMemory = false;

                    // Exe file generation
                    compilerParameters.GenerateExecutable = true;

                    // Do not treat warning as errors
                    compilerParameters.TreatWarningsAsErrors = false;

                    // Set extracted icon 
                    Icon extractedIcon = Icon.ExtractAssociatedIcon(this.targetFilePath);

                    // Create launcher icon
                    using (FileStream fileStream = new FileStream("launcher.ico", FileMode.Create))
                    {
                        // Save icon file stream to disk
                        extractedIcon.Save(fileStream);
                    }

                    // Declare target file name
                    string targetFileName = string.Empty;

                    // Declare output file name
                    string outputFileName = string.Empty;

                    // Check if must generate in-place
                    if (this.inPlaceCheckBox.Checked)
                    {
                        // Set target file name (append "-original")
                        targetFileName = $"{fileNameWithoutExtension}-original{Path.GetExtension(this.targetFilePath)}";

                        // Set output file name based on initial path
                        outputFileName = Path.GetFileName(this.targetFilePath);

                        // Rename by appending "-original"
                        File.Move(this.targetFilePath, Path.Combine(Path.GetDirectoryName(this.targetFilePath), targetFileName));
                    }
                    else
                    {
                        // Set target file name to the one in path
                        targetFileName = Path.GetFileName(this.targetFilePath);

                        // Set output file name ("-launcher")
                        outputFileName = fileNameWithLauncher;
                    }

                    // Set output assembly
                    compilerParameters.OutputAssembly = Path.Combine(Path.GetDirectoryName(this.targetFilePath), outputFileName);

                    // Target windows executable, set program icon
                    compilerParameters.CompilerOptions = "-target:winexe -win32icon:launcher.ico";

                    // Declare replaced launcher code
                    string replacedLauncherCode = this.launcherCode;

                    // Replace file name
                    replacedLauncherCode = replacedLauncherCode.Replace("[|>ENCODED-FILE-NAME<|]", this.Base64Encode(targetFileName));

                    // Replace new title
                    replacedLauncherCode = replacedLauncherCode.Replace("[|>ENCODED-NEW-TITLE<|]", this.Base64Encode(this.newTitleTextBox.Text));

                    // Compile launcher
                    CompilerResults compilerResults = codeProvider.CompileAssemblyFromSource(compilerParameters, new string[] { replacedLauncherCode });

                    // Remove launcher icon
                    File.Delete("launcher.ico");

                    // Update status
                    this.mainToolStripStatusLabel.Text = $"{(this.inPlaceCheckBox.Checked ? "In-place" : string.Empty)} Launcher generated!";
                }
                catch (Exception ex)
                {
                    // Update status
                    this.mainToolStripStatusLabel.Text = "Error. Please retry.";

                    // Advise user
                    MessageBox.Show($"Could not generate launcher for {fileNameWithoutExtension}.{Environment.NewLine}{Environment.NewLine}Reason:{Environment.NewLine}{ex.Message}", "Launcher compilation error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Update status
                this.mainToolStripStatusLabel.Text = $"Reverted to initial state successfully.";
            }
        }

        /// <summary>
        /// Handles the new tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Clear target file path
            this.targetFilePath = string.Empty;

            // Clear text boxes
            this.newTitleTextBox.Clear();

            // Check in place check box
            this.inPlaceCheckBox.Checked = true;

            // Check generate radio button
            this.generateRadioButton.Checked = true;

            // Reset status
            this.mainToolStripStatusLabel.Text = "1) Set target, previous and new title. 2) Generate!";
        }

        /// <summary>
        /// Handles the exit tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Close program
            this.Close();
        }

        /// <summary>
        /// Handles the about tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnAboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code.
        }

        /// <summary>
        /// Handles the browse button click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnBrowseButtonClick(object sender, EventArgs e)
        {
            // Show open file dialog
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Set target file path
                this.targetFilePath = this.openFileDialog.FileName;
            }
        }

        /// <summary>
        /// Handles the headquarters patreon.com tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnHeadquartersPatreoncomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open Patreon headquarters
            Process.Start("https://www.patreon.com/publicdomain/");
        }

        /// <summary>
        /// Handles the source code github.com tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSourceCodeGithubcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open GitHub
            Process.Start("https://github.com/publicdomain/");
        }

        /// <summary>
        /// Handles the thread donationcoder.com tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnThreadDonationCodercomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // Open original thread @ DonationCoder
            Process.Start("https://www.donationcoder.com/forum/index.php?topic=48711.0");
        }

        /// <summary>
        /// Handles the revert radio button checked changed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRevertRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            // Set revert button text
            this.generateRevertButton.Text = "&Revert to original state";

            // Set color to blue
            this.generateRevertButton.ForeColor = Color.Blue;
        }

        /// <summary>
        /// Handles the generate radio button checked changed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnGenerateRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            // Set generate button text
            this.SetGenerateButtonText();
        }

        /// <summary>
        /// Handles the in place check box checked changed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnInPlaceCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            // Check if generate radio button is checked
            if (this.generateRadioButton.Checked)
            {
                // Set generate button text
                this.SetGenerateButtonText();
            }
        }

        /// <summary>
        /// Sets the generate button text.
        /// </summary>
        private void SetGenerateButtonText()
        {
            // Set accounting for in-place checkbox
            this.generateRevertButton.Text = $"&Generate{(this.inPlaceCheckBox.Checked ? " in-place " : " ")}Launcher";

            // Set color to red
            this.generateRevertButton.ForeColor = Color.Red;
        }

        /// <summary>
        /// Encodes input string using Base64.
        /// </summary>
        /// <returns>The encode.</returns>
        /// <param name="inputString">Input string.</param>
        private string Base64Encode(string inputString)
        {
            // Return encoded string
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(inputString));
        }
    }
}