using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Globalization;


namespace Library.Models
{
    [Serializable]
    public class Currency : IXmlSerializable
    {
        private static IFormatProvider SERIALIZATION_CULTURE = CultureInfo.CreateSpecificCulture("en-GB");
        private static IFormatProvider UI_CULTURE = CultureInfo.CreateSpecificCulture("de-DE");
      

        private decimal? value;

        public Currency(String strVal)
        {
            value = Parse(strVal, UI_CULTURE);
        }

        public override string ToString()
        {
            return ToString(this.value, UI_CULTURE);
        }

        private static decimal? Parse(String strVal, IFormatProvider formatProvider)
        {
            decimal val;
            return decimal.TryParse(strVal, NumberStyles.Currency, formatProvider, out val) ? (decimal?)val : null;
        }

        private static String ToString(decimal? value, IFormatProvider formatProvider)
        {
            return value.HasValue ? value.Value.ToString(formatProvider) : "";
        }


        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString(value,SERIALIZATION_CULTURE));
        }

        public void ReadXml(XmlReader reader)
        {
            value = Parse(reader.ReadString(), SERIALIZATION_CULTURE);
        }

        public XmlSchema GetSchema()
        {
            return (null);
        }       
    }
}