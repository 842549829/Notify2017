namespace Notify.Infrastructure.DomainBase
{
    /// <summary>
    /// 实体抽象类
    /// </summary>
    public abstract class EntityBase : IEntity
    {
        /// <summary>
        /// 标识key
        /// </summary>
        private readonly object key;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class. 
        /// 默认构造函数 default constructor
        /// </summary>
        protected EntityBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        /// <param name="key">key</param>
        protected EntityBase(object key)
        {
            this.key = key;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public object Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Equals(EntityBase other)
        {
            return Equals(this.Key, other.Key);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Equals(object entity)
        {
            if (!(entity is EntityBase))
            {
                return false;
            }

            return this == (EntityBase)entity;
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Key?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// The ==.
        /// </summary>
        /// <param name="left">
        /// The base 1.
        /// </param>
        /// <param name="right">
        /// The base 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator ==(EntityBase left, EntityBase right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }

            if ((object)left == null || (object)right == null)
            {
                return false;
            }

            if (left.Key == null && left.Key == null)
            {
                return true;
            }

            return left.Key.ToString() == right.Key.ToString();
        }

        /// <summary>
        /// The !=.
        /// </summary>
        /// <param name="left">
        /// The base 1.
        /// </param>
        /// <param name="right">
        /// The base 2.
        /// </param>
        /// <returns>
        /// </returns>
        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    /// The entity.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public abstract class EntityBase<T> : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase{T}"/> class. 
        /// Initializes a new instance of the <see cref="EntityBase"/> class. 
        /// 默认构造函数 default constructor
        /// </summary>
        protected EntityBase()
            : base(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase{T}"/> class. 
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        /// <param name="key">
        /// key
        /// </param>
        protected EntityBase(T key)
            : base(key)
        {
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public new T Key
        {
            get
            {
                T @default = default(T);
                if (base.Key == null)
                {
                    return @default;
                }

                return (T)base.Key;
            }
        }
    }
}
