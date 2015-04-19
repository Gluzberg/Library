using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    abstract public class RepositoryFactory
    {
        public static IRepository<T> create<T>(String argument) where T : class
        {
            return create<T>(argument == null ? new String[] { } : argument.Split(DELEMITER));
        }

        public static IRepository<T> create<T>(String[] args) where T : class
        {
            Type typeVal;

            if (args.Count() > 0 && Enum.TryParse(args[0], out typeVal))
            {
                String[] repArgs = args.Count() < 2 || args[1] == null  ? new String[] {} : args[1].Split(DELEMITER_PARAM);

                try 
                {
                    switch (typeVal)
                    {
                        case Type.MEMORY:
                            return new MemoryRepository<T>();

                        case Type.SESSION_CACHE:
                            return new SessionCacheRepository<T>(repArgs);

                        case Type.APP_CACHE:
                            return new ApplicationCacheRepository<T>(repArgs);

                        case Type.XML_STREAM:
                            return new XmlStreamRepository<T>(repArgs);

                        case Type.XML_FILE:
                            return new XmlFileRepository<T>(repArgs);

                        case Type.DB:
                            return new DbRepository<T>(repArgs);
                    }
                }
                catch  (InvalidParameterException) { }
            }

            return new ApplicationCacheRepository<T>(args);
        }

        public enum Type
        {
            XML_FILE, XML_STREAM, DB, MEMORY, SESSION_CACHE, APP_CACHE
        }

        readonly private static char[] DELEMITER = new char[] {'#'};
        readonly private static char[] DELEMITER_PARAM = new char[] { '?' };

        readonly public static string SERVER_FILE = "SERVER";
    }
}
