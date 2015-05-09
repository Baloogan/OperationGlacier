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
        public class Game
        {
            public List<string> date_strs { get; set; }
        }
        private static Dictionary<string, Game> game_name_to_game = new Dictionary<string,Game>();
        private static void read_game_json(string game_name)
        {
            if (game_name_to_game.ContainsKey(game_name))
            {
                return;
            }
            const string game_data_root = @"C:\inetpub\GameData";
            string game_filename = Path.Combine(game_data_root, game_name, "Game.json");

            Game game = Newtonsoft.Json.JsonConvert.DeserializeObject<Game>(File.ReadAllText(game_filename));
            game_name_to_game[game_name] = game;

        }
        public static string LatestTurn(string game_name)
        {
            game_name = get_game_name(game_name);
            read_game_json(game_name);

            return game_name_to_game[game_name].date_strs.Last();
        }

        public static string get_game_name(string game_name)
        {
            if (game_name == null)
                return "OperationGlacierI";
            if (game_name == "OperationGlacierI")
                return "OperationGlacierI";
            throw new Exception();
        }
    }
}