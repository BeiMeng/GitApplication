using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Web;
using System.Drawing.Imaging;

namespace Util.Webs.Controls.ValidateCodes {
    /// <summary>
    /// 验证码
    /// </summary>
    public class ValidateCode : IValidateCode {

        #region 字段

        /// <summary>
        /// 颜色集合
        /// </summary>
        private List<Color> _colors;
        /// <summary>
        /// 背景色
        /// </summary>
        private readonly Color _bgColor;
        /// <summary>
        /// 可选的验证码序列
        /// </summary>
        private const string Chars = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";
        /// <summary>
        /// 随机数
        /// </summary>
        private readonly System.Random _random;
        /// <summary>
        /// 键
        /// </summary>
        private readonly string _name;
        /// <summary>
        /// 值
        /// </summary>
        private string _value;

        #endregion

        #region 属性

        /// <summary>
        /// 宽
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// 是否创建干扰线
        /// </summary>
        public bool IsGenereateDisturbLine { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 初始化
        /// </summary>
        public ValidateCode() {
            InitColors();
            _bgColor = Color.White;
            _random = new System.Random();
            _name = "ValidateCode";
            Width = 80;
            Height = 22;
            FontSize = 14;
        }

        /// <summary>
        /// 初始化颜色集合
        /// </summary>
        private void InitColors() {
            _colors = new List<Color> {
                Color.Black,
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Brown,
                Color.DarkMagenta,
                Color.DarkRed,
                Color.Indigo,
                Color.DarkBlue,
                Color.Maroon
            };
        }

        #endregion

        #region Generate(生成验证码)

        /// <summary>
        /// 生成验证码
        /// </summary>
        public void Generate() {
            GenerateValidateCode();
            GenerateValidateCodeImage();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        private void GenerateValidateCode() {
            _value = GenerateRandomCode();
            var value = Encrypt.Md5By32( _value.Trim().ToUpper() );
            Web.SetSession( _name, value );
        }

        /// <summary>
        /// 生成随机码
        /// </summary>
        private string GenerateRandomCode() {
            var result = new StringBuilder();
            int length = _random.Next( 4, 6 );
            char[] chars = Chars.ToCharArray();
            for ( int i = 0; i < length; i++ ) {
                result.Append( chars[_random.Next( chars.Length )] );
            }
            return result.ToString();
        }

        /// <summary>
        /// 生成验证码图片
        /// </summary>
        private void GenerateValidateCodeImage() {
            Color fcolor = _colors[DateTime.Now.Millisecond % 10];
            var brush = new SolidBrush( fcolor );
            using ( var image = new Bitmap( Width, Height ) ) {
                using ( Graphics g = Graphics.FromImage( image ) ) {
                    GenerateImage( image, fcolor, g, brush );
                    Response( image );
                }
            }
        }

        /// <summary>
        /// 生成图片
        /// </summary>
        private void GenerateImage( Bitmap image, Color fcolor, Graphics g, SolidBrush brush ) {
            g.Clear( _bgColor );
            g.DrawRectangle( new Pen( brush ), 0, 0, Width-1, Height-1 );
            GenereateDisturbLine( image, g );
            GenereateNoise( image, fcolor );
            GenerateText( g, brush );
        }

        /// <summary>
        /// 生成干扰线
        /// </summary>
        private void GenereateDisturbLine( Bitmap image, Graphics g ) {
            if ( IsGenereateDisturbLine == false )
                return;
            int x1, x2, y1, y2;
            for ( int i = 0; i < _value.Length; i++ ) {
                Pen p = new Pen( _colors[DateTime.Now.Second % 10], 1.5f );
                x1 = _random.Next( image.Width );
                y1 = _random.Next( image.Height );
                x2 = _random.Next( image.Width );
                y2 = _random.Next( image.Height );
                g.DrawLine( p, x1, y1, x2, y2 );
            }
        }

        /// <summary>
        /// 生成随机噪点
        /// </summary>
        private void GenereateNoise( Bitmap image, Color fcolor ) {
            for ( int i = 0; i < image.Width * 2; i++ ) {
                int x = _random.Next( image.Width );
                int y = _random.Next( image.Height );
                image.SetPixel( x, y, fcolor );
            }
        }

        /// <summary>
        /// 生成文本
        /// </summary>
        private void GenerateText( Graphics g, SolidBrush brush ) {
            for ( int i = 0; i < _value.Length; i++ ) {
                string c = _value.Substring( i, 1 );
                int v = c[0];
                Font f = ( v + DateTime.Now.Millisecond ) % 2 == 0
                             ? new Font( "Verdana", FontSize )
                             : new Font( "Verdana", FontSize, FontStyle.Italic );
                g.DrawString( c, f, brush, 4 + i * FontSize - DateTime.Now.Millisecond % 2, DateTime.Now.Second % 2 );
            }
        }

        /// <summary>
        /// //输出图片
        /// </summary>
        /// <param name="image">验证码图片</param>
        private void Response( Bitmap image ) {
            using ( var ms = new MemoryStream() ) {
                image.Save( ms, ImageFormat.Gif );
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Gif";
                HttpContext.Current.Response.Expires = -1;
                HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddDays( -1 );
                HttpContext.Current.Response.Cache.SetNoStore();
                HttpContext.Current.Response.BinaryWrite( ms.ToArray() );
            }
        }

        #endregion

        #region Check(检查验证码)

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="input">用户填写的验证码</param>
        public bool Check( string input ) {
            if ( input.IsEmpty() )
                return false;
            input = input.Trim().ToUpper().Replace( "O", "0" ).Replace( "I", "1" );
            input = Encrypt.Md5By32( input );
            string validateCode = Web.GetSession( _name );
            if ( validateCode.IsEmpty() )
                return false;
            Web.RemoveSession( _name );
            return validateCode.ToUpper().Trim() == input.ToUpper().Trim();
        }

        #endregion
    }
}
