using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpClient.Api.Models
{
    public class BranchModel
    {
        public string name { get; set; }
        public CommitModel commit { get; set; }
        public bool @protected { get; set; }
    }

    public class CommitModel
    {
        public string sha { get; set; }
        public string url { get; set; }
    }
}
