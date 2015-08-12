using System.Collections.Generic;
using System.Linq;
using Applications.Domains.Repositories.Systems;
using Util;

namespace Applications.Domains.Services.Systems {
    /// <summary>
    /// 基站点管理器
    /// </summary>
    public abstract class SiteManagerBase : ISiteManager {
        /// <summary>
        /// 初始化站点管理器
        /// </summary>
        /// <param name="tenantRepository">租户仓储</param>
        protected SiteManagerBase( ITenantRepository tenantRepository ) {
            TenantRepository = tenantRepository;
        }

        /// <summary>
        /// 租户仓储
        /// </summary>
        public ITenantRepository TenantRepository { get; set; }

        /// <summary>
        /// 获取皮肤列表
        /// </summary>
        public abstract List<Item> GetSkins();

        /// <summary>
        /// 获取菜单样式列表
        /// </summary>
        public abstract List<Item> GetMenuStyles();

        /// <summary>
        /// 获取皮肤描述
        /// </summary>
        /// <param name="skin">皮肤</param>
        public string GetSkinDescription( string skin ) {
            var item = GetSkins().FirstOrDefault( t => t.Value == skin );
            return item == null ? string.Empty : item.Text;
        }

        /// <summary>
        /// 获取菜单样式描述
        /// </summary>
        /// <param name="style">菜单样式</param>
        public string GetMenuStyleDescription( string style ) {
            var item = GetMenuStyles().FirstOrDefault( t => t.Value == style );
            return item == null ? string.Empty : item.Text;
        }

        /// <summary>
        /// 获取皮肤
        /// </summary>
        public string GetSkin() {
            return "default";
        }
    }
}