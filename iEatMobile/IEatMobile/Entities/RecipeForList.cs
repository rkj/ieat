using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IEatMobile
{
    [XmlRoot("recipe")]
    public class RecipeForList
    {
        [XmlElement("name")]
        public String name;
        [XmlElement("id")]
        public int id;
    }
}
