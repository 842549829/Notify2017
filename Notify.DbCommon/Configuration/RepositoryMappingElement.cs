using System.Configuration;

namespace Notify.DbCommon.Configuration
{
    /// <summary>
    /// 配置文件映射元素
    /// </summary>
    public sealed class RepositoryMappingElement : ConfigurationElement
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        [ConfigurationProperty(RepositoryMappingConstants.InterfaceShortTypeNameAttributeName, IsKey = true,
            IsRequired = true)]
        public string InterfaceShortTypeName
        {
            get
            {
                return (string) this[RepositoryMappingConstants.InterfaceShortTypeNameAttributeName];
            }

            set
            {
                this[RepositoryMappingConstants.InterfaceShortTypeNameAttributeName] = value;
            }
        }

        /// <summary>
        /// 全称
        /// </summary>
        [ConfigurationProperty(RepositoryMappingConstants.RepositoryFullTypeNameAttributeName, IsRequired = true)]
        public string RepositoryFullTypeName
        {
            get
            {
                return (string) this[RepositoryMappingConstants.RepositoryFullTypeNameAttributeName];
            }

            set
            {
                this[RepositoryMappingConstants.RepositoryFullTypeNameAttributeName] = value;
            }
        }
    }
}