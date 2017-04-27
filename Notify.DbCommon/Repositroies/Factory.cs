using System;
using System.Collections.Generic;
using System.Configuration;
using Notify.DbCommon.Configuration;

namespace Notify.DbCommon.Repositroies
{
    /// <summary>
    /// 仓储工厂
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// 对象缓存字典
        /// </summary>
        private static readonly Dictionary<string, object> MRespository = new Dictionary<string, object>();

        /// <summary>
        /// 反射工厂
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <returns>结果</returns>
        public static T GetFactory<T>()
            where T : class
        {
            T respository = default(T);
            string interfaceShortName = typeof(T).Name;
            if (!MRespository.ContainsKey(interfaceShortName))
            {
                RepositorySettings settings = (RepositorySettings)ConfigurationManager.GetSection(RepositoryMappingConstants.RepositoryMappingsConfigurationSectionName);
                string repositoryFullTypeName = settings.RepositoryMappings[interfaceShortName].RepositoryFullTypeName;
                Type type = Type.GetType(repositoryFullTypeName);
                if (type != null)
                {
                    respository = Activator.CreateInstance(type) as T;
                    MRespository.Add(interfaceShortName, respository);
                }
            }
            else
            {
                respository = (T)MRespository[interfaceShortName];
            }

            return respository;
        }
    }
}