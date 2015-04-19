using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Reflection;
using System.Data;
using System.Data.Entity;

namespace Repository
{
    public class XmlStreamRepository<T> : IDisposable, IRepository<T> where T : class
    {
            //readonly private String[] args;
            readonly private FileStream fileStream;
            
            private XDocument document;
           // private XElement collectionElement;
            private KeyExtractor<T> keyExtractor = new KeyExtractor<T>();

            public XmlStreamRepository(String[] args)
            {
                String fileName;

                if (args.Count() < 2 || args[0] == null || args[1] == null ||
                    (fileName = RepositoryFactory.SERVER_FILE.Equals(args[1]) ? HttpContext.Current.Server.MapPath(args[0]) : args[0]) == null ||
                    (fileName = fileName + typeof(T).Name + "s.xml") == null ||
                    !File.Exists(fileName))
                {
                    throw new InvalidParameterException();
                }

                this.fileStream = tryGetFile(fileName);
            }

            private static FileStream tryGetFile(String fileName)
            {
                var autoResetEvent = new System.Threading.AutoResetEvent(false);

                while (true)
                {
                    try
                    {
                        
                        return new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    }
                    catch (IOException)
                    {
                        var fileSystemWatcher =
                            new FileSystemWatcher(Path.GetDirectoryName(fileName))
                            {
                                EnableRaisingEvents = true
                            };

                        fileSystemWatcher.Changed +=
                            (o, e) =>
                            {
                                if (Path.GetFullPath(e.FullPath) == fileName)
                                {
                                    autoResetEvent.Set();
                                }
                            };

                        autoResetEvent.WaitOne();
                    }
                }
            }

            public IEnumerable<T> SelectAll()
            {
                List<T> items = new List<T>(Elements.Count());

                foreach (XElement xelem in Elements)
                {
                    try
                    {
                        items.Add(Deserialize(xelem));
                    }
                    catch (InvalidParameterException)
                    {
                        // Elements that are specified incorrectly in the XML file (= cannot be deserialized) are ignored
                    }
                }

                return items;
            }

            public T SelectByID(params object[] keyValues)
            {
                try
                {
                    XElement xelem = Elements.FirstOrDefault(x => Compare(x, keyValues));

                    return xelem == null ? null : this.Deserialize(xelem);
                }
                catch (InvalidParameterException)
                {
                    // Elements that are specified incorrectly in the XML file (= cannot be deserialized) are ignored

                    return null;
                }
            }

            public void Insert(T entity)
            {
                CollectionElement.Add(Serialize(entity));
                Save();
            }

            public void Update(T entity)
            {
                Elements.First(x => Compare(x, entity)).ReplaceWith(Serialize(entity));
                Save();
            }

            public void Delete(params object[] keyValues)
            {
                Elements.Where(x => Compare(x, keyValues)).Remove();
                Save();
            }

            public void Save()
            {
                fileStream.SetLength(0);
                Document.Save(fileStream);
            }



            private XDocument Document
            {
                get 
                {
                    if (document == null) 
                    {
                        document = XDocument.Load(fileStream);
                    }
                    return document;
                }
            }

            private T Deserialize(XElement xelem)
            {
                try
                {
                    return (T)new XmlSerializer(typeof(T)).Deserialize(xelem.CreateReader());
                }
                catch (Exception e)
                {
                    if (e is NullReferenceException || e is InvalidOperationException)
                    {
                        throw new InvalidParameterException();
                    }

                    throw;
                }
            }

            private XElement Serialize(T entity)
            {
                XDocument target = new XDocument();
                XmlWriter writer = target.CreateWriter();

                try
                {
                    new XmlSerializer(typeof(T)).Serialize(writer, entity);
                }
                catch (Exception e)
                {
                    if (e is NullReferenceException || e is InvalidOperationException)
                    {
                        throw new InvalidParameterException();
                    }

                    throw;
                }

                writer.Close();
                return target.Root;
            }

            private IEnumerable<XElement> Elements
            {
                get
                {
                    XElement collectionElem = CollectionElement;

                    if (collectionElem == null)
                    {
                        throw new RepositoryNotAvaliableException();
                    }

                    return collectionElem.Elements();
                }
            }

            private XElement CollectionElement
            {
                get
                {
                    return Document.Elements().First();

                    /*
                    if (this.collectionElement == null)
                    {
                        this.collectionElement = Document.Elements().First(d => d.Name == args[2]);
                    }

                    return this.collectionElement; */
                }
            }


            private bool Compare(XElement xelem, T entity)
            {
                return Compare(xelem, keyExtractor.KeyValues(entity));
            }

            private bool Compare(XElement xelem, params object[] keyValues)
            {
                List<PropertyInfo> keyFields = keyExtractor.KeyFields();

                if (keyValues.Count() != keyFields.Count()) return false;

                for (int i = 0, count = keyValues.Count(); i < count; i++)
                {
                    if (xelem.Attribute(keyFields[i].Name) != null &&
                        !keyValues[i].Equals(xelem.Attribute(keyFields[i].Name).Value))
                    {
                        return false;
                    }
                }
                return true;
            }

            public void Dispose()
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }

            
        }
    }
