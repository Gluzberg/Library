using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;
using Library.Models;
using System.Text;

using Library.Configurations;

namespace Library.Services
{
    public class BorrowService : IBorrowingService
    {
        private IRepository<Book> booksRepository;
        private IRepository<Borrowing> borrowingRepository;
        private IRepository<Person> peopleRepository;

        public BorrowService(LibraryContext context)
        {
            booksRepository = context.Item1;
            peopleRepository = context.Item2;
            borrowingRepository = context.Item3;
        }


        public bool BookAvaliable(String book_id)
        {
            return this.SelectAllByBookId(book_id).Where(x => x.end_date == null || x.end_date > DateTime.Now).Count() == 0;
        }

        public IEnumerable<Book> SelectAllAvaliableBooks()
        {
            return this.booksRepository.SelectAll().Where(x => bookAvaliable(x.id));
        }

        public IEnumerable<Person> SelectAllBorrowingPersons()
        {
            return this.peopleRepository.SelectAll().Where(x => personCanBorrow(x.id));
        }

        public IEnumerable<Borrowing> SelectAllByBookId(String book_id)
        {
            return join(this.borrowingRepository.SelectAll().Where(x => x.book_id == book_id)).OrderByDescending(x => x.start_date);
        }

        public IEnumerable<Borrowing> SelectAllByPersonId(String person_id)
        {
            return join(this.borrowingRepository.SelectAll().Where(x => x.person_id == person_id)).OrderByDescending(x => x.start_date);
        }

        public void DeleteAllByBookId(String book_id)
        {
            KeyExtractor<Borrowing> keyExtractor = new KeyExtractor<Borrowing>();

            foreach (Borrowing borrowing in new List<Borrowing>(this.borrowingRepository.SelectAll().Where(x => x.book_id == book_id)))
            {
                this.borrowingRepository.Delete(keyExtractor.KeyValues(borrowing));
            }
        }

        public void DeleteAllByPersonId(String person_id)
        {
            KeyExtractor<Borrowing> keyExtractor = new KeyExtractor<Borrowing>();

            foreach (Borrowing borrowing in new List<Borrowing>(this.borrowingRepository.SelectAll().Where(x => x.person_id == person_id)))
            {
                this.borrowingRepository.Delete(keyExtractor.KeyValues(borrowing));
            }
        }


        public bool BorrowBook(string book_id, string person_id)
        {
            if (!bookAvaliable(book_id))
            {
                return false;
            }

            this.borrowingRepository.Insert(new Borrowing()
                                                    {
                                                        id = Guid.NewGuid().ToString(),
                                                        person_id = person_id,
                                                        book_id = book_id,
                                                        start_date = DateTime.Now,
                                                        end_date = DateTime.MaxValue
                                                    });
            return true;
        }

        public bool ReturnBorrowing(string id)
        {
            Borrowing borrowing = this.borrowingRepository.SelectByID(id);

            if (borrowing == null || !borrowing.isOpen)
            {
                return false;
            }

            borrowing.end_date = DateTime.Now;
            this.borrowingRepository.Update(borrowing);

            return true;
        }



        private bool bookExists(Book book)
        {
            return book != null && bookExists(book.id);
        }

        private bool bookExists(String id)
        {
            return this.booksRepository.SelectByID(id) != null;
        }

        private bool bookAvaliable(String id)
        {
            return this.SelectAllByBookId(id).Where(x => x.end_date == null || x.end_date > DateTime.Now).Count() < 1;
        }

        private bool personCanBorrow(String id)
        {
            return this.SelectAllByPersonId(id).Where(x => x.end_date == null || x.end_date > DateTime.Now).Count() < Configurations.Configuration.MaxBookPerPerson;
        }

        private IEnumerable<Borrowing> join()
        {
            return join(this.borrowingRepository.SelectAll(), this.booksRepository.SelectAll(), this.peopleRepository.SelectAll());
        }

        private Borrowing join(String borrowing_id)
        {
            Borrowing borrowing = this.borrowingRepository.SelectByID(borrowing_id);

            if (borrowing != null)
            {
                join(new Borrowing[] { borrowing });
            }

            return borrowing;
        }

        private IEnumerable<Borrowing> join(IEnumerable<Borrowing> borrowings)
        {
            return join(borrowings, this.booksRepository.SelectAll(), this.peopleRepository.SelectAll());
        }

        private IEnumerable<Borrowing> join(IEnumerable<Borrowing> borrowings, IEnumerable<Book> books, IEnumerable<Person> persons)
        {
            foreach (Borrowing borrowing in borrowings)
            {
                join(borrowing, books, persons);
            }
            return borrowings;
        }

        private void join(Borrowing borrowing, IEnumerable<Book> books, IEnumerable<Person> persons)
        {
            borrowing.book = books.FirstOrDefault(x => x.id == borrowing.book_id);
            borrowing.person = persons.FirstOrDefault(x => x.id == borrowing.person_id);
        }

    /*    private IEnumerable<Borrowing> borrwingsPerBook(String book_id)
        {
            return borrwingsPerBook(book_id, this.borrowingRepository.SelectAll());
        }

        private IEnumerable<Borrowing> borrwingsPerBook(String book_id, IEnumerable<Borrowing> borrowings)
        {
            return borrowings.Where(x => x.book_id == book_id).OrderByDescending(x => x.end_date);
        }

*/

    }
}