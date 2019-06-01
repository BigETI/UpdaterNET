using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;

/// <summary>
/// Updater .NET namespace
/// </summary>
namespace UpdaterNET
{
    /// <summary>
    /// Update task class
    /// </summary>
    public class UpdateTask
    {
        /// <summary>
        /// On download progress changed event handler
        /// </summary>
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;

        /// <summary>
        /// On update task finished event handler
        /// </summary>
        public event UpdateTaskFinishedEventHandler UpdateTaskFinished;

        /// <summary>
        /// Serializer
        /// </summary>
        private static readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(UpdateDataContract));

        /// <summary>
        /// Update data
        /// </summary>
        private UpdateDataContract updateData;

        /// <summary>
        /// Original version number
        /// </summary>
        private uint originalVersionNumber;

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
        /// Initialize object
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="pathToExe">Path to executable</param>
        /// <param name="timeout">Timeout</param>
        private void Init(string endpoint, string pathToExe, int timeout)
        {
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
                    //
                }
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="pathToExe">Path to executable</param>
        public UpdateTask(string endpoint, string pathToExe)
        {
            Init(endpoint, pathToExe, 3000);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="pathToExe">Path to executable</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        public UpdateTask(string endpoint, string pathToExe, int timeout)
        {
            Init(endpoint, pathToExe, timeout);
        }

        /// <summary>
        /// Install updates
        /// </summary>
        public void InstallUpdates()
        {
            if (updateData == null)
            {
                UpdateTaskFinished.Invoke(this, new UpdateTaskFinishedEventArgs(true, null));
            }
            else
            {
                WebClient wc = new WebClient();
                wc.DownloadProgressChanged += OnDownloadProgressChanged;
                wc.DownloadFileCompleted += OnDownloadFileCompleted;
                try
                {
                    if (!(Directory.Exists("./updates")))
                    {
                        Directory.CreateDirectory("./updates");
                    }
                    wc.DownloadFileAsync(new Uri(updateData.uri), "./updates/" + updateData.version + ".zip");
                }
                catch (Exception e)
                {
                    UpdateTaskFinished.Invoke(this, new UpdateTaskFinishedEventArgs(true, e.ToString()));
                }
            }
        }

        /// <summary>
        /// On download progress changed event
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event arguments</param>
        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChanged.Invoke(sender, e);
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
        private static string SHA512FromFile(string path, ref string error)
        {
            string ret = "";
            try
            {
                if (File.Exists(path))
                {
                    using (SHA512 sha512 = System.Security.Cryptography.SHA512.Create())
                    {
                        if (File.Exists(path))
                        {
                            using (StreamReader reader = new StreamReader(path))
                            {
                                ret = Convert.ToBase64String(sha512.ComputeHash(reader.BaseStream));
                            }
                        }
                        else
                        {
                            error = "File \"" + path + "\" not found.";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                error = e.ToString();
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
        public static void GenerateUpdateJSON(string destinationPath, string pathToExe, string pathToArchive, string uri, ref string error)
        {
            UpdateDataContract update_data = new UpdateDataContract();
            update_data.uri = uri;
            if (File.Exists(pathToExe))
            {
                GetFileVersionInfo(pathToExe, out update_data.versionNumber, out update_data.version);
            }
            if (File.Exists(pathToArchive))
            {
                update_data.sha512 = SHA512FromFile(pathToArchive, ref error);
            }
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
            bool is_canceled = e.Cancelled;
            string error = ((e.Error == null) ? null : e.Error.ToString());
            if ((!is_canceled) && (error == null))
            {
                try
                {
                    // Verify checksum
                    string update_file_name = "./updates/" + updateData.version + ".zip";
                    string hash = SHA512FromFile(update_file_name, ref error);
                    if (error == null)
                    {
                        if (hash.Length > 0)
                        {
                            if (hash == updateData.sha512)
                            {
                                // Backup old files
                                if (!(Directory.Exists("./backups")))
                                {
                                    Directory.CreateDirectory("./backups");
                                }
                                string backup_file_name = "./backups/pre." + updateData.version + ".zip";
                                try
                                {
                                    if (File.Exists(backup_file_name))
                                    {
                                        File.Delete(backup_file_name);
                                    }
                                    using (ZipArchive zip_archive = ZipFile.Open(backup_file_name, ZipArchiveMode.Create))
                                    {
                                        string[] file_names = Directory.GetFiles(".", "*", SearchOption.AllDirectories);
                                        string current_directory = Directory.GetCurrentDirectory();
                                        if (current_directory.Length > 0)
                                        {
                                            if (current_directory[current_directory.Length - 1] != Path.DirectorySeparatorChar)
                                            {
                                                current_directory += Path.DirectorySeparatorChar;
                                            }
                                        }
                                        foreach (string file_name in file_names)
                                        {
                                            string path = Path.GetFullPath(file_name);
                                            if (path.StartsWith(current_directory))
                                            {
                                                if (!(path.Contains("backups" + Path.DirectorySeparatorChar)))
                                                {
                                                    zip_archive.CreateEntryFromFile(path, path.Substring(current_directory.Length).Replace('\\', '/'));
                                                }
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
                                                    try
                                                    {
                                                        using (FileStream file_stream = new FileStream("./" + entry_name, FileMode.Create))
                                                        {
                                                            reader.CopyTo(file_stream);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Console.Error.WriteLine(ex);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // Revert changes
                                        Console.Error.WriteLine(ex);
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
                                                            try
                                                            {
                                                                using (FileStream file_stream = new FileStream("./" + entry_name, FileMode.Create))
                                                                {
                                                                    reader.CopyTo(file_stream);
                                                                }
                                                            }
                                                            catch (Exception exc)
                                                            {
                                                                Console.Error.WriteLine(exc);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            is_canceled = true;
                                            error = "Attempt on reverting changes failed.\r\n\r\nBackup file: " + backup_file_name;
                                        }
                                    }
                                }
                                catch
                                {
                                    is_canceled = true;
                                    error = "Can't create backup from files.";
                                }
                            }
                            else
                            {
                                is_canceled = true;
                                error = "SHA512 hash for file \"" + update_file_name + "\" is invalid.";
                            }
                        }
                    }
                    else
                    {
                        is_canceled = true;
                    }
                }
                catch (Exception ex)
                {
                    is_canceled = true;
                    error = ex.ToString();
                }
            }
            UpdateTaskFinished.Invoke(this, new UpdateTaskFinishedEventArgs(is_canceled, error));
        }
    }
}
