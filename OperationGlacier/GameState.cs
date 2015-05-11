using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace OperationGlacier
{

    public class GameState
    {

        public class TimelineIndex
        {
            public Dictionary<string, string> timeline_id_to_name;
        }
        public static Dictionary<string, TimelineIndex> timeline_indexes = new Dictionary<string, TimelineIndex>();
        public static Dictionary<string, Dictionary<string, string>> timeline_reverse_indexes = new Dictionary<string, Dictionary<string, string>>();
        public static TimelineIndex get_timeline_index(string game_name)
        {
            game_name = get_game_name(game_name);
            if (timeline_indexes.ContainsKey(game_name))
            {
                return timeline_indexes[game_name];
            }

            const string game_data_root = @"C:\inetpub\GameData";
            string filename = Path.Combine(game_data_root, game_name, "TimelineIndex.json");
            TimelineIndex t = Newtonsoft.Json.JsonConvert.DeserializeObject<TimelineIndex>(File.ReadAllText(filename));
            timeline_indexes[game_name] = t;

            return t;
        }
        public static Dictionary<string, string> get_timeline_reverse_index(string game_name)
        {
            //var t = .ToDictionary(f => f.Value, f => f.Key);
            var t = new Dictionary<string, string>();
            foreach (var a in get_timeline_index(game_name).timeline_id_to_name)
            {
                if (!t.ContainsKey(a.Value))
                {
                    t[a.Value] = a.Key;
                }
            }
            timeline_reverse_indexes[game_name] = t;
            return t;
        }
        public static string get_name_from_timeline_id(string game_name, string timeline_id)
        {
            return get_timeline_index(game_name).timeline_id_to_name[timeline_id];
        }
        public class Game
        {
            public List<string> date_strs { get; set; }
        }
        private static Dictionary<string, Game> game_name_to_game = new Dictionary<string, Game>();
        public static IEnumerable<string> get_date_strs(string game_name)
        {
            return get_game_json(game_name).date_strs.OrderBy(s => s);
        }
        public static IEnumerable<string> get_date_strs_reverse(string game_name)
        {
            return get_game_json(game_name).date_strs.OrderByDescending(s => s);
        }
        private static Game get_game_json(string game_name)
        {
            game_name = get_game_name(game_name);
            if (game_name_to_game.ContainsKey(game_name))
            {
                return game_name_to_game[game_name];
            }

            const string game_data_root = @"C:\inetpub\GameData";
            string game_filename = Path.Combine(game_data_root, game_name, "Game.json");
            Game game = Newtonsoft.Json.JsonConvert.DeserializeObject<Game>(File.ReadAllText(game_filename));
            game_name_to_game[game_name] = game;

            return game;
        }
        public static string LatestTurn(string game_name)
        {
            return get_date_strs_reverse(game_name).First();
        }

        public static string get_game_name(string game_name)
        {
            if (game_name == null)
                return "OperationGlacierI";
            if (game_name == "")
                return "OperationGlacierI";
            if (game_name == "OperationGlacierI")
                return "OperationGlacierI";
            throw new Exception();
        }
    }
}