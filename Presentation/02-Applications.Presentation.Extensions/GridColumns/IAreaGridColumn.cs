using System;
using System.Linq.Expressions;
using Util.Webs.EasyUi.Forms.Comboxs;
using Util.Webs.EasyUi.Grids;

namespace Presentation.Extensions.GridColumns {
    /// <summary>
    /// 地区控件
    /// </summary>
    public interface IAreaGridColumn<T>
    {
        /// <summary>
        /// 显示省份列表
        /// </summary>
        /// <param name="name">省份控件name</param>
        /// <param name="value">省份地区Id</param>
        /// <param name="hiddenName">省份名称hidden控件name</param>
        /// <param name="text">省份显示的文本</param>
        IDataGridColumn Province(string name, string value = "", string hiddenName = "", string text = "");
        /// <summary>
        /// 显示城市列表
        /// </summary>
        /// <param name="name">城市控件name</param>
        /// <param name="value">城市地区Id</param>
        /// <param name="hiddenName">城市名称hidden控件name</param>
        /// <param name="text">城市显示的文本</param>
        ICombox City( string name, string value = "", string hiddenName = "", string text = "" );
        /// <summary>
        /// 显示区县列表
        /// </summary>
        /// <param name="name">区县控件name</param>
        /// <param name="value">区县地区Id</param>
        /// <param name="hiddenName">区县名称hidden控件name</param>
        /// <param name="text">区县显示的文本</param>
        ICombox County( string name, string value = "", string hiddenName = "", string text = "" );
        /// <summary>
        /// 显示省份列表
        /// </summary>
        /// <param name="expressionAreaId">省份Id表达式</param>
        /// <param name="expressionAreaName">省份名称表达式</param>
        ICombox Province<TProperty>( Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null );
        /// <summary>
        /// 显示城市列表
        /// </summary>
        /// <param name="expressionAreaId">城市Id表达式</param>
        /// <param name="expressionAreaName">城市名称表达式</param>
        ICombox City<TProperty>( Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null );
        /// <summary>
        /// 显示区县列表
        /// </summary>
        /// <param name="expressionAreaId">区县Id表达式</param>
        /// <param name="expressionAreaName">区县名称表达式</param>
        ICombox County<TProperty>( Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null );
    }
}
