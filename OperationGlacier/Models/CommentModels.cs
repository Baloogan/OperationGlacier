using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OperationGlacier.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string Username { get; set; }
        public DateTime date_in_game{get;set;}
        public DateTime date_in_world { get; set; }
        public string unit_timeline_id { get; set; }
        public string unit_side_str { get; set; }
        public string message { get; set; }
        public int ReplyToCommentID { get; set; }
        public string side_restriction { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public string unit_name { get; set; }
        public string unit_location { get; set; }
        public string unit_report_first_line { get; set; }
    }
}