namespace Notify.DbCommon.Configuration
{
    /// <summary>
    /// 配置节点名称
    /// </summary>
    public class RepositoryMappingConstants
    {
        /// <summary>
        /// 集合映射节点
        /// </summary>
        public const string ConfigurationPropertypeName = "repositoryMappings";

        /// <summary>
        /// 映射节点
        /// </summary>
        public const string ConfigurationElementName = "repositoryMapping";

        /// <summary>
        /// 接口名称映射节点属性
        /// </summary>
        public const string InterfaceShortTypeNameAttributeName = "interfaceShortTypeName";

        /// <summary>
        /// 接口实现全称映射节点属性
        /// </summary>
        public const string RepositoryFullTypeNameAttributeName = "repositoryFullTypeName";

        /// <summary>
        /// 配置文件映射节点
        /// </summary>
        public const string RepositoryMappingsConfigurationSectionName = "repositoryMappingsConfiguration";
    }
}