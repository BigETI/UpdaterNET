using System;
using System.Net;

/// <summary>
/// Updater .NET namespace
/// </summary>
namespace UpdaterNET
{
    /// <summary>
    /// Web client extended
    /// </summary>
    public class WebClientEx : WebClient
    {
        /// <summary>
        /// Timeout
        /// </summary>
        private int timeout;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="timeout">Timeout</param>
        public WebClientEx(int timeout)
        {
            this.timeout = timeout;
        }

        /// <summary>
        /// Get web request
        /// </summary>
        /// <param name="uri">URI</param>
        /// <returns>Web request</returns>
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = timeout;
            return w;
        }
    }
}
