using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace net.ELF.DBTool
{
    [XmlRoot("Config")]
    public class Config
    {
        public static Config toolConfig;

        [XmlIgnore]
        private static string ConfigPath = System.Windows.Forms.Application.StartupPath + "\\dbtoolconfig.xml";
        [XmlElement("PDPath")]
        public string PDPath;
        [XmlElement("DBBasePath")]
        public string DBBasePath;
        [XmlElement("DBConnectStr")]
        public string DBConnectStr;
        [XmlElement("Namespace")]
        public string Namespace;
        [XmlElement("BaseObjectName")]
        public string BaseObjectName;
        [XmlElement("BaseTreeName")]
        public string BaseTreeName;
        static Config()
        {
            string config = FileHelper.ReadFile(ConfigPath);
            if (!string.IsNullOrEmpty(config))
                toolConfig = XMLHelper.Deserialize<Config>(config);
            else
                toolConfig = new Config();
        }

        /// <summary>
        /// 保存config
        /// </summary>
        public static void SaveConfig()
        {
            if (toolConfig != null)
                FileHelper.WriteFile(ConfigPath, XMLHelper.ToSerialize(toolConfig), false,Encoding.UTF8);
        }
    }
}
