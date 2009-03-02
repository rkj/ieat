using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace IEatMobile
{
    [XmlRoot("container")]
    public class FridgeForList
    { 
        [XmlElement("id")]
        public int id;
        [XmlElement("type_id")]
        public int type;
        [XmlElement("owner")]
        public string username;


    
    }
}
