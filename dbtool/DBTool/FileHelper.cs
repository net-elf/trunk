using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.ELF.DBTool
{
    public class FileHelper
    {
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public static void WriteFile(string path, string text, bool append,Encoding encoding)
        {
            StreamWriter sw = new StreamWriter(path, append, encoding);
            try
            {
                sw.Write(text);
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            if (!System.IO.File.Exists(path))
                return "";
            StreamReader sr = new StreamReader(path);
            try
            {
                return sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
            }
        }

        public static string ReadFile(string path,Encoding encoding)
        {
            if (!System.IO.File.Exists(path))
                return "";
            StreamReader sr = new StreamReader(path,encoding);
            try
            {
                return sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
            }
        }
    }
}
