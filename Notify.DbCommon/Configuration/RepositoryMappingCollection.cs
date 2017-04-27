using System.Configuration;
using System.Linq;

namespace Notify.DbCommon.Configuration
{
    /// <summary>
    /// 映射集合
    /// </summary>
    public sealed class RepositoryMappingCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新的节点元素
        /// </summary>
        /// <returns>节点元素</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RepositoryMappingElement();
        }

        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>结果</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RepositoryMappingElement)element).InterfaceShortTypeName;
        }

        /// <summary>
        /// 映射集合类型
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        /// <summary>
        /// 元素名称
        /// </summary>
        protected override string ElementName => RepositoryMappingConstants.ConfigurationElementName;

        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>结果</returns>
        public RepositoryMappingElement this[int index]
        {
            get
            {
                return (RepositoryMappingElement)this.BaseGet(index);
            }

            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemove(index);
                }

                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 获取配置节点
        /// </summary>
        /// <param name="interfaceShorTypeName">名称</param>
        /// <returns>结果</returns>
        public new RepositoryMappingElement this[string interfaceShorTypeName] => (RepositoryMappingElement)this.BaseGet(interfaceShorTypeName);

        /// <summary>
        /// 是否存在keyName
        /// </summary>
        /// <param name="keyName">keyName</param>
        /// <returns>结果</returns>
        public bool ContainsKey(string keyName)
        {
            object[] keys = this.BaseGetAllKeys();
            return keys.Any(key => (string)key == keyName);
        }
    }
}