using System;
using Repository;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Library.Models
{
    [Serializable, XmlRoot("person")]
    public class Borrowing
    {
        [KeyField]
        [XmlAttribute]
        public String id { get; set; }

        public String book_id { get; set; }
        public String person_id { get; set; }

        [DisplayName("Ausleihdatum")]
        [DataType(DataType.DateTime)]
        public DateTime start_date { get; set; }

        [DisplayName("Rückgabedatum")]
        [DataType(DataType.DateTime)]
        public DateTime end_date { get; set; }

        [ForeignKey("book_id")]
        [XmlIgnore]
        public Book book { get; set; }

        [ForeignKey("person_id")]
        [XmlIgnore]
        public Person person { get; set; }

        [XmlIgnore]
        public bool isOpen
        {
            get
            {
                return end_date == null || end_date > DateTime.Now;
            }
        }
    }
}