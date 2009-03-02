using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;


namespace IEatMobile
{
    [XmlRoot("constainerList")]
    public class FridgeList
    {
        [XmlArrayItem("container")]
        [XmlArray("containers")]
        public List<FridgeForList> containers;

        public void Deserialize(XmlReader xmlr)
        {
            XmlSerializer xs = new XmlSerializer(typeof(FridgeList));
            FridgeList list = (FridgeList)xs.Deserialize(xmlr);
            this.containers = list.containers;
        }
    }
}
