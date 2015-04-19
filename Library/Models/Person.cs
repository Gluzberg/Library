using System;
using System.Collections.Generic;
using Repository;
using System.Xml.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections.Generic;
using Library.Configurations;

namespace Library.Models
{
    [Serializable, XmlRoot("person")]
    public class Person
    {
        [KeyField]
        [XmlAttribute]
        public String id { get; set; }


       
        [DisplayName("Vorname")]
        [RegularExpression(@"[a-zA-Z''-'\s]*$")]
        [Required]
        [XmlElement(ElementName = "first_name")]
        public String firstName { get; set; }

        [DisplayName("Nachname")]
        [RegularExpression(@"[a-zA-Z''-'\s]*$")]
        [Required]
        [XmlElement(ElementName = "last_name")]
        public String lastName { get; set; }

        [DisplayName("Mitglied seit")]
        [XmlElement(ElementName = "member_since")]
        [DataType(DataType.Date)]
        public DateTime memberSince { get; set; }

        [XmlIgnore]
        public String borrowed { get; set;  }

        [XmlIgnore]
        [DisplayName("Name")]
        public String fullName 
        { 
            get 
            {
                return lastName + ", " + firstName; 
            }
        }

        [XmlIgnore]
        public IEnumerable<Borrowing> borrowings { get; set; }

        [XmlIgnore]
        public bool canBorrow
        {
            get
            {
                return borrowings == null ||
                        new List<Borrowing>(borrowings).FindAll(
                            x => x.end_date == null ||
                            x.end_date > DateTime.Now).Count < Configuration.MaxBookPerPerson;
            }
        }
    }
}