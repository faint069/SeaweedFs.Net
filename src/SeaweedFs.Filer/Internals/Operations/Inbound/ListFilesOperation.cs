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
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SeaweedFs.Filer.Internals.Operations.Abstractions;
using SeaweedFs.Infrastructure.Protocol;
using SeaweedFs.Operations;
using SeaweedFs.Store;

namespace SeaweedFs.Filer.Internals.Operations.Inbound
{
    /// <summary>
    /// Class ListFilesOperation.
    /// Implements the <see cref="SeaweedFs.Operations.OperationBase" />
    /// Implements the <see cref="BlobInfo" />
    /// </summary>
    /// <seealso cref="SeaweedFs.Operations.OperationBase" />
    /// <seealso cref="BlobInfo" />
    internal class ListFilesOperation : OperationBase, IFilerOperation<DirectoryFileEntriesResponse>
    {
        /// <summary>
        /// The path
        /// </summary>
        private readonly string _path;

        private readonly string _lastFileName = null;
        private readonly string _namePattern = null;
        private readonly string _namePatternExclude = null;
        private readonly int _limit;
        private readonly bool _pretty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListFilesOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="pretty">if set to <c>true</c> [pretty].</param>
        internal ListFilesOperation(string path, string lastFileName = null, string namePattern = null, string namePatternExclude = null, int limit = 100, bool pretty = false)
        {
            _pretty = pretty;
            _lastFileName = lastFileName;
            _namePattern = namePattern;
            _namePatternExclude = namePatternExclude;
            _limit = limit < 1 ? 10 : limit;

            var list = new List<(string paramName, string value)> {
                ("path",_path),
                ("pretty",_pretty?"true":"false"),
                ("lastFileName",_lastFileName),
                ("namePattern",_namePattern),
                ("namePatternExclude",_namePatternExclude),
                ( "limit",_limit.ToString()),
            };
            var queryString = string.Join("&",
                    list.Where(x => !string.IsNullOrEmpty(x.value))
                        .Select(x => $"{x.paramName}={Uri.EscapeDataString(x.value)}")
                        );
            _path = $"{path}?{queryString}";
        }
        async Task<DirectoryFileEntriesResponse> IFilerOperation<DirectoryFileEntriesResponse>.Execute(IFilerClient filerClient)
        {
            var response = await filerClient.SendAsync(BuildRequest(), HttpCompletionOption.ResponseContentRead);
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<DirectoryFileEntriesResponse>(await response.Content.ReadAsStringAsync());
            return new DirectoryFileEntriesResponse(response.StatusCode);
        }
        ///// <summary>
        ///// Executes the specified filerClient.
        ///// </summary>
        ///// <param name="filerClient">The filerClient.</param>
        ///// <returns>Task&lt;TResult&gt;.</returns>
        //async Task<List<BlobInfo>> IFilerOperation<List<BlobInfo>>.Execute(IFilerClient filerClient)
        //{
        //    var response = await ExecuteListAsync(filerClient);
        //    if (response?.Entries != null)
        //        return response.Entries
        //                   .Where(ex => Convert.ToString(ex.Mode, 8) == "660")
        //                   .Select(e => new BlobInfo(Path.GetFileName(e.FullPath)))
        //                   .ToList()
        //               ?? new List<BlobInfo>();
        //    return new List<BlobInfo>();
        //}
        /// <summary>
        /// Builds the request.
        /// </summary>
        /// <returns>HttpRequestMessage.</returns>
        protected virtual HttpRequestMessage BuildRequest()
        {
            return HttpRequestBuilder.WithRelativeUrl(_path)
                .WithMethod(HttpMethod.Get)
                .Build();
        }
    }
}