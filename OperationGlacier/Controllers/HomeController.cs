using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationGlacier.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string game_name)
        {
            
                game_name = GameState.get_game_name(game_name);
            ViewBag.game_name = game_name;
            return View("Index_"+game_name);
        }

        public ActionResult About(string game_name)
        {

            game_name = GameState.get_game_name(game_name);
            
            ViewBag.game_name = game_name;
            //ViewBag.Message = "Your application description page.";

            return View("About_" + game_name);
        }

    }
}