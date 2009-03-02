using System;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Xml;

namespace IEatMobile
{   [XmlRoot("productsList")]
    public class ProductsList
    {
        private Product[] products;

        [XmlArrayItem(ElementName = "product", Type = typeof(Product))]
        [XmlArray(ElementName = "products")]
        public Product[] Products
        {
            get { return products; }
            set { products = value; }
        }
        public Product getByName(string name)
        {
            foreach (Product p in products)
                if (p.Name.Equals(name))
                    return p;
            return null;
        }
        public Product getById(int id)
        {
            foreach (Product p in products)
                if (p.Id == id)
                    return p;
            return null;
        }
        public ProductsList()
        {
        }
        
        public void Serialize(System.IO.Stream oStream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ProductsList));
            xs.Serialize(oStream, this);
        }
        public void Serialize(XmlWriter xmlWr)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ProductsList));
            xs.Serialize(xmlWr, this);
        }

        public void Deserialize(StreamReader iStream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ProductsList));
            ProductsList list = (ProductsList)xs.Deserialize(iStream);
            this.Products = list.Products;
        }
        public void Deserialize(XmlReader xmlr)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ProductsList));
            ProductsList list = (ProductsList)xs.Deserialize(xmlr);
            this.Products = list.Products;
        }
        public void fillWithTest()
        {
            /*products = new Product[]();

            string[] produkty = { "jablko", "sliwka", "wanilia", "kawa", "oranzada" };
            foreach (string prod in produkty)
            {
                    Product product = new Product();
                    product.Name = prod;
            }
             */ 
        }
    }
}