using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationGlacier.Models
{
    public class MapModel
    {
        public string side { get; set; }
        public string date_str { get; set; }
        public List<List<CommentModel>> comments { get; set; }
    }
}