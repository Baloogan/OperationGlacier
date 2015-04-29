using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

using System.Text.RegularExpressions;
namespace WITPJSON
{
    partial class Unit
    {
        public enum Type
        {
            //Aircraft,
            //AirframeProd,
            AirGroup,
            //AirLoss,
            Base,
            //Device,
            //Engine,
            LCU,
            //Leader,
            //Pilot,
            //ShipClass,
            //ShipUpgrade,
            Ship,
            //SunkShip,
            TF,
            //VP,

            SigInt,
            CombatEvent,
            AfterAction,
            OperationalReport
        }
        public Dictionary<string, string> row;
        public string name;
        public int id;
        public int x, y;
        public Type type;
        public string type_str { get { return type.ToString(); } }
        public string location;
        public int owner;
        public string report;

    }
}
