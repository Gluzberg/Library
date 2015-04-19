using System;
using System.Collections.Generic;
using Library.Models;
using Repository;

namespace Library.Services
{
    public interface IBookService : IService<Book>, IBasicBorrowingService 
    {
        IEnumerable<Person> SelectAllBorrowingPersons();


    }
}
