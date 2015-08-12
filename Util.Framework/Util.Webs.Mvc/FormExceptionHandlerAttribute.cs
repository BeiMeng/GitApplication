using System;
using System.Web.Mvc;
using Util.ApplicationServices;
using Util.Exceptions;

namespace Util.Webs.Mvc {
    /// <summary>
    /// 表单异常处理器
    /// </summary>
    [AttributeUsage( AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
    public class FormExceptionHandlerAttribute : HandleErrorAttribute {
        /// <summary>
        /// 处理异常
        /// </summary>
        public override void OnException( ExceptionContext context ) {
            base.OnException( context );
            context.ExceptionHandled = true;
            string errorMsg = ExceptionPrompt.Instance.GetPrompt( context.Exception );
            context.Result = new Result( StateCode.Fail, errorMsg ).GetResult();
        }
    }
}
