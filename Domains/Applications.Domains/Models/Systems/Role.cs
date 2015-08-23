using System;
using Util;

namespace Applications.Domains.Models.Systems {
    /// <summary>
    /// 角色
    /// </summary>
    public partial class Role {
        /// <summary>
        /// 租户
        /// </summary>
        public virtual Tenant Tenant { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            base.Init();
            CreateTime = DateTime.Now;
            PinYin = Str.PinYin(Name);
        }
    }
}