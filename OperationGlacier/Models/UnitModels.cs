using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationGlacier.Models
{
    public class UnitModel
    {
        public string timeline_id { get; set; }
        public List<CommentModel> comments { get; set; }
    }
}