using System.Collections.Generic;
using System.Web.Mvc;
using Util.ApplicationServices;

namespace Util.Webs.Mvc {
    /// <summary>
    /// Mvc返回结果
    /// </summary>
    public class Result {
        /// <summary>
        /// 初始化Mvc返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public Result( StateCode code, string message, IEnumerable<object> data = null ) {
            _code = code;
            _message = message;
            _data = data;
        }

        /// <summary>
        /// 状态码
        /// </summary>
        private readonly StateCode _code;
        /// <summary>
        /// 消息
        /// </summary>
        private readonly string _message;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly IEnumerable<object> _data;

        /// <summary>
        /// 获取输出结果
        /// </summary>
        public ActionResult GetResult() {
            return new ContentResult { Content = Json.ToJson( new { Code = _code.Value(), Message = _message, Data = _data } ) };
        }
    }
}
