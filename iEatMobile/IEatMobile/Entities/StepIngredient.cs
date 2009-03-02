using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IEatMobile
{
    public class StepIngredient
    {
        private int id;
        private decimal amount;
        private int productId;
        private int unitId;
        private int recipeStepId;

        [XmlElement("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlElement("amount")]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        [XmlElement("product_id")]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [XmlElement("unit_id")]
        public int UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }

     

        [XmlIgnore]
        public Product Product
        {
            get { return null; }
            set { }
        }
    }
}
