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
    public class UploadFileOption : IUploadFileOption
    {
        /// <summary>
        /// data center
        /// </summary>
        public string dataCenter { get; set; }
        public string rack { get; set; }
        public string collection { get; set; }
        public string replication { get; set; }
        /// <summary>
        /// time to live, examples, 3m: 3 minutes, 4h: 4 hours, 5d: 5 days, 6w: 6 weeks, 7M: 7 months, 8y: 8 years
        /// </summary>
        public Ttl ttl { get; set; }
        public string maxMB { get; set; }
        /// <summary>
        /// filer服务端默认值 0660
        /// </summary>
        public string mode { get; set; }
        public string op { get; set; }
    }
}