using System;
using System.ComponentModel.DataAnnotations;
using Util.Domains.Repositories;

namespace Util.Domains {
    /// <summary>
    /// 树型实体查询参数
    /// </summary>
    public abstract class TreeEntityQuery : Pager,ITreeEntityQuery {
        /// <summary>
        /// 初始化树型实体查询参数
        /// </summary>
        protected TreeEntityQuery() {
            Order = "SortId";
        }

        /// <summary>
        /// 父编号
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 级数
        /// </summary>
        public int? Level { get; set; }

        private string _path = string.Empty;
        /// <summary>
        /// 路径
        /// </summary>
        public string Path {
            get { return _path == null ? string.Empty : _path.Trim(); }
            set { _path = value; }
        }

        /// <summary>
        /// 启用
        /// </summary>
        [Display( Name = "启用" )]
        public bool? Enabled { get; set; }

        /// <summary>
        /// 添加描述
        /// </summary>
        protected override void AddDescriptions() {
            base.AddDescriptions();
            AddDescription( "父编号", ParentId );
            AddDescription( "路径", Path );
            AddDescription( "级数", Level );
            AddDescription( "启用", Enabled.Description() );
        }

        /// <summary>
        /// 参数是否为空
        /// </summary>
        private bool _isEmpty = true;

        /// <summary>
        /// 查询参数是否全部为空
        /// </summary>
        public bool IsEmpty() {
            CheckEmpty();
            return _isEmpty;
        }

        /// <summary>
        /// 检查查询参数是否全部为空值
        /// </summary>
        protected virtual void CheckEmpty() {
            AddValue( ParentId );
            AddValue( Path );
            AddValue( Level );
            AddValue( Enabled );
        }

        /// <summary>
        /// 添加参数值
        /// </summary>
        protected void AddValue<T>( T value ) {
            if ( value.ToStr().IsEmpty() )
                return;
            _isEmpty = false;
        }
    }
}
