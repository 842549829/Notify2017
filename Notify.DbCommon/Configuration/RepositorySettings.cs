using System.Configuration;

namespace Notify.DbCommon.Configuration
{
    /// <summary>
    /// 配置文件设置
    /// </summary>
    public class RepositorySettings : ConfigurationSection
    {
        /// <summary>
        /// 配置文件映射集合
        /// </summary>
        [ConfigurationProperty(RepositoryMappingConstants.ConfigurationPropertypeName, IsDefaultCollection = true)]
        public RepositoryMappingCollection RepositoryMappings => (RepositoryMappingCollection)base[RepositoryMappingConstants.ConfigurationPropertypeName];
    }
}