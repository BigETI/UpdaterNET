using System;

/// <summary>
/// Updater .NET namespace
/// </summary>
namespace UpdaterNET
{
    /// <summary>
    /// Update task finished event arguments
    /// </summary>
    public class UpdateTaskFinishedEventArgs : EventArgs
    {
        /// <summary>
        /// Is canceled
        /// </summary>
        private readonly bool isCanceled;

        /// <summary>
        /// Error
        /// </summary>
        private readonly string error;

        /// <summary>
        /// Is canceled
        /// </summary>
        public bool IsCanceled
        {
            get
            {
                return isCanceled;
            }
        }

        /// <summary>
        /// Error
        /// </summary>
        public string Error
        {
            get
            {
                return ((error == null) ? "" : error);
            }
        }

        /// <summary>
        /// Is error
        /// </summary>
        public bool IsError
        {
            get
            {
                return (error != null);
            }
        }

        /// <summary>
        /// Update task finished event arguments
        /// </summary>
        /// <param name="isCanceled">Is canceled</param>
        public UpdateTaskFinishedEventArgs(bool isCanceled)
        {
            this.isCanceled = isCanceled;
            error = null;
        }

        /// <summary>
        /// Update task finished event arguments
        /// </summary>
        /// <param name="isCanceled">Is canceled</param>
        /// <param name="error">Error</param>
        public UpdateTaskFinishedEventArgs(bool isCanceled, string error)
        {
            this.isCanceled = isCanceled;
            this.error = error;
        }
    }
}
