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

namespace Repository
{
    public class XmlFileRepository<T> : IRepository<T> where T : class
    {
        readonly private String[] args;

        public XmlFileRepository(String[] args)
        {
            if (args.Count() < 2 || args[0] == null || args[1] == null)
            {
                throw new InvalidParameterException();
            }

            this.args = (String[])args.Clone();
        }


        public IEnumerable<T> SelectAll()
        {
            using (IRepository<T> repository = new XmlStreamRepository<T>(args))
            {
                return repository.SelectAll();
            }
        }

        public T SelectByID(params object[] keyValues)
        {
            using (IRepository<T> repository = new XmlStreamRepository<T>(args))
            {
                return repository.SelectByID(keyValues);
            }
        }

        public void Insert(T entity)
        {
            using (IRepository<T> repository = new XmlStreamRepository<T>(args))
            {
                repository.Insert(entity);
            }
        }

        public void Update(T entity)
        {
            using (IRepository<T> repository = new XmlStreamRepository<T>(args))
            {
                repository.Update(entity);
            }
        }

        public void Delete(params object[] keyValues)
        {
            using (IRepository<T> repository = new XmlStreamRepository<T>(args))
            {
                repository.Delete(keyValues);
            }
        }

        public void Save() { }

        public void Dispose() { }

        readonly private static String EXTENSION = ".xml";

        /*
                private class XmlCollection<T> : IDisposable where T : class
                {
                    readonly private String collectionName;
                    readonly private FileStream fileStream;
                    private XDocument document;
                    private XElement collectionElement;
                    private KeyExtractor<T> keyExtractor = new KeyExtractor<T>();

                    public XmlCollection(String path, String collectionName)
                    {
                        if (path == null || !File.Exists(path) || collectionName == null)
                        {
                            throw new InvalidParameterException();
                        }

                        this.collectionName = collectionName;
                        this.fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                    }

                    public IEnumerable<T> getAll()
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

                    public T getByID(params object[] keyValues)
                    {
                        try
                        {
                            return this.Deserialize(Elements.First(x => Compare(x, keyValues)));
                        }
                        catch (InvalidParameterException)
                        {
                            // Elements that are specified incorrectly in the XML file (= cannot be deserialized) are ignored

                            return null;
                        }
                    }

                    public void add(T entity)
                    {
                        CollectionElement.Add(Serialize(entity));
                        Save();
                    }

                    public void edit(T entity)
                    {
                        Elements.First(x => Compare<T>(x, entity)).ReplaceWith(Serialize(entity));
                        Save();
                    }

                    public void remove(params object[] keyValues)
                    {
                        Elements.Where(x => Compare<T>(x, keyValues)).Remove();
                        Save();
                    }

                    public void Save()
                    {
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
                            if (this.collectionElement == null)
                            {
                                this.collectionElement = Document.Elements().First(d => d.Name == this.collectionName);
                            }

                            return this.collectionElement;
                        }
                    }

                    private static bool Compare<T>(XElement xelem, T entity)
                    {
                        return Compare(xelem, new KeyExtractor<T>().KeyValues(entity));
                    }

                    private static bool Compare<T>(XElement xelem, params object[] keyValues)
                    {
                        List<PropertyInfo> keyFields = new KeyExtractor<T>().KeyFields();

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
        */
    }


}