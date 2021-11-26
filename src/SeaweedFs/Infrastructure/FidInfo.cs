// ***********************************************************************
// Assembly         : SeaweedFs
// Author           : piechpatrick
// Created          : 10-11-2021
//
// Last Modified By : piechpatrick
// Last Modified On : 10-11-2021
// ***********************************************************************

namespace SeaweedFs.Infrastructure
{
    public class FidInfo
    {
        public int volume_id { get; set; }
        public int file_key { get; set; }
        public long cookie { get; set; }
    }

}