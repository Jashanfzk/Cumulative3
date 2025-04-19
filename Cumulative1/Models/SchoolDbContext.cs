using MySql.Data.MySqlClient;

namespace Cumulative1.Models
{
    public class SchoolDbContext
    {
        private static string user { get { return "jashan.gill"; } }
        private static string password { get { return "Jj@90413"; } }
        private static string database { get { return "School"; } }
        private static string server { get { return "localhost"; } }
        private static string port { get { return "3306"; } }
        // 
        protected static string ConnectionString
        {
            get
            {
                return "server=" + server 
                    + ";user=" + user
                    + ";database=" + database 
                    + ";port=" + port 
                    + ";password=" + password
                    + "; convert zero datetime = True";
            
            }
        }

        public MySqlConnection AccessDataBase()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
