using System;
using System.Collections.Generic;
using Library.Models;

namespace Library.Services
{
    public interface IBorrowingService : IBasicBorrowingService
    {
        IEnumerable<Borrowing> SelectAllByPersonId(String person_id);
        IEnumerable<Borrowing> SelectAllByBookId(String book_id);

        IEnumerable<Book> SelectAllAvaliableBooks();
        IEnumerable<Person> SelectAllBorrowingPersons();

        void DeleteAllByPersonId(String person_id);
        void DeleteAllByBookId(String book_id); 
    }

    public interface IBasicBorrowingService 
    {
        bool BorrowBook(String book_id, String person_id);
        bool ReturnBorrowing(String id);

        
    }
}
