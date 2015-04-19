using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;
using System.Configuration;
using Library.Models;

namespace Library.Services
{
    public class PersonServiceFactory
    {
        public static IPersonService create (String dataStorage) 
        {
            return new PersonService(new LibraryContext(dataStorage));
        }
    }
}