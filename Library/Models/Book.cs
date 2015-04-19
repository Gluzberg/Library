using System;
using System.Collections.Generic;
using Repository;
using System.Xml.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;

namespace Library.Models
{
    [Serializable, XmlRoot("book")]
    public class Book
    {
        [KeyField]
        [XmlAttribute]
        public String id { get; set; }

        [DisplayName("Autor")]
        [RegularExpression(@"^[,a-zA-Z''-'\s]*$")]
        [Required]
        public String author { get; set; }

        [DisplayName("Titel")]
        [Required]
        public String title { get; set; }

        [DisplayName("Genre")]
        [RegularExpression(@"[a-zA-Z''-'\s]*$")]
        [Required]
        public String genre { get; set; }
       
        [DisplayName("Preis")]
        [RegularExpression(@"[0-9]+.[0-9][0-9]")]
        [Required]
        public String price 
        { 
            get 
            {
                return priceValue.HasValue ? priceValue.Value.ToString(CultureInfo.CreateSpecificCulture("en-GB")) : "";
            } 
            set 
            {
                decimal val;
                priceValue = decimal.TryParse(value, NumberStyles.Currency, CultureInfo.CreateSpecificCulture("en-GB"), out val) ? (decimal?)val : priceValue;
            }  
        }

        [XmlIgnore]
        private decimal? priceValue;


        [DisplayName("VÖ-Datum")]
        [XmlElement(ElementName = "publish_date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime publishDate { get; set; }

        [DisplayName("Beschreibung")]
        [DataType(DataType.MultilineText)]
        public String description { get; set; }



        [XmlIgnore]
        public String borrower { get; set; }

        [XmlIgnore]
        public IEnumerable<Borrowing> borrowings { get; set; }

        [XmlIgnore]
        [DisplayName("Verfügbarkeit")]
        public bool isAvaliable 
        {
            get
            {
                return (borrowings == null) ||
                        new List<Borrowing>(borrowings).FindAll(x => x.end_date == null || x.end_date > DateTime.Now).Count < 1;
            }
        } 



    }
}