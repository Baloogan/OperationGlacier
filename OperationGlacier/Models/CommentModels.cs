using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

using System.Text.RegularExpressions;
namespace OperationGlacier.Models
{
    public class Comment
    {


        public int CommentID { get; set; }


        public string Username { get; set; }


        public DateTime date_in_game { get; set; }


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

        public string game_name { get; set; }

    }
    public class CommentModel
    {
        public int x { get; set; }
        public int y { get; set; }
        public string message { get; set; }
        public string Username { get; set; }
        public string date_str { get; set; }
        public string unit_timeline_id { get; set; }
        public string side_restriction { get; set; }
        public string unit_name { get; set; }
        public string game_name { get; set; }
        public int CommentID { get; set; }
        public CommentModel(Comment comment)
        {
            this.CommentID = comment.CommentID;
            this.x = comment.x;
            this.y = comment.y;
            this.message = comment.message;
            this.date_str = WitpUtility.to_date_str(comment.date_in_game);
            this.unit_timeline_id = comment.unit_timeline_id;
            this.unit_name = comment.unit_name;
            this.Username = comment.Username;
            this.side_restriction = comment.side_restriction;
            this.game_name = comment.game_name;
            this.render_message();
        }
        private void render_message()
        {
            if (this.message != null)
            {

                //get_timeline_reverse_index
                var myRegex = new Regex(@"\[(.+?)\]");

                string text = this.message;
                var matches = myRegex.Matches(text);
                var already_done = new List<string>();
                foreach (Match match in matches)
                {
                    if (already_done.Contains(match.Value))
                    {
                        continue;
                    }
                    string a = match.Value.Substring(1, match.Value.Length - 2);
                    //text = text + " | " + match.Value + ",'" + a +"'";

                    if (GameState.get_timeline_reverse_index(game_name).ContainsKey(a))
                    {
                        string timeline_id = GameState.get_timeline_reverse_index(game_name)[a];
                        string result = match.Value + "(http://operationglacier.baloogancampaign.com/Unit?tid=" + timeline_id + "&game_name=" + game_name + ")";
                        already_done.Add(match.Value);
                        text = text.Replace(match.Value, result);
                    }
                }
                this.message = text;
            }
        }
    }
}