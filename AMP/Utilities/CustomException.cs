using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AMP.Utilities
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException()
            : base() { }

        public BusinessLogicException(string ErrorMessage)
            : base(ErrorMessage) { }
    }


}