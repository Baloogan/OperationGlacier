using LinqToDB.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.DataProvider.SQLite;
using LinqToDB.DataProvider.MySql;
using LinqToDB;
using LinqToDB.Mapping;

namespace AutoBaloogan
{

    public partial class baloogan_chatDB : LinqToDB.Data.DataConnection
    {
        private baloogan_chatDB(IDataProvider provider, string conn)
            : base(provider, conn)
        {

        }

        public static baloogan_chatDB connect()
        {
            return new baloogan_chatDB(new MySqlDataProvider(), System.IO.File.ReadAllText(@"C:\inetpub\config\autobaloogan.txt"));

        }
        public static void transmit(string channel, string message)
        {
            using (var autobaloogan_db = AutoBaloogan.baloogan_chatDB.connect())
            {
                var t = new AutoBaloogan.Transmit()
                {
                    Channel = channel,
                    Message = message
                };
                autobaloogan_db.Insert(t);
            }
        }
    }
    public partial class baloogan_chatDB : LinqToDB.Data.DataConnection
    {
        
        public ITable<Transmit> Transmits { get { return this.GetTable<Transmit>(); } }
        
    }

    [Table("Transmit")]
    public partial class Transmit
    {
        [PrimaryKey, Identity]
        public int ID { get; set; } // int(11)
        [Column, NotNull]
        public string Channel { get; set; } // text
        [Column, NotNull]
        public string Message { get; set; } // text
    }

    public static partial class TableExtensions
    {
    
        public static Transmit Find(this ITable<Transmit> table, int ID)
        {
            return table.FirstOrDefault(t =>
                t.ID == ID);
        }

    
    }
}
