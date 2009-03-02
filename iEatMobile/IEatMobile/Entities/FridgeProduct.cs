using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IEatMobile
{
    public class FridgeProduct
    {
        private float amount;
        private DateTime expireDate;
        private string description;
        private int productId;
        private Product product;
        private int unit_id;
        private int id;

        [XmlElement("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlElement("description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [XmlIgnore]
        public string Name
        {
            get { if (Product != null) return Product.Name; else return null; }
            set { Product.Name = value;}
        }

        [XmlIgnore]
        public Product Product
        {
            get { return product; }
            set { product = value; }
        }

        [XmlElement("amount")]
        public float Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [XmlElement("expiration_date")]
        public DateTime ExpireDate
        {
            get { return expireDate; }
            set { expireDate = value; }
        }

        [XmlElement("product_id")]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [XmlElement("unite_id")]
        public int UnitID
        {
            get { return unit_id; }
            set { unit_id = value; }
        }
    }
}
