using System;
using System.Collections.Generic;
using Util.Exceptions.Prompts;

namespace Util.Exceptions {
    /// <summary>
    /// 异常提示
    /// </summary>
    public sealed class ExceptionPrompt{
        /// <summary>
        /// 初始化异常提示
        /// </summary>
        private ExceptionPrompt() {
            _prompts.Add( new ConcurrencyExceptionPrompt() );
            _prompts.Add( new DataBaseRefrencePrompt() );
        }

        /// <summary>
        /// 异常提示组件集合
        /// </summary>
        private readonly List<IExceptionPrompt> _prompts = new List<IExceptionPrompt>();

        /// <summary>
        /// 单实例
        /// </summary>
        private static readonly ExceptionPrompt SingleInstance = new ExceptionPrompt();

        /// <summary>
        /// 获取异常提示实例
        /// </summary>
        public static ExceptionPrompt Instance { get { return SingleInstance; } }

        /// <summary>
        /// 获取异常提示
        /// </summary>
        /// <param name="exception">异常</param>
        public string GetPrompt( Exception exception ) {
            var prompt = GetSystemExceptionPrompt( exception );
            if ( !prompt.IsEmpty() )
                return prompt;
            var warning = exception as Warning;
            if ( warning != null )
                return warning.GetPrompt();
            return R.SystemError;
        }

        /// <summary>
        /// 获取系统异常提示
        /// </summary>
        private string GetSystemExceptionPrompt( Exception exception ) {
            foreach( var prompt in _prompts ) {
                var result = prompt.GetPrompt( exception );
                if ( !result.IsEmpty() )
                    return result;
            }
            return string.Empty;
        }
    }
}
