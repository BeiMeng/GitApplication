using System.Web;
using Infrastructure.Security;

//启动注册
[assembly: PreApplicationStartMethod( typeof( AuthorizeModuleRegister ), "Start" )]

namespace Infrastructure.Security {
    /// <summary>
    /// 授权模块注册器
    /// </summary>
    public class AuthorizeModuleRegister {
        /// <summary>
        /// 注册授权模块
        /// </summary>
        public static void Start() {
            Util.Webs.Helper.RegisterModule<AuthorizeModule>();
        }
    }
}
