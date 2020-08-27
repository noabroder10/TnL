using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace TnL_DAL
{
     public class UserTBL
    {
        public static DataTable GetUserByUserName(string username)
        {
            return DBHelper.GetDataTable("SELECT * FROM UserTBL WHERE [Username]='" + username + "'");
        }

        public static bool IsExist(string username)
        {
            return (DBHelper.GetDataTable($"SELECT * FROM UserTBL WHERE [Username]='{username}'").Rows.Count != 0);
        }

        public static bool TruePass(string Pass, string username)
        {
            return (DBHelper.GetDataTable($"SELECT * FROM UserTBL WHERE [Username]='{username}' AND [Pass]='{Pass}'").Rows.Count != 0);
        }
    }
}
