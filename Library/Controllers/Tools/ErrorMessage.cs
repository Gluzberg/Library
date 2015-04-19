using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers.Tools
{
    public class ErrorMessage
    {
        public static void Set(Controller controller)
        {
            controller.TempData[ERROR_DATA_LABEL] = true;
        }

        public static void Set(Controller controller, String message)
        {
            controller.TempData[ERROR_DATA_LABEL] = message;
        }

        

        public static string ERROR_DATA_LABEL = "Error";
    }
}