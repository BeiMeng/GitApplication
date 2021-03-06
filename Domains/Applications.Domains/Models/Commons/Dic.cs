﻿using System;
using Applications.Domains.Models.Systems;
using Util;

namespace Applications.Domains.Models.Commons {
    /// <summary>
    /// 字典
    /// </summary>
    public partial class Dic {
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
            if (CreateTime == null)
                CreateTime = DateTime.Now;
            PinYin = Str.PinYin(Text);
        }
    }
}