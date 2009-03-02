using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace IEatMobile
{
    [XmlRoot("confing")]
    public class config
    {
        private static config conf;
        [XmlIgnore]
        public static config Current
        {
            get 
            {
                if (conf == null)
                {
                    conf = new config();
                    XmlTextReader xmlr = new XmlTextReader("/Program Files/iEatMobile/config.xml");

                    conf.Deserialize(xmlr);
                    xmlr.Close();
                }
                return conf; 
            }
        }

        [XmlElement("user")]
        public string user;
       
        [XmlElement("serverAddress")]
        public string serverAddress;
        [XmlElement("serverAddress")]
        public int ojej;
        [XmlIgnore()]
        public string path
        {
            get { return "/Program Files/iEatMobile/"; }
        }

        public void Serialize(XmlTextWriter xmlr)
        {
            XmlSerializer xs = new XmlSerializer(typeof(config));
            xs.Serialize(xmlr, conf);
        }
        public void Deserialize(XmlTextReader xmlr)
        {
            XmlSerializer xs = new XmlSerializer(typeof(config));
            config c = (config)xs.Deserialize(xmlr);
            conf.serverAddress = c.serverAddress;
            conf.user = c.user;
           
        }

    }
}
