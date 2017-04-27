using System.Collections.Generic;
using System.Data;
using System.Linq;
using Notify.Infrastructure.DomainBase;
using Notify.Infrastructure.EntityFactoryFramework;
using Notify.Infrastructure.UnitOfWork;

namespace Notify.DbCommon.Repositroies
{
    /// <summary>
    /// 工作单元仓储基类
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    /// <typeparam name="TValue">实体</typeparam>
    public abstract class SqlRepositoryBase<TKey, TValue> : UnitOfWorkRepositoryBase<TKey, TValue>
        where TValue : IEntity
    {
        /// <summary>
        /// 有子对象的回调委托
        /// </summary>
        /// <param name="entityAggregate">实体聚合根</param>
        /// <param name="childEntityKeyValue">子实体键</param>
        public delegate void AppendChildData(TValue entityAggregate, object childEntityKeyValue);

        /// <summary>
        /// 实体工厂
        /// </summary>
        protected readonly IEntityFactory<TValue> m_entityFactory;

        /// <summary>
        /// 子对象集
        /// </summary>
        private readonly Dictionary<string, AppendChildData> m_childCallbacks;

        /// <summary>
        /// 子对象数据集
        /// </summary>
        private readonly Dictionary<string, object> m_childKeyDatas;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitOfWork">工作单元</param>
        /// <param name="name">数据连接</param>
        protected SqlRepositoryBase(IPowerUnitOfWork unitOfWork, string name)
            : base(unitOfWork, name)
        {
            this.m_entityFactory = this.BuildEntityFactory();
            this.m_childCallbacks = new Dictionary<string, AppendChildData>();
            this.m_childKeyDatas = new Dictionary<string, object>();
            this.BuildChildCallbacks(this.m_childCallbacks);
        }

        /// <summary>
        /// 改为由子类创建实体，不使用工厂
        /// </summary>
        /// <returns>TValue</returns>
        protected abstract IEntityFactory<TValue> BuildEntityFactory();

        /// <summary>
        /// 创建子对象回调
        /// </summary>
        /// <param name="childCallbacks">子对象集</param>
        protected abstract void BuildChildCallbacks(Dictionary<string, AppendChildData> childCallbacks);

        /// <summary>
        /// 子对象回调集
        /// </summary>
        protected Dictionary<string, AppendChildData> ChildCallbacks
        {
            get
            {
                return this.m_childCallbacks;
            }
        }

        /// <summary>
        /// 创建实体对象
        /// </summary>
        /// <param name="reader">IDataReader</param>
        /// <returns>实体对象</returns>
        protected virtual TValue BuildEntityFromReader(IDataReader reader)
        {
            TValue entity = this.m_entityFactory.BuildEntity(reader);
            if (this.m_childCallbacks != null && this.m_childCallbacks.Count > 0)
            {
                DataTable columnData = reader.GetSchemaTable();
                foreach (string childKeyName in this.m_childCallbacks.Keys)
                {
                    object childKeyValue;
                    ////判断 DataReader 的数据集合中是否存在一个特定的列名（或字段名）
                    if (columnData != null && columnData.Rows.Cast<DataRow>().Any(row => row["ColumnName"].ToString() == childKeyName))
                    {
                        childKeyValue = reader[childKeyName];
                    }
                    else
                    {
                        childKeyValue = null;
                    }
                    if (m_childKeyDatas.ContainsKey(childKeyName))
                    {
                        m_childKeyDatas[childKeyName] = childKeyValue;
                    }
                    else
                    {
                        m_childKeyDatas.Add(childKeyName, childKeyValue);
                    }
                }
            }
            return entity;
        }

        /// <summary>
        /// 创建实体对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>实体对象</returns>
        protected virtual TValue BuildEntityFromSql(string sql)
        {
            TValue entity = default(TValue);
            using (IDataReader reader = this.ExecuteReader(sql))
            {
                if (reader.Read())
                {
                    entity = this.BuildEntityFromReader(reader);
                }
            }
            if (entity != null)
            {
                this.InvokeChildCallbacks(entity);
            }
            return entity;
        }

        /// <summary>
        /// 创建实体对象集合
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>实体对象集合</returns>
        protected virtual List<TValue> BuildEntitiesFromSql(string sql)
        {
            List<TValue> entities = new List<TValue>();
            using (IDataReader reader = this.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    entities.Add(this.BuildEntityFromReader(reader));
                }
            }
            return entities;
        }

        /// <summary>
        /// 加载子对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void InvokeChildCallbacks(TValue entity)
        {
            if (this.m_childCallbacks != null && this.m_childCallbacks.Any())
            {
                foreach (string childKeyName in this.m_childKeyDatas.Keys)
                {
                    object childKeyValue;
                    this.m_childKeyDatas.TryGetValue(childKeyName, out childKeyValue);
                    this.m_childCallbacks[childKeyName](entity, childKeyValue);
                }
            }
        }
    }
}