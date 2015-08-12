using System;

namespace Util.Exceptions.Prompts {
    /// <summary>
    /// 数据库外键约束异常提示
    /// </summary>
    public class DataBaseRefrencePrompt : IExceptionPrompt {
        /// <summary>
        /// 获取异常提示
        /// </summary>
        /// <param name="exception">异常</param>
        public string GetPrompt( Exception exception ) {
            if ( IsRefrenceError( exception ) )
                return R.DataBaseRefrenceError;
            return string.Empty;
        }

        /// <summary>
        /// 是否数据库外键约束错误
        /// </summary>
        private static bool IsRefrenceError( Exception exception ) {
            var message = new Warning( exception ).Message;
            return message.Contains( "DELETE 语句与 REFERENCE 约束" ) 
                || message.Contains( "a foreign key constraint fails" );
        }
    }
}
