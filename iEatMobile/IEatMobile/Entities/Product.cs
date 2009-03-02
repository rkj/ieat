using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IEatMobile
{   [XmlRoot("product")]
    public class Product
    {
        private int id;
        private string name;

        [XmlElement(ElementName="id",Type=typeof(int))]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [XmlElement(ElementName="name",Type=typeof(String))]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
    }
}
