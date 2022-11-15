using System.Collections.Generic;
using System.Xml.Serialization;

namespace BartMarket
{



    [XmlRoot(ElementName = "param")]
    public class Param
    {

        [XmlAttribute(AttributeName = "filter")]
        public bool Filter { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "offer")]
    public class Offer
    {
        [XmlElement(ElementName = "quantity")]
        public int Quanity { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
        [XmlElement(ElementName = "oldprice")]
        public string OldPrice { get; set; }



        [XmlElement(ElementName = "param")]
        public List<Param> Param { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "available")]
        public bool Available { get; set; }

    }

    [XmlRoot(ElementName = "offers")]
    public class Offers
    {

        [XmlElement(ElementName = "offer")]
        public List<Offer> Offer { get; set; }
    }

    [XmlRoot(ElementName = "shop")]
    public class Shop
    {
        [XmlElement(ElementName = "offers")]
        public Offers Offers { get; set; }
    }

    [XmlRoot(ElementName = "yml_catalog")]
    public class YmlCatalog
    {

        [XmlElement(ElementName = "shop")]
        public Shop Shop { get; set; }

    }


}
