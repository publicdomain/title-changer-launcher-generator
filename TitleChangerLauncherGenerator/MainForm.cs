// <copyright file="MainForm.cs" company="PublicDomain.com">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>

namespace TitleChangerLauncherGenerator
{
    // Directives
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

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
        private string[] launcherCode = new string[]
        {
            @"// Directives
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
                        MessageBox.Show($""Could not run program {targetFileName}.{Environment.NewLine}{Environment.NewLine}Reason:{Environment.NewLine}{ex.Message}"", ""Process start error"", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
            }"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TitleChangerLauncherGenerator.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            this.InitializeComponent();
        }

        /// <summary>
        /// Finds the window.
        /// </summary>
        /// <returns>The window.</returns>
        /// <param name="className">Class name.</param>
        /// <param name="windowName">Window name.</param>
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string className, string windowName);

        /// <summary>
        /// Sets the window text.
        /// </summary>
        /// <returns><c>true</c>, if window text was set, <c>false</c> otherwise.</returns>
        /// <param name="hwnd">The Hwnd.</param>
        /// <param name="longPointerToString">Long pointer to string. Instead of lpString, for stylecop.</param>
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        public static extern bool SetWindowText(IntPtr hwnd, string longPointerToString);

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
            Process.Start("https://github.com/publicdomain//");
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