using System.Collections.Generic;
using System.Web.Mvc;
using Util.ApplicationServices;

namespace Util.Webs.Mvc {
    /// <summary>
    /// 基控制器
    /// </summary>
    public abstract class ControllerBase : Controller {
        /// <summary>
        /// 转换为Json字符串
        /// </summary>
        /// <param name="data">对象</param>
        public string ToJson( object data ) {
            return Util.Json.ToJson( data );
        }

        /// <summary>
        /// 转换为Json字符串
        /// </summary>
        /// <param name="data">对象</param>
        public string ToJson( IEnumerable<object> data ) {
            return Util.Json.ToJson( data );
        }

        /// <summary>
        /// 转换为Json结果
        /// </summary>
        /// <param name="data">对象</param>
        public ActionResult ToJsonResult( object data ) {
            return Content( ToJson( data ) );
        }

        /// <summary>
        /// 转换为Json结果
        /// </summary>
        /// <param name="data">对象</param>
        public ActionResult ToJsonResult( IEnumerable<object> data ) {
            return Content( ToJson( data ) );
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        protected virtual ActionResult Ok( string message = "操作成功", IEnumerable<object> data = null ) {
            return new Result( StateCode.Ok, message, data ).GetResult();
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        protected ActionResult Fail( string message ) {
            return new Result( StateCode.Fail, message ).GetResult();
        }
    }
}
