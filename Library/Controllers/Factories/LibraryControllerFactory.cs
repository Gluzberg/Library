using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Library.Services;
using Library.Configurations;
using Library.Models;
using Library.Controllers;
using System.Reflection;
using System.Diagnostics;

namespace Library.Controllers
{
    public class LibraryControllerFactory : DefaultControllerFactory
    {
        private Dictionary<string, Func<RequestContext, IController>> controllers;

        public LibraryControllerFactory()
        {
            controllers = new Dictionary<string, Func<RequestContext, IController>>();
            controllers["Book"] = controller => new BookController(BookServiceFactory.create(Configuration.DataStorage));
            controllers["Person"] = controller => new PersonController(PersonServiceFactory.create(Configuration.DataStorage));
        }

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (controllers.ContainsKey(controllerName))
            {
                return controllers[controllerName](requestContext);
            }
            else
            {
                return base.CreateController(requestContext, controllerName);
            }
        }
    }


    public class ControllerFactoryHelper
    {
        public static IControllerFactory GetControllerFactory()
        {
            return new LibraryControllerFactory();
        }
    }


    public class RepositoriesInit
    {
        public static void Init()
        {
            String repositoryType = Configuration.DataStorage;
            String sourceType = Configuration.DataInitSource;

            if (sourceType == null || sourceType.Equals(repositoryType)) return;

            // In the following loop we use reflecton
            // Instead of writing for each model: new BaseService<[model]>(repositoryType).Init([model]);
            // Fails quietly
           
            foreach (Type mType in new Type[] { typeof(Book), typeof(Borrowing), typeof(Person) }) 
            {
                try
                {
                    Type serviceType = typeof(BaseService<>).MakeGenericType(mType);

                    object obj = serviceType.GetConstructor(new Type[] { typeof(String) }).Invoke(new String[] { repositoryType });
                    MethodInfo initMethod = serviceType.GetMethod("Init", new Type[] { typeof(String) });

                    initMethod.Invoke(obj, new object[] { sourceType });
                }
                catch
                {
                    Debug.WriteLine("Failed to initialize model " + mType.Name);
                }
            }  
        }


    }
}