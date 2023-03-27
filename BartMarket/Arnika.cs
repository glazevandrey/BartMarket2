using System.Collections.Generic;
using System.Xml.Serialization;

namespace BartMarket

{

    [XmlRoot(ElementName = "category")]
    public class Category
    {

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlAttribute(AttributeName = "parentId")]
        public int ParentId { get; set; }
    }

    [XmlRoot(ElementName = "categories")]
    public class Categories
    {

        [XmlElement(ElementName = "category")]
        public List<Category> Category { get; set; }
    }


    [XmlRoot(ElementName = "export")]
    public class Export
    {

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "company")]
        public string Company { get; set; }

        [XmlElement(ElementName = "categories")]
        public Categories Categories { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "offer")]
        public OffersArnika Offers { get; set; }

        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
    [XmlRoot(ElementName = "offer")]
    public class OffersArnika
    {

        [XmlElement(ElementName = "offer")]
        public List<OfferArnika> Offer { get; set; }
    }

    [XmlRoot(ElementName = "offer")]
    public class OfferArnika
    {
        [XmlElement(ElementName = "old_price")]
        public string OldPrice { get; set; }

        [XmlElement(ElementName = "category")]
        public List<int> Category { get; set; }

        [XmlElement(ElementName = "assembly")]
        public bool Assembly { get; set; }

        [XmlElement(ElementName = "currencyId")]
        public string CurrencyId { get; set; }

        [XmlElement(ElementName = "price")]
        public int Price { get; set; }

        [XmlElement(ElementName = "stock")]
        public int Stock { get; set; }

        [XmlElement(ElementName = "count")]
        public int Count { get; set; }

        [XmlElement(ElementName = "article")]
        public string Article { get; set; }

        [XmlElement(ElementName = "model")]
        public string Model { get; set; }

        [XmlElement(ElementName = "vendor")]
        public string Vendor { get; set; }

        [XmlElement(ElementName = "country_of_origin")]
        public string CountryOfOrigin { get; set; }

        [XmlElement(ElementName = "available")]
        public bool Available { get; set; }

        [XmlElement(ElementName = "param")]
        public List<Param> Param { get; set; }

        [XmlElement(ElementName = "id")]
        public int Id { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        [XmlElement(ElementName = "picture")]
        public List<string> Pictures { get; set; }

        [XmlElement(ElementName = "main_category")]
        public int MainCategory { get; set; }
    }

}
