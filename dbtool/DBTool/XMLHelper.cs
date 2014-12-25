using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace net.ELF.DBTool
{
    public class XMLHelper
    {
        public static string ToSerialize(object obj)
        {
            XmlSerializer xs = new XmlSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            try
            {
                xs.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                string s = new StreamReader(ms).ReadToEnd();
                return s;
            }
            finally
            {
                ms.Close();
            }
        }

        private static string ReplaceLowOrderASCIICharacters(string tmp)
        {

            StringBuilder info = new StringBuilder();

            foreach (char cc in tmp)
            {

                int ss = (int)cc;

                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))

                    info.AppendFormat(" ", ss);

                else info.Append(cc);

            }

            return info.ToString();

        }
        public static T Deserialize<T>(string s)
        {
            if (!s.Contains("xml version"))
            {
                s = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" + s;
            }
            s = ReplaceLowOrderASCIICharacters(s);
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            try
            {
                s = s.Replace("\t", "");
                byte[] b = System.Text.Encoding.UTF8.GetBytes(s);
                ms.Write(b, 0, b.Length);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)xs.Deserialize(ms);
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
