using System;
using System.Security.Cryptography;
using System.Text;

namespace Util {
    /// <summary>
    /// 加密操作
    /// </summary>
    public class Encrypt {

        #region Md5加密

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="text">待加密字符串</param>
        public static string Md5By16( string text ) {
            return Md5By16( text, Encoding.UTF8 );
        }

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <param name="encoding">字符编码</param>
        public static string Md5By16( string text,Encoding encoding ) {
            return Md5( text,encoding, 4, 8 );
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        private static string Md5( string text, Encoding encoding,int? startIndex,int? length ) {
            if ( string.IsNullOrWhiteSpace( text ) )
                return string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            string result;
            try {
                if( startIndex == null )
                    result = BitConverter.ToString( md5.ComputeHash( encoding.GetBytes( text ) ) );
                else
                    result = BitConverter.ToString( md5.ComputeHash( encoding.GetBytes( text ) ), startIndex.SafeValue(), length.SafeValue() );
            }
            finally {
                md5.Clear();
            }
            return result.Replace( "-", "" );
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="text">待加密字符串</param>
        public static string Md5By32( string text ) {
            return Md5By32( text, Encoding.UTF8 );
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <param name="encoding">字符编码</param>
        public static string Md5By32( string text, Encoding encoding ) {
            return Md5( text, encoding, null, null );
        }

        #endregion

        #region Des加密

        /// <summary>
        /// Des加密
        /// </summary>
        /// <param name="value">原始值</param>
        public static string EncodeDes( object value ) {
            return value == null ? string.Empty : EncodeDes( value.ToString(), Config.GetKey() );
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="value">原始值</param>
        /// <param name="key">密钥,必须24位</param>
        public static string EncodeDes( object value, string key ) {
            string text = value.ToStr();
            if ( !ValidateDes( text, key ) )
                return string.Empty;
            var provider = CreateProvider( key );
            using ( var transform = provider.CreateEncryptor() ) {
                var bytes = Encoding.UTF8.GetBytes( text );
                var result = transform.TransformFinalBlock( bytes, 0, bytes.Length );
                return Convert.ToBase64String( result );
            }
        }

        /// <summary>
        /// 验证参数
        /// </summary>
        private static bool ValidateDes( string value, string key ) {
            if ( string.IsNullOrWhiteSpace( value ) || string.IsNullOrWhiteSpace( key ) )
                return false;
            return key.Length == 24;
        }

        /// <summary>
        /// 创建加密服务提供程序
        /// </summary>
        private static TripleDESCryptoServiceProvider CreateProvider( string key ) {
            return new TripleDESCryptoServiceProvider { Key = Encoding.ASCII.GetBytes( key ), Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="value">内容</param>
        public static string DecodeDes( object value ) {
            return value == null ? string.Empty : DecodeDes( value.ToString(), Config.GetKey() );
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="value">内容</param>
        /// <param name="key">密钥,必须24位</param>
        public static string DecodeDes( object value, string key ) {
            string text = value.ToStr();
            if ( !ValidateDes( text, key ) )
                return string.Empty;
            var provider = CreateProvider( key );
            using ( var transform = provider.CreateDecryptor() ) {
                var bytes = Convert.FromBase64String( text );
                var result = transform.TransformFinalBlock( bytes, 0, bytes.Length );
                return Encoding.UTF8.GetString( result );
            }
        }

        #endregion
    }
}
