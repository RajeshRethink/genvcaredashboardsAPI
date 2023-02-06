using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Genworks.API.Helpers
{
    public class XmlHelper
    {
        public static XElement GetXmlData(string path)
        {
            var xmlStr = File.ReadAllText(path);

            return XElement.Parse(xmlStr);

        }
    }
}
