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
        public string timeline_id { get; set; }
        public string message { get; set; }
        public string side_restriction { get; set; }
        public int x { get; set; }
        public int y { get; set; }
    }
}