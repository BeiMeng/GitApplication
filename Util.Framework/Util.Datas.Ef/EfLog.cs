using System;
using System.Text.RegularExpressions;
using Util.Logs;

namespace Util.Datas.Ef {
    /// <summary>
    /// Ef日志
    /// </summary>
    public class EfLog {
        /// <summary>
        /// 初始化Ef日志
        /// </summary>
        /// <param name="traceId">跟踪号</param>
        /// <param name="log">日志</param>
        public EfLog( string traceId, ILog log ) {
            Log = log;
            Log.Caption.Add( string.Format( EfResource.LogCaption, traceId ) );
        }

        /// <summary>
        /// 日志
        /// </summary>
        public ILog Log { get; set; }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">sql日志</param>
        public void Write( string log ) {
            if ( !Log.IsDebugEnabled )
                return;
            if ( log.IsEmpty() )
                return;
            AddLog( log );
            if ( IsSql( log ) ) {
                AddSql( log );
                return;
            }
            if ( IsParam( log ) ) {
                ReplaceParam( log );
                return;
            }
            if ( IsClose( log ) )
                Log.Debug();
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        private void AddLog( string log ) {
            log = log.Trim();
            if( !IsSql(log) )
                log = log.Replace( Sys.Line, " " );
            Log.Content.AddLine( log );
        }

        /// <summary>
        /// 是否Sql语句
        /// </summary>
        private bool IsSql( string log ) {
            const string pattern = "打开了连接|关闭了连接|启动了事务|提交了事务|^Started transaction|^Committed transaction|^Opened connection|^Closed connection|^--";
            return !Regex.IsMatch( log, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled );
        }

        /// <summary>
        /// 添加Sql
        /// </summary>
        private void AddSql( string log ) {
            Log.Sql.AddLine( log );
        }

        /// <summary>
        /// 是否参数
        /// </summary>
        private bool IsParam( string log ) {
            const string pattern = @"^-- p|^-- @\d";
            return Regex.IsMatch( log, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled );
        }

        /// <summary>
        /// 替换Sql中的参数
        /// </summary>
        private void ReplaceParam( string value ) {
            var param = ResolveParam( value );
            var paramName = string.Format( @"(@|:|\?)?{0}", param.Item1 );
            var sql = Regex.Replace( Log.Sql.ToString(), paramName, param.Item2 );
            Log.Sql.Replace( sql );
        }

        /// <summary>
        /// 是否关闭连接
        /// </summary>
        private bool IsClose( string log ) {
            const string pattern = "关闭了连接|^Closed connection";
            return Regex.IsMatch( log, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled );
        }

        /// <summary>
        /// 从sql日志中解析出参数名和参数值
        /// </summary>
        /// <param name="log">sql日志</param>
        public static Tuple<string, string> ResolveParam( string log ) {
            string pattern = @"--\s(.*):\s('?.*'?)\s\(Type";
            var match = System.Text.RegularExpressions.Regex.Match( log, pattern, RegexOptions.IgnoreCase );
            return new Tuple<string, string>( match.Result( "$1" ), match.Result( "$2" ) );
        }
    }
}
