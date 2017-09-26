﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Windows.Forms;

/// <summary>
/// Updater .NET namespace
/// </summary>
namespace UpdaterNET
{
    /// <summary>
    /// Update class
    /// </summary>
    public class Update
    {
        /// <summary>
        /// Updates installed event handler delegate
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        public delegate void UpdatesInstalledEventHandler(object sender, EventArgs e);

        /// <summary>
        /// On updates installed event
        /// </summary>
        public event UpdatesInstalledEventHandler onUpdatesInstalled;

        /// <summary>
        /// Serializer
        /// </summary>
        private static readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UpdateDataContract));

        /// <summary>
        /// Update data
        /// </summary>
        private static UpdateDataContract updateData = null;

        /// <summary>
        /// Endpoint
        /// </summary>
        private string endpoint;

        /// <summary>
        /// Original version number
        /// </summary>
        private uint originalVersionNumber = 0;

        /// <summary>
        /// Is update available
        /// </summary>
        public bool IsUpdateAvailable
        {
            get
            {
                return ((updateData == null) ? false : (updateData.versionNumber > originalVersionNumber));
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get
            {
                return ((updateData == null) ? "0.0.0.0" : updateData.version);
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        public uint VersionNumber
        {
            get
            {
                return ((updateData == null) ? 0U : updateData.versionNumber);
            }
        }

        /// <summary>
        /// URI
        /// </summary>
        public string URI
        {
            get
            {
                return ((updateData == null) ? "" : updateData.uri);
            }
        }

        /// <summary>
        /// SHA512
        /// </summary>
        public string SHA512
        {
            get
            {
                return ((updateData == null) ? "" : updateData.sha512);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        public Update(string endpoint, string pathToExe, int timeout = 3000)
        {
            this.endpoint = endpoint;
            string version;
            GetFileVersionInfo(pathToExe, out originalVersionNumber, out version);
            using (WebClientEx wc = new WebClientEx(timeout))
            {
                wc.Headers.Set(HttpRequestHeader.Accept, "application/json");
                wc.Headers.Set(HttpRequestHeader.UserAgent, "Mozilla/3.0 (compatible; Updater .NET)");
                try
                {
                    using (MemoryStream stream = new MemoryStream(wc.DownloadData(endpoint)))
                    {
                        updateData = serializer.ReadObject(stream) as UpdateDataContract;
                    }
                }
                catch
                {
                    Application.Exit();
                }
            }
        }

        /// <summary>
        /// Install updates
        /// </summary>
        public void InstallUpdates(DownloadProgressChangedEventHandler downloadProgressChangedEventHandler = null, UpdatesInstalledEventHandler updatesInstalledEventHandler = null)
        {
            onUpdatesInstalled += updatesInstalledEventHandler;
            if (updateData == null)
                onUpdatesInstalled.Invoke(this, new EventArgs());
            else
            {
                WebClient wc = new WebClient();
                if (downloadProgressChangedEventHandler != null)
                    wc.DownloadProgressChanged += downloadProgressChangedEventHandler;
                wc.DownloadFileCompleted += OnDownloadFileCompleted;
                try
                {
                    if (!(Directory.Exists("./updates")))
                        Directory.CreateDirectory("./updates");
                    wc.DownloadFileAsync(new Uri(updateData.uri), "./updates/" + updateData.version + ".zip");
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    onUpdatesInstalled.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Get file version info
        /// </summary>
        /// <param name="path">Path</param>
        /// <param name="versionNumber">Version number</param>
        /// <param name="version">Version</param>
        private static void GetFileVersionInfo(string path, out uint versionNumber, out string version)
        {
            versionNumber = 0U;
            version = "0.0.0.0";
            try
            {
                FileVersionInfo version_info = FileVersionInfo.GetVersionInfo(path);
                versionNumber = (uint)((version_info.FileMajorPart * 1000000) + (version_info.FileMinorPart * 10000) + (version_info.FileBuildPart * 100) + (version_info.FilePrivatePart));
                version = version_info.FileVersion;
            }
            catch
            {
                //
            }
        }

        /// <summary>
        /// SHA512 from file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>SHA512 in Base64</returns>
        private static string SHA512FromFile(string path)
        {
            string ret = "";
            if (File.Exists(path))
            {
                SHA512 sha512 = System.Security.Cryptography.SHA512.Create();
                if (sha512 == null)
                    MessageBox.Show("Failed to create SHA512 instance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (File.Exists(path))
                    {
                        using (StreamReader reader = new StreamReader(path))
                        {
                            ret = Convert.ToBase64String(sha512.ComputeHash(reader.BaseStream));
                        }
                    }
                    else
                        MessageBox.Show("File \"" + path + "\" not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return ret;
        }

        /// <summary>
        /// Generate update.json
        /// </summary>
        /// <param name="destinationPath">Destination path</param>
        /// <param name="pathToExe">Path to executable</param>
        /// <param name="pathToArchive">Path to archive</param>
        /// <param name="uri">URI</param>
        public static void GenerateUpdateJSON(string destinationPath, string pathToExe, string pathToArchive, string uri)
        {
            UpdateDataContract update_data = new UpdateDataContract();
            update_data.uri = uri;
            if (File.Exists(pathToExe))
                GetFileVersionInfo(pathToExe, out update_data.versionNumber, out update_data.version);
            if (File.Exists(pathToArchive))
                update_data.sha512 = SHA512FromFile(pathToArchive);
            using (StreamWriter writer = new StreamWriter(destinationPath))
            {
                serializer.WriteObject(writer.BaseStream, update_data);
            }
        }

        /// <summary>
        /// On download file completed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Asynchronous operation event arguments</param>
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                // Verify checksum
                string update_file_name = "./updates/" + updateData.version + ".zip";
                string hash = SHA512FromFile(update_file_name);
                if (hash.Length > 0)
                {
                    if (hash == updateData.sha512)
                    {
                        // Backup old files
                        if (!(Directory.Exists("./backups")))
                            Directory.CreateDirectory("./backups");
                        string backup_file_name = "./backups/pre." + updateData.version + ".zip";
                        try
                        {
                            if (File.Exists(backup_file_name))
                                File.Delete(backup_file_name);
                            using (ZipArchive zip_archive = ZipFile.Open(backup_file_name, ZipArchiveMode.Create))
                            {
                                string[] file_names = Directory.GetFiles(".");
                                string current_directory = Directory.GetCurrentDirectory();
                                if (current_directory.Length > 0)
                                {
                                    if (current_directory[current_directory.Length - 1] != Path.DirectorySeparatorChar)
                                        current_directory += Path.DirectorySeparatorChar;
                                }
                                foreach (string file_name in file_names)
                                {
                                    if ((!(file_name.StartsWith("backups" + Path.DirectorySeparatorChar))) && (!(file_name.StartsWith("updates" + Path.DirectorySeparatorChar))))
                                    {
                                        if (file_name.StartsWith(current_directory))
                                            zip_archive.CreateEntryFromFile(file_name, file_name.Substring(current_directory.Length).Replace('\\', '/'));
                                    }
                                }
                            }

                            // Unzip updates
                            try
                            {
                                using (ZipArchive zip_archive = ZipFile.Open(update_file_name, ZipArchiveMode.Read))
                                {
                                    foreach (ZipArchiveEntry entry in zip_archive.Entries)
                                    {
                                        string entry_name = entry.FullName.Replace('\\', '/');
                                        using (Stream reader = entry.Open())
                                        {
                                            using (FileStream file_stream = new FileStream("./" + entry_name, FileMode.Create))
                                            {
                                                int b;
                                                while ((b = reader.ReadByte()) != -1)
                                                    file_stream.WriteByte((byte)b);
                                            }
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // Revert changes
                                try
                                {
                                    if (File.Exists(backup_file_name))
                                    {
                                        using (ZipArchive zip_archive = ZipFile.Open(backup_file_name, ZipArchiveMode.Read))
                                        {
                                            foreach (ZipArchiveEntry entry in zip_archive.Entries)
                                            {
                                                string entry_name = entry.FullName.Replace('\\', '/');
                                                using (Stream reader = entry.Open())
                                                {
                                                    using (FileStream file_stream = new FileStream("./" + entry_name, FileMode.Create))
                                                    {
                                                        int b;
                                                        while ((b = reader.ReadByte()) != -1)
                                                            file_stream.WriteByte((byte)b);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("Attempt on reverting changes failed.\r\n\r\nBackup file: " + backup_file_name, "Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Can't create backup from files.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                        MessageBox.Show("SHA512 hash for file \"" + update_file_name + "\" is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            onUpdatesInstalled.Invoke(this, new EventArgs());
        }
    }
}
