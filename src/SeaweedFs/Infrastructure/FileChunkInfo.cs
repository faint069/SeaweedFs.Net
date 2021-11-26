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
    public class FileChunkInfo
    {
        public string file_id { get; set; }
        public int size { get; set; }
        public long mtime { get; set; }
        public string e_tag { get; set; }
        public FidInfo fid { get; set; }
        public bool is_compressed { get; set; }
        public int offset { get; set; }
    }

}