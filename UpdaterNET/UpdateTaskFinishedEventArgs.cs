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
        private bool isCanceled;

        /// <summary>
        /// Error
        /// </summary>
        private string error;

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
        public UpdateTaskFinishedEventArgs(bool isCanceled, string error = null) : base()
        {
            this.isCanceled = isCanceled;
            this.error = error;
        }
    }
}
