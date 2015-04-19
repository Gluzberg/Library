using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Models;

namespace Library.Controllers
{
    public class StateController : Controller
    {
        //
        // GET: /State/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View(new State());
        }

    }
}
