using System.Text.Json;
using System.Xml;

namespace Hw11
{
    internal class Program
    {
        //Напишите приложение, конвертирующее произвольный JSON в XML. Используйте JsonDocument.
        static void Main(string[] args)
        {
            string jsonStr = String.Empty;
            if (args.Length == 1)
            {
                jsonStr = args[0];
                
            }else
            {
                jsonStr = """
                {
                "MySettings": {
                "setting1": "teen",
                "setting2": "two",
                "setting3": "three",
                "setting4": {
                 "string": 
                        ["1","2"]
                 }
                 }
                }
        """;
            }
                JsonDocument jsonDoc = JsonDocument.Parse(jsonStr);
            XmlWriterSettings settingsXml = new XmlWriterSettings();
            settingsXml.Indent = true;

            using (XmlWriter wr = XmlWriter.Create("output.xml", settingsXml))
            {
                wr.WriteStartDocument();
                wr.WriteStartElement("root");
                foreach (JsonProperty prop in jsonDoc.RootElement.EnumerateObject())
                {
                    wr.WriteStartElement(prop.Name);
                    wr.WriteString(prop.Value.ToString());
                    wr.WriteEndElement();
                }
                wr.WriteEndElement();
                wr.WriteEndDocument();
            }
        }
    }
}
