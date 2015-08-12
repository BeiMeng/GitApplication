using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EntityFramework.Mapping {
    /// <summary>
    /// A class representing a property map
    /// </summary>
    [DebuggerDisplay( "Property: {PropertyName}, Column: {ColumnName}" )]
    public class PropertyMap {
        public PropertyMap() : this( "","",null ){
        }

        public PropertyMap( string propertyName, string columnName, Type propertyType ) {
            _childs = new List<PropertyMap>();
            PropertyName = propertyName;
            ColumnName = columnName;
            PropertyType = propertyType;
        }

        /// <summary>
        /// 子属性
        /// </summary>
        private readonly List<PropertyMap> _childs;

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 是否复杂属性
        /// </summary>
        public bool IsComplex { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// 添加子属性
        /// </summary>
        /// <param name="child">子属性</param>
        public void AddProperty( PropertyMap child ) {
            if ( child == null )
                return;
            _childs.Add( child );
        }

        /// <summary>
        /// 获取子属性
        /// </summary>
        public List<PropertyMap> GetChilds() {
            return _childs;
        }
    }
}