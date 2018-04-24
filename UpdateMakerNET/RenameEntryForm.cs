using MaterialSkin.Controls;
using System;
using System.Windows.Forms;

/// <summary>
/// Update maker .NET namespace
/// </summary>
namespace UpdateMakerNET
{
    /// <summary>
    /// Rename entry form class
    /// </summary>
    public partial class RenameEntryForm : MaterialForm
    {
        /// <summary>
        /// Entry name
        /// </summary>
        public string EntryName
        {
            get
            {
                return entryNameSingleLineTextField.Text.Trim();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public RenameEntryForm(string entry)
        {
            InitializeComponent();
            entryNameSingleLineTextField.Text = entry;
        }

        /// <summary>
        /// OK button click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
