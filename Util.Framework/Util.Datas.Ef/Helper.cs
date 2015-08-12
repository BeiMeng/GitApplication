using System;
using System.Collections.Generic;
using System.Data;
using EntityFramework.Mapping;

namespace Util.Datas.Ef {
    /// <summary>
    /// 数据操作
    /// </summary>
    public static class Helper {
        /// <summary>
        /// 将实体集合转换为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        /// <param name="map">实体映射配置</param>
        public static DataTable ToDataTable<T>( List<T> entities, EntityMap map ) {
            var result = CreateTable<T>( map );
            FillData( result, entities, map );
            return result;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        private static DataTable CreateTable<T>( EntityMap map ) {
            var result = new DataTable();
            var type = typeof( T );
            foreach( var each in map.PropertyMaps )
                AddColumn( result, type, each );
            return result;
        }

        /// <summary>
        /// 添加列
        /// </summary>
        private static void AddColumn( DataTable result,Type type,PropertyMap propertyMap ) {
            if ( propertyMap.IsComplex ) {
                foreach ( var child in propertyMap.GetChilds() )
                    AddColumn( result, child.PropertyType, child.ColumnName );
                return;
            }
            var propertyType = type.GetProperty( propertyMap.PropertyName ).PropertyType;
            AddColumn( result, propertyType, propertyMap.ColumnName );
        }

        /// <summary>
        /// 添加列
        /// </summary>
        private static void AddColumn( DataTable result, Type propertyType, string columnName ) {
            if ( ( propertyType.IsGenericType ) && ( propertyType.GetGenericTypeDefinition() == typeof( Nullable<> ) ) )
                propertyType = propertyType.GetGenericArguments()[0];
            result.Columns.Add( columnName, propertyType );
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private static void FillData<T>( DataTable dt, IEnumerable<T> entities, EntityMap map ) {
            foreach ( var entity in entities ) {
                var row = CreateRow( dt, entity, map );
                if( row != null )
                    dt.Rows.Add( row );
            }
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private static DataRow CreateRow<T>( DataTable dt, T entity, EntityMap map ) {
            DataRow row = dt.NewRow();
            var type = typeof( T );
            foreach( var propertyMap in map.PropertyMaps )
                AddRow( row, type, entity, propertyMap );
            return row;
        }

        /// <summary>
        /// 添加行
        /// </summary>
        private static void AddRow<T>( DataRow row, Type type, T entity, PropertyMap propertyMap ) {
            if ( propertyMap.IsComplex ) {
                AddComplexRow( row, type.GetProperty( propertyMap.PropertyName ).GetValue( entity ), propertyMap );
                return;
            }
            AddRow( row, propertyMap.ColumnName, type.GetProperty( propertyMap.PropertyName ).GetValue( entity ) );
        }

        /// <summary>
        /// 添加复杂属性
        /// </summary>
        private static void AddComplexRow( DataRow row, object complex, PropertyMap propertyMap ) {
            foreach( var child in propertyMap.GetChilds() )
                AddRow( row, child.ColumnName, complex.GetType().GetProperty( child.PropertyName ).GetValue( complex ) );
        }

        /// <summary>
        /// 添加行
        /// </summary>
        private static void AddRow( DataRow row, string columnName, object value ) {
            row[columnName] = value ?? DBNull.Value;
        }
    }
}
