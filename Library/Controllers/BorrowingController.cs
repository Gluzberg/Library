using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repository;
using Library.Models;

namespace Library.Controllers
{
    public class BorrowingController : Controller
    {
        IRepository<Borrowing> BorrowingsRepository = new MemoryRepository<Borrowing>();
           // = new XmlRepository<Borrowing>("C:\\Users\\Manya\\Documents\\Visual Studio 2013\\Projects\\Library\\Borrowings.xml\\borrowings");


        //
        // GET: /Borrowing/DetailsPerPerson/5

        public ActionResult DetailsPerPerson(String id)
        {
            return View(BorrowingsRepository.SelectAll().Select(b => b.person_id == id));
        }

        //
        // GET: /Borrowing/DetailsPerBook/5

        public ActionResult DetailsPerBook(String id)
        {
            return View(BorrowingsRepository.SelectAll().Select(b => b.book_id == id));
        }

        //
        // GET: /Borrowing/

        public ActionResult Index()
        {
            return View(BorrowingsRepository.SelectAll());
        }

        //
        // GET: /Borrowing/Details/5

        public ActionResult Details(String id)
        {
            return View(BorrowingsRepository.SelectByID(id));
        }

        //
        // GET: /Borrowing/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Borrowing/Create

        [HttpPost]
        public ActionResult Create(Borrowing Borrowing)
        {
            if (ModelState.IsValid)
            {
                BorrowingsRepository.Insert(Borrowing);
                return RedirectToAction("Index");
            }


            return View(Borrowing);
        }


        //
        // GET: /Borrowing/CreatePerBook

        public ActionResult CreatePerBook()
        {
            return View();
        }

        //
        // POST: /Borrowing/CreatePerBook

        [HttpPost]
        public ActionResult CreatePerBook(String book_id, String person_id)
        {



            return View();
        }

        //
        // GET: /Borrowing/Edit/5

        public ActionResult Edit(String id)
        {
            Borrowing Borrowing = BorrowingsRepository.SelectByID(id);

            if (Borrowing == null)
            {
                return HttpNotFound();
            }

            return View(Borrowing);
        }

        //
        // POST: /Borrowing/Edit/5

        [HttpPost]
        public ActionResult Edit(Borrowing Borrowing)
        {
            if (ModelState.IsValid)
            {
                BorrowingsRepository.Update(Borrowing);
                return RedirectToAction("Index");
            }

            return View(Borrowing);
        }

        //
        // GET: /Borrowing/Delete/5

        public ActionResult Delete(String id)
        {
            return Details(id);
        }

        //
        // POST: /Borrowing/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            BorrowingsRepository.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            BorrowingsRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
