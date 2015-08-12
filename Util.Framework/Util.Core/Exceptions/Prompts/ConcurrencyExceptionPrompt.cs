using System;

namespace Util.Exceptions.Prompts {
    /// <summary>
    /// 并发异常提示
    /// </summary>
    public class ConcurrencyExceptionPrompt : IExceptionPrompt{
        /// <summary>
        /// 获取异常提示
        /// </summary>
        /// <param name="exception">异常</param>
        public string GetPrompt( Exception exception ) {
            if ( exception is ConcurrencyException )
                return R.ConcurrencyExceptionMessage;
            return string.Empty;
        }
    }
}
