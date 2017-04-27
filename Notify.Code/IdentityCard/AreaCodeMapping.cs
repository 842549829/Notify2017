using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Notify.Code.IdentityCard
{
    /// <summary>
    /// 区域配置
    /// </summary>
    public class AreaCodeMapping
    {
        /// <summary>
        /// 区域字典
        /// </summary>
        private static readonly Dictionary<string, Area> areas;

        /// <summary>
        /// Initializes static members of the <see cref="AreaCodeMapping"/> class. 
        /// </summary>
        static AreaCodeMapping()
        {
            areas = LoadAreaInfo();
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        /// <returns>区域信息</returns>
        private static Dictionary<string, Area> LoadAreaInfo()
        {
            XmlDocument doc = LoadXmlDocument("AreaCodes.xml");
            XmlNode areasNode = doc.SelectSingleNode("AreaCode");
            if (areasNode != null)
            {
                XmlNodeList provinceNodeList = areasNode.ChildNodes;
                return LoadProvinces(provinceNodeList);
            }

            return null;
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>XmlDocument</returns>
        private static XmlDocument LoadXmlDocument(string fileName)
        {
            var declaringType = MethodBase.GetCurrentMethod().DeclaringType;
            if (declaringType != null)
            {
                string resourceName = declaringType.Namespace + "." + fileName;
                Assembly assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream(resourceName);
                XmlDocument result = new XmlDocument();
                if (stream != null)
                {
                    result.Load(stream);
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 解析XML节点
        /// </summary>
        /// <param name="provinceNodeList">provinceNodeList</param>
        /// <returns>结果</returns>
        private static Dictionary<string, Area> LoadProvinces(XmlNodeList provinceNodeList)
        {
            Dictionary<string, Area> result = new Dictionary<string, Area>();
            foreach (XmlNode provinceNode in provinceNodeList)
            {
                string code = GetAttribute(provinceNode, "code");
                string name = GetAttribute(provinceNode, "name");
                Area province = new Area(code, name, null);
                var cities = LoadCities(province, provinceNode.ChildNodes);
                foreach (var city in cities)
                {
                    province.AppendChild(city);
                }
                result.Add(code, province);
            }
            return result;
        }

        /// <summary>
        /// 加载城市
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="cityNodeList">节点</param>
        /// <returns>结果</returns>
        private static IEnumerable<Area> LoadCities(Area province, XmlNodeList cityNodeList)
        {
            List<Area> result = new List<Area>();
            if (cityNodeList != null)
            {
                foreach (XmlNode cityNode in cityNodeList)
                {
                    string code = GetAttribute(cityNode, "code");
                    string name = GetAttribute(cityNode, "name");
                    Area city = new Area(code, name, province);
                    var counties = loadCounties(city, cityNode.ChildNodes);
                    foreach (var county in counties)
                    {
                        city.AppendChild(county);
                    }
                    result.Add(city);
                }
            }
            return result;
        }

        /// <summary>
        /// 加载区域
        /// </summary>
        /// <param name="city">市</param>
        /// <param name="countyNodeList">节点</param>
        /// <returns>结果</returns>
        private static IEnumerable<Area> loadCounties(Area city, XmlNodeList countyNodeList)
        {
            List<Area> result = new List<Area>();
            if (countyNodeList != null)
            {
                foreach (XmlNode countyNode in countyNodeList)
                {
                    string code = GetAttribute(countyNode, "code");
                    string name = GetAttribute(countyNode, "name");
                    Area county = new Area(code, name, city);
                    result.Add(county);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取节点属性
        /// </summary>
        /// <param name="node">node</param>
        /// <param name="attributeName">attributeName</param>
        /// <returns>结果</returns>
        private static string GetAttribute(XmlNode node, string attributeName)
        {
            if (node.Attributes != null)
            {
                XmlAttribute attribute = node.Attributes[attributeName];
                return attribute == null ? string.Empty : attribute.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取区域信息
        /// </summary>
        /// <param name="areaCode">区域代码</param>
        /// <returns>结果</returns>
        public static AreaInformation GetArea(string areaCode)
        {
            Area targetArea = null;
            if (!string.IsNullOrWhiteSpace(areaCode) && areaCode.Length == 6)
            {
                string provinceCode = areaCode.Substring(0, 2);
                if (areas.ContainsKey(provinceCode))
                {
                    var province = areas[provinceCode];
                    string cityCode = areaCode.Substring(2, 2);
                    if (province.ContainsChild(cityCode))
                    {
                        var city = province.GetChild(cityCode);
                        string countyCode = areaCode.Substring(4);
                        if (city.ContainsChild(countyCode))
                        {
                            targetArea = city.GetChild(countyCode);
                        }
                        else
                        {
                            targetArea = city;
                        }
                    }
                    else if (province.ContainsChild(areaCode.Substring(2)))
                    {
                        targetArea = province.GetChild(areaCode.Substring(2));
                    }
                    else
                    {
                        targetArea = province;
                    }
                }
            }
            return targetArea == null ? null : targetArea.ToAreaInformation();
        }
    }

    /// <summary>
    /// 区域
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 子区域
        /// </summary>
        private readonly Dictionary<string, Area> childrenDic;

        /// <summary>
        /// 区域集
        /// </summary>
        private readonly List<Area> childrenList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Area"/> class. 
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="name">名称</param>
        /// <param name="parent">父区域</param>
        internal Area(string code, string name, Area parent)
        {
            this.Info = new CodeNameMapping(code, name);
            this.Parent = parent;
            this.childrenDic = new Dictionary<string, Area>();
            this.childrenList = new List<Area>();
        }

        /// <summary>
        /// 代码名称映射信息
        /// </summary>
        public CodeNameMapping Info
        {
            get;
            private set;
        }

        /// <summary>
        /// 父区域
        /// </summary>
        public Area Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// 子区域
        /// </summary>
        public ReadOnlyCollection<Area> Children
        {
            get
            {
                return this.childrenList.AsReadOnly();
            }
        }

        /// <summary>
        /// 区域集是否包含
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>结果</returns>
        internal bool ContainsChild(string code)
        {
            return this.childrenDic.ContainsKey(code);
        }

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>区域</returns>
        internal Area GetChild(string code)
        {
            return this.childrenDic[code];
        }

        /// <summary>
        /// 父亲区域
        /// </summary>
        internal Area TopParent
        {
            get
            {
                return this.Parent == null ? this : this.Parent.TopParent;
            }
        }

        /// <summary>
        /// 添加子区域
        /// </summary>
        /// <param name="child">子节点</param>
        internal void AppendChild(Area child)
        {
            if (!this.childrenDic.ContainsKey(child.Info.Code))
            {
                this.childrenDic.Add(child.Info.Code, child);
                this.childrenList.Add(child);
            }
        }

        /// <summary>
        /// 区域信息转化
        /// </summary>
        /// <returns>区域信息</returns>
        internal AreaInformation ToAreaInformation()
        {
            CodeNameMapping province = this.TopParent.Info;
            CodeNameMapping city = default(CodeNameMapping);
            CodeNameMapping county = default(CodeNameMapping);
            if (this.Parent != null)
            {
                if (this.Parent.Info == province)
                {
                    city = this.Info;
                }
                else
                {
                    city = this.Parent.Info;
                    county = this.Info;
                }
            }
            return new AreaInformation(province, city, county);
        }
    }

    /// <summary>
    /// 区域信息
    /// </summary>
    public class AreaInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AreaInformation"/> class. 
        /// </summary>
        /// <param name="province">省</param>
        /// <param name="city">市</param>
        /// <param name="county">区</param>
        public AreaInformation(CodeNameMapping province, CodeNameMapping city, CodeNameMapping county)
        {
            this.Province = province;
            this.City = city;
            this.County = county;
        }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get
            {
                return this.Province.Code + this.City.Code + this.County.Code;
            }
        }

        /// <summary>
        /// 省
        /// </summary>
        public CodeNameMapping Province
        {
            get;
            private set;
        }

        /// <summary>
        /// 市
        /// </summary>
        public CodeNameMapping City
        {
            get;
            private set;
        }

        /// <summary>
        /// 区
        /// </summary>
        public CodeNameMapping County
        {
            get;
            private set;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string FullName
        {
            get
            {
                return this.Province.Name + this.City.Name + this.County.Name;
            }
        }

        /// <summary>
        /// 重写ToString
        /// </summary>
        /// <returns>结果</returns>
        public override string ToString()
        {
            return this.FullName;
        }
    }

    /// <summary>
    /// 代码名称映射
    /// </summary>
    public struct CodeNameMapping
    {
        /// <summary>
        /// 代码
        /// </summary>
        private readonly string code;

        /// <summary>
        /// 名称
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeNameMapping"/> struct. 
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="name">名称</param>
        internal CodeNameMapping(string code, string name)
        {
            this.code = code;
            this.name = name;
        }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get { return this.code; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// 重写比较
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>结果</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is CodeNameMapping)
            {
                return ((CodeNameMapping)obj).Code == this.Code;
            }
            return false;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }

        /// <summary>
        /// 相等比较器
        /// </summary>
        /// <param name="left">left</param>
        /// <param name="right">right</param>
        /// <returns>结果</returns>
        public static bool operator ==(CodeNameMapping left, CodeNameMapping right)
        {
            return left.Code != right.Code;
        }

        /// <summary>
        /// 不相等比较器
        /// </summary>
        /// <param name="left">left</param>
        /// <param name="right">right</param>
        /// <returns>结果</returns>
        public static bool operator !=(CodeNameMapping left, CodeNameMapping right)
        {
            return left.Code != right.Code;
        }
    }
}