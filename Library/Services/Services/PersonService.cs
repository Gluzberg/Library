using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;
using Library.Models;
using Library.Configurations;

namespace Library.Services
{
    public class PersonService : IPersonService
    {
        private IRepository<Person> peopleRepository;
        private IRepository<Book> bookRepository;

        private IBorrowingService borrowingService;

        public PersonService(LibraryContext context)
        {
            this.bookRepository = context.Item1;
            this.peopleRepository = context.Item2;
            this.borrowingService = new BorrowService(context);
        }

        public bool BorrowBook(string book_id, string person_id)
        {
            return this.borrowingService.BorrowBook(book_id, person_id);
        }

        public bool ReturnBorrowing(string id)
        {
            return this.borrowingService.ReturnBorrowing(id);
        }

        public IEnumerable<Person> SelectAll()
        {
            return join();
        }

        public Person SelectByID(params object [] keyValues)
        {
            if (keyValues == null || keyValues.Count() < 1 || !(keyValues[0] is String)) return null;

            String id = (String)keyValues[0];

            return join(id);
        }

        public bool Insert(Models.Person person)
        {
            person.memberSince = DateTime.Now;
            person.id = "pr" + Guid.NewGuid().ToString();
            this.peopleRepository.Insert(person);
            return true;
        }

        public bool Update(Models.Person Person)
        {
            if (this.PersonExists(Person))
            {
                this.peopleRepository.Update(Person);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(params object[] keyValues)
        {
            if (keyValues == null || keyValues.Count() < 1 || !(keyValues[0] is String)) return false;

            String id = (String)keyValues[0];

            if (this.PersonExists(id))
            {
                this.borrowingService.DeleteAllByPersonId(id);
                this.peopleRepository.Delete(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Book> SelectAllAvaliableBooks()
        {
            return this.borrowingService.SelectAllAvaliableBooks();
        }

        public void Dispose()
        {

        }

        private bool PersonExists(Person Person)
        {
            return Person != null && PersonExists(Person.id);
        }

        private bool PersonExists(String id)
        {
            return this.peopleRepository.SelectByID(id) != null;
        }

        private IEnumerable<Person> join()
        {
            return join(this.peopleRepository.SelectAll());
        }

        private Person join(String Person_id)
        {
            Person person = this.peopleRepository.SelectByID(Person_id);

            return join(person);
        }

        private IEnumerable<Person> join(IEnumerable<Person> persons)
        {
            foreach (Person person in persons)
            {
                join(person);
            }
            return persons;
        }

        private Person join(Person person)
        {
            person.borrowings = this.borrowingService.SelectAllByPersonId(person.id);

            return person;
        }
    }
}