// ***********************************************************************
// Assembly         : SeaweedFs.Filer
// Author           : piechpatrick
// Created          : 10-10-2021
//
// Last Modified By : piechpatrick
// Last Modified On : 10-11-2021
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SeaweedFs.Filer.Internals.Operations.Outbound;
using SeaweedFs.Infrastructure.Protocol;
using SeaweedFs.Store;

namespace SeaweedFs.Filer.Store.Catalog
{
    /// <summary>
    ///     Interface IFilerCatalog
    /// </summary>
    public interface IFilerCatalog
    {
        /// <summary>
        ///     Gets the directory.
        /// </summary>
        /// <value>The directory.</value>
        string Directory { get; }

        /// <summary>
        ///     Uploads the file.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <param name="progress">The progress.</param>
        /// <param name="progress">uploadFileOption</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        Task<bool> PushAsync(Blob blob, CancellationToken cancellationToken = default, IProgress<int> progress = null, IUploadFileOption uploadFileOption = null);

        /// <summary>
        ///     Gets the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="progress">The progress.</param>
        /// <returns>Task&lt;Blob&gt;.</returns>
        Task<Blob> GetAsync(string fileName, CancellationToken cancellationToken = default,
            IProgress<int> progress = null);

        /// <summary>
        ///     Gets the asynchronous.
        /// </summary>
        /// <param name="blobInfo">The BLOB information.</param>
        /// <param name="progress">The progress.</param>
        /// <returns>Task&lt;Blob&gt;.</returns>
        Task<Blob> GetAsync(BlobInfo blobInfo, CancellationToken cancellationToken = default,
            IProgress<int> progress = null);

        /// <summary>
        ///     Deletes the asynchronous.
        /// </summary>
        /// <param name="blobInfo">The BLOB information.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        Task<bool> DeleteAsync(BlobInfo blobInfo);
        /// <summary>
        ///     Lists this instance.
        /// </summary>
        /// <param name="lastFileName">用作游标的文件名，可以用来翻页（下一页）</param>
        /// <param name="namePattern">文件名通配符 match file names, case-sensitive wildcard characters '*' and '?'</param>
        /// <param name="namePatternExclude">用于排除的文件名通配符 nagetive match file names, case-sensitive wildcard characters '*' and '?'</param>
        /// <returns>Task&lt;IEnumerable&lt;BlobInfo&gt;&gt;.</returns>
        Task<DirectoryFileEntriesResponse> ListAsync(string lastFileName = null, string namePattern = null, string namePatternExclude = null, int limit = 100);
    }
}