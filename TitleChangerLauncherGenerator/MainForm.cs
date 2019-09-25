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
        /// The name of the target file.
        /// </summary>
        private string targetFileName = string.Empty;

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
            // Check there's a target file name
            if (this.targetFileName.Length == 0)
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
            // Clear target file name
            this.targetFileName = string.Empty;

            // Clear text boxes
            this.newTiTLeTextBox.Clear();

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
                // Set target file name
                this.targetFileName = this.openFileDialog.FileName;
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
    }
}