using System;
using System.Net.Mime;
using Util;

namespace Applications.Domains.Models.Systems {
    /// <summary>
    /// 租户
    /// </summary>
    public partial class Tenant {
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            PinYin = Str.PinYin(Name);
        }
    }
}