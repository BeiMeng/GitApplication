using System;
using System.Diagnostics;
using System.Web;
using Util.Security;

namespace Util.Logs.Log4 {
    /// <summary>
    /// 日志
    /// </summary>
    public class Log : ILog {

        #region 构造方法

        /// <summary>
        /// 初始化日志
        /// </summary>
        private Log( log4net.ILog log ) : this( log, Guid.NewGuid().ToString(),new Stopwatch() ) {
        }

        /// <summary>
        /// 初始化日志
        /// </summary>
        private Log( log4net.ILog log,string traceId, Stopwatch stopwatch ) {
            _log = log;
            TraceId = traceId;
            _stopwatch = stopwatch;
            _stopwatch.Start();
            Params = new Str();
            Caption = new Str();
            Content = new Str();
            Sql = new Str();
            SqlParams = new Str();
            BusinessId = string.Empty;
            Application = string.Empty;
            Tenant = string.Empty;
            Category = string.Empty;
            Class = string.Empty;
            Method = string.Empty;
            ErrorMessage = string.Empty;
            StackTrace = string.Empty;
        }

        #endregion

        #region 工厂方法

        /// <summary>
        /// 获取日志
        /// </summary>
        public static ILog GetLog() {
            return GetLog( string.Empty );
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="className">类名</param>
        public static ILog GetLog( string className ) {
            return new Log( log4net.LogManager.GetLogger( className ) ) {
                Class = className
            };
        }

        /// <summary>
        /// 获取日志
        /// </summary>
        /// <param name="instance">实例</param>
        public static ILog GetLog( object instance ) {
            if ( instance == null )
                return GetLog();
            return GetLog( instance.GetType().ToString() );
        }

        /// <summary>
        /// 获取上下文日志
        /// </summary>
        public static ILog GetContextLog() {
            return GetContextLog( string.Empty );
        }

        /// <summary>
        /// 获取上下文日志
        /// </summary>
        private static ILog GetContextLog( Func<ILog> handler ) {
            string key = Config.GetLogContextKey();
            var log = Context.Get<ILog>( key );
            if ( log != null )
                return log;
            log = handler();
            Context.Add( key, log );
            return log;
        }

        /// <summary>
        /// 获取上下文日志
        /// </summary>
        /// <param name="className">类名</param>
        public static ILog GetContextLog( string className ) {
            var log = GetContextLog( () => GetLog( className ) );
            log.Class = className;
            return log;
        }

        /// <summary>
        /// 获取上下文日志
        /// </summary>
        /// <param name="instance">实例</param>
        public static ILog GetContextLog( object instance ) {
            if ( instance == null )
                return GetContextLog();
            return GetContextLog( instance.GetType().ToString() );
        }

        #endregion

        #region 字段(私有属性)

        /// <summary>
        /// log4日志接口
        /// </summary>
        private readonly log4net.ILog _log;
        /// <summary>
        /// 计时器
        /// </summary>
        private readonly Stopwatch _stopwatch;
        /// <summary>
        /// 日志级别
        /// </summary>
        private LogLevel Level { get; set; }
        /// <summary>
        /// 用户身份标识
        /// </summary>
        private Identity Identity { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        private string ErrorMessage { get; set; }
        /// <summary>
        /// 堆栈跟踪
        /// </summary>
        private string StackTrace { get; set; }

        #endregion

        #region 公共属性

        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string BusinessId { get; set; }
        /// <summary>
        /// 应用程序名称
        /// </summary>
        public string Application { get; set; }
        /// <summary>
        /// 租户名称
        /// </summary>
        public string Tenant { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string Class { get; set; }
        /// <summary>
        /// 方法名
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public Str Params { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public Str Caption { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public Str Content { get; set; }
        /// <summary>
        /// Sql语句
        /// </summary>
        public Str Sql { get; set; }
        /// <summary>
        /// Sql参数
        /// </summary>
        public Str SqlParams { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        #endregion

        #region AddBusinessId(添加业务编号)

        /// <summary>
        /// 添加业务编号
        /// </summary>
        /// <param name="businessId">业务编号</param>
        public void AddBusinessId( string businessId ) {
            if ( !BusinessId.IsEmpty() )
                BusinessId += ",";
            BusinessId += businessId;
        }

        #endregion

        #region Debug(调试)

        /// <summary>
        /// 调试
        /// </summary>
        public void Debug() {
            Execute( () => {
                Level = LogLevel.Debug;
                _log.Debug( GetMessage() );
            } );
        }

        /// <summary>
        /// 执行
        /// </summary>
        private void Execute( Action action ) {
            action();
            Clear();
        }

        /// <summary>
        /// 获取日志消息
        /// </summary>
        private LogMessage GetMessage() {
            InitException();
            return CreateMessage();
        }

        /// <summary>
        /// 初始化异常
        /// </summary>
        private void InitException() {
            if ( Exception == null )
                return;
            var warning = new Warning( Exception );
            ErrorMessage = warning.Message;
            StackTrace = warning.StackTrace;
        }

        /// <summary>
        /// 创建消息
        /// </summary>
        private LogMessage CreateMessage() {
            return new LogMessage() {
                Level = Level.Description(),
                TraceId = TraceId,
                Time = DateTime.Now.ToMillisecondString(),
                TotalSeconds = GetTotalSeconds(),
                Url = GetUrl(),
                BusinessId = BusinessId,
                Application = GetApplication(),
                Tenant = GetTenant(),
                Category = Category,
                Class = Class,
                Method = Method,
                Params = Params.ToString(),
                Ip = Net.Ip,
                Host = Net.Host,
                ThreadId = Thread.ThreadId,
                UserId = GetIdentity().UserId,
                Operator = GetIdentity().FullName,
                Role = GetIdentity().Role,
                Caption = Caption.ToString(),
                Content = Content.ToString(),
                Sql = Sql.ToString(),
                SqlParams = SqlParams.ToString(),
                ErrorCode = ErrorCode,
                Error = ErrorMessage,
                StackTrace = StackTrace
            };
        }

        /// <summary>
        /// 获取已执行时间
        /// </summary>
        private string GetTotalSeconds() {
            return _stopwatch.Elapsed.TotalSeconds.ToString();
        }

        /// <summary>
        /// 获取Url
        /// </summary>
        private string GetUrl() {
            if ( HttpContext.Current == null )
                return string.Empty;
            try {
                return HttpContext.Current.Request.Url.ToString();
            }
            catch {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取应用程序
        /// </summary>
        private string GetApplication() {
            if ( Application.IsEmpty() )
                return GetIdentity().Application;
            return Application;
        }

        /// <summary>
        /// 获取租户
        /// </summary>
        private string GetTenant() {
            if ( Tenant.IsEmpty() )
                return GetIdentity().Tenant;
            return Tenant;
        }

        /// <summary>
        /// 获取用户身份标识
        /// </summary>
        private Identity GetIdentity() {
            if ( Identity != null )
                return Identity;
            Identity = SecurityContext.Identity;
            return Identity;
        }

        /// <summary>
        /// 清理
        /// </summary>
        private void Clear() {
            BusinessId = string.Empty;
            Method = string.Empty;
            Params.Clear();
            Caption.Clear();
            Content.Clear();
            Sql.Clear();
            SqlParams.Clear();
            Exception = null;
            ErrorMessage = string.Empty;
            StackTrace = string.Empty;
        }

        #endregion

        #region Info(信息)

        /// <summary>
        /// 信息
        /// </summary>
        public void Info() {
            Execute( () => {
                Level = LogLevel.Information;
                _log.Info( GetMessage() );
            } );
        }

        #endregion

        #region Warn(警告)

        /// <summary>
        /// 警告
        /// </summary>
        public void Warn() {
            Execute( () => {
                Level = LogLevel.Warning;
                _log.Warn( GetMessage() );
            } );
        }

        #endregion

        #region Error(错误)

        /// <summary>
        /// 错误
        /// </summary>
        public void Error() {
            Execute( () => {
                Level = LogLevel.Error;
                _log.Error( GetMessage() );
            } );
        }

        #endregion

        #region Fatal(致命错误)

        /// <summary>
        /// 致命错误
        /// </summary>
        public void Fatal() {
            Execute( () => {
                Level = LogLevel.Fatal;
                _log.Fatal( GetMessage() );
            } );
        }

        #endregion

        #region 创建新日志实例

        /// <summary>
        /// 创建新日志实例，保留跟踪号和计时器
        /// </summary>
        /// <param name="logName">日志名</param>
        public ILog CreateNew( string logName ) {
            return new Log( log4net.LogManager.GetLogger( logName ),TraceId,_stopwatch );
        }

        /// <summary>
        /// Debug级别是否启用
        /// </summary>
        public bool IsDebugEnabled { get { return true; } }
        #endregion
    }
}
