using System;

namespace Applications.Domains.Models.Systems
{
    public partial class Application
    {
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