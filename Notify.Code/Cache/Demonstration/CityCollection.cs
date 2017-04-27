namespace Notify.Code.Cache.Demonstration
{
    #region 缓存数据集
    /// <summary>
    /// 缓存数据集(RepositoryCache: key:城市代码，value:City对象)
    /// </summary>
    internal class CityCollection : RepositoryCache<string, City>
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// 实例
        /// </summary>
        private static CityCollection instance;

        /// <summary>
        /// 单列模式(创建实例)
        /// </summary>
        public static CityCollection Instance
        {
            get
            {
                if (null != instance)
                {
                    return instance;
                }

                lock (Locker)
                {
                    return instance ?? (instance = new CityCollection());
                }
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="CityCollection"/> class from being created.
        /// </summary>
        private CityCollection()
            : base(Factory.CreateCityRepository(), 1000)
        {
        }
    } 
    #endregion

    #region 数据库底层仓储接口(可分层到其他dll)
    /// <summary>
    /// 仓储接口
    /// </summary>
    internal interface ICityRepository : RepositoryCache<string, City>.IRepository
    {
    } 
    #endregion

    #region 创建数据库底层仓储工厂方法(可分层到其他dll)
    /// <summary>
    /// 工作方法
    /// </summary>
    internal static class Factory
    {
        internal static ICityRepository CreateCityRepository()
        {
            return new CityRepository();
        }
    } 
    #endregion

    #region 数据持久化到数据库(可分层到其他dll)
    /// <summary>
    /// 持久化数据层
    /// </summary>
    internal class CityRepository : ICityRepository
    {
        #region IRepository 成员

        public System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, City>> Query()
        {
            throw new System.NotImplementedException();
        }

        public object Insert(City value)
        {
            throw new System.NotImplementedException();
        }

        public void Modify(City value)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(City value)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    } 
    #endregion
}
