using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Utilities
{
    public class IdentityUtilities
    {
        public static string getLoginName()
        {

            char[] SplitArray = new char[1];
            SplitArray[0] = '\\';

            string loginName = HttpContext.Current.User.Identity.Name;
            string[] logonName = loginName.Split(SplitArray);
            return logonName[1];

        }

    }
}