using System.Data.SqlClient;

namespace SQL_Server
{
    public class sqlcon
    {
        public static SqlConnection conn { get; set; }
        public static SqlCommand cmd { get; set; }
        public static SqlDataReader dr { get; set; }
        public static string cmdText { get; set; }
    }
}
