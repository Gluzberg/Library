using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Data;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;


namespace Repository.Tests
{
    [Table("Model")]
    public class Model
    {
        [Key]
        [KeyField]
        [XmlAttribute]
        public String Id { get; set; }
        public String Name { get; set; }

        [DataType(DataType.Currency)]
        public Decimal Price { get; set; }

        public int Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        override public bool Equals(object obj)
        {
            if (obj != null && obj is Model)
            {
                Model model = (Model)obj;

                return
                    Id == model.Id &&
                    Name != null && Name.Equals(model.Name) &&
                    Price == model.Price &&
                    Amount == model.Amount &&
                    Date == model.Date;
            }

            return false;
        }
    }


    public class RepositoryUnitTest
    {
        private readonly String argument;
        

        public RepositoryUnitTest(String argument)
        {
            this.argument = argument;
        }

        private IRepository<Model> createRepository()
        {
            return RepositoryFactory.create<Model>(argument);
        }

        

        private static Model createModel(int id)
        {
            String strId = id.ToString();

            return new Model()
            {
                Id = "md-" + strId,
                Name = "Name_" + strId,
                Price = new decimal(id + 0.14),
                Amount = 17 * id,
                Date = DateTime.Parse("2015-04-16T17:19:03.731598+02:00").AddDays(id)
            };
        }

        private static List<Model> createModels(int count)
        {
            List<Model> models = new List<Model>(count);

            for (int i = 0; i < count; i++)
            {
                models.Add(createModel(i));
            }

            return models;
        }

        private static Model DefaultModel
        {
            get
            {
                return createModel(0);
            }
        }

        [TestMethod]
        public void TestInsert()
        {
            using (IRepository<Model> repository = createRepository())
            {
                repository.Insert(DefaultModel);
                Assert.IsNotNull(repository.SelectByID(DefaultModel.Id));
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            using (IRepository<Model> repository = createRepository())
            {
                Model model = DefaultModel;
                repository.Insert(DefaultModel);

                model = createModel(7);
                model.Id = DefaultModel.Id;

                repository.Update(model);

                Assert.AreEqual(model,repository.SelectByID(model.Id));
            }
        }

        [TestMethod]
        public void TestDelete()
        {
            using (IRepository<Model> repository = createRepository())
            {
                int count = 10;
                foreach (Model m in createModels(count))
                {
                    repository.Insert(m);
                }

                Model model = createModel(5);

                Assert.IsNotNull(repository.SelectByID(model.Id));

                repository.Delete(model.Id);

                Assert.IsNull(repository.SelectByID(model.Id));
            }
        }

        [TestMethod]
        public void TestSelectById()
        {
            using (IRepository<Model> repository = createRepository())
            {
                int count = 10;
                foreach (Model m in createModels(count))
                {
                    repository.Insert(m);
                }

                foreach (Model m in createModels(count))
                {
                    Assert.IsTrue(m.Equals(repository.SelectByID(m.Id)));
                }  
            }
        }

        [TestMethod]
        public void TestSelectAll()
        {
            int count = 10;
            IEnumerable<Model> selectedModels;

            using (IRepository<Model> repository = createRepository())
            {
                foreach (Model m in createModels(count))
                {
                    repository.Insert(m);
                }

                selectedModels = repository.SelectAll();
            }

            Assert.IsNotNull(selectedModels);

            List<Model> models = createModels(count);
            
            foreach (Model m in selectedModels)
            {
                models.Remove(m);
            }

            Assert.IsTrue(models.Count == 0);
        }
    }




    [TestClass]
    public class MemoryRepositoryUnitTest : RepositoryUnitTest
    {
        public MemoryRepositoryUnitTest() : base("MEMORY") { }
    }

    [TestClass]
    public class XmlStreamRepositoryUnitTest : XmlRepositoryUnitTest
    {
        public XmlStreamRepositoryUnitTest() : base("STREAM") { }
    }

    [TestClass]
    public class XmlFileRepositoryUnitTest : XmlRepositoryUnitTest
    {
        public XmlFileRepositoryUnitTest() : base("FILE") { }
    }

    public class XmlRepositoryUnitTest : RepositoryUnitTest
    {
        static private String fileDir = System.IO.Directory.GetCurrentDirectory() + "\\tmp" + Guid.NewGuid().ToString() + "\\"; 
        static private String innerXml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><models></models>";

        public XmlRepositoryUnitTest(String type)
            : base("XML_" + type + "#" + fileDir + "?") 
        {
            System.IO.Directory.CreateDirectory(fileDir);

            using (StreamWriter file = new StreamWriter(fileDir + "Models.xml"))
            {
                file.Write(innerXml);
            }
        }
    }
}
