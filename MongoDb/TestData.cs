using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDb
{
    /// <summary>
    /// 测试数据
    /// </summary>
    public class TestData
    {
        /// <summary>
        /// 初始化字典数据
        /// </summary>
        public static void InitDicData()
        {
            var data = Dic.Default;
            string code = "01234";
            DateTime tempTime = GetLastUpdateTime(code);
            if (tempTime != default(DateTime))
            {
                UpdateData(data);
            }
            else
            {
                AddData(data);
            }
            // 更新更新时间
            SetUpdateTime(code, DateTime.Now);
        }

        /// <summary>
        /// 根据key查看数据
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>数据</returns>
        public static List<Dic> ViewTheData(string key)
        {
            // 初始化数据库
            MongoDatabase mongoDatabase = MongoFactory.GetMongoDatabaseByDic();
            // 初始化表
            MongoCollection mongoCollection = mongoDatabase.GetCollection("dicTable");
            List<IMongoQuery> queryList = new List<IMongoQuery>
            {
                Query.EQ("Key", key)
            };
            IMongoQuery query = Query.And(queryList);
            List<Dic> dics = mongoCollection.FindAs<Dic>(query).ToList();
            return dics;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">数据</param>
        public static void AddData(List<Dic> data)
        {
            // 初始化数据库
            MongoDatabase mongoDatabase = MongoFactory.GetMongoDatabaseByDic();
            // 初始化表
            MongoCollection mongoCollection = mongoDatabase.GetCollection("dicTable");
            // 插入数据
            foreach (var item in data)
            {
                mongoCollection.Insert(item);
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="data">数据</param>
        public static void UpdateData(List<Dic> data)
        {
            // 初始化数据库
            MongoDatabase mongoDatabase = MongoFactory.GetMongoDatabaseByDic();
            // 初始化表
            MongoCollection mongoCollection = mongoDatabase.GetCollection("dicTable");
            foreach (var item in data)
            {
                var query = GetQueryCondition(item);
                mongoCollection.Remove(query);
                mongoCollection.Insert(item);
            }
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="item">dic</param>
        /// <returns>查询条件</returns>
        public static IMongoQuery GetQueryCondition(Dic item)
        {
            // 时间列子
            // queryList.Add(Query.EQ("列名", new BsonDateTime(DateTime.Now)));
            List<IMongoQuery> queryList = new List<IMongoQuery>
            {
                Query.EQ("Key", item.Key),
                Query.EQ("Value", item.Value)
            };
            IMongoQuery query = Query.And(queryList);
            return query;
        }

        /// <summary>
        /// 设置更新时间
        /// </summary>
        /// <param name="code">业务代码</param>
        /// <param name="dateTime">更新时间</param>
        public static void SetUpdateTime(string code, DateTime dateTime)
        {

            MongoDatabase mongoDatabase = MongoFactory.GetMongoDatabaseByDic();
            MongoCollection mongoCollection = mongoDatabase.GetCollection("UpdateTime");
            List<IMongoQuery> queryList = new List<IMongoQuery>
            {
                Query.EQ("Key", code)
            };
            var query = Query.And(queryList);
            mongoCollection.Remove(query);
            UpdateTime updateTime = new UpdateTime
            {
                Key = code,
                Time = dateTime
            };
            mongoCollection.Insert(updateTime);
        }

        /// <summary>
        /// 获取最后更新时间
        /// </summary>
        /// <param name="code">业务代码</param>
        /// <returns>时间</returns>
        public static DateTime GetLastUpdateTime(string code)
        {
            MongoDatabase mongoDatabase = MongoFactory.GetMongoDatabaseByDic();
            MongoCollection mongoCollection = mongoDatabase.GetCollection("UpdateTime");
            List<IMongoQuery> queryList = new List<IMongoQuery>
            {
                Query.EQ("Key", code)
            };
            var query = Query.And(queryList);
            List<UpdateTime> time = mongoCollection.FindAs<UpdateTime>(query).ToList();
            if (time.Count == 0)
            {
                return default(DateTime);
            }
            else
            {
                return time[0].Time;
            }
        }
    }

    /// <summary>
    /// 字典实体
    /// </summary>
    public class Dic
    {
        /// <summary>
        ///  BsonType.ObjectId 这个对应了 MongoDB.Bson.ObjectId
        /// </summary>
        public ObjectId _id { get; set; }

        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 默认数据用于测试
        /// </summary>
        public static List<Dic> Default => new List<Dic>
        {
            new Dic
            {
                Key = "k1",
                Value = "v1"
            },
            new Dic
            {
                Key = "k2",
                Value = "v2"
            },
            new Dic
            {
                Key = "k3",
                Value = "v3"
            }
        };
    }

    /// <summary>
    /// 更新时间
    /// </summary>
    public class UpdateTime
    {
        /// <summary>
        ///  BsonType.ObjectId 这个对应了 MongoDB.Bson.ObjectId
        /// </summary>
        public ObjectId _id { get; set; }

        /// <summary>
        /// 业务代码
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}