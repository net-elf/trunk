using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace net.ELF.DBTool
{
    [XmlRoot("Class")]
    public class PDObject
    {
        //<o:Class Id="o218">
        //<a:ObjectID>AB988705-4FF3-43D7-B507-CCFBFBE38099</a:ObjectID>
        //<a:Name>PID自动执行结果记录</a:Name>
        //<a:Code>PidAutoMissionLog</a:Code>
        //<a:CreationDate>1417506121</a:CreationDate>
        //<a:Creator>Administrator</a:Creator>
        //<a:ModificationDate>1417506287</a:ModificationDate>
        //<a:Modifier>Administrator</a:Modifier>
        //<a:UseParentNamespace>0</a:UseParentNamespace>

        [XmlAttribute("Id")]
        public string Id;
        [XmlElement("ObjectID")]
        public string ObjectID;
        [XmlElement("Name")]
        public string Name;
        [XmlElement("Code")]
        public string Code;
        [XmlElement("CreationDate")]
        public string CreationDate;
        [XmlElement("Creator")]
        public string Creator;
        [XmlElement("ModificationDate")]
        public string ModificationDate;
        [XmlElement("Modifier")]
        public string Modifier;
        [XmlElement("UseParentNamespace")]
        public string UseParentNamespace;
        [XmlArray("Attributes"), XmlArrayItem("Attribute")]
        public PDAttribute[] Attributes;

        [XmlIgnore]
        public PDObject Parent;
        public string ToCode()
        {
            string format = @"// File:    {0}.cs
// Author:  {6}
// Created: {5}
// Purpose: Definition of Class {0}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using {4}.Common.Enums;

namespace {4}.DB.Base
{{
    /// <summary>
    /// database object
    /// </summary>
    public class {7} {3}
    {{
{1}   
    }}
}}

namespace {4}.DB
{{
    public enum E{0}
    {{
{2}
    }}
}}";
            StringBuilder fields = new StringBuilder();
            foreach (PDAttribute a in Attributes)
            {
                fields.Append(a.ToCode());
            }
            string extendStr = "";
            string codeName = this.Code + "Base";
            if (Parent != null)
            {
                extendStr = ": " + Parent.Code;
                if (Parent.Code == Config.toolConfig.BaseTreeName)
                {
                    extendStr = string.Format(": {0}<T> where T : {0}Base", Parent.Code);
                    codeName = this.Code + "Base<T>";
                }
            }
            string enumStr = GetEnumStr();

            return string.Format(format, this.Code, fields.ToString(), enumStr, extendStr, Config.toolConfig.Namespace, this.CreationDate, this.Creator,codeName);
        }

        public string GetEnumStr()
        {
            StringBuilder enumStr = new StringBuilder();
            if (this.Parent != null)
                enumStr.Append(Parent.GetEnumStr());
            foreach (PDAttribute pda in this.Attributes)
            {
                enumStr.Append( pda.ToEnum());
            }
            return enumStr.ToString();
        }
    }

    [XmlRoot("Attribute")]
    public class PDAttribute
    {
        //<o:Attribute Id="o774">
        //<a:ObjectID>59BC867F-42AB-4999-891C-820409DDCC24</a:ObjectID>
        //<a:Name>计划</a:Name>
        //<a:Code>PAML_PlanID</a:Code>
        //<a:CreationDate>1417506123</a:CreationDate>
        //<a:Creator>Administrator</a:Creator>
        //<a:ModificationDate>1417507271</a:ModificationDate>
        //<a:Modifier>Administrator</a:Modifier>
        //<a:Comment>index</a:Comment>
        //<a:DataType>long</a:DataType>
        //<a:Attribute.Visibility>-</a:Attribute.Visibility>
        //</o:Attribute>
        [XmlAttribute("Id")]
        public string Id;
        [XmlElement("ObjectID")]
        public string ObjectID;
        [XmlElement("Name")]
        public string Name;
        [XmlElement("Code")]
        public string Code;
        [XmlElement("CreationDate")]
        public string CreationDate;
        [XmlElement("Creator")]
        public string Creator;
        [XmlElement("ModificationDate")]
        public string ModificationDate;
        [XmlElement("Modifier")]
        public string Modifier;
        [XmlElement("Comment")]
        public string Comment;
        [XmlElement("DataType")]
        public string DataType;
        [XmlElement("Attribute.Visibility")]
        public string Visibility;

        public string ToCode()
        {
            string format = @"
        private {3} {0};
        /// <summary>
        /// {2}
        /// </summary>
        public {3} {1} 
        {{ 
            get 
            {{ 
               return {0}; 
            }} 
            set 
            {{
                {0} = value; 
                if (RecordModify) ModifyProperties.Add(""{1}"");
            }} 
        }}
";
            return string.Format(format, GetFieldName(), Code, GetSummary(), DataType);
        }

        private string GetFieldName()
        {
            return Code.Substring(0, 1).ToLower() + Code.Substring(1);
        }

        private string GetSummary()
        {
            string summary = Name;
            if (!string.IsNullOrEmpty(Comment))
            {
                summary += "\r\n        ///" + Comment.Replace("\r\n","\n").Replace("\n", "\n        /// ");
            }
            return summary;
        }

        public string ToEnum()
        {
            string format = @"
        /// <summary>
        /// {1}
        /// </summary>
        {0},
";
            return string.Format(format, Code, GetSummary());
        }
    }

    [XmlRoot("Generalization")]
    public class PDGeneralization
    {
        //<o:Generalization Id="o160">
        //<a:ObjectID>7A59CA1B-74AC-4C9E-B24E-04B25EC1F20E</a:ObjectID>
        //<a:Name>Generalization_60</a:Name>
        //<a:Code>Generalization_60</a:Code>
        //<a:CreationDate>1417506286</a:CreationDate>
        //<a:Creator>Administrator</a:Creator>
        //<a:ModificationDate>1417506287</a:ModificationDate>
        //<a:Modifier>Administrator</a:Modifier>
        //<c:Object1>
        //<o:Class Ref="o162"/>
        //</c:Object1>
        //<c:Object2>
        //<o:Class Ref="o218"/>
        //</c:Object2>
        //</o:Generalization>

        [XmlAttribute("Id")]
        public string Id;
        [XmlElement("ObjectID")]
        public string ObjectID;
        [XmlArray("Object1"), XmlArrayItem("Class")]
        public PDGeneralizationClass[] Object1;
        [XmlArray("Object2"), XmlArrayItem("Class")]
        public PDGeneralizationClass[] Object2;
    }

    [XmlRoot("Class")]
    public class PDGeneralizationClass
    {
        [XmlAttribute("Ref")]
        public string Ref;
    }

    public class ObjectField
    {
        public string FieldName;
        public string FieldDataType;
        public string FieldLength;
        public bool IsIndex = false;
        public string Comment;
        public string Name;
        public string FieldCodeType;
    }

    public class DBFiled
    {
        public DBFiled(string typeName, string defaultValue)
        {
            this.TypeName = typeName;
            this.DefaultValue = defaultValue;
        }

        public string TypeName;
        public string DefaultValue;
        static Dictionary<string, DBFiled> dicNormalType;
        public static DBFiled GetDBField(string fieldType)
        {
            if (dicNormalType == null)
            {
                dicNormalType = new Dictionary<string, DBFiled>();
                dicNormalType.Add("string", new DBFiled("nvarchar", "''"));
                dicNormalType.Add("int", new DBFiled("int", "0"));
                dicNormalType.Add("double", new DBFiled("float", "0"));
                dicNormalType.Add("long", new DBFiled("bigint", "0"));
                dicNormalType.Add("DateTime", new DBFiled("datetime", "'1900-01-01'"));
                dicNormalType.Add("bool", new DBFiled("int", "0"));
            }
            if (dicNormalType.ContainsKey(fieldType))
                return dicNormalType[fieldType];
            else
                return new DBFiled("int", "0");
        }
    }

    public class DBObject
    {
        public string ObjectName;
        public List<ObjectField> FieldList;
        public string ToCreateSql()
        {

            string sql = @"create table {0} (
   Tid                  bigint               not null default 0,
   CreateId             bigint               null default 0,
   UpdateDate           datetime             null default '1900-1-1',
   UpdateId             bigint               null default 0,
   Status               int                  null default 0,
{1}
   constraint PK_{2} primary key (Tid)
)
go

";
            string fieldSQL = "";
            foreach (ObjectField o in FieldList)
            {
                fieldSQL += ToFieldSql(o) + ",\r\n";
            }
            string returnSql = string.Format(sql, ObjectName, fieldSQL.TrimEnd(new char[] { '\r', '\n' }), ObjectName.ToUpper());
            foreach (ObjectField o in FieldList)
            {
                returnSql += ToIndexSql(o);
            }
            return returnSql;
        }
        static string fieldSqlBase = "   {0} {1} null default {2}";
        private string ToFieldSql(ObjectField o)
        {
            DBFiled df = DBFiled.GetDBField(o.FieldDataType);
            string type = df.TypeName;
            if (df.TypeName == "nvarchar")
            {
                if (o.FieldLength == "text")
                    type = "text";
                else
                    type = string.Format("nvarchar({0})", string.IsNullOrEmpty(o.FieldLength) ? "100" : o.FieldLength);
            }
            return string.Format(fieldSqlBase, o.FieldName.PadRight(20), type.PadRight(20), df.DefaultValue);
        }
        static string addFieldBase = "alter table {0} add {1}";
        static string addUpdateFieldBase = "update {0} set {1} = {2}";
        public string ToAddFieldSql(ObjectField o)
        {
            DBFiled df = DBFiled.GetDBField(o.FieldDataType);
            string sql = string.Format(addFieldBase, this.ObjectName, ToFieldSql(o)) + "\r\ngo\r\n" +
                string.Format(addUpdateFieldBase, this.ObjectName, o.FieldName, df.DefaultValue + ";\r\n");
            return sql;
        }

        public string ToIndexSql(ObjectField o)
        {
            string sql = "";
            if (o.IsIndex)
                sql += string.Format("create index {0} on {1} ( {0} ASC ) \r\ngo\r\n", o.FieldName, this.ObjectName);
            return sql;
        }
    }
}
