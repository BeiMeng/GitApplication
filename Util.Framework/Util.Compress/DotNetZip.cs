using System.Collections.Generic;
using Ionic.Zip;

namespace Util.Compress {
    /// <summary>
    /// Ionic压缩操作
    /// </summary>
    public class DotNetZip : ICompress {
        /// <summary>
        /// 初始化Ionic压缩操作
        /// </summary>
        public DotNetZip() {
            _fromPathList = new List<string>();
        }

        /// <summary>
        /// 源文件路径列表
        /// </summary>
        private readonly List<string> _fromPathList;

        /// <summary>
        /// 密码
        /// </summary>
        private string _password;

        /// <summary>
        /// 设置密码
        /// </summary>
        /// <param name="password">密码</param>
        public ICompress Password( string password ) {
            _password = password;
            return this;
        }

        /// <summary>
        /// 添加源目录,用于压缩或解压缩
        /// </summary>
        /// <param name="fromDirectory">源目录绝对路径</param>
        public ICompress AddDirectory( params string[] fromDirectory ) {
            if ( fromDirectory == null )
                return this;
            foreach ( var directory in fromDirectory ) {
                var files = File.GetAllFiles( directory );
                files.ForEach( file => AddFile( file ) );
            }
            return this;
        }

        /// <summary>
        /// 添加源文件,用于压缩或解压缩
        /// </summary>
        /// <param name="fromPath">源文件绝对路径</param>
        public ICompress AddFile( params string[] fromPath ) {
            if ( fromPath == null )
                return this;
            foreach( string path in fromPath ) {
                if ( System.IO.File.Exists( path ) )
                    _fromPathList.Add( path );
            }
            return this;
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="toDirectory">压缩到该目录</param>
        /// <param name="toFileName">压缩文件名，不带扩展名，自动添加.zip扩展名</param>
        public void Compress( string toDirectory, string toFileName ) {
            using ( var zip = new ZipFile() ) {
                zip.Password = _password;
                AddFiles( zip );
                zip.Save( GetFilePath( toDirectory, toFileName ) );
            }
        }

        /// <summary>
        /// 添加文件列表
        /// </summary>
        private void AddFiles( ZipFile zip ) {
            foreach ( var path in _fromPathList )
                zip.AddFile( path );
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        private string GetFilePath( string toDirectory, string toFileName ) {
            return File.JoinPath( toDirectory, toFileName ) + ".zip";
        }

        /// <summary>
        /// 解压缩文件
        /// </summary>
        /// <param name="toDirectory">解压到该目录</param>
        public void Decompress( string toDirectory ) {
            foreach ( var path in _fromPathList ) {
                Decompress( path, toDirectory );
            }
        }

        /// <summary>
        /// 解压缩单个zip文件
        /// </summary>
        private void Decompress( string zipPath, string toDirectory ) {
            if ( zipPath.IsEmpty() )
                return;
            if ( !zipPath.ToLower().Trim().EndsWith( "zip" ) )
                return;
            using ( ZipFile zip = ZipFile.Read( zipPath ) ) {
                foreach ( ZipEntry each in zip ) {
                    if ( _password.IsEmpty() )
                        each.Extract( toDirectory );
                    else
                        each.ExtractWithPassword( toDirectory, _password );
                }
            }
        }
    }
}
