namespace Util.Security.Webs {
    /// <summary>
    /// Http上下文适配器
    /// </summary>
    public interface IHttpContextAdapter {
        /// <summary>
        /// 获取身份标识
        /// </summary>
        Identity GetIdentity();
        /// <summary>
        /// 获取请求地址
        /// </summary>
        string GetRequestUrl();
    }
}
