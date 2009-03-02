using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Net;
using System.IO;

namespace IEatMobile
{
    [XmlRoot("container")]
    public class Fridge
    {
        private List<FridgeProduct> products;

        [XmlAttribute("id")]
        public int id;
        [XmlAttribute("type_id")]
        public int type_id;
        [XmlAttribute("owner_id")]
        public int owner_id;

        [XmlArrayItem("container_product", typeof(FridgeProduct))]
        [XmlArray("container_products")]
        public List<FridgeProduct> Products
        {
            get { return products; }
            set { products = value; }
        }

        public Fridge()
        {
        }

        public void fillWithTest()
        {
            products = new List<FridgeProduct>();

            Product pr = new Product();
            pr.Name = "jablka";

            FridgeProduct fp = new FridgeProduct();
            fp.Product = pr;
            fp.Amount = 3;
            fp.ExpireDate = DateTime.Now;
            fp.Description = "3 kg jablek";
            products.Add(fp);

            pr = new Product();
            pr.Name = "oranzada";
            fp = new FridgeProduct();
            fp.Product = pr;
            fp.Amount = 2;
            fp.ExpireDate = DateTime.Now;
            fp.Description = "Oranzada marki Hoop";
            products.Add(fp);

            pr = new Product();
            pr.Name = "kawior";
            fp = new FridgeProduct();
            fp.Product = pr;
            fp.Amount = 1;
            fp.ExpireDate = DateTime.Now;
            fp.Description = "Kawior z jesiotra, pierwszej swiezosci";
            products.Add(fp);
        }
    }
}
