using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository;
using Library.Models;
using Library.Services;

namespace Library.Controllers
{
    public class PersonController : Controller
    {
        IPersonService peopleService;

        public PersonController(IPersonService peopleService)
        {
            this.peopleService = peopleService;
        }

        // GET: /Person/

        public ActionResult Index()
        {
            return View(peopleService.SelectAll());
        }

        //
        // GET: /Person/Details/5

        public ActionResult Details(String id)
        {
            Person person = this.peopleService.SelectByID(id);

            if (person == null)
            {
                return HttpNotFound();
            }

            return View(peopleService.SelectByID(id));
        }

        //
        // GET: /Person/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Person/Create

        [HttpPost]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                if (peopleService.Insert(person))
                {
                    return RedirectToAction("Details",person);
                }
                else
                {
                    Tools.ErrorMessage.Set(this);
                }
            }


            return View("Create");
        }

        //
        // GET: /Person/Edit/5

        public ActionResult Edit(String id)
        {
            Person Person = peopleService.SelectByID(id);

            if (Person == null)
            {
                return HttpNotFound();
            }

            return View(Person);
        }

        //
        // POST: /Person/Edit/5

        [HttpPost]
        public ActionResult Edit(Person Person)
        {
            if (ModelState.IsValid)
            {
                if (peopleService.Update(Person))
                {
                    return RedirectToAction("Details",Person);
                }
                else
                {
                    Tools.ErrorMessage.Set(this);
                }
            }

            return View("Edit",Person);
        }

        //
        // GET: /Person/Delete/5

        public ActionResult Delete(String id)
        {
            return Details(id);
        }

        //
        // POST: /Person/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            if (peopleService.Delete(id))
            {
                return RedirectToAction("Index");
            }

            Tools.ErrorMessage.Set(this);
            Person person = this.peopleService.SelectByID(id);
            return RedirectToAction("Details", person);
        }



        //
        // GET: /Person/Return/5

        public ActionResult Return(String borrow_id, String person_id)
        {
            Person person = this.peopleService.SelectByID(person_id);
            this.peopleService.ReturnBorrowing(borrow_id);
            return RedirectToAction("Details", person);           
        }

        //
        // GET: /Person/Borrow/5

        public ActionResult Borrow(String id)
        {
            Person person = this.peopleService.SelectByID(id);

            if (person == null)
            {
                return HttpNotFound();
            }

            ViewBag.Borrowed = new SelectList(peopleService.SelectAllAvaliableBooks(), "id", "title", person.borrowed);
            return View(person);
        }

        //
        // POST: /Person/Borrow/5

        [HttpPost]
        public ActionResult Borrow(Person person)
        {
            if (peopleService.BorrowBook(person.borrowed, person.id))
            {
                return RedirectToAction("Details", person);
            }

            person.borrowed = null;
            Tools.ErrorMessage.Set(this);
            return RedirectToAction("Borrow", person);
        }


        protected override void Dispose(bool disposing)
        {
           // peopleService.Dispose();
            base.Dispose(disposing);
        }
    }
}
