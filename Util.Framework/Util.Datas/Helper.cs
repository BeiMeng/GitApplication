using System;
using System.Collections.Generic;
using System.Data;

namespace Util.Datas {
    /// <summary>
    /// 数据操作
    /// </summary>
    public static class Helper {
        /// <summary>
        /// 将实体集合转换为DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public static DataTable ToDataTable<T>( List<T> entities ) {
            var result = CreateTable<T>();
            FillData( result, entities );
            return result;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        private static DataTable CreateTable<T>() {
            var result = new DataTable();
            var type = typeof( T );
            foreach ( var property in type.GetProperties( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance ) ) {
                var propertyType = property.PropertyType;
                if ( ( propertyType.IsGenericType ) && ( propertyType.GetGenericTypeDefinition() == typeof( Nullable<> ) ) )
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add( property.Name, propertyType );
            }
            return result;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private static void FillData<T>( DataTable dt, IEnumerable<T> entities ) {
            foreach ( var entity in entities ) {
                dt.Rows.Add( CreateRow( dt, entity ) );
            }
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private static DataRow CreateRow<T>( DataTable dt, T entity ) {
            DataRow row = dt.NewRow();
            var type = typeof( T );
            foreach ( var property in type.GetProperties( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance ) ) {
                row[property.Name] = property.GetValue( entity ) ?? DBNull.Value;
            }
            return row;
        }
    }
}
