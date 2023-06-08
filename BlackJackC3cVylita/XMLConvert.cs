using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace BlackJackC3cVylita
{
    public class XMLConvert
    {
        public static string ObjectToXml(List<Player> playerList)
        {
            string returnXml = string.Empty;

            try
            {
                XmlSerializer xmlserializer = new XmlSerializer(typeof(List<Player>));
                StringWriter stringWriter = new StringWriter();
                using (XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, playerList);
                    returnXml = stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred. Error code:: {ex}");
            }

            return returnXml;
        }

        public static List<Player> XMLToObject(string xml)
        {
            List<Player> playerList = new List<Player>();

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    playerList = (List<Player>)new XmlSerializer(typeof(List<Player>))
                        .Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred. Error code:: {ex}");
            }

            return playerList;
        }
    }
}
