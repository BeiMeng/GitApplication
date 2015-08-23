using System;
using System.Net.Mime;
using Util;

namespace Applications.Domains.Models.Systems {
    /// <summary>
    /// 用户
    /// </summary>
    public partial class User {
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
        }
    }
}