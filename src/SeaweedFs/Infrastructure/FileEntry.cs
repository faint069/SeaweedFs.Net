// ***********************************************************************
// Assembly         : SeaweedFs
// Author           : piechpatrick
// Created          : 10-11-2021
//
// Last Modified By : piechpatrick
// Last Modified On : 10-11-2021
// ***********************************************************************
using System;
using System.IO;

namespace SeaweedFs.Infrastructure
{
    /// <summary>
    /// Class FileEntry.
    /// </summary>
    public class FileEntry
    {
        /// <summary>
        /// Gets or sets the crtime.
        /// </summary>
        /// <value>The crtime.</value>
        public DateTime Crtime { get; set; }
        /// <summary>
        /// Gets or sets the mtime.
        /// </summary>
        /// <value>The mtime.</value>
        public DateTime Mtime { get; set; }
        /// <summary>
        /// Gets or sets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath { get; set; }
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public uint Mode { get; set; }
        public long Uid { get; set; }
        public long Gid { get; set; }
        public string Mime { get; set; }
        public string Replication { get; set; }
        public string Collection { get; set; }
        public int TtlSec { get; set; }
        public string DiskType { get; set; }
        public string UserName { get; set; }
        public object GroupNames { get; set; }
        public string SymlinkTarget { get; set; }
        public string Md5 { get; set; }
        public int FileSize { get; set; }
        public FileExtendedInfo Extended { get; set; }
        public FileChunkInfo[] chunks { get; set; }
        public int HardLinkCounter { get; set; }
        //public object HardLinkId { get; set; }
        //public object Content { get; set; }
        //public object Remote { get; set; }

        ///// <summary>
        ///// Returns a <see cref="System.String" /> that represents this instance.
        ///// </summary>
        ///// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        //public override string ToString()
        //{
        //    return Path.GetFileName(FullPath);
        //}
    }

}