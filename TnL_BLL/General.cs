using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TnL_DAL;
using System.Data;


namespace TnL_BLL
{

  public  class General
  {
        //0- all good
        //1- pass isnt good
        //2- user doesnt exist
        public static int Login(string pass, string username)
        {
            if (!TnL_DAL.UserTBL.IsExist(username))
            {
                return 2;
            }
            if (!TnL_DAL.UserTBL.TruePass(pass, username))
            {
                return 1;
            }
            return 0;
        }


  }
}
