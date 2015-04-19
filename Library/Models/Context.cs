using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;

namespace Library.Models
{
    public class LibraryContext : Tuple<IRepository<Book>, IRepository<Person>, IRepository<Borrowing>>
    {
        public LibraryContext(IRepository<Book> books, IRepository<Person> persons, IRepository<Borrowing> borrowings)
            : base(books, persons, borrowings) { }

        public LibraryContext(String dataSource) : this(
            RepositoryFactory.create<Book>(dataSource),
            RepositoryFactory.create<Person>(dataSource),
            RepositoryFactory.create<Borrowing>(dataSource)) {}
    }
}