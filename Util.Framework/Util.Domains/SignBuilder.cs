using System;
using System.Collections.Generic;
using System.Text;
using Util.Algorithm;

namespace Util.Domains {
    /// <summary>
    /// 签名生成器
    /// </summary>
    public class SignBuilder {
        /// <summary>
        /// 初始化签名生成器
        /// </summary>
        public SignBuilder() {
            _params = new Str();
            _data = new List<string>();
            _sort = new BubbleSort();
            _key = string.Empty;
        }

        /// <summary>
        /// 签名参数
        /// </summary>
        private readonly Str _params;
        /// <summary>
        /// 数据
        /// </summary>
        private readonly List<string> _data;
        /// <summary>
        /// 排序
        /// </summary>
        private readonly ISort _sort;
        /// <summary>
        /// 密钥
        /// </summary>
        private string _key;

        /// <summary>
        /// 创建签名
        /// </summary>
        public string CreateSign() {
            string result = GetSignParams();
            if ( _key.IsEmpty() == false )
                result += "&" + _key;
            return Encrypt.Md5By32( result, Encoding.GetEncoding( "gb2312" ) );
        }

        /// <summary>
        /// 获取签名参数
        /// </summary>
        public string GetSignParams() {
            var sortList = _sort.Sort( _data.ToArray() );
            foreach ( var each in sortList )
                _params.Add( "{0}&", each );
            _params.RemoveEnd( "&" );
            return _params.ToString();
        }

        /// <summary>
        /// 添加签名数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void Add( string name, string value ) {
            if ( name == null )
                name = string.Empty;
            if ( value == null )
                value = string.Empty;
            _data.Add( string.Format( "{0}={1}",name.ToLower().Trim(),value.ToLower().Trim() ) );
        }

        /// <summary>
        /// 添加签名数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void Add( string name, int value ) {
            Add( name,value.ToString() );
        }

        /// <summary>
        /// 添加签名数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void Add( string name, bool value ) {
            Add( name, value.ToString() );
        }

        /// <summary>
        /// 添加签名数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void Add( string name, double value ) {
            Add( name, value.ToString() );
        }

        /// <summary>
        /// 添加签名数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void Add( string name, decimal value ) {
            Add( name, Format.RemoveEnd0( value ) );
        }

        /// <summary>
        /// 添加签名数据
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public void Add( string name, Guid value ) {
            Add( name, value.ToString() );
        }

        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key">密钥</param>
        public void SetKey( string key ) {
            _key = key;
        }
    }
}
