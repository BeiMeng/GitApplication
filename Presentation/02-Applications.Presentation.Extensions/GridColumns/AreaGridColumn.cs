using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Util;
using Util.Webs.EasyUi;
using Util.Webs.EasyUi.Forms.Comboxs;
using Util.Webs.EasyUi.Grids;

namespace Presentation.Extensions.GridColumns {
    /// <summary>
    /// 地区控件
    /// </summary>
    public class AreaGridColumn<T> : IAreaGridColumn<T> {
        /// <summary>
        /// 初始化地区控件
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        public AreaGridColumn(HtmlHelper<T> helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// HtmlHelper
        /// </summary>
        private readonly HtmlHelper<T> _helper;
        /// <summary>
        /// 远程Url
        /// </summary>
        protected const string Url = "/Commons/Area/AreaControl";
        /// <summary>
        /// 控件下拉框高度
        /// </summary>
        protected const string Height = "250";
        /// <summary>
        /// 控件宽度
        /// </summary>
        protected const int Width = 150;

        /// <summary>
        /// 显示省份列表
        /// </summary>
        /// <param name="name">省份控件name</param>
        /// <param name="value">省份地区Id</param>
        /// <param name="hiddenName">省份名称hidden控件name</param>
        /// <param name="text">省份显示的文本</param>
        //public IDataGridColumn Province(string name, string value = "", string hiddenName = "", string text = "")
        //{
        //    return EasyUiFactory.CreateCombox( name, "control_Commons_Area_Province", Url, value ).Hidden( hiddenName, text ).Child( "control_Commons_Area_City" ).PanelHeight( Height ).Width( Width ).Editable( false );
        //    //return EasyUiFactory.CreateDataGridColumn(name, "control_Commons_Area_Province", Url, value).Hidden(hiddenName, text).Child("control_Commons_Area_City").PanelHeight(Height).Width(Width).Editable(false);
        //}

        /// <summary>
        /// 显示城市列表
        /// </summary>
        /// <param name="name">城市控件name</param>
        /// <param name="value">城市地区Id</param>
        /// <param name="hiddenName">城市名称hidden控件name</param>
        /// <param name="text">城市显示的文本</param>
        public ICombox City( string name, string value = "", string hiddenName = "", string text = "" ) {
            return EasyUiFactory.CreateCombox( name, "control_Commons_Area_City", Url, value ).Hidden( hiddenName, text ).Child( "control_Commons_Area_County" ).PanelHeight( Height ).Width( Width ).Editable( false );
        }

        /// <summary>
        /// 显示区县列表
        /// </summary>
        /// <param name="name">区县控件name</param>
        /// <param name="value">区县地区Id</param>
        /// <param name="hiddenName">区县名称hidden控件name</param>
        /// <param name="text">区县显示的文本</param>
        public ICombox County( string name, string value = "", string hiddenName = "", string text = "" ) {
            return EasyUiFactory.CreateCombox( name, "control_Commons_Area_County", Url, value ).Hidden( hiddenName, text ).PanelHeight( Height ).Width( Width ).Editable( false );
        }

        ///// <summary>
        ///// 显示省份列表
        ///// </summary>
        ///// <param name="expressionAreaId">省份Id表达式</param>
        ///// <param name="expressionAreaName">省份名称表达式</param>
        //public ICombox Province<TProperty>( Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null ) {
        //    return Operation( Province, expressionAreaId, expressionAreaName );
        //}

        ///// <summary>
        ///// 操作
        ///// </summary>
        //private ICombox Operation<TProperty>( Func<string, string, string, string, ICombox> action, Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName ) {
        //    var name = Lambda.GetName( expressionAreaId );
        //    var value = _helper.Value( expressionAreaId ).ToStr();
        //    var hiddenName = Lambda.GetName( expressionAreaName );
        //    var text = _helper.Value( expressionAreaName ).ToStr();
        //    return action( name, value, hiddenName, text );
        //}

        ///// <summary>
        ///// 显示城市列表
        ///// </summary>
        ///// <param name="expressionAreaId">城市Id表达式</param>
        ///// <param name="expressionAreaName">城市名称表达式</param>
        //public ICombox City<TProperty>( Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null ) {
        //    return Operation( City, expressionAreaId, expressionAreaName );
        //}

        ///// <summary>
        ///// 显示区县列表
        ///// </summary>
        ///// <param name="expressionAreaId">区县Id表达式</param>
        ///// <param name="expressionAreaName">区县名称表达式</param>
        //public ICombox County<TProperty>( Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null ) {
        //    return Operation( County, expressionAreaId, expressionAreaName );
        //}

        public IDataGridColumn Province(string name, string value = "", string hiddenName = "", string text = "")
        {
            throw new NotImplementedException();
        }


        public ICombox Province<TProperty>(Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null)
        {
            throw new NotImplementedException();
        }

        public ICombox City<TProperty>(Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null)
        {
            throw new NotImplementedException();
        }

        public ICombox County<TProperty>(Expression<Func<T, TProperty>> expressionAreaId, Expression<Func<T, string>> expressionAreaName = null)
        {
            throw new NotImplementedException();
        }
    }
}
