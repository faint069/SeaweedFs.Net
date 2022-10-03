﻿// ***********************************************************************
// Assembly         : SeaweedFs.Filer
// Author           : piechpatrick
// Created          : 10-11-2021
//
// Last Modified By : piechpatrick
// Last Modified On : 10-11-2021
// ***********************************************************************

using System.Net.Http;
using System.Threading.Tasks;
using SeaweedFs.Filer.Internals.Operations.Abstractions;
using SeaweedFs.Operations;

namespace SeaweedFs.Filer.Internals.Operations.Outbound
{
    /// <summary>
    ///     Class DeleteAllOperation.
    ///     Implements the <see cref="SeaweedFs.Operations.OperationBase" />
    ///     Implements the <see cref="SeaweedFs.Filer.Internals.Operations.Abstractions.IFilerOperation{System.Boolean}" />
    /// </summary>
    /// <seealso cref="SeaweedFs.Operations.OperationBase" />
    /// <seealso cref="SeaweedFs.Filer.Internals.Operations.Abstractions.IFilerOperation{System.Boolean}" />
    internal class DeleteCatalogOperation : OperationBase, IFilerOperation<bool>
    {
        /// <summary>
        ///     The path
        /// </summary>
        private readonly string _path;

        private readonly bool _recursive;


        /// <summary>
        ///     Initializes a new instance of the <see cref="DeleteOperation" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="recursive">Delete recursively</param>
        internal DeleteCatalogOperation(string path, bool recursive)
        {
            _path      = path;
            _recursive = recursive;
        }

        /// <summary>
        ///     Executes the specified filerClient.
        /// </summary>
        /// <param name="filerClient">The filerClient.</param>
        /// <returns>Task&lt;TResult&gt;.</returns>
        async Task<bool> IFilerOperation<bool>.Execute(IFilerClient filerClient)
        {
            var response = await filerClient.SendAsync(BuildRequest(), HttpCompletionOption.ResponseContentRead,
                _cancellationToken);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        ///     Builds the request.
        /// </summary>
        /// <returns>HttpRequestMessage.</returns>
        protected virtual HttpRequestMessage BuildRequest() =>
            HttpRequestBuilder.WithRelativeUrl($"{_path}")
                              .WithHeader( "recursive", "true" )
                              .WithMethod(HttpMethod.Delete)
                              .WithParameter( "recursive", _recursive )
                              .Build();
    }
}