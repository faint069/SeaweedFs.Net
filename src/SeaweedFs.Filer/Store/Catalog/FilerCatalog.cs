﻿// ***********************************************************************
// Assembly         : SeaweedFs.Filer
// Author           : piechpatrick
// Created          : 10-10-2021
//
// Last Modified By : piechpatrick
// Last Modified On : 10-11-2021
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SeaweedFs.Filer.Internals.Operations;
using SeaweedFs.Filer.Internals.Operations.Inbound;
using SeaweedFs.Filer.Internals.Operations.Outbound;
using SeaweedFs.Infrastructure.Protocol;
using SeaweedFs.Store;

namespace SeaweedFs.Filer.Store.Catalog
{
    /// <summary>
    ///     Class FilerCatalog. This class cannot be inherited.
    ///     Implements the <see cref="SeaweedFs.Filer.Store.Catalog.IFilerCatalog" />
    /// </summary>
    /// <seealso cref="SeaweedFs.Filer.Store.Catalog.IFilerCatalog" />
    internal sealed class FilerCatalog : IFilerCatalog
    {
        /// <summary>
        ///     The executor
        /// </summary>
        private readonly IFilerOperationsExecutor _executor;

        /// <summary>
        ///     The filer store
        /// </summary>
        private readonly IFilerStore _filerStore;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilerCatalog" /> class.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filerStore">The filer store.</param>
        /// <param name="executor">The executor.</param>
        /// <exception cref="System.ArgumentException">directory</exception>
        internal FilerCatalog(string directory, IFilerStore filerStore, IFilerOperationsExecutor executor)
        {
            if (!directory.EndsWith("/")) directory += "/";
            if (!string.IsNullOrEmpty(Path.GetFileName(directory))) throw new ArgumentException(nameof(directory));
            Directory = Path.GetDirectoryName(directory) ?? string.Empty;
            _filerStore = filerStore;
            _executor = executor;
        }

        /// <summary>
        ///     Gets the directory.
        /// </summary>
        /// <value>The directory.</value>
        public string Directory { get; }

        /// <summary>
        ///     Deletes the asynchronous.
        /// </summary>
        /// <param name="blobInfo">The BLOB information.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        public Task<bool> DeleteAsync(BlobInfo blobInfo)
        {
            var operation = new DeleteOperation(Path.Combine(Directory, blobInfo.Name));
            return _executor.Execute(operation);
        }

        /// <summary>
        ///     Lists this instance.
        /// </summary>
        /// <param name="lastFileName">用作游标的文件名，可以用来翻页（下一页）</param>
        /// <param name="namePattern">文件名通配符 match file names, case-sensitive wildcard characters '*' and '?'</param>
        /// <param name="namePatternExclude">用于排除的文件名通配符 nagetive match file names, case-sensitive wildcard characters '*' and '?'</param>
        /// <param name="limit">limit</param>
        /// <returns>Task&lt;IEnumerable&lt;BlobInfo&gt;&gt;.</returns>
        public Task<DirectoryFileEntriesResponse> ListAsync(string lastFileName = null, string namePattern = null, string namePatternExclude = null, int limit = 100)
        {
            var operation = new ListFilesOperation(
                path: Directory,
                lastFileName: lastFileName,
                namePattern: namePattern,
                namePatternExclude: namePatternExclude,
                limit: limit,
                pretty: false
                );
            return _executor.Execute(operation);
        }
        ///// <summary>
        /////     Lists this instance.
        ///// </summary>
        ///// <param name="lastFileName">用作游标的文件名，可以用来翻页（下一页）</param>
        ///// <param name="namePattern">文件名通配符 match file names, case-sensitive wildcard characters '*' and '?'</param>
        ///// <param name="namePatternExclude">用于排除的文件名通配符 nagetive match file names, case-sensitive wildcard characters '*' and '?'</param>
        ///// <param name="limit">limit</param>
        ///// <returns>Task&lt;IEnumerable&lt;BlobInfo&gt;&gt;.</returns>
        //public Task<List<BlobInfo>> ListAsync(string lastFileName = null, string namePattern = null, string namePatternExclude = null, int limit = 100)
        //{
        //    var operation = new ListFilesOperation(
        //        path: Directory,
        //        lastFileName: lastFileName,
        //        namePattern: namePattern,
        //        namePatternExclude: namePatternExclude,
        //        limit: limit,
        //        pretty: false
        //        );
        //    return _executor.Execute(operation);
        //}
        /// <summary>
        ///     Gets the specified BLOB information.
        /// </summary>
        /// <param name="blobInfo">The BLOB information.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress">The progress.</param>
        /// <returns>Blob.</returns>
        public Task<Blob> GetAsync(BlobInfo blobInfo, CancellationToken cancellationToken = default,
            IProgress<int> progress = null) =>
            GetAsync(blobInfo.Name, cancellationToken, progress);

        /// <summary>
        ///     Gets the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress">The progress.</param>
        /// <returns>Blob.</returns>
        public async Task<Blob> GetAsync(string fileName, CancellationToken cancellationToken = default,
            IProgress<int> progress = null)
        {
            var operation = new GetFileStreamOperation(Path.Combine(Directory, Path.GetFileName(fileName)),
                cancellationToken, progress);
            var response = await _executor.Execute(operation);
            var blobInfo = new BlobInfo(fileName);
            foreach (var header in response.Item1.Headers)
                blobInfo.Headers.Add(header.Key, header.Value);
            return new Blob(blobInfo, response.Item2);
        }

        /// <summary>
        ///     Uploads the file.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress">The progress.</param>
        /// <returns>Task&lt;HttpResponseMessage&gt;.</returns>
        public async Task<bool> PushAsync(Blob blob, CancellationToken cancellationToken = default,
            IProgress<int> progress = null, IUploadFileOption uploadFileOption = null)
        {
#if NET5_0_OR_GREATER
            await
#endif
                using var operation = new UploadFileOutboundStreamOperation(
                Path.Combine(Directory, blob.BlobInfo.Name) + "?" + getQueryString(uploadFileOption), blob.BlobInfo, blob.Content, cancellationToken, progress);
            return await _executor.Execute(operation);
        }

        private string getQueryString(IUploadFileOption uploadFileOption)
        {
            if (uploadFileOption == null)
                return string.Empty;
            var paramList = new List<(string key, string value)>() {
                ("collection",uploadFileOption.collection?.ToString()),
                ("dataCenter",uploadFileOption.dataCenter?.ToString()),
                ("maxMB",uploadFileOption.maxMB?.ToString()),
                ("mode",uploadFileOption.mode?.ToString()),
                ("op",uploadFileOption.op?.ToString()),
                ("rack",uploadFileOption.rack?.ToString()),
                ("replication",uploadFileOption.replication?.ToString()),
                ("ttl",uploadFileOption.ttl?.ToString()),
            }.Where(x => !string.IsNullOrEmpty(x.value))
            .Select(x => $"{x.key}={Uri.EscapeDataString(x.value)}")
            .ToList();
            return string.Join("&", paramList);
        }
    }
}