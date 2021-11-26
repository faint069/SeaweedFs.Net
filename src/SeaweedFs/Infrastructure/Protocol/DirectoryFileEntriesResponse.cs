// ***********************************************************************
// Assembly         : SeaweedFs
// Author           : piechpatrick
// Created          : 10-11-2021
//
// Last Modified By : piechpatrick
// Last Modified On : 10-11-2021
// ***********************************************************************
using System.Collections.Generic;
using System.Net;

namespace SeaweedFs.Infrastructure.Protocol
{
    /// <summary>
    /// Class DirectoryFileEntriesResponse.
    /// </summary>
    public class DirectoryFileEntriesResponse
    {
        public DirectoryFileEntriesResponse()
        {
            StatusCode = HttpStatusCode.OK;
        }
        public DirectoryFileEntriesResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }
        public int Limit { get; set; }
        public string LastFileName { get; set; }
        public bool ShouldDisplayLoadMore { get; set; }
        /// <summary>
        /// Gets or sets the entries.
        /// </summary>
        /// <value>The entries.</value>
        public List<FileEntry> Entries { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        public bool Success => StatusCode == HttpStatusCode.OK;
    }
}