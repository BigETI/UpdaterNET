using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;

/// <summary>
/// Updater .NET namespace
/// </summary>
namespace UpdaterNET
{
    /// <summary>
    /// GitHub update task
    /// </summary>
    public class GitHubUpdateTask
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
        /// GitHub user name
        /// </summary>
        private string user;

        /// <summary>
        /// GitHub repository name
        /// </summary>
        private string repository;

        /// <summary>
        /// Assets regular expression
        /// </summary>
        public string assetsRegex;

        /// <summary>
        /// Assets regular expression ignore case
        /// </summary>
        private RegexOptions assetsRegexOptions;

        /// <summary>
        /// Update data
        /// </summary>
        private GitHubLatestReleaseDataContract lastReleaseData;

        /// <summary>
        /// Update data
        /// </summary>
        private GitHubLatestReleaseDataContract latestReleaseData;

        /// <summary>
        /// Download path
        /// </summary>
        private string downloadPath;

        /// <summary>
        /// Last update data path
        /// </summary>
        private string lastUpdateDataPath;

        /// <summary>
        /// Serializer
        /// </summary>
        private static readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(GitHubLatestReleaseDataContract));

        /// <summary>
        /// Is update available
        /// </summary>
        public bool IsUpdateAvailable
        {
            get
            {
                return ((lastReleaseData == null) ? (latestReleaseData != null) : ((latestReleaseData == null) ? false : (latestReleaseData.TagName != lastReleaseData.TagName)));
            }
        }

        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get
            {
                return ((latestReleaseData == null) ? "0.0.0.0" : latestReleaseData.TagName);
            }
        }

        /// <summary>
        /// URIs
        /// </summary>
        public string[] URIs
        {
            get
            {
                List<string> ret = new List<string>();
                if (latestReleaseData != null)
                {
                    try
                    {
                        Regex regex = new Regex(assetsRegex, assetsRegexOptions);
                        foreach (GitHubReleaseAssetDataContract asset in latestReleaseData.Assets)
                        {
                            if (regex.IsMatch(asset.Name))
                            {
                                ret.Add(asset.BrowserDownloadURL);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e.Message);
                    }
                }
                return ret.ToArray();
            }
        }

        /// <summary>
        /// URI
        /// </summary>
        public string URI
        {
            get
            {
                string[] uris = URIs;
                return ((uris.Length > 0) ? uris[0] : "");
            }
        }

        /// <summary>
        /// Initialize object
        /// </summary>
        /// <param name="user">GitHub user name</param>
        /// <param name="repository">GitHub repository name</param>
        /// <param name="timeout">Timeout</param>
        /// <param name="assetsRegex">Assets regular expression</param>
        /// <param name="assetsRegexOptions">Assets regular expression options</param>
        private void Init(string user, string repository, int timeout, string assetsRegex, RegexOptions assetsRegexOptions)
        {
            this.user = user;
            this.repository = repository;
            this.assetsRegex = assetsRegex;
            this.assetsRegexOptions = assetsRegexOptions;
            try
            {
                lastUpdateDataPath = Path.GetFullPath("./" + user + "$" + repository + ".last.json");
                if (File.Exists(lastUpdateDataPath))
                {
                    using (FileStream stream = File.Open(lastUpdateDataPath, FileMode.Open))
                    {
                        lastReleaseData = serializer.ReadObject(stream) as GitHubLatestReleaseDataContract;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
            try
            {
                using (WebClientEx wc = new WebClientEx(timeout))
                {
                    wc.Headers.Set(HttpRequestHeader.Accept, "application/json");
                    wc.Headers.Set(HttpRequestHeader.UserAgent, "Mozilla/3.0 (compatible; Updater .NET)");
                    using (MemoryStream stream = new MemoryStream(wc.DownloadData("https://api.github.com/repos/" + user + "/" + repository + "/releases/latest")))
                    {
                        latestReleaseData = serializer.ReadObject(stream) as GitHubLatestReleaseDataContract;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">GitHub user name</param>
        /// <param name="repository">GitHub repository name</param>
        /// <param name="assetsRegex">Assets regular expression</param>
        /// <param name="assetsRegexOptions">Assets regular expression options</param>
        public GitHubUpdateTask(string user, string repository)
        {
            Init(user, repository, 3000, "*", RegexOptions.None);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">GitHub user</param>
        /// <param name="repository">GitHub repository name</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        public GitHubUpdateTask(string user, string repository, int timeout)
        {
            Init(user, repository, timeout, "*", RegexOptions.None);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">GitHub user name</param>
        /// <param name="repository">GitHub repository name</param>
        /// <param name="assetsRegex">Assets regular expression</param>
        /// <param name="assetsRegexOptions">Assets regular expression options</param>
        public GitHubUpdateTask(string user, string repository, string assetsRegex, RegexOptions assetsRegexOptions)
        {
            Init(user, repository, 3000, assetsRegex, assetsRegexOptions);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">GitHub user</param>
        /// <param name="repository">GitHub repository name</param>
        /// <param name="timeout">Timeout in milliseconds</param>
        public GitHubUpdateTask(string user, string repository, int timeout, string assetRegex, RegexOptions assetRegexOptions)
        {
            Init(user, repository, timeout, assetsRegex, assetsRegexOptions);
        }

        /// <summary>
        /// Install updates
        /// </summary>
        public void InstallUpdates()
        {
            if (latestReleaseData == null)
            {
                UpdateTaskFinished.Invoke(this, new UpdateTaskFinishedEventArgs(true, null));
            }
            else
            {
                try
                {
                    Uri uri = new Uri(URI);
                    WebClient wc = new WebClient();
                    wc.DownloadProgressChanged += OnDownloadProgressChanged;
                    wc.DownloadFileCompleted += OnDownloadFileCompleted;
                    if (!(Directory.Exists("./updates")))
                    {
                        Directory.CreateDirectory("./updates");
                    }
                    downloadPath = "./updates/" + Path.GetFileName(uri.LocalPath);
                    wc.DownloadFileAsync(uri, downloadPath);
                }
                catch (Exception e)
                {
                    UpdateTaskFinished.Invoke(this, new UpdateTaskFinishedEventArgs(true, e.Message));
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
        /// On download file completed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Asynchronous operation event arguments</param>
        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            bool is_canceled = e.Cancelled;
            string error = ((e.Error == null) ? null : e.Error.Message);
            if ((!is_canceled) && (error == null))
            {
                try
                {
                    // Backup old files
                    if (!(Directory.Exists("./backups")))
                    {
                        Directory.CreateDirectory("./backups");
                    }
                    string backup_file_name = "./backups/pre." + Version + ".zip";
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

                        // Move or unzip updates
                        try
                        {
                            if (downloadPath.ToLower().EndsWith(".zip"))
                            {
                                using (ZipArchive zip_archive = ZipFile.Open(downloadPath, ZipArchiveMode.Read))
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
                                                Console.Error.WriteLine(ex.Message);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string dest_file_path = "./" + Path.GetFileName(downloadPath);
                                if (File.Exists(dest_file_path))
                                {
                                    File.Delete(dest_file_path);
                                }
                                File.Copy(downloadPath, dest_file_path);
                            }

                            // Write last data
                            try
                            {
                                if (File.Exists(lastUpdateDataPath))
                                {
                                    File.Delete(lastUpdateDataPath);
                                }
                                using (FileStream file_stream = File.Open(lastUpdateDataPath, FileMode.Create))
                                {
                                    serializer.WriteObject(file_stream, latestReleaseData);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine(ex.Message);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Revert changes
                            Console.Error.WriteLine(ex.Message);
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
                                                    Console.Error.WriteLine(exc.Message);
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
                catch (Exception ex)
                {
                    is_canceled = true;
                    error = ex.Message;
                }
            }
            UpdateTaskFinished.Invoke(this, new UpdateTaskFinishedEventArgs(is_canceled, error));
        }
    }
}
