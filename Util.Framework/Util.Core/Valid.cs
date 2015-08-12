using System.Linq;

namespace Util {
    /// <summary>
    /// 验证
    /// </summary>
    public class Valid {

        #region IsInt(是否整数)

        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="number">数值</param>
        /// <param name="digit">位数</param>
        public static bool IsInt( string number, int? digit = null ) {
            if( number.IsEmpty() )
                return false;
            number = number.Trim();
            return Regex.IsMatch( number, GetIsIntPattern( digit ) );
        }

        /// <summary>
        /// 获取整数模式
        /// </summary>
        private static string GetIsIntPattern( int? digit ) {
            if( digit == null )
                return @"^\d+$";
            return string.Format( "^[0-9]{{{0}}}$", digit );
        }

        #endregion

        #region IsNumber(是否数字)

        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="number">数值</param>        
        public static bool IsNumber( string number ) {
            if( number.IsEmpty() )
                return false;
            const string pattern = @"^(-?\d*)(\.\d+)?$";
            return Regex.IsMatch( number, pattern );
        }

        #endregion

        #region IsRepeat(是否重复)

        /// <summary>
        /// 是否重复，范例：112,返回true
        /// </summary>
        /// <param name="value">值</param>
        public static bool IsRepeat( string value ) {
            if( value.IsEmpty() )
                return false;
            var array = value.ToCharArray();
            return array.Any( c => array.Count( t => t == c ) > 1 );
        }

        #endregion

        #region IsMobile(是否手机号)

        /// <summary>
        /// 是否手机号
        /// </summary>
        /// <param name="value">手机号</param>
        public static bool IsMobile( string value ) {
            if ( value.IsEmpty() )
                return false;
            const string pattern = @"^1[3-8]\d{9}$";
            return Regex.IsMatch( value, pattern );
        }

        #endregion

        #region IsEmail(是否电子邮件)

        /// <summary>
        /// 是否电子邮件
        /// </summary>
        /// <param name="value">电子邮件</param>
        public static bool IsEmail( string value ) {
            if ( value.IsEmpty() )
                return false;
            const string pattern = @"^(\w)+.*@(\w)+((\.\w+)+)$";
            return Regex.IsMatch( value, pattern );
        }

        #endregion
    }
}
