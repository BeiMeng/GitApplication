using System.Data.Entity;
using Infrastructure.Core;
using Util.Datas.Ef;

namespace Datas.Ef {
    /// <summary>
    /// 工作单元
    /// </summary>
    public class BeiDreamAppUnitOfWork : EfUnitOfWork, IBeiDreamAppUnitOfWork
    {
        /// <summary>
        /// 初始化工作单元
        /// </summary>
        static BeiDreamAppUnitOfWork() {
            Database.SetInitializer<BeiDreamAppUnitOfWork>( null );
        }

        /// <summary>
        /// 初始化工作单元
        /// </summary>
        public BeiDreamAppUnitOfWork() : base("Application") { }
    }
}