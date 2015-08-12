using Util;

namespace Applications.Domains.Models.Commons {
    /// <summary>
    /// 地区
    /// </summary>
    public partial class Area {
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init() {
            base.Init();
            if ( !PinYin.IsEmpty() )
                PinYin = PinYin.ToLower();
            Code = "1";
        }
    }
}