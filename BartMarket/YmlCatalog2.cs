using System.Collections.Generic;
using System.Xml.Serialization;

namespace BartMarket
{
    [XmlRoot(ElementName = "offer")]
    public class Offer2
    {

        [XmlElement(ElementName = "picture")]
        public List<string> Picture { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "currencyId")]
        public string CurrencyId { get; set; }
        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
        [XmlElement(ElementName = "oldprice")]
        public string OldPrice { get; set; }

        [XmlElement(ElementName = "presence")]
        public string Presence { get; set; }

        [XmlElement(ElementName = "ordering")]
        public int Ordering { get; set; }

        [XmlElement(ElementName = "param")]
        public List<Param> Param { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "available")]
        public bool Available { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "offers")]
    public class Offers2
    {

        [XmlElement(ElementName = "offer")]
        public List<Offer> Offer { get; set; }
    }
    [XmlRoot(ElementName = "yml_catalog")]
    public class YmlCatalog2
    {

        [XmlElement(ElementName = "shop")]
        public Shop2 Shop { get; set; }

        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
    [XmlRoot(ElementName = "shop")]
    public class Shop2
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "company")]
        public string Company { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "platform")]
        public string Platform { get; set; }

        [XmlElement(ElementName = "currencies")]
        public Currencies Currencies { get; set; }

        [XmlElement(ElementName = "categories")]
        public Categories Categories { get; set; }

        [XmlElement(ElementName = "offers")]
        public Offers2 Offers { get; set; }
    }
}
