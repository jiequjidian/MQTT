using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace serverForReadSQL
{
    
        public class BLL
        {
            public static string table(string userName)
            {
                return DAL.fieldInquire(null, null, userName, null);
            }

            public static bool addAccount(string userStr, string pwdStr)
            {
                return DAL.addUser(userStr, pwdStr);
            }

        }
}