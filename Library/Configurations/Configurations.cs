using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository;
using System.Configuration;
using Repository;

namespace Library.Configurations
{
    public abstract class Configuration
    {
        public static String DataStorage
        {
            get 
            {
                return ConfigurationManager.AppSettings["DataStorage"];
            }
        }

        public static String DataInitSource
        {
            get
            {
                return ConfigurationManager.AppSettings["DataInitSource"];
            }
        }

        public static int MaxBookPerPerson
        {
            get
            {
                String val = ConfigurationManager.AppSettings["MaxBookPerPerson"];

                int intVal;

                return (val != null && int.TryParse(val, out intVal)) ? intVal : int.MaxValue;
            }
        }

        public static bool usingSessionCashe
        {
            get 
            {
                return RepositoryFactory.Type.SESSION_CACHE.ToString().Equals(ConfigurationManager.AppSettings["DataStorage"]);
            }
        }
    }
}