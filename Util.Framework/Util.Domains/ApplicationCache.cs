using Util.Caches;

namespace Util.Domains {
    /// <summary>
    /// 应用程序缓存
    /// </summary>
    /// <typeparam name="T">缓存对象</typeparam>
    public abstract class ApplicationCache<T> where T : class {
        /// <summary>
        /// 获取对象
        /// </summary>
        public T Get() {
            return GetCache().Get<T>( GetKey() ) ?? Load();
        }

        /// <summary>
        /// 获取缓存操作
        /// </summary>
        protected virtual ICacheProvider GetCache() {
            return new LocalCacheProvider();
        }

        /// <summary>
        /// 获取缓存键
        /// </summary>
        protected abstract string GetKey();

        /// <summary>
        /// 加载对象
        /// </summary>
        private T Load() {
            var entity = LoadByRepository();
            if ( entity == null )
                return default( T );
            GetCache().Add( GetKey(), entity, GetTime() );
            return entity;
        }

        /// <summary>
        /// 从仓储中加载对象
        /// </summary>
        protected abstract T LoadByRepository();

        /// <summary>
        /// 获取缓存时间,单位：秒
        /// </summary>
        protected virtual int GetTime() {
            return 3600;
        }
    }
}
