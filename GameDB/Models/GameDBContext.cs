using MySql.Data.MySqlClient;

namespace GameDB.Models
{
    public class GameDBContext
    {
 
        private static string User { get { return "mayureshnaidu"; } }
        private static string Password { get { return "mydatabase"; } }
        private static string Database { get { return "gamedb"; } }
        private static string Server { get { return "localhost"; } }
        private static string Port { get { return "3306"; } }

        protected static string ConnectionString
        {
            get
            {
                return "server=" + Server
                    + ";user=" + User
                    + ";database=" + Database
                    + ";port=" + Port
                    + ";password=" + Password
                    + ";convert zero datetime=True";
            }
        }

        public MySqlConnection AccessDatabase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}