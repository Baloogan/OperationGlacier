using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

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

        [DataType(DataType.MultilineText)]
        public string message { get; set; }

        
        public int ReplyToCommentID { get; set; }

        
        public string side_restriction { get; set; }

        
        public int x { get; set; }

        
        public int y { get; set; }

        
        public string unit_name { get; set; }

        
        public string unit_location { get; set; }

        
        public string unit_report_first_line { get; set; }
    }
    public class CommentModel
    {
        public int x { get; set; }
        public int y { get; set; }
        public string message { get; set; }
        public string Username { get; set; }
        public string date_str { get; set; }
        public string unit_timeline_id { get; set; }
        public string unit_name { get; set; }
        public CommentModel(Comment comment)
        {
            this.x = comment.x;
            this.y = comment.y;
            this.message = comment.message;
            this.date_str = WitpUtility.to_date_str(comment.date_in_game);
            this.unit_timeline_id = comment.unit_timeline_id;
            this.unit_name = comment.unit_name;
            this.Username = comment.Username;
        }
    }
}