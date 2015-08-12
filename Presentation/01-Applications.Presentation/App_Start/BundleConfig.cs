using System.Web.Optimization;

namespace Presentation {
    /// <summary>
    /// 资源打包配置
    /// </summary>
    public class BundleConfig {
        /// <summary>
        /// 注册资源打包
        /// </summary>
        public static void RegisterBundles( BundleCollection bundles ) {
            //启用打包压缩
            BundleTable.EnableOptimizations = false;
            //css样式
            bundles.Add( new StyleBundle( "~/Css/css" ).Include(
                "~/Css/icon.css",
                "~/Css/common.css" ) );
            //Easyui
            bundles.Add( new ScriptBundle( "~/Scripts/EasyUi/source/js" ).Include(
                "~/Scripts/EasyUi/source/jquery.parser.js",
                "~/Scripts/EasyUi/source/jquery.draggable.js",
                "~/Scripts/EasyUi/source/jquery.droppable.js",
                "~/Scripts/EasyUi/source/jquery.resizable.js",
                "~/Scripts/EasyUi/source/jquery.linkbutton.js",
                "~/Scripts/EasyUi/source/jquery.pagination.js",
                "~/Scripts/EasyUi/source/jquery.tree.js",
                "~/Scripts/EasyUi/source/jquery.progressbar.js",
                "~/Scripts/EasyUi/source/jquery.tooltip.js",
                "~/Scripts/EasyUi/source/jquery.panel.js",
                "~/Scripts/EasyUi/source/jquery.window.js",
                "~/Scripts/EasyUi/source/jquery.dialog.js",
                "~/Scripts/EasyUi/source/jquery.accordion.js",
                "~/Scripts/EasyUi/source/jquery.tabs.js",
                "~/Scripts/EasyUi/source/jquery.layout.js",
                "~/Scripts/EasyUi/source/jquery.menu.js",
                "~/Scripts/EasyUi/source/jquery.menubutton.js",
                "~/Scripts/EasyUi/source/jquery.splitbutton.js",
                "~/Scripts/EasyUi/source/jquery.validatebox.js",
                "~/Scripts/EasyUi/source/jquery.textbox.js",
                "~/Scripts/EasyUi/source/jquery.searchbox.js",
                "~/Scripts/EasyUi/source/jquery.form.js",
                "~/Scripts/EasyUi/source/jquery.numberbox.js",
                "~/Scripts/EasyUi/source/jquery.calendar.js",
                "~/Scripts/EasyUi/source/jquery.spinner.js",
                "~/Scripts/EasyUi/source/jquery.numberspinner.js",
                "~/Scripts/EasyUi/source/jquery.timespinner.js",
                "~/Scripts/EasyUi/source/jquery.datetimespinner.js",
                "~/Scripts/EasyUi/source/jquery.datagrid.js",
                "~/Scripts/EasyUi/source/jquery.propertygrid.js",
                "~/Scripts/EasyUi/source/jquery.treegrid.js",
                "~/Scripts/EasyUi/source/jquery.combo.js",
                "~/Scripts/EasyUi/source/jquery.combobox.js",
                "~/Scripts/EasyUi/source/jquery.combotree.js",
                "~/Scripts/EasyUi/source/jquery.combogrid.js",
                "~/Scripts/EasyUi/source/jquery.datebox.js",
                "~/Scripts/EasyUi/source/jquery.datetimebox.js",
                "~/Scripts/EasyUi/source/jquery.slider.js",
                "~/Scripts/EasyUi/source/jquery.filebox.js",
                "~/Scripts/EasyUi/source/jquery.messager.js",
                "~/Scripts/EasyUi/source/jquery.datalist.js"
            ) );
            //Easyui扩展
            bundles.Add( new ScriptBundle( "~/Scripts/EasyUi/js" ).Include(
                "~/Scripts/EasyUi/easyui-lang-zh_CN.js",
                "~/scripts/EasyUi/jquery.easyui.edatagrid.js",
                "~/scripts/EasyUi/jquery.easyui.treegrid.dnd.js",
                "~/scripts/EasyUi/jquery.easyui.etreegrid.js",
                "~/scripts/EasyUi/jquery.easyui.datagrid.detailview.js",
                "~/scripts/EasyUi/jquery.easyui.datagrid.lookup.js" ) );
            //util js
            bundles.Add( new ScriptBundle( "~/Scripts/Utils/js" ).Include(
                "~/Scripts/Utils/util.js",
                "~/Scripts/Utils/jquery.util.js",
                "~/Scripts/Utils/jquery.util.webuploader.js",
                "~/Scripts/Utils/jquery.util.easyui.extension.js",
                "~/Scripts/Utils/jquery.util.easyui.js",
                "~/Scripts/Utils/jquery.util.easyui.config.js",
                "~/Scripts/Utils/jquery.util.easyui.form.js",
                "~/Scripts/Utils/jquery.util.easyui.grid.js",
                "~/Scripts/Utils/jquery.util.easyui.tree.js",
                "~/Scripts/Utils/jquery.util.easyui.fn.js" ) );
        }
    }
}