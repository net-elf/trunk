using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace net.ELF.DBTool
{
    public partial class Form1 : Form
    {
        private static Form1 instance;

        public static Form1 Instance
        {
            get
            {
                if (instance == null)
                    instance = new Form1();
                return instance;
            }
        }

        private Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 所有sql
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            SaveConfig();
            StringBuilder sql = new StringBuilder();

            List<DBObject> l = GetAllDBObject(Config.toolConfig.DBBasePath);
            foreach (DBObject d in l)
            {
                sql.Append(d.ToCreateSql());
            }
            Form2 f = new Form2();
            f.Show();
            f.SetShow(sql.ToString());
        }

        private List<DBObject> GetAllDBObject(string dir)
        {
            List<DBObject> l = new List<DBObject>();
            DirectoryInfo d = new DirectoryInfo(dir);
            List<FileInfo> allFile = ListFiles(d, ".cs");
            foreach (FileInfo fi in allFile)
            {
                StreamReader fs = new StreamReader(fi.FullName, Encoding.Default);
                try
                {
                    DBObject dbo = GetDBObject(fs.ReadToEnd());
                    if (string.IsNullOrEmpty(dbo.ObjectName))
                        continue;
                    if (dbo.FieldList.Count == 0)
                        continue;
                    l.Add(dbo);
                }
                finally
                {
                    fs.Close();
                }
            }
            return l;
        }

        public static List<FileInfo> ListFiles(FileSystemInfo info, string searchSuffix)
        {
            if (!info.Exists)
                return new List<FileInfo>();
            DirectoryInfo dir = info as DirectoryInfo;      //不是目录      
            if (dir == null)
                return new List<FileInfo>();
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            List<FileInfo> list = new List<FileInfo>();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;        //是文件       
                if (file != null)
                {
                    if (!string.IsNullOrEmpty(searchSuffix) && file.Name.EndsWith(searchSuffix))
                        list.Add(file);
                }
                else
                    list.AddRange(ListFiles(files[i], searchSuffix));
            }
            return list;
        }       

        Regex regFiled = new Regex("(?<=public )([a-z!enum]{1,}) [a-z_0-9]{1,}", RegexOptions.IgnoreCase);


        private DBObject GetDBObject(string code)
        {
            if (!code.Contains("/// database object"))//DB标识
                return new DBObject();

            DBObject o = new DBObject();
            List<ObjectField> fileds = new List<ObjectField>();
            MatchCollection mc = regFiled.Matches(code);
            foreach (Match m in mc)
            {
                string[] typeAndName = m.Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string type = typeAndName[0];
                string name = typeAndName[1];
                if (type == "enum")
                    continue;
                if (type == "class")
                    o.ObjectName = name.EndsWith("Base") ? name.Substring(0, name.Length - 4) : name;
                else
                {
                    ObjectField of = new ObjectField();
                    of.FieldName = name;
                    of.FieldDataType = type;
                    /* /// 长度100
        private string cA_Account;
                     */
                    Regex regSummary = new Regex("(?<=///[ ]*<summary>)[^<]*(?=///[ ]*</summary>(\\r\\n|\\n)[ ]*public " + type + " " + name + ")", RegexOptions.IgnoreCase);
                    string summary = regSummary.Match(code).Value;
                    Regex regFiledLength = new Regex("(?<=///[ ]*长度)[0-9a-z]{1,}", RegexOptions.IgnoreCase);
                    Regex regFiledLength3 = new Regex("(?<=/// )TEXT", RegexOptions.IgnoreCase);
                    Regex regIndex = new Regex("(?<=/// )index", RegexOptions.IgnoreCase);
                    if (regFiledLength.IsMatch(summary))
                        of.FieldLength = regFiledLength.Match(summary).Value;
                    else if (regFiledLength3.IsMatch(summary))
                        of.FieldLength = "text";
                    if (regIndex.IsMatch(summary))
                        of.IsIndex = true;
                    fileds.Add(of);
                }
            }
            o.FieldList = fileds;
            return o;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Config.toolConfig.PDPath))
                PDFilePath.Text = Config.toolConfig.PDPath;
            if (!string.IsNullOrEmpty(Config.toolConfig.DBBasePath))
                CodeFilePath.Text = Config.toolConfig.DBBasePath;
            if (!string.IsNullOrEmpty(Config.toolConfig.DBConnectStr))
                DBConnectStr.Text = Config.toolConfig.DBConnectStr;
            if (!string.IsNullOrEmpty(Config.toolConfig.Namespace))
                DefaultNamespace.Text = Config.toolConfig.Namespace;
            if (!string.IsNullOrEmpty(Config.toolConfig.BaseObjectName))
                DefaultBaseObject.Text = Config.toolConfig.BaseObjectName;
            if (!string.IsNullOrEmpty(Config.toolConfig.BaseTreeName))
                DefaultTreeBaseName.Text = Config.toolConfig.BaseTreeName;
        }

        /// <summary>
        /// 代码-数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            SaveConfig();
            CodeToSql();
        }

        private List<DBObject> GetDBTables()
        {
            List<DBObject> l = new List<DBObject>();
            SqlServer server = new SqlServer(Config.toolConfig.DBConnectStr, 50);
            DataTable dt = server.FillDataTable("SELECT OBJECT_NAME (id) FROM sysobjects WHERE xtype = 'U' AND OBJECTPROPERTY (id, 'IsMSShipped') = 0", 0);
            foreach (DataRow dr in dt.Rows)
            {
                DBObject dbo = new DBObject();
                dbo.ObjectName = dr[0].ToString();
                DataTable dtField = server.FillDataTable("select * from syscolumns where id= OBJECT_ID('" + dbo.ObjectName + "')", 0);
                dbo.FieldList = new List<ObjectField>();
                foreach (DataRow dr1 in dtField.Rows)
                {
                    ObjectField of = new ObjectField();
                    of.FieldName = dr1[0].ToString();
                    dbo.FieldList.Add(of);
                }
                l.Add(dbo);
            }
            server.Close();
            return l;
        }

        public void ExecuteSql(string sql)
        {
            SqlServer server = new SqlServer(Config.toolConfig.DBConnectStr, 50);
            server.ExecuteNonQuery(sql, 0);
            server.Close();
        }

        private void SaveConfig()
        {
            Config.toolConfig.DBBasePath = CodeFilePath.Text.Trim();
            Config.toolConfig.DBConnectStr = DBConnectStr.Text.Trim();
            Config.toolConfig.PDPath = PDFilePath.Text.Trim();
            Config.toolConfig.BaseObjectName = DefaultBaseObject.Text.Trim();
            Config.toolConfig.Namespace = DefaultNamespace.Text.Trim();
            Config.toolConfig.BaseTreeName = DefaultTreeBaseName.Text.Trim();
            Config.SaveConfig();
        }

        /// <summary>
        /// PD-代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveConfig();

            PDToCode();

            MessageBox.Show("完成");
        }

        /// <summary>
        /// PD到代码
        /// </summary>
        private void PDToCode()
        {
            string allpd = FileHelper.ReadFile(Config.toolConfig.PDPath, Encoding.UTF8);
            //对象
            Regex regClass = new Regex("<o:Class Id[^>]*>.+?</o:Class>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection mc = regClass.Matches(allpd);
            Dictionary<string, PDObject> dicPD = new Dictionary<string, PDObject>();
            List<PDObject> listPD = new List<PDObject>();
            foreach (Match m in mc)
            {
                PDObject pd = null;
                try { pd = XMLHelper.Deserialize<PDObject>(ReplaceXMLAttribute(m.Value)); }
                catch (Exception ex)
                {
                    continue;
                }
                
                listPD.Add(pd);
                dicPD.Add(pd.Id, pd);
            }

            //继承关系
            Regex regGeneralization = new Regex("<o:Generalization Id[^>]*>.+?</o:Generalization>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection mg = regGeneralization.Matches(allpd);
            List<PDGeneralization> listPDG = new List<PDGeneralization>();
            foreach (Match m in mg)
            {
                PDGeneralization pdg = null;
                try { pdg = XMLHelper.Deserialize<PDGeneralization>(ReplaceXMLAttribute(m.Value)); }
                catch (Exception ex)
                {
                    continue;
                }
                listPDG.Add(pdg);
                if (pdg.Object1 != null && pdg.Object1.Length > 0 && pdg.Object2 != null && pdg.Object2.Length > 0)
                {
                    if (dicPD.ContainsKey(pdg.Object2[0].Ref) && dicPD.ContainsKey(pdg.Object1[0].Ref))
                        dicPD[pdg.Object2[0].Ref].Parent = dicPD[pdg.Object1[0].Ref];
                }
            }

            PDObject parent = null;
            foreach (PDObject pd in listPD)
            {
                if (Config.toolConfig.BaseObjectName.Contains(pd.Code))
                    parent = pd;
            }
            foreach (PDObject pd in listPD)
            {
                if (Config.toolConfig.BaseObjectName.Contains(pd.Code))//最高继承对象不重写文件
                    continue;

                string path = Config.toolConfig.DBBasePath + "\\" + pd.Code + ".cs";
                FileHelper.WriteFile(path, pd.ToCode(), false, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 代码到数据库
        /// </summary>
        private void CodeToSql()
        {
            List<DBObject> lCode = GetAllDBObject(Config.toolConfig.DBBasePath);
            List<DBObject> lDB = GetDBTables();
            StringBuilder sql = new StringBuilder();
            foreach (DBObject dbo in lCode)
            {
                if (dbo.ObjectName == Config.toolConfig.BaseTreeName || Config.toolConfig.BaseObjectName.Contains(dbo.ObjectName))//默认继承对象不管
                    continue;

                DBObject[] temp = (from d in lDB where dbo.ObjectName == d.ObjectName select d).ToArray();
                if (temp.Length == 0)
                    sql.Append(dbo.ToCreateSql());
                else
                {
                    foreach (ObjectField df in dbo.FieldList)
                    {
                        ObjectField[] temp1 = (from f in temp[0].FieldList where df.FieldName == f.FieldName select f).ToArray();
                        if (temp1.Length == 0)
                        {
                            sql.Append(dbo.ToAddFieldSql(df));
                            sql.Append(dbo.ToIndexSql(df));
                        }
                    }
                }
            }
            Form2 form = new Form2();
            form.Show();
            form.SetShow(sql.ToString());
        }

        private string ReplaceXMLAttribute(string s)
        {
            return s.Replace("<o:", "<").Replace("</o:", "</").Replace("<a:", "<").Replace("</a:", "</").Replace("<c:", "<").Replace("</c:", "</");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveConfig();

            PDToCode();

            CodeToSql();
        }

    }
}
