using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;
using Library.Models;
using Library.Configurations;

namespace Library.Services
{
    public class BookServiceFactory
    {
        public static IBookService create (String dataStorage) 
        {
            return new BookService(new LibraryContext(dataStorage));
        }
    }
}