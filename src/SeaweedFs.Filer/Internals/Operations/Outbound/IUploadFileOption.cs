// ***********************************************************************
// Assembly         : SeaweedFs.Filer
// Author           : piechpatrick
// Created          : 10-09-2021
//
// Last Modified By : piechpatrick
// Last Modified On : 10-11-2021
// ***********************************************************************


namespace SeaweedFs.Filer.Internals.Operations.Outbound
{
    public interface IUploadFileOption
    {
        string collection { get; set; }
        string dataCenter { get; set; }
        string maxMB { get; set; }
        string mode { get; set; }
        string op { get; set; }
        string rack { get; set; }
        string replication { get; set; }
        Ttl ttl { get; set; }
    }
}