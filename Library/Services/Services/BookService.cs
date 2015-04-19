using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;
using Library.Models;
using System.Text;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private IRepository<Book> booksRepository;

        IBorrowingService borrowService;
        IPersonService personService;

        public BookService(LibraryContext context)
        {
            this.booksRepository = context.Item1;

            this.borrowService = new BorrowService(context);
            this.personService = new PersonService(context);
        }

        public IEnumerable<Models.Book> SelectAll()
        {
            return join();
        }

        public Models.Book SelectByID(params object[] keyValues)
        {
            if (keyValues.Count() < 1 || keyValues[0] == null || !(keyValues[0] is String)) return null;

            return join((String)keyValues[0]);
        }

        public bool Insert(Models.Book book)
        {
            book.id = "bk" + Guid.NewGuid().ToString();
            this.booksRepository.Insert(book);
            return true;
        }

        public bool Update(Models.Book book)
        {
            if (this.bookExists(book))
            {
                this.booksRepository.Update(book);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(params object [] keyValues)
        {
            if (keyValues == null || keyValues.Count() < 1 || !(keyValues[0] is String)) return false;

            String id = (String)keyValues[0];

            if (this.bookExists(id))
            {
                this.borrowService.DeleteAllByBookId(id);
                this.booksRepository.Delete(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<Person> SelectAllBorrowingPersons() 
        {
            return this.borrowService.SelectAllBorrowingPersons();
        }


        public bool BorrowBook(string book_id, string person_id)
        {
            return this.borrowService.BorrowBook(book_id, person_id);
        }

        public bool ReturnBorrowing(string id)
        {
            return this.borrowService.ReturnBorrowing(id);
        }

        public void Dispose()
        {
           

        }


        private bool bookExists(Book book)
        {
            return book != null && bookExists(book.id);
        }

        private bool bookExists(String id)
        {
            return this.booksRepository.SelectByID(id) != null;
        }


        private IEnumerable<Book> join()
        {
            return join(this.booksRepository.SelectAll());
        }

        private Book join(String book_id)
        {
            Book book = this.booksRepository.SelectByID(book_id);

            if (book != null)
            {
                join(book);
            }

            return book;
        }

        private IEnumerable<Book> join(IEnumerable<Book> books)
        {
            foreach (Book book in books)
            {
                join(book);
            }
            return books;
        }

        private void join(Book book)
        {
            book.borrowings = this.borrowService.SelectAllByBookId(book.id);
        }
    }
}