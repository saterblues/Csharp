using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Csharp.Liqing.Config
{
    /// <summary>
    /// 用于ini格式读取,自动忽略空格
    /// 
    /// example,  path:  d:/liqing/config.ini
    /// [author]
    /// name=liqing
    /// age = 0
    /// 
    /// IniFileHelper config = new IniFileHelper("d:/liqing/config.ini");
    /// string name = config.GetValue("author","name");
    /// string age =  config.GetValue("author","age");
    /// </summary>
    public class IniFileHelper
    {
        #region inner class
        class Section 
        {
            private const int NOT_FOUND = -1;

            private List<string> lines = new List<string>();
            #region praivet functions
            private bool isKeyValue(string text, out string key, out string value)
            {
                key = "";
                value = "";
                char[] cs = text.ToCharArray();
                for (int i = 0; i < cs.Length; i++)
                {
                    if (cs[i] == '=' && i != 0 && i != cs.Length - 1)
                    {
                        key = text.Substring(0, i);
                        value = text.Substring(i + 1, text.Length - i - 1);

                        key = key.Trim();
                        value = value.Trim();

                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// 返回Section中的行号
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            private int GetValue(string key, out string value)
            {
                value = "";
                string k, v;
                for (int i = 0; i < lines.Count; i++)
                {
                    if (!isKeyValue(lines[i], out k, out v)) { continue; }
                    if (!k.Equals(key)) { continue; }
                    value = v;
                    return i;
                }
                return NOT_FOUND;
            } 
            #endregion

            public void AddLine(string text) {
                lines.Add(text);
            }

            public string GetValue(string key) {
                string value;
                GetValue(key, out value);
                return value;
            }

            public void Add(string key, string value)
            {
                string v;
                if (NOT_FOUND != GetValue(key, out v)) { return;}
                lines.Add(string.Format("{0}={1}",key,value));
            }

            public void Set(string key, string value) {
                string v;
                int index = GetValue(key, out v);
                if (NOT_FOUND != index)
                {
                    lines[index] = string.Format("{0}={1}", key, value);
                }
                else {
                    lines.Add(string.Format("{0}={1}", key, value));
                }
            }

            public void Remove(string key)
            {
                string value;
                int index = GetValue(key, out value);
                if (NOT_FOUND != index) {
                    lines.Remove(lines[index]);
                }
            }

            public List<string>.Enumerator GetEnumerator() {
                return lines.GetEnumerator();
            }
           
        } 
        #endregion

        #region private menbers
        private string path = ""; 
        #endregion

        #region constructor
        /// <summary>
        /// 创建类后自动读取分析ini文件, 如果文件不存在, 会抛出异常
        /// </summary>
        /// <param name="path"></param>
        public IniFileHelper(string path)
        {
            this.path = path;
            ReadConfigFile();
        } 
        #endregion

        #region private functions
        private Dictionary<string, Section> sections = new Dictionary<string, Section>();

        private void ReadConfigFile()
        {

            using (StreamReader reader = new StreamReader(File.Open(path, FileMode.Open)))
            {
                Section currentSection = null;
                while (!reader.EndOfStream)
                {
                    string text = reader.ReadLine();
                    string sectionName = "";
                    if (isSection(text, out sectionName))
                    {
                        sectionName = sectionName.Trim();
                        currentSection = GetOrCreateSection(sectionName);
                        continue;
                    }

                    if (currentSection == null) { continue; }
                    currentSection.AddLine(text);
                    
                }
            }
        }

        private bool isSection(string text, out string sectionName)
        {
            sectionName = "";
            if (string.IsNullOrEmpty(text)) { return false; }
            if (text[0] == '[' && text[text.Length - 1] == ']')
            {
                sectionName = text.Substring(1, text.Length - 2);
                return true;
            }
            return false;
        }

        private Section GetOrCreateSection(string sectionName)
        {
            if (sections.ContainsKey(sectionName))
            {
                return sections[sectionName];
            }
            Section s = new Section();
            sections.Add(sectionName, s);
            return s;
        } 
        #endregion

        #region public functions
        public void UpdateConfigFile()
        {
            using (StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Truncate)))
            {
                foreach (var section in sections)
                {
                    writer.WriteLine(string.Format("[{0}]", section.Key));
                    foreach (var line in section.Value)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }

        public void AddSection(string sectionName)
        {
            sectionName = sectionName.Trim();
            if (sections.ContainsKey(sectionName))
            {
                return;
            }
            sections.Add(sectionName, new Section());
        }

        public string GetValue(string sectionName, string key)
        {
            sectionName = sectionName.Trim();
            key = key.Trim();
            if (!sections.ContainsKey(sectionName))
            {
                return "";
            }
            return sections[sectionName].GetValue(key);
        }

        /// <summary>
        /// If The Key Not Exsits,Add Key
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetKeyValue(string sectionName, string key, string value)
        {
            sectionName = sectionName.Trim();
            key = key.Trim();
            value = value.Trim();
            Section tmp = GetOrCreateSection(sectionName);
            tmp.Set(key, value);
        }
        /// <summary>
        /// If The Key Exsits,Ignore Operation
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddKeyValue(string sectionName, string key, string value)
        {
            sectionName = sectionName.Trim();
            key = key.Trim();
            value = value.Trim();
            Section tmp = GetOrCreateSection(sectionName);
            tmp.Add(key, value);
        } 
        #endregion

       
    }
}
