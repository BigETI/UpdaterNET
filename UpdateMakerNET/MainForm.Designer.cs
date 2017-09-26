namespace UpdateMakerNET
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.inputPanel = new System.Windows.Forms.Panel();
            this.destinationDirectoryLabel = new MaterialSkin.Controls.MaterialLabel();
            this.destinationDirectorySingleLineTextField = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.exeFileSingleLineTextField = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.exeFileLabel = new MaterialSkin.Controls.MaterialLabel();
            this.addFilesButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.createVersionButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.urlSingleLineTextField = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.urlLabel = new MaterialSkin.Controls.MaterialLabel();
            this.filesListView = new MaterialSkin.Controls.MaterialListView();
            this.entryColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pathColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainTableLayoutPanel.SuspendLayout();
            this.inputPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainTableLayoutPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.filesListView, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.inputPanel, 0, 1);
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(12, 64);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 193F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(776, 524);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // inputPanel
            // 
            this.inputPanel.Controls.Add(this.urlSingleLineTextField);
            this.inputPanel.Controls.Add(this.urlLabel);
            this.inputPanel.Controls.Add(this.createVersionButton);
            this.inputPanel.Controls.Add(this.addFilesButton);
            this.inputPanel.Controls.Add(this.exeFileSingleLineTextField);
            this.inputPanel.Controls.Add(this.exeFileLabel);
            this.inputPanel.Controls.Add(this.destinationDirectorySingleLineTextField);
            this.inputPanel.Controls.Add(this.destinationDirectoryLabel);
            this.inputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inputPanel.Location = new System.Drawing.Point(3, 334);
            this.inputPanel.Name = "inputPanel";
            this.inputPanel.Size = new System.Drawing.Size(770, 187);
            this.inputPanel.TabIndex = 1;
            // 
            // destinationDirectoryLabel
            // 
            this.destinationDirectoryLabel.AutoSize = true;
            this.destinationDirectoryLabel.Depth = 0;
            this.destinationDirectoryLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.destinationDirectoryLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.destinationDirectoryLabel.Location = new System.Drawing.Point(3, 0);
            this.destinationDirectoryLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.destinationDirectoryLabel.Name = "destinationDirectoryLabel";
            this.destinationDirectoryLabel.Size = new System.Drawing.Size(149, 19);
            this.destinationDirectoryLabel.TabIndex = 0;
            this.destinationDirectoryLabel.Text = "Destination directory";
            // 
            // destinationDirectorySingleLineTextField
            // 
            this.destinationDirectorySingleLineTextField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationDirectorySingleLineTextField.Depth = 0;
            this.destinationDirectorySingleLineTextField.Hint = "...";
            this.destinationDirectorySingleLineTextField.Location = new System.Drawing.Point(3, 22);
            this.destinationDirectorySingleLineTextField.MaxLength = 32767;
            this.destinationDirectorySingleLineTextField.MouseState = MaterialSkin.MouseState.HOVER;
            this.destinationDirectorySingleLineTextField.Name = "destinationDirectorySingleLineTextField";
            this.destinationDirectorySingleLineTextField.PasswordChar = '\0';
            this.destinationDirectorySingleLineTextField.SelectedText = "";
            this.destinationDirectorySingleLineTextField.SelectionLength = 0;
            this.destinationDirectorySingleLineTextField.SelectionStart = 0;
            this.destinationDirectorySingleLineTextField.Size = new System.Drawing.Size(764, 23);
            this.destinationDirectorySingleLineTextField.TabIndex = 1;
            this.destinationDirectorySingleLineTextField.TabStop = false;
            this.destinationDirectorySingleLineTextField.UseSystemPasswordChar = false;
            // 
            // exeFileSingleLineTextField
            // 
            this.exeFileSingleLineTextField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exeFileSingleLineTextField.Depth = 0;
            this.exeFileSingleLineTextField.Hint = "...";
            this.exeFileSingleLineTextField.Location = new System.Drawing.Point(3, 70);
            this.exeFileSingleLineTextField.MaxLength = 32767;
            this.exeFileSingleLineTextField.MouseState = MaterialSkin.MouseState.HOVER;
            this.exeFileSingleLineTextField.Name = "exeFileSingleLineTextField";
            this.exeFileSingleLineTextField.PasswordChar = '\0';
            this.exeFileSingleLineTextField.SelectedText = "";
            this.exeFileSingleLineTextField.SelectionLength = 0;
            this.exeFileSingleLineTextField.SelectionStart = 0;
            this.exeFileSingleLineTextField.Size = new System.Drawing.Size(764, 23);
            this.exeFileSingleLineTextField.TabIndex = 3;
            this.exeFileSingleLineTextField.TabStop = false;
            this.exeFileSingleLineTextField.UseSystemPasswordChar = false;
            // 
            // exeFileLabel
            // 
            this.exeFileLabel.AutoSize = true;
            this.exeFileLabel.Depth = 0;
            this.exeFileLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.exeFileLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exeFileLabel.Location = new System.Drawing.Point(3, 48);
            this.exeFileLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.exeFileLabel.Name = "exeFileLabel";
            this.exeFileLabel.Size = new System.Drawing.Size(107, 19);
            this.exeFileLabel.TabIndex = 2;
            this.exeFileLabel.Text = "Executable file";
            // 
            // addFilesButton
            // 
            this.addFilesButton.AutoSize = true;
            this.addFilesButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addFilesButton.Depth = 0;
            this.addFilesButton.Icon = null;
            this.addFilesButton.Location = new System.Drawing.Point(3, 147);
            this.addFilesButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.addFilesButton.Name = "addFilesButton";
            this.addFilesButton.Primary = true;
            this.addFilesButton.Size = new System.Drawing.Size(86, 36);
            this.addFilesButton.TabIndex = 4;
            this.addFilesButton.Text = "Add files";
            this.addFilesButton.UseVisualStyleBackColor = true;
            this.addFilesButton.Click += new System.EventHandler(this.addFilesButton_Click);
            // 
            // createVersionButton
            // 
            this.createVersionButton.AutoSize = true;
            this.createVersionButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.createVersionButton.Depth = 0;
            this.createVersionButton.Icon = null;
            this.createVersionButton.Location = new System.Drawing.Point(95, 147);
            this.createVersionButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.createVersionButton.Name = "createVersionButton";
            this.createVersionButton.Primary = true;
            this.createVersionButton.Size = new System.Drawing.Size(132, 36);
            this.createVersionButton.TabIndex = 5;
            this.createVersionButton.Text = "Create version";
            this.createVersionButton.UseVisualStyleBackColor = true;
            this.createVersionButton.Click += new System.EventHandler(this.createVersionButton_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            // 
            // urlSingleLineTextField
            // 
            this.urlSingleLineTextField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlSingleLineTextField.Depth = 0;
            this.urlSingleLineTextField.Hint = "...";
            this.urlSingleLineTextField.Location = new System.Drawing.Point(3, 118);
            this.urlSingleLineTextField.MaxLength = 32767;
            this.urlSingleLineTextField.MouseState = MaterialSkin.MouseState.HOVER;
            this.urlSingleLineTextField.Name = "urlSingleLineTextField";
            this.urlSingleLineTextField.PasswordChar = '\0';
            this.urlSingleLineTextField.SelectedText = "";
            this.urlSingleLineTextField.SelectionLength = 0;
            this.urlSingleLineTextField.SelectionStart = 0;
            this.urlSingleLineTextField.Size = new System.Drawing.Size(764, 23);
            this.urlSingleLineTextField.TabIndex = 7;
            this.urlSingleLineTextField.TabStop = false;
            this.urlSingleLineTextField.UseSystemPasswordChar = false;
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Depth = 0;
            this.urlLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.urlLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.urlLabel.Location = new System.Drawing.Point(3, 96);
            this.urlLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(36, 19);
            this.urlLabel.TabIndex = 6;
            this.urlLabel.Text = "URL";
            // 
            // filesListView
            // 
            this.filesListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.filesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.entryColumnHeader,
            this.pathColumnHeader});
            this.filesListView.Depth = 0;
            this.filesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesListView.Font = new System.Drawing.Font("Roboto", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.filesListView.FullRowSelect = true;
            this.filesListView.GridLines = true;
            this.filesListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.filesListView.Location = new System.Drawing.Point(3, 3);
            this.filesListView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.filesListView.MouseState = MaterialSkin.MouseState.OUT;
            this.filesListView.Name = "filesListView";
            this.filesListView.OwnerDraw = true;
            this.filesListView.Size = new System.Drawing.Size(770, 325);
            this.filesListView.TabIndex = 8;
            this.filesListView.UseCompatibleStateImageBehavior = false;
            this.filesListView.View = System.Windows.Forms.View.Details;
            this.filesListView.DoubleClick += new System.EventHandler(this.filesListView_DoubleClick);
            this.filesListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filesListView_KeyUp);
            // 
            // entryColumnHeader
            // 
            this.entryColumnHeader.Text = "Entry";
            // 
            // pathColumnHeader
            // 
            this.pathColumnHeader.Text = "Path";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Name = "MainForm";
            this.Text = "Update maker .NET";
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.inputPanel.ResumeLayout(false);
            this.inputPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Panel inputPanel;
        private MaterialSkin.Controls.MaterialRaisedButton createVersionButton;
        private MaterialSkin.Controls.MaterialRaisedButton addFilesButton;
        private MaterialSkin.Controls.MaterialSingleLineTextField exeFileSingleLineTextField;
        private MaterialSkin.Controls.MaterialLabel exeFileLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField destinationDirectorySingleLineTextField;
        private MaterialSkin.Controls.MaterialLabel destinationDirectoryLabel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private MaterialSkin.Controls.MaterialSingleLineTextField urlSingleLineTextField;
        private MaterialSkin.Controls.MaterialLabel urlLabel;
        private MaterialSkin.Controls.MaterialListView filesListView;
        private System.Windows.Forms.ColumnHeader entryColumnHeader;
        private System.Windows.Forms.ColumnHeader pathColumnHeader;
    }
}

