using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Updater .NET namespace
/// </summary>
namespace UpdaterNET
{
    /// <summary>
    /// Update data contract class
    /// </summary>
    [DataContract]
    internal class UpdateDataContract
    {
        /// <summary>
        /// Version string
        /// </summary>
        [DataMember(Name = "version")]
        public string version = "";

        /// <summary>
        /// Version number
        /// </summary>
        [DataMember(Name = "version_number")]
        public uint versionNumber = 0U;

        /// <summary>
        /// URI
        /// </summary>
        [DataMember(Name = "uri")]
        public string uri = "";

        /// <summary>
        /// SHA512
        /// </summary>
        [DataMember(Name = "sha512")]
        public string sha512 = "";
    }
}
