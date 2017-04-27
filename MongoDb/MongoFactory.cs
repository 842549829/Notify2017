using MongoDB.Driver;

namespace MongoDb
{
    /// <summary>
    /// mongodb工厂
    /// </summary>
    public class MongoFactory
    {
        /// <summary>
        /// 获取mongodb数据连接
        /// </summary>
        /// <param name="dataBaseName">数据库名</param>
        /// <returns>数据连接基础类</returns>
        public static MongoDatabase GetMongoDatabase(string dataBaseName)
        {
            MongoServerSettings mongoServerSettings = new MongoServerSettings
            {
                Server = new MongoServerAddress("127.0.0.1", 27017),
                MaxConnectionPoolSize = 20,
                MinConnectionPoolSize = 5,
            };
            MongoServer mongoServer = new MongoServer(mongoServerSettings);
            return mongoServer.GetDatabase(dataBaseName);
        }

        /// <summary>
        ///  获取字典数据库
        /// </summary>
        /// <returns>数据连接基础类</returns>
        public static MongoDatabase GetMongoDatabaseByDic()
        {
            const string dbName = "Dic"; // 可以写成配置文件
            MongoDatabase mongoDatabase = GetMongoDatabase(dbName);
            return mongoDatabase;
        }
    }
}