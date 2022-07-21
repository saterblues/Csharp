using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Csharp.Liqing.Sql.SqlServerProfilterHelper
{
    /// <summary>
    /// 配合SqlServerProfilter使用的工具，对导出的XML过滤，获取表明等信息，方便跟踪表记录,XML记录如下例,多条xml中有多列涉及表的语句
    /// ...
    /// <Column id="1" name="TextData">Select FValue from t_SystemProfile where FCategory='BOS' and FKey='AccessDataUsed'</Column>
    /// ...
    /// 共同点 <Column id="1" name="TextData">
    /// </summary>
    /// 

    public class SqlServerProfilterXmlHelper
    {
        private HashSet<string> tables = new HashSet<string>();
        private HashSet<string> storedProcedures = new HashSet<string>();

        public IEnumerable<string> GetTables(){ 
            return tables; 
        }
        public IEnumerable<string> GetStoreProcedures() {
            return storedProcedures;
        }

        public static bool IsUpdateOrFromOrJoinOrInto(string keywords)
        {
            return string.Compare(keywords, "join", StringComparison.OrdinalIgnoreCase) == 0 ||
                   string.Compare(keywords, "from", StringComparison.OrdinalIgnoreCase) == 0 ||
                   string.Compare(keywords, "into", StringComparison.OrdinalIgnoreCase) == 0 ||
                   string.Compare(keywords, "update", StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool IsStoredProcedure(string keywords) {
            return string.Compare(keywords, "exec",StringComparison.OrdinalIgnoreCase) == 0;
        }

        public void ReadXml(string xmlfilePath) {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.Load(xmlfilePath);
            XmlNodeList list = doc.GetElementsByTagName("Column");
            foreach (XmlNode item in list)
            {
                XmlAttribute attr = item.Attributes["name"];
                if (attr == null || string.Compare(attr.Value, "TextData", StringComparison.OrdinalIgnoreCase) != 0)
                { continue; }

                string[] arr = item.InnerText.Split(' ','\r','\n');

                for (int i = 0; i < arr.Length; i++)
                {
                    if (IsUpdateOrFromOrJoinOrInto(arr[i])  && i < arr.Length - 2 && !string.IsNullOrEmpty(arr[i+1]))
                    {
                        tables.Add(arr[i + 1]);
                    }

                    if (IsStoredProcedure(arr[i])) {
                        string text = "";
                        for (int j = i; j < arr.Length; j++) {text += arr[j] + " ";}
                        storedProcedures.Add(text);
                        break;
                    }
                }
            }
        }

    }
}
