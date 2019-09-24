// <copyright file="MainForm.cs" company="PublicDomain.com">
//     CC0 1.0 Universal (CC0 1.0) - Public Domain Dedication
//     https://creativecommons.org/publicdomain/zero/1.0/legalcode
// </copyright>

namespace TitleChangerLauncherGenerator
{
    // Directives
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TitleChangerLauncherGenerator.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // The InitializeComponent() call is required for Windows Forms designer support.
            this.InitializeComponent();
        }

        /// <summary>
        /// Handles the new tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnNewToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code.
        }

        /// <summary>
        /// Handles the exit tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code.
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
            // TODO Add code.
        }

        /// <summary>
        /// Handles the generate/revert button click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnGenerateRevertButtonClick(object sender, EventArgs e)
        {
            // TODO Add code.
        }

        /// <summary>
        /// Handles the headquarters patreon.com tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnHeadquartersPatreoncomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code.
        }

        /// <summary>
        /// Handles the source code github.com tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnSourceCodeGithubcomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code.
        }

        /// <summary>
        /// Handles the thread donationcoder.com tool strip menu item click event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnThreadDonationCodercomToolStripMenuItemClick(object sender, EventArgs e)
        {
            // TODO Add code.
        }

        /// <summary>
        /// Handles the revert radio button checked changed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnRevertRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            // TODO Add code.
        }

        /// <summary>
        /// Handles the generate radio button checked changed event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnGenerateRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            // TODO Add code.
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
        }
    }
}