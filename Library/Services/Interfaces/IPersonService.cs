using System;
using System.Collections.Generic;
using Library.Models;

namespace Library.Services
{
    public interface IPersonService : IService<Person>, IBasicBorrowingService
    {
       /* IEnumerable<Person> getAllPeople();
        Person getPersonById(String id);
        bool addPerson(Person person);
        bool editPerson(Person person);
        bool removePerson(String id);*/
        IEnumerable<Book> SelectAllAvaliableBooks();
    }
}
