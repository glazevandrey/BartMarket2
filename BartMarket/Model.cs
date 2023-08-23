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
        

        [XmlElement(ElementName = "categoryId")]
        public string CategoryId { get; set; }
        [XmlElement(ElementName = "quantity")]
        public int Quanity { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "price")]
        public string Price { get; set; }
        [XmlElement(ElementName = "oldprice")]
        public string OldPrice { get; set; }


        [XmlElement(ElementName = "picture")]
        public List<string> Pictures { get; set; }



        [XmlElement(ElementName = "param")]
        public List<Param> Param { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }


        [XmlAttribute(AttributeName = "vendor")]
        public string Vendor { get; set; }

        [XmlAttribute(AttributeName = "available")]
        public bool Available { get; set; }

    }
    [XmlRoot(ElementName = "offer")]
    public class Offer2
    {

        [XmlElement(ElementName = "picture")]
        public List<string> Pictures { get; set; }

        [XmlElement(ElementName = "param")]
        public List<Param> Param { get; set; }

        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "name_back")]
        public string NameBack { get; set; }

        [XmlElement(ElementName = "price")]
        public int Price { get; set; }

        [XmlElement(ElementName = "oldprice")]
        public int Oldprice { get; set; }

        [XmlElement(ElementName = "min_price")]
        public int MinPrice { get; set; }

        [XmlElement(ElementName = "formula")]
        public string Formula { get; set; }

        [XmlElement(ElementName = "outlets")]
        public Outlets Outlets { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlText]
        public string Text { get; set; }

    }
    [XmlRoot(ElementName = "outlets")]
    public class Outlets
    {

        [XmlElement(ElementName = "outlet")]
        public List<Outlet> Outlet { get; set; }
    }
    [XmlRoot(ElementName = "outlet")]
    public class Outlet
    {

        [XmlAttribute(AttributeName = "instock")]
        public int Instock { get; set; }

        [XmlAttribute(AttributeName = "warehouse_name")]
        public string WarehouseName { get; set; }
    }
    [XmlRoot(ElementName = "offers")]
    public class Offers
    {

        [XmlElement(ElementName = "offer")]
        public List<Offer> Offer { get; set; }
    }
    [XmlRoot(ElementName = "offers")]
    public class Offers2
    {

        [XmlElement(ElementName = "offer")]
        public List<Offer2> Offer { get; set; }
    }
    [XmlRoot(ElementName = "shop")]
    public class Shop
    {
        [XmlElement(ElementName = "categories")]
        public Categories Categories { get; set; }
        [XmlElement(ElementName = "offers")]
        public Offers Offers { get; set; }
    }
    [XmlRoot(ElementName = "shop")]
    public class Shop2
    {
        [XmlElement(ElementName = "offers")]
        public Offers2 Offers { get; set; }
    }
    
    [XmlRoot(ElementName = "yml_catalog")]
    public class YmlCatalog
    {
       


        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }


        [XmlElement(ElementName = "shop")]
        public Shop Shop { get; set; }


    }
    [XmlRoot(ElementName = "yml_catalog")]
    public class YmlCatalog2
    {

        [XmlAttribute(AttributeName = "date")]
        public string Date { get; set; }

        [XmlElement(ElementName = "shop")]
        public Shop2 Shop { get; set; }

    }

}
