using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using UpdaterNET;

/// <summary>
/// Update maker .NET namespace
/// </summary>
namespace UpdateMakerNET
{
    /// <summary>
    /// Main form class
    /// </summary>
    public partial class MainForm : MaterialForm
    {
        /// <summary>
        /// Files
        /// </summary>
        private readonly Dictionary<string, string> files = new Dictionary<string, string>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            filesListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            MaterialSkinManager material_skin_manager = MaterialSkinManager.Instance;
            material_skin_manager.AddFormToManage(this);
            material_skin_manager.Theme = MaterialSkinManager.Themes.DARK;
            material_skin_manager.ColorScheme = new ColorScheme(Primary.Blue700, Primary.Blue800, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
            destinationDirectorySingleLineTextField.Text = Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Delete selected entries
        /// </summary>
        private void DeleteSelectedEntries()
        {
            foreach (ListViewItem item in filesListView.SelectedItems)
            {
                string key = item.Text;
                if (files.ContainsKey(key))
                {
                    files.Remove(key);
                }
            }
            ReloadEntries();
        }

        /// <summary>
        /// Reload entries
        /// </summary>
        private void ReloadEntries()
        {
            filesListView.Items.Clear();
            foreach (KeyValuePair<string, string> kv in files)
            {
                ListViewItem item = filesListView.Items.Add(kv.Key);
                item.SubItems.Add(kv.Value);
            }
        }

        /// <summary>
        /// Files list view key up event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Key event arguments</param>
        private void filesListView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedEntries();
            }
        }

        /// <summary>
        /// Add files button click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void addFilesButton_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            DialogResult = DialogResult.None;
            if (result == DialogResult.OK)
            {
                foreach (string file_name in openFileDialog.FileNames)
                {
                    string entry_name = Path.GetFileName(file_name);
                    if (files.ContainsKey(entry_name))
                    {
                        files[entry_name] = file_name;
                    }
                    else
                    {
                        files.Add(entry_name, file_name);
                    }
                }
                ReloadEntries();
            }
        }

        /// <summary>
        /// Create version button click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void createVersionButton_Click(object sender, EventArgs e)
        {
            string destination_directory = destinationDirectorySingleLineTextField.Text.Trim();
            if (destination_directory.Length > 0)
            {
                if (destination_directory[destination_directory.Length - 1] != Path.DirectorySeparatorChar)
                {
                    destination_directory += Path.DirectorySeparatorChar;
                }
            }
            string archive_path = destination_directory + "update.zip";
            try
            {
                if (File.Exists(archive_path))
                {
                    File.Delete(archive_path);
                }
                using (ZipArchive zip_archive = ZipFile.Open(archive_path, ZipArchiveMode.Create))
                {
                    foreach (KeyValuePair<string, string> kv in files)
                    {
                        if (File.Exists(kv.Value))
                        {
                            zip_archive.CreateEntryFromFile(kv.Value, kv.Key);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            string error = null;
            UpdateTask.GenerateUpdateJSON(destination_directory + "update.json", exeFileSingleLineTextField.Text, archive_path, urlSingleLineTextField.Text, ref error);
            if (error != null)
            {
                Console.Error.WriteLine(error);
                MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// FIles list view double click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void filesListView_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in filesListView.SelectedItems)
            {
                string key = item.Text;
                if (files.ContainsKey(key))
                {
                    exeFileSingleLineTextField.Text = files[key];
                    break;
                }
            }
        }

        /// <summary>
        /// Set as executable tool strip menu item click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void setAsExecutableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in filesListView.SelectedItems)
            {
                string key = item.Text;
                if (files.ContainsKey(key))
                {
                    exeFileSingleLineTextField.Text = files[key];
                    break;
                }
            }
        }

        /// <summary>
        /// Rename entry tool strip menu item click event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void renameEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in filesListView.SelectedItems)
            {
                string key = item.Text;
                if (files.ContainsKey(key))
                {
                    RenameEntryForm rename_entry_form = new RenameEntryForm(key);
                    DialogResult result = rename_entry_form.ShowDialog();
                    DialogResult = DialogResult.None;
                    if (result == DialogResult.OK)
                    {
                        if (!(files.ContainsKey(rename_entry_form.EntryName)))
                        {
                            files[rename_entry_form.EntryName] = files[key];
                            files.Remove(key);
                            ReloadEntries();
                        }
                    }
                    break;
                }
            }
        }
    }
}
