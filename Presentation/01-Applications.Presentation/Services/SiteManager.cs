using System.Collections.Generic;
using Applications.Domains.Repositories.Systems;
using Applications.Domains.Services.Systems;
using Util;

namespace Presentation.Services {
    /// <summary>
    /// 站点管理器
    /// </summary>
    public class SiteManager : SiteManagerBase{
        /// <summary>
        /// 初始化站点管理器
        /// </summary>
        /// <param name="tenantRepository">租户仓储</param>
        public SiteManager( ITenantRepository tenantRepository )
            : base( tenantRepository ) {
        }

        /// <summary>
        /// 获取皮肤列表
        /// </summary>
        public override List<Item> GetSkins() {
            return new List<Item> {
                new Item("default","default"),
                new Item("bootstrap","bootstrap"),
                new Item("black","black"),
                new Item("gray","gray"),
                new Item("metro","metro")
            };
        }

        /// <summary>
        /// 获取菜单样式列表
        /// </summary>
        public override List<Item> GetMenuStyles() {
            return new List<Item> {
                new Item("横向菜单","HorizontalMenus"),
                new Item("一级横向菜单+手风琴","HorizontalMenus_Accordion")
            };
        }
    }
}
