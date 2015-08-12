using Util.Logs;
using Util.Security;

namespace Util.Domains {
    /// <summary>
    /// 领域服务
    /// </summary>
    public abstract class DomainServiceBase : IDomainService {
        /// <summary>
        /// 初始化领域服务
        /// </summary>
        protected DomainServiceBase() {
            Log = Logs.Log4.Log.GetContextLog( this );
        }

        /// <summary>
        /// 日志操作
        /// </summary>
        protected ILog Log { get; set; }

        /// <summary>
        /// 获取当前用户编号
        /// </summary>
        protected string SelfId {
            get { return SecurityContext.Identity.UserId; }
        }
    }
}
