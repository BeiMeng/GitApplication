using System.Collections.Generic;
using Util;

namespace Applications.Services.Contracts.Identity
{
    /// <summary>
    /// 系统服务
    /// </summary>
    public interface IIdentityService : IDependency
    {
        /// <summary>
        /// 获取皮肤列表
        /// </summary>
        List<Item> GetSkins();
        /// <summary>
        /// 获取菜单样式列表
        /// </summary>
        List<Item> GetMenuStyles();
        /// <summary>
        /// 获取皮肤描述
        /// </summary>
        /// <param name="skin">皮肤</param>
        string GetSkinDescription(string skin);
        /// <summary>
        /// 获取菜单样式描述
        /// </summary>
        /// <param name="style">菜单样式</param>
        string GetMenuStyleDescription(string style);
        /// <summary>
        /// 获取皮肤
        /// </summary>
        string GetSkin();
        ///// <summary>
        ///// 获取模块列表
        ///// </summary>
        //List<Module> GetModules();
    }
}