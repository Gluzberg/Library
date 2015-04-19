using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;
using Library.Models;
using Library.Services;
using System.Diagnostics;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        IBookService bookServive;

        public BookController(IBookService bookService)
        {
            this.bookServive = bookService;
        }

        //
        // GET: /Book/

        public ActionResult Index()
        {
            ModelState.AddModelError("Error", "Unable to..");
            return View(bookServive.SelectAll());
        }

        //
        // GET: /Book/Details/5

        public ActionResult Details(String id)
        {
            Book book = this.bookServive.SelectByID(id);
            ViewBag.Borrower = new SelectList(bookServive.SelectAllBorrowingPersons(), "id", "fullName", book.borrower);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        //
        // GET: /Book/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Book/Create

        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                if (bookServive.Insert(book))
                {
                    return RedirectToAction("Details", book);
                }
                else
                {
                    Tools.ErrorMessage.Set(this);
                }
            }

            
            return View(book);
        }

        //
        // GET: /Book/Edit/5

        public ActionResult Edit(String id)
        {
            Book book = bookServive.SelectByID(id);
           
            if (book == null)
            {
                return HttpNotFound();
            }
            
            return View(book);
        }

        //
        // POST: /Book/Edit/5

        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                if (bookServive.Update(book))
                {
                    return RedirectToAction("Details", book);
                }
                else
                {
                    Tools.ErrorMessage.Set(this);
                }
            }
            
            return View(book);
        }

        //
        // GET: /Book/Delete/5

        public ActionResult Delete(String id)
        {
            return Details(id);
        }

        //
        // POST: /Book/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            bookServive.Delete(id);
            return RedirectToAction("Index");            
        }


        //
        // GET: /Book/Return/5

        public ActionResult Return(String borrow_id, String book_id)
        {
            Book book = this.bookServive.SelectByID(book_id);
            
            if (this.bookServive.ReturnBorrowing(borrow_id))
            {
                return RedirectToAction("Details", book);
            }

            Tools.ErrorMessage.Set(this);
            return View(book);
        }



        protected override void Dispose(bool disposing)
        {
            bookServive.Dispose();
            base.Dispose(disposing);
        }

        //
        // GET: /Book/Borrow/5

        public ActionResult Borrow(String id)
        {
            Book book = bookServive.SelectByID(id);

            if (book == null)
            {
                return HttpNotFound();
            }

            ViewBag.Borrower = new SelectList(bookServive.SelectAllBorrowingPersons(), "id", "fullName", book.borrower);
            return View(book);
        }

        //
        // POST: /Book/Borrow/5

        [HttpPost]
        public ActionResult Borrow(Book book)
        {
            if (bookServive.BorrowBook(book.id, book.borrower))
            {
                return RedirectToAction("Details", book);
            }

            book.borrower = null;
            Tools.ErrorMessage.Set(this);
            return RedirectToAction("Borrow", book);
        }


        
    }
}
