using System;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace Util.Webs {
    /// <summary>
    /// Web操作
    /// </summary>
    public class Helper {
        /// <summary>
        /// 注册模块
        /// </summary>
        public static void RegisterModule<T>() {
            RegisterModule( typeof(T) );
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        public static void RegisterModule( Type moduleType ) {
            DynamicModuleUtility.RegisterModule( moduleType );
        }
    }
}
