/**
 * jQuery EasyUI 1.4.2
 * 
 * Copyright (c) 2009-2015 www.jeasyui.com. All rights reserved.
 *
 * Licensed under the GPL license: http://www.gnu.org/licenses/gpl.txt
 * To use it on other terms please contact us at info@jeasyui.com
 *
 */
(function ($) {
    var tableCount = 0;

    function getObjectIndex(rows, idField) {
        for (var i = 0, rowsLength = rows.length; i < rowsLength; i++) {
            if (rows[i] == idField) {
                return i;
            }
        }
        return -1;
    };

    function unSelectedRowsById(rows, idField, id) {
        if (typeof idField == "string") {
            for (var i = 0, rowsLength = rows.length; i < rowsLength; i++) {
                if (rows[i][idField] == id) {
                    rows.splice(i, 1);
                    return;
                }
            }
        } else {
            var rowIndex = getObjectIndex(rows, idField);
            if (rowIndex != -1) {
                rows.splice(rowIndex, 1);
            }
        }
    };

    function addToRows(selectedOrCheckedRows, idField, row) {
        for (var i = 0, rowLength = selectedOrCheckedRows.length; i < rowLength; i++) {
            if (selectedOrCheckedRows[i][idField] == row[idField]) {
                return;
            }
        }
        selectedOrCheckedRows.push(row);
    };

    function getRowData(target, aa) {
        return $.data(target, "treegrid") ? aa.slice(1) : aa;
    };

    function createStyleSheet(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var panel = state.panel;
        var dc = state.dc;
        var ss = null;
        if (opts.sharedStyleSheet) {
            ss = typeof opts.sharedStyleSheet == "boolean" ? "head" : opts.sharedStyleSheet;
        } else {
            ss = panel.closest("div.datagrid-view");
            if (!ss.length) {
                ss = dc.view;
            }
        }
        var cc = $(ss);
        var ssState = $.data(cc[0], "ss");
        if (!ssState) {
            ssState = $.data(cc[0], "ss", { cache: {}, dirty: [] });
        }
        return {
            add: function (cssRules) {
                var ss = ["<style type=\"text/css\" easyui=\"true\">"];
                for (var i = 0; i < cssRules.length; i++) {
                    ssState.cache[cssRules[i][0]] = { width: cssRules[i][1] };
                }
                var itemCount = 0;
                for (var s in ssState.cache) {
                    var cacheItem = ssState.cache[s];
                    cacheItem.index = itemCount++;
                    ss.push(s + "{width:" + cacheItem.width + "}");
                }
                ss.push("</style>");
                $(ss.join("\n")).appendTo(cc);
                cc.children("style[easyui]:not(:last)").remove();
            },
            getRule: function (ruleIndex) {
                var styleDom = cc.children("style[easyui]:last")[0];
                var styleSheet = styleDom.styleSheet ? styleDom.styleSheet : (styleDom.sheet || document.styleSheets[document.styleSheets.length - 1]);
                var cssRules = styleSheet.cssRules || styleSheet.rules;
                return cssRules[ruleIndex];
            },
            set: function (name, value) {
                var cssItemInCache = ssState.cache[name];
                if (cssItemInCache) {
                    cssItemInCache.width = value;
                    var cssRule = this.getRule(cssItemInCache.index);
                    if (cssRule) {
                        cssRule.style["width"] = value;
                    }
                }
            },
            remove: function (name) {
                var tmp = [];
                for (var s in ssState.cache) {
                    if (s.indexOf(name) == -1) {
                        tmp.push([s, ssState.cache[s].width]);
                    }
                }
                ssState.cache = {};
                this.add(tmp);
            },
            dirty: function (dirtyItem) {
                if (dirtyItem) {
                    ssState.dirty.push(dirtyItem);
                }
            },
            clean: function () {
                for (var i = 0; i < ssState.dirty.length; i++) {
                    this.remove(ssState.dirty[i]);
                }
                ssState.dirty = [];
            }
        };
    };

    function setSize(target, param) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var panel = state.panel;
        if (param) {
            $.extend(opts, param);
        }
        if (opts.fit == true) {
            var p = panel.panel("panel").parent();
            opts.width = p.width();
            opts.height = p.height();
        }
        panel.panel("resize", opts);
    };

    function fitGridSize(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        var panel = state.panel;
        var panelWidth = panel.width();
        var panelHeight = panel.height();
        var dcView = dc.view;
        var dcView1 = dc.view1;
        var dcView2 = dc.view2;
        var view1Header = dcView1.children("div.datagrid-header");
        var view2Header = dcView2.children("div.datagrid-header");
        var view1Table = view1Header.find("table");
        var view2Table = view2Header.find("table");
        dcView.width(panelWidth);
        var headerInner = view1Header.children("div.datagrid-header-inner").show();
        dcView1.width(headerInner.find("table").width());
        if (!opts.showHeader) {
            headerInner.hide();
        }
        dcView2.width(panelWidth - dcView1._outerWidth());
        dcView1.children()._outerWidth(dcView1.width());
        dcView2.children()._outerWidth(dcView2.width());
        var all = view1Header.add(view2Header).add(view1Table).add(view2Table);
        all.css("height", "");
        var hh = Math.max(view1Table.height(), view2Table.height());
        all._outerHeight(hh);
        dc.body1.add(dc.body2).children("table.datagrid-btable-frozen").
            css({ position: "absolute", top: dc.header2._outerHeight() });
        var top = dc.body2.children("table.datagrid-btable-frozen")._outerHeight();
        var height = top + view2Header._outerHeight() + dcView2.children(".datagrid-footer")._outerHeight();
        panel.children(":not(.datagrid-view)").each(function () {
            height += $(this)._outerHeight();
        });
        var paddingHeight = panel.outerHeight() - panel.height();
        var panelMinHeight = panel._size("minHeight") || "";
        var panelMaxHeight = panel._size("maxHeight") || "";
        dcView1.add(dcView2).children("div.datagrid-body").css({ marginTop: top, height: (isNaN(parseInt(opts.height)) ? "" : (panelHeight - height)), minHeight: (panelMinHeight ? panelMinHeight - paddingHeight - height : ""), maxHeight: (panelMaxHeight ? panelMaxHeight - paddingHeight - height : "") });
        dcView.height(dcView2.height());
    };

    function fixRowHeight(target, index, height) {
        var rows = $.data(target, "datagrid").data.rows;
        var opts = $.data(target, "datagrid").options;
        var dc = $.data(target, "datagrid").dc;
        if (!dc.body1.is(":empty") && (!opts.nowrap || opts.autoRowHeight || height)) {
            if (index != undefined) {
                var tr1 = opts.finder.getTr(target, index, "body", 1);
                var tr2 = opts.finder.getTr(target, index, "body", 2);
                fixHeight(tr1, tr2);
            } else {
                var tr1 = opts.finder.getTr(target, 0, "allbody", 1);
                var tr2 = opts.finder.getTr(target, 0, "allbody", 2);
                fixHeight(tr1, tr2);
                if (opts.showFooter) {
                    var tr1 = opts.finder.getTr(target, 0, "allfooter", 1);
                    var tr2 = opts.finder.getTr(target, 0, "allfooter", 2);
                    fixHeight(tr1, tr2);
                }
            }
        }
        fitGridSize(target);
        if (opts.height == "auto") {
            var parent = dc.body1.parent();
            var body2 = dc.body2;
            var body2Size = getSize(body2);
            var height2 = body2Size.height;
            if (body2Size.width > body2.width()) {
                height2 += 18;
            }
            height2 -= parseInt(body2.css("marginTop")) || 0;
            parent.height(height2);
            body2.height(height2);
            dc.view.height(dc.view2.height());
        }
        dc.body2.triggerHandler("scroll");

        function fixHeight(trs1, trs2) {
            for (var i = 0; i < trs2.length; i++) {
                var tr1 = $(trs1[i]);
                var tr2 = $(trs2[i]);
                tr1.css("height", "");
                tr2.css("height", "");
                var height = Math.max(tr1.height(), tr2.height());
                tr1.css("height", height);
                tr2.css("height", height);
            }
        };

        function getSize(cc) {
            var width = 0;
            var height1 = 0;
            $(cc).children().each(function () {
                var c = $(this);
                if (c.is(":visible")) {
                    height1 += c._outerHeight();
                    if (width < c._outerWidth()) {
                        width = c._outerWidth();
                    }
                }
            });
            return { width: width, height: height1 };
        };
    };

    function freezeRow(target, index) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        if (!dc.body2.children("table.datagrid-btable-frozen").length) {
            dc.body1.add(dc.body2).prepend("<table class=\"datagrid-btable datagrid-btable-frozen\" cellspacing=\"0\" cellpadding=\"0\"></table>");
        }
        freeze(true);
        freeze(false);
        fitGridSize(target);

        function freeze(frozen) {
            var frozenType = frozen ? 1 : 2;
            var tr = opts.finder.getTr(target, index, "body", frozenType);
            (frozen ? dc.body1 : dc.body2).children("table.datagrid-btable-frozen").append(tr);
        };
    };

    function wrapGrid(jq, rownumbers) {
        function getColumns() {
            var frozenColumns = [];
            var unfrozenColumns = [];
            $(jq).children("thead").each(function () {
                var opt = $.parser.parseOptions(this, [{ frozen: "boolean" }]);
                $(this).find("tr").each(function () {
                    var cols = [];
                    $(this).find("th").each(function () {
                        var th = $(this);
                        var col = $.extend({}, $.parser.parseOptions(this, ["field", "align", "halign", "order", "width", { sortable: "boolean", checkbox: "boolean", resizable: "boolean", fixed: "boolean" }, { rowspan: "number", colspan: "number" }]), { title: (th.html() || undefined), hidden: (th.attr("hidden") ? true : undefined), formatter: (th.attr("formatter") ? eval(th.attr("formatter")) : undefined), styler: (th.attr("styler") ? eval(th.attr("styler")) : undefined), sorter: (th.attr("sorter") ? eval(th.attr("sorter")) : undefined) });
                        if (col.width && String(col.width).indexOf("%") == -1) {
                            col.width = parseInt(col.width);
                        }
                        if (th.attr("editor")) {
                            var s = $.trim(th.attr("editor"));
                            if (s.substr(0, 1) == "{") {
                                col.editor = eval("(" + s + ")");
                            } else {
                                col.editor = s;
                            }
                        }
                        cols.push(col);
                    });
                    opt.frozen ? frozenColumns.push(cols) : unfrozenColumns.push(cols);
                });
            });
            return [frozenColumns, unfrozenColumns];
        };

        var wrap = $("<div class=\"datagrid-wrap\">" + "<div class=\"datagrid-view\">" + "<div class=\"datagrid-view1\">" + "<div class=\"datagrid-header\">" + "<div class=\"datagrid-header-inner\"></div>" + "</div>" + "<div class=\"datagrid-body\">" + "<div class=\"datagrid-body-inner\"></div>" + "</div>" + "<div class=\"datagrid-footer\">" + "<div class=\"datagrid-footer-inner\"></div>" + "</div>" + "</div>" + "<div class=\"datagrid-view2\">" + "<div class=\"datagrid-header\">" + "<div class=\"datagrid-header-inner\"></div>" + "</div>" + "<div class=\"datagrid-body\"></div>" + "<div class=\"datagrid-footer\">" + "<div class=\"datagrid-footer-inner\"></div>" + "</div>" + "</div>" + "</div>" + "</div>").insertAfter(jq);
        wrap.panel({ doSize: false, cls: "datagrid" });
        $(jq).addClass("datagrid-f").hide().appendTo(wrap.children("div.datagrid-view"));
        var cc = getColumns();
        var gridView = wrap.children("div.datagrid-view");
        var gridView1 = gridView.children("div.datagrid-view1");
        var gridView2 = gridView.children("div.datagrid-view2");
        return { panel: wrap, frozenColumns: cc[0], columns: cc[1], dc: { view: gridView, view1: gridView1, view2: gridView2, header1: gridView1.children("div.datagrid-header").children("div.datagrid-header-inner"), header2: gridView2.children("div.datagrid-header").children("div.datagrid-header-inner"), body1: gridView1.children("div.datagrid-body").children("div.datagrid-body-inner"), body2: gridView2.children("div.datagrid-body"), footer1: gridView1.children("div.datagrid-footer").children("div.datagrid-footer-inner"), footer2: gridView2.children("div.datagrid-footer").children("div.datagrid-footer-inner") } };
    };

    function initGrid(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        var panel = state.panel;
        state.ss = $(target).datagrid("createStyleSheet");
        panel.panel($.extend({}, opts, {
            id: null,
            doSize: false,
            onResize: function (width, height) {
                if ($.data(target, "datagrid")) {
                    fitGridSize(target);
                    $(target).datagrid("fitColumns");
                    opts.onResize.call(panel, width, height);
                }
            },
            onExpand: function () {
                fixRowHeight(target);
                opts.onExpand.call(panel);
            }
        }));
        state.rowIdPrefix = "datagrid-row-r" + (++tableCount);
        state.cellClassPrefix = "datagrid-cell-c" + tableCount;
        buildGridHeader(dc.header1, opts.frozenColumns, true);
        buildGridHeader(dc.header2, opts.columns, false);
        setColumnsWidth();
        dc.header1.add(dc.header2).css("display", opts.showHeader ? "block" : "none");
        dc.footer1.add(dc.footer2).css("display", opts.showFooter ? "block" : "none");
        if (opts.toolbar) {
            if ($.isArray(opts.toolbar)) {
                $("div.datagrid-toolbar", panel).remove();
                var tb = $("<div class=\"datagrid-toolbar\"><table cellspacing=\"0\" cellpadding=\"0\"><tr></tr></table></div>").prependTo(panel);
                var tr = tb.find("tr");
                for (var i = 0; i < opts.toolbar.length; i++) {
                    var btn = opts.toolbar[i];
                    if (btn == "-") {
                        $("<td><div class=\"datagrid-btn-separator\"></div></td>").appendTo(tr);
                    } else {
                        var td = $("<td></td>").appendTo(tr);
                        var tool = $("<a href=\"javascript:void(0)\"></a>").appendTo(td);
                        tool[0].onclick = eval(btn.handler || function () {
                        });
                        tool.linkbutton($.extend({}, btn, { plain: true }));
                    }
                }
            } else {
                $(opts.toolbar).addClass("datagrid-toolbar").prependTo(panel);
                $(opts.toolbar).show();
            }
        } else {
            $("div.datagrid-toolbar", panel).remove();
        }
        $("div.datagrid-pager", panel).remove();
        if (opts.pagination) {
            var pager = $("<div class=\"datagrid-pager\"></div>");
            if (opts.pagePosition == "bottom") {
                pager.appendTo(panel);
            } else {
                if (opts.pagePosition == "top") {
                    pager.addClass("datagrid-pager-top").prependTo(panel);
                } else {
                    var pagerContainer = $("<div class=\"datagrid-pager datagrid-pager-top\"></div>").prependTo(panel);
                    pager.appendTo(panel);
                    pager = pager.add(pagerContainer);
                }
            }
            pager.pagination({
                total: (opts.pageNumber * opts.pageSize),
                pageNumber: opts.pageNumber,
                pageSize: opts.pageSize,
                pageList: opts.pageList,
                onSelectPage: function (pageNumber, pageSize) {
                    opts.pageNumber = pageNumber || 1;
                    opts.pageSize = pageSize;
                    pager.pagination("refresh", { pageNumber: pageNumber, pageSize: pageSize });
                    request(target);
                }
            });
            opts.pageSize = pager.pagination("options").pageSize;
        }

        function buildGridHeader(header, cloums, frozen) {
            if (!cloums) {
                return;
            }
            $(header).show();
            $(header).empty();
            var sortNames = [];
            var sortOrders = [];
            if (opts.sortName) {
                sortNames = opts.sortName.split(",");
                sortOrders = opts.sortOrder.split(",");
            }
            var t = $("<table class=\"datagrid-htable\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody></tbody></table>").appendTo(header);
            for (var i = 0; i < cloums.length; i++) {
                var tr = $("<tr class=\"datagrid-header-row\"></tr>").appendTo($("tbody", t));
                var col = cloums[i];
                for (var j = 0; j < col.length; j++) {
                    var col = col[j];
                    var tdHTML = "";
                    if (col.rowspan) {
                        tdHTML += "rowspan=\"" + col.rowspan + "\" ";
                    }
                    if (col.colspan) {
                        tdHTML += "colspan=\"" + col.colspan + "\" ";
                    }
                    var td = $("<td " + tdHTML + "></td>").appendTo(tr);
                    if (col.checkbox) {
                        td.attr("field", col.field);
                        $("<div class=\"datagrid-header-check\"></div>").html("<input type=\"checkbox\"/>").appendTo(td);
                    } else {
                        if (col.field) {
                            td.attr("field", col.field);
                            td.append("<div class=\"datagrid-cell\"><span></span><span class=\"datagrid-sort-icon\"></span></div>");
                            $("span", td).html(col.title);
                            $("span.datagrid-sort-icon", td).html("&nbsp;");
                            var cell = td.find("div.datagrid-cell");
                            var pos = getObjectIndex(sortNames, col.field);
                            if (pos >= 0) {
                                cell.addClass("datagrid-sort-" + sortOrders[pos]);
                            }
                            if (col.resizable == false) {
                                cell.attr("resizable", "false");
                            }
                            if (col.width) {
                                var width = $.parser.parseValue("width", col.width, dc.view, opts.scrollbarSize);
                                cell._outerWidth(width - 1);
                                col.boxWidth = parseInt(cell[0].style.width);
                                col.deltaWidth = width - col.boxWidth;
                            } else {
                                col.auto = true;
                            }
                            cell.css("text-align", (col.halign || col.align || ""));
                            col.cellClass = state.cellClassPrefix + "-" + col.field.replace(/[\.|\s]/g, "-");
                            cell.addClass(col.cellClass).css("width", "");
                        } else {
                            $("<div class=\"datagrid-cell-group\"></div>").html(col.title).appendTo(td);
                        }
                    }
                    if (col.hidden) {
                        td.hide();
                    }
                }
            }
            if (frozen && opts.rownumbers) {
                var td = $("<td rowspan=\"" + opts.frozenColumns.length + "\"><div class=\"datagrid-header-rownumber\"></div></td>");
                if ($("tr", t).length == 0) {
                    td.wrap("<tr class=\"datagrid-header-row\"></tr>").parent().appendTo($("tbody", t));
                } else {
                    td.prependTo($("tr:first", t));
                }
            }
        };

        function setColumnsWidth() {
            var columWidths = [];
            var field = getColumnFields(target, true).concat(getColumnFields(target));
            for (var i = 0; i < field.length; i++) {
                var col = getColumnOption(target, field[i]);
                if (col && !col.checkbox) {
                    columWidths.push(["." + col.cellClass, col.boxWidth ? col.boxWidth + "px" : "auto"]);
                }
            }
            state.ss.add(columWidths);
            state.ss.dirty(state.cellSelectorPrefix);
            state.cellSelectorPrefix = "." + state.cellClassPrefix;
        };
    };

    function bindEvents(target) {
        var state = $.data(target, "datagrid");
        var panel = state.panel;
        var opts = state.options;
        var dc = state.dc;
        var header = dc.header1.add(dc.header2);
        header.find("input[type=checkbox]").unbind(".datagrid").bind("click.datagrid", function (e) {
            if (opts.singleSelect && opts.selectOnCheck) {
                return false;
            }
            if ($(this).is(":checked")) {
                checkAll(target);
            } else {
                uncheckAll(target);
            }
            e.stopPropagation();
        });
        var cells = header.find("div.datagrid-cell");
        cells.closest("td").unbind(".datagrid").bind("mouseenter.datagrid", function () {
            if (state.resizing) {
                return;
            }
            $(this).addClass("datagrid-header-over");
        }).bind("mouseleave.datagrid", function () {
            $(this).removeClass("datagrid-header-over");
        }).bind("contextmenu.datagrid", function (e) {
            var field = $(this).attr("field");
            opts.onHeaderContextMenu.call(target, e, field);
        });
        cells.unbind(".datagrid").bind("click.datagrid", function (e) {
            var p1 = $(this).offset().left + 5;
            var p2 = $(this).offset().left + $(this)._outerWidth() - 5;
            if (e.pageX < p2 && e.pageX > p1) {
                sort(target, $(this).parent().attr("field"));
            }
        }).bind("dblclick.datagrid", function (e) {
            var p1 = $(this).offset().left + 5;
            var p2 = $(this).offset().left + $(this)._outerWidth() - 5;
            var resizeable = opts.resizeHandle == "right" ? (e.pageX > p2) : (opts.resizeHandle == "left" ? (e.pageX < p1) : (e.pageX < p1 || e.pageX > p2));
            if (resizeable) {
                var parentField = $(this).parent().attr("field");
                var col = getColumnOption(target, parentField);
                if (col.resizable == false) {
                    return;
                }
                $(target).datagrid("autoSizeColumn", parentField);
                col.auto = false;
            }
        });
        var resizehandlers = opts.resizeHandle == "right" ? "e" : (opts.resizeHandle == "left" ? "w" : "e,w");
        cells.each(function () {
            $(this).resizable({
                handles: resizehandlers,
                disabled: ($(this).attr("resizable") ? $(this).attr("resizable") == "false" : false),
                minWidth: 25,
                onStartResize: function (e) {
                    state.resizing = true;
                    header.css("cursor", $("body").css("cursor"));
                    if (!state.proxy) {
                        state.proxy = $("<div class=\"datagrid-resize-proxy\"></div>").appendTo(dc.view);
                    }
                    state.proxy.css({ left: e.pageX - $(panel).offset().left - 1, display: "none" });
                    setTimeout(function () {
                        if (state.proxy) {
                            state.proxy.show();
                        }
                    }, 500);
                },
                onResize: function (e) {
                    state.proxy.css({ left: e.pageX - $(panel).offset().left - 1, display: "block" });
                    return false;
                },
                onStopResize: function (e) {
                    header.css("cursor", "");
                    $(this).css("height", "");
                    var parentField = $(this).parent().attr("field");
                    var col = getColumnOption(target, parentField);
                    col.width = $(this)._outerWidth();
                    col.boxWidth = col.width - col.deltaWidth;
                    col.auto = undefined;
                    $(this).css("width", "");
                    $(target).datagrid("fixColumnSize", parentField);
                    state.proxy.remove();
                    state.proxy = null;
                    if ($(this).parents("div:first.datagrid-header").parent().hasClass("datagrid-view1")) {
                        fitGridSize(target);
                    }
                    $(target).datagrid("fitColumns");
                    opts.onResizeColumn.call(target, parentField, col.width);
                    setTimeout(function () {
                        state.resizing = false;
                    }, 0);
                }
            });
        });
        var bb = dc.body1.add(dc.body2);
        bb.unbind();
        for (var rowEvent in opts.rowEvents) {
            bb.bind(rowEvent, opts.rowEvents[rowEvent]);
        }
        dc.body1.bind("mousewheel DOMMouseScroll", function (e) {
            var event = e.originalEvent || window.event;
            var wheelDelta = event.wheelDelta || event.detail * (-1);
            var dg = $(e.target).closest("div.datagrid-view").children(".datagrid-f");
            var dc = dg.data("datagrid").dc;
            dc.body2.scrollTop(dc.body2.scrollTop() - wheelDelta);
        });
        dc.body2.bind("scroll", function () {
            var b1 = dc.view1.children("div.datagrid-body");
            b1.scrollTop($(this).scrollTop());
            var c1 = dc.body1.children(":first");
            var c2 = dc.body2.children(":first");
            if (c1.length && c2.length) {
                var top1 = c1.offset().top;
                var top2 = c2.offset().top;
                if (top1 != top2) {
                    b1.scrollTop(b1.scrollTop() + top1 - top2);
                }
            }
            dc.view2.children("div.datagrid-header, div.datagrid-footer")._scrollLeft($(this)._scrollLeft());
            dc.body2.children("table.datagrid-btable-frozen").css("left", -$(this)._scrollLeft());
        });
    };

    function handleOnmouseover(highlight) {
        return function (e) {
            var tr = getRow(e.target);
            if (!tr) {
                return;
            }
            var targetView = getGridView(tr);
            if ($.data(targetView, "datagrid").resizing) {
                return;
            }
            var rowIndex = getTrRowIndex(tr);
            if (highlight) {
                highlightRow(targetView, rowIndex);
            } else {
                var opts = $.data(targetView, "datagrid").options;
                opts.finder.getTr(targetView, rowIndex).removeClass("datagrid-row-over");
            }
        };
    };

    function handlerOnclick(e) {
        var tr = getRow(e.target);
        if (!tr) {
            return;
        }
        var targetView = getGridView(tr);
        var opts = $.data(targetView, "datagrid").options;
        var rowIndex = getTrRowIndex(tr);
        var tt = $(e.target);
        if (tt.parent().hasClass("datagrid-cell-check")) {
            if (opts.singleSelect && opts.selectOnCheck) {
                tt._propAttr("checked", !tt.is(":checked"));
                checkRow(targetView, rowIndex);
            } else {
                if (tt.is(":checked")) {
                    tt._propAttr("checked", false);
                    checkRow(targetView, rowIndex);
                } else {
                    tt._propAttr("checked", true);
                    uncheckRow(targetView, rowIndex);
                }
            }
        } else {
            var row = opts.finder.getRow(targetView, rowIndex);
            var td = tt.closest("td[field]", tr);
            if (td.length) {
                var field = td.attr("field");
                opts.onClickCell.call(targetView, rowIndex, field, row[field]);
            }
            if (opts.singleSelect == true) {
                selectRow(targetView, rowIndex);
            } else {
                if (opts.ctrlSelect) {
                    if (e.ctrlKey) {
                        if (tr.hasClass("datagrid-row-selected")) {
                            unselectRow(targetView, rowIndex);
                        } else {
                            selectRow(targetView, rowIndex);
                        }
                    } else {
                        if (e.shiftKey) {
                            $(targetView).datagrid("clearSelections");
                            var startIndex = Math.min(opts.lastSelectedIndex || 0, rowIndex);
                            var endIndex = Math.max(opts.lastSelectedIndex || 0, rowIndex);
                            for (var i = startIndex; i <= endIndex; i++) {
                                selectRow(targetView, i);
                            }
                        } else {
                            $(targetView).datagrid("clearSelections");
                            selectRow(targetView, rowIndex);
                            opts.lastSelectedIndex = rowIndex;
                        }
                    }
                } else {
                    if (tr.hasClass("datagrid-row-selected")) {
                        unselectRow(targetView, rowIndex);
                    } else {
                        selectRow(targetView, rowIndex);
                    }
                }
            }
            opts.onClickRow.apply(targetView, getRowData(targetView, [rowIndex, row]));
        }
    };

    function handlerOndblclick(e) {
        var tr = getRow(e.target);
        if (!tr) {
            return;
        }
        var target = getGridView(tr);
        var opts = $.data(target, "datagrid").options;
        var rowIndex = getTrRowIndex(tr);
        var row = opts.finder.getRow(target, rowIndex);
        var td = $(e.target).closest("td[field]", tr);
        if (td.length) {
            var field = td.attr("field");
            opts.onDblClickCell.call(target, rowIndex, field, row[field]);
        }
        opts.onDblClickRow.apply(target, getRowData(target, [rowIndex, row]));
    };

    function handlerContextmenu(e) {
        var tr = getRow(e.target);
        if (!tr) {
            return;
        }
        var target = getGridView(tr);
        var opts = $.data(target, "datagrid").options;
        var rowIndex = getTrRowIndex(tr);
        var row = opts.finder.getRow(target, rowIndex);
        opts.onRowContextMenu.call(target, e, rowIndex, row);
    };

    function getGridView(t) {
        return $(t).closest("div.datagrid-view").children(".datagrid-f")[0];
    };

    function getRow(t) {
        var tr = $(t).closest("tr.datagrid-row");
        if (tr.length && tr.parent().length) {
            return tr;
        } else {
            return undefined;
        }
    };

    function getTrRowIndex(tr) {
        if (tr.attr("datagrid-row-index")) {
            return parseInt(tr.attr("datagrid-row-index"));
        } else {
            return tr.attr("node-id");
        }
    };

    function sort(target, param) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        param = param || {};
        var sortData = { sortName: opts.sortName, sortOrder: opts.sortOrder };
        if (typeof param == "object") {
            $.extend(sortData, param);
        }
        var sortNames = [];
        var sortOrders = [];
        if (sortData.sortName) {
            sortNames = sortData.sortName.split(",");
            sortOrders = sortData.sortOrder.split(",");
        }
        if (typeof param == "string") {
            var sortFieldName = param;
            var col = getColumnOption(target, sortFieldName);
            if (!col.sortable || state.resizing) {
                return;
            }
            var sortOrder = col.order || "asc";
            var pos = getObjectIndex(sortNames, sortFieldName);
            if (pos >= 0) {
                var order = sortOrders[pos] == "asc" ? "desc" : "asc";
                if (opts.multiSort && order == sortOrder) {
                    sortNames.splice(pos, 1);
                    sortOrders.splice(pos, 1);
                } else {
                    sortOrders[pos] = order;
                }
            } else {
                if (opts.multiSort) {
                    sortNames.push(sortFieldName);
                    sortOrders.push(sortOrder);
                } else {
                    sortNames = [sortFieldName];
                    sortOrders = [sortOrder];
                }
            }
            sortData.sortName = sortNames.join(",");
            sortData.sortOrder = sortOrders.join(",");
        }
        if (opts.onBeforeSortColumn.call(target, sortData.sortName, sortData.sortOrder) == false) {
            return;
        }
        $.extend(opts, sortData);
        var dc = state.dc;
        var header = dc.header1.add(dc.header2);
        header.find("div.datagrid-cell").removeClass("datagrid-sort-asc datagrid-sort-desc");
        for (var i = 0; i < sortNames.length; i++) {
            var col = getColumnOption(target, sortNames[i]);
            header.find("div." + col.cellClass).addClass("datagrid-sort-" + sortOrders[i]);
        }
        if (opts.remoteSort) {
            request(target);
        } else {
            renderGrid(target, $(target).datagrid("getData"));
        }
        opts.onSortColumn.call(target, opts.sortName, opts.sortOrder);
    };

    function fitColumns(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        var header = dc.view2.children("div.datagrid-header");
        dc.body2.css("overflow-x", "");
        fitHeadWidth();
        doFit();
        fitLeftWidth();
        fitHeadWidth(true);
        if (header.width() >= header.find("table").width()) {
            dc.body2.css("overflow-x", "hidden");
        }

        function fitLeftWidth() {
            if (!opts.fitColumns) {
                return;
            }
            if (!state.leftWidth) {
                state.leftWidth = 0;
            }
            var colWidth = 0;
            var cc = [];
            var fields = getColumnFields(target, false);
            for (var i = 0; i < fields.length; i++) {
                var col = getColumnOption(target, fields[i]);
                if (isFitColumn(col)) {
                    colWidth += col.width;
                    cc.push({ field: col.field, col: col, addingWidth: 0 });
                }
            }
            if (!colWidth) {
                return;
            }
            cc[cc.length - 1].addingWidth -= state.leftWidth;
            var headerInner = header.children("div.datagrid-header-inner").show();
            var fullwidth = header.width() - header.find("table").width() - opts.scrollbarSize + state.leftWidth;
            var rate = fullwidth / colWidth;
            if (!opts.showHeader) {
                headerInner.hide();
            }
            for (var i = 0; i < cc.length; i++) {
                var c = cc[i];
                var width = parseInt(c.col.width * rate);
                c.addingWidth += width;
                fullwidth -= width;
            }
            cc[cc.length - 1].addingWidth += fullwidth;
            for (var i = 0; i < cc.length; i++) {
                var c = cc[i];
                if (c.col.boxWidth + c.addingWidth > 0) {
                    c.col.boxWidth += c.addingWidth;
                    c.col.width += c.addingWidth;
                }
            }
            state.leftWidth = fullwidth;
            $(target).datagrid("fixColumnSize");
        };

        function doFit() {
            var isFitColumn = false;
            var fields = getColumnFields(target, true).concat(getColumnFields(target, false));
            $.map(fields, function (field) {
                var col = getColumnOption(target, field);
                if (String(col.width || "").indexOf("%") >= 0) {
                    var width = $.parser.parseValue("width", col.width, dc.view, opts.scrollbarSize) - col.deltaWidth;
                    if (width > 0) {
                        col.boxWidth = width;
                        isFitColumn = true;
                    }
                }
            });
            if (isFitColumn) {
                $(target).datagrid("fixColumnSize");
            }
        };

        function fitHeadWidth(fit) {
            var cells = dc.header1.add(dc.header2).find(".datagrid-cell-group");
            if (cells.length) {
                cells.each(function () {
                    $(this)._outerWidth(fit ? $(this).parent().width() : 10);
                });
                if (fit) {
                    fitGridSize(target);
                }
            }
        };

        function isFitColumn(col) {
            if (String(col.width || "").indexOf("%") >= 0) {
                return false;
            }
            if (!col.hidden && !col.checkbox && !col.auto && !col.fixed) {
                return true;
            }
        };
    };

    function autoSizeColumn(target, field) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        var tmp = $("<div class=\"datagrid-cell\" style=\"position:absolute;left:-9999px\"></div>").appendTo("body");
        if (field) {
            autoSizeColumnWidth(field);
            if (opts.fitColumns) {
                fitGridSize(target);
                $(target).datagrid("fitColumns");
            }
        } else {
            var frozen = false;
            var fields = getColumnFields(target, true).concat(getColumnFields(target, false));
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
                var col = getColumnOption(target, field);
                if (col.auto) {
                    autoSizeColumnWidth(field);
                    frozen = true;
                }
            }
            if (frozen && opts.fitColumns) {
                fitGridSize(target);
                $(target).datagrid("fitColumns");
            }
        }
        tmp.remove();

        function autoSizeColumnWidth(field) {
            var cell = dc.view.find("div.datagrid-header td[field=\"" + field + "\"] div.datagrid-cell");
            cell.css("width", "");
            var col = $(target).datagrid("getColumnOption", field);
            col.width = undefined;
            col.boxWidth = undefined;
            col.auto = true;
            $(target).datagrid("fixColumnSize", field);
            var width = Math.max(getCellWidth("header"), getCellWidth("allbody"), getCellWidth("allfooter")) + 1;
            cell._outerWidth(width - 1);
            col.width = width;
            col.boxWidth = parseInt(cell[0].style.width);
            col.deltaWidth = width - col.boxWidth;
            cell.css("width", "");
            $(target).datagrid("fixColumnSize", field);
            opts.onResizeColumn.call(target, field, col.width);

            function getCellWidth(part) {
                var cellWidth = 0;
                if (part == "header") {
                    cellWidth = getPartCellWidth(cell);
                } else {
                    opts.finder.getTr(target, 0, part).find("td[field=\"" + field + "\"] div.datagrid-cell").each(function () {
                        var w = getPartCellWidth($(this));
                        if (cellWidth < w) {
                            cellWidth = w;
                        }
                    });
                }
                return cellWidth;

                function getPartCellWidth(cell) {
                    return cell.is(":visible") ? cell._outerWidth() : tmp.html(cell.html())._outerWidth();
                };
            };
        };
    };

    function fixColumnSize(target, field) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        var table = dc.view.find("table.datagrid-btable, table.datagrid-ftable");
        table.css("table-layout", "fixed");
        if (field) {
            fix(field);
        } else {
            var ff = getColumnFields(target, true).concat(getColumnFields(target, false));
            for (var i = 0; i < ff.length; i++) {
                fix(ff[i]);
            }
        }
        table.css("table-layout", "");
        fixMergedCellsSize(target);
        fixRowHeight(target);
        fixEditorSize(target);

        function fix(cell) {
            var col = getColumnOption(target, cell);
            if (col.cellClass) {
                state.ss.set("." + col.cellClass, col.boxWidth ? col.boxWidth + "px" : "auto");
            }
        };
    };

    function fixMergedCellsSize(target) {
        var dc = $.data(target, "datagrid").dc;
        dc.view.find("td.datagrid-td-merged").each(function () {
            var td = $(this);
            var colspan = td.attr("colspan") || 1;
            var col = getColumnOption(target, td.attr("field"));
            var width = col.boxWidth + col.deltaWidth - 1;
            for (var i = 1; i < colspan; i++) {
                td = td.next();
                col = getColumnOption(target, td.attr("field"));
                width += col.boxWidth + col.deltaWidth;
            }
            $(this).children("div.datagrid-cell")._outerWidth(width);
        });
    };

    function fixEditorSize(target) {
        var dc = $.data(target, "datagrid").dc;
        dc.view.find("div.datagrid-editable").each(function () {
            var cell = $(this);
            var field = cell.parent().attr("field");
            var col = $(target).datagrid("getColumnOption", field);
            cell._outerWidth(col.boxWidth + col.deltaWidth - 1);
            var ed = $.data(this, "datagrid.editor");
            if (ed.actions.resize) {
                ed.actions.resize(ed.target, cell.width());
            }
        });
    };

    function getColumnOption(target, field) {
        function getOption(colums) {
            if (colums) {
                for (var i = 0; i < colums.length; i++) {
                    var cc = colums[i];
                    for (var j = 0; j < cc.length; j++) {
                        var c = cc[j];
                        if (c.field == field) {
                            return c;
                        }
                    }
                }
            }
            return null;
        };

        var opts = $.data(target, "datagrid").options;
        var col = getOption(opts.columns);
        if (!col) {
            col = getOption(opts.frozenColumns);
        }
        return col;
    };

    function getColumnFields(target, frozen) {
        var opts = $.data(target, "datagrid").options;
        var columns = (frozen == true) ? (opts.frozenColumns || [[]]) : opts.columns;
        if (columns.length == 0) {
            return [];
        }
        var aa = [];
        var colspanCount = getColspanCount();
        for (var i = 0; i < columns.length; i++) {
            aa[i] = new Array(colspanCount);
        }
        for (var columIndex = 0; columIndex < columns.length; columIndex++) {
            $.map(columns[columIndex], function (col) {
                var fieldIndex = getFixedColumnIndex(aa[columIndex]);
                if (fieldIndex >= 0) {
                    var fieldName = col.field || "";
                    for (var c = 0; c < (col.colspan || 1) ; c++) {
                        for (var r = 0; r < (col.rowspan || 1) ; r++) {
                            aa[columIndex + r][fieldIndex] = fieldName;
                        }
                        fieldIndex++;
                    }
                }
            });
        }
        return aa[aa.length - 1];

        function getColspanCount() {
            var colspanCount = 0;
            $.map(columns[0], function (col) {
                colspanCount += col.colspan || 1;
            });
            return colspanCount;
        };

        function getFixedColumnIndex(a) {
            for (var i = 0; i < a.length; i++) {
                if (a[i] == undefined) {
                    return i;
                }
            }
            return -1;
        };
    };

    function renderGrid(target, data) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        data = opts.loadFilter.call(target, data);
        data.total = parseInt(data.total);
        state.data = data;
        if (data.footer) {
            state.footer = data.footer;
        }
        if (!opts.remoteSort && opts.sortName) {
            var sortNames = opts.sortName.split(",");
            var sortOrders = opts.sortOrder.split(",");
            data.rows.sort(function (r1, r2) {
                var r = 0;
                for (var i = 0; i < sortNames.length; i++) {
                    var sn = sortNames[i];
                    var so = sortOrders[i];
                    var col = getColumnOption(target, sn);
                    var sorter = col.sorter || function (a, b) {
                        return a == b ? 0 : (a > b ? 1 : -1);
                    };
                    r = sorter(r1[sn], r2[sn]) * (so == "asc" ? 1 : -1);
                    if (r != 0) {
                        return r;
                    }
                }
                return r;
            });
        }
        if (opts.view.onBeforeRender) {
            opts.view.onBeforeRender.call(opts.view, target, data.rows);
        }
        opts.view.render.call(opts.view, target, dc.body2, false);
        opts.view.render.call(opts.view, target, dc.body1, true);
        if (opts.showFooter) {
            opts.view.renderFooter.call(opts.view, target, dc.footer2, false);
            opts.view.renderFooter.call(opts.view, target, dc.footer1, true);
        }
        if (opts.view.onAfterRender) {
            opts.view.onAfterRender.call(opts.view, target);
        }
        state.ss.clean();
        var pager = $(target).datagrid("getPager");
        if (pager.length) {
            var pagerOpts = pager.pagination("options");
            if (pagerOpts.total != data.total) {
                pager.pagination("refresh", { total: data.total });
                if (opts.pageNumber != pagerOpts.pageNumber && pagerOpts.pageNumber > 0) {
                    opts.pageNumber = pagerOpts.pageNumber;
                    request(target);
                }
            }
        }
        fixRowHeight(target);
        dc.body2.triggerHandler("scroll");
        $(target).datagrid("setSelectionState");
        $(target).datagrid("autoSizeColumn");
        opts.onLoadSuccess.call(target, data);
    };

    function setSelectionState(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var dc = state.dc;
        dc.header1.add(dc.header2).find("input[type=checkbox]")._propAttr("checked", false);
        if (opts.idField) {
            var treegridState = $.data(target, "treegrid") ? true : false;
            var selectHandler = opts.onSelect;
            var checkHandler = opts.onCheck;
            opts.onSelect = opts.onCheck = function () {
            };
            var rows = opts.finder.getRows(target);
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                var rowValueOrIndex = treegridState ? row[opts.idField] : i;
                if (isRowInRows(state.selectedRows, row)) {
                    selectRow(target, rowValueOrIndex, true);
                }
                if (isRowInRows(state.checkedRows, row)) {
                    checkRow(target, rowValueOrIndex, true);
                }
            }
            opts.onSelect = selectHandler;
            opts.onCheck = checkHandler;
        }

        function isRowInRows(a, r) {
            for (var i = 0; i < a.length; i++) {
                if (a[i][opts.idField] == r[opts.idField]) {
                    a[i] = r;
                    return true;
                }
            }
            return false;
        };
    };

    function getRowIndex(target, row) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var rows = state.data.rows;
        if (typeof row == "object") {
            return getObjectIndex(rows, row);
        } else {
            for (var i = 0; i < rows.length; i++) {
                if (rows[i][opts.idField] == row) {
                    return i;
                }
            }
            return -1;
        }
    };

    function getSelected(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var data = state.data;
        if (opts.idField) {
            return state.selectedRows;
        } else {
            var rows = [];
            opts.finder.getTr(target, "", "selected", 2).each(function () {
                rows.push(opts.finder.getRow(target, $(this)));
            });
            return rows;
        }
    };

    function getChecked(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        if (opts.idField) {
            return state.checkedRows;
        } else {
            var rows = [];
            opts.finder.getTr(target, "", "checked", 2).each(function () {
                rows.push(opts.finder.getRow(target, $(this)));
            });
            return rows;
        }
    };

    function scrollToRow(target, index) {
        var state = $.data(target, "datagrid");
        var dc = state.dc;
        var opts = state.options;
        var tr = opts.finder.getTr(target, index);
        if (tr.length) {
            if (tr.closest("table").hasClass("datagrid-btable-frozen")) {
                return;
            }
            var height = dc.view2.children("div.datagrid-header")._outerHeight();
            var body2 = dc.body2;
            var padding = body2.outerHeight(true) - body2.outerHeight();
            var top = tr.position().top - height - padding;
            if (top < 0) {
                body2.scrollTop(body2.scrollTop() + top);
            } else {
                if (top + tr._outerHeight() > body2.height() - 18) {
                    body2.scrollTop(body2.scrollTop() + top + tr._outerHeight() - body2.height() + 18);
                }
            }
        }
    };

    function highlightRow(target, index) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        opts.finder.getTr(target, state.highlightIndex).removeClass("datagrid-row-over");
        opts.finder.getTr(target, index).addClass("datagrid-row-over");
        state.highlightIndex = index;
    };

    function selectRow(target, rowIndex, selectState) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var row = opts.finder.getRow(target, rowIndex);
        if (opts.onBeforeSelect.apply(target, getRowData(target, [rowIndex, row])) == false) {
            return;
        }
        if (opts.singleSelect) {
            unselectAll(target, true);
            state.selectedRows = [];
        }
        if (!selectState && opts.checkOnSelect) {
            checkRow(target, rowIndex, true);
        }
        if (opts.idField) {
            addToRows(state.selectedRows, opts.idField, row);
        }
        opts.finder.getTr(target, rowIndex).addClass("datagrid-row-selected");
        opts.onSelect.apply(target, getRowData(target, [rowIndex, row]));
        scrollToRow(target, rowIndex);
    };

    function unselectRow(target, index, selectState) {
        var state = $.data(target, "datagrid");
        var dc = state.dc;
        var opts = state.options;
        var row = opts.finder.getRow(target, index);
        if (opts.onBeforeUnselect.apply(target, getRowData(target, [index, row])) == false) {
            return;
        }
        if (!selectState && opts.checkOnSelect) {
            uncheckRow(target, index, true);
        }
        opts.finder.getTr(target, index).removeClass("datagrid-row-selected");
        if (opts.idField) {
            unSelectedRowsById(state.selectedRows, opts.idField, row[opts.idField]);
        }
        opts.onUnselect.apply(target, getRowData(target, [index, row]));
    };

    function selectAll(target, selectState) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var rows = opts.finder.getRows(target);
        var selectedRows = $.data(target, "datagrid").selectedRows;
        if (!selectState && opts.checkOnSelect) {
            checkAll(target, true);
        }
        opts.finder.getTr(target, "", "allbody").addClass("datagrid-row-selected");
        if (opts.idField) {
            for (var i = 0; i < rows.length; i++) {
                addToRows(selectedRows, opts.idField, rows[i]);
            }
        }
        opts.onSelectAll.call(target, rows);
    };

    function unselectAll(target, selectState) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var rows = opts.finder.getRows(target);
        var selectedRows = $.data(target, "datagrid").selectedRows;
        if (!selectState && opts.checkOnSelect) {
            uncheckAll(target, true);
        }
        opts.finder.getTr(target, "", "selected").removeClass("datagrid-row-selected");
        if (opts.idField) {
            for (var rowIndex = 0; rowIndex < rows.length; rowIndex++) {
                unSelectedRowsById(selectedRows, opts.idField, rows[rowIndex][opts.idField]);
            }
        }
        opts.onUnselectAll.call(target, rows);
    };

    function checkRow(target, index, selectState) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var row = opts.finder.getRow(target, index);
        if (opts.onBeforeCheck.apply(target, getRowData(target, [index, row])) == false) {
            return;
        }
        if (opts.singleSelect && opts.selectOnCheck) {
            uncheckAll(target, true);
            state.checkedRows = [];
        }
        if (!selectState && opts.selectOnCheck) {
            selectRow(target, index, true);
        }
        var tr = opts.finder.getTr(target, index).addClass("datagrid-row-checked");
        tr.find("div.datagrid-cell-check input[type=checkbox]")._propAttr("checked", true);
        tr = opts.finder.getTr(target, "", "checked", 2);
        if (tr.length == opts.finder.getRows(target).length) {
            var dc = state.dc;
            dc.header1.add(dc.header2).find("input[type=checkbox]")._propAttr("checked", true);
        }
        if (opts.idField) {
            addToRows(state.checkedRows, opts.idField, row);
        }
        opts.onCheck.apply(target, getRowData(target, [index, row]));
    };

    function uncheckRow(target, index, selectState) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var row = opts.finder.getRow(target, index);
        if (opts.onBeforeUncheck.apply(target, getRowData(target, [index, row])) == false) {
            return;
        }
        if (!selectState && opts.selectOnCheck) {
            unselectRow(target, index, true);
        }
        var tr = opts.finder.getTr(target, index).removeClass("datagrid-row-checked");
        tr.find("div.datagrid-cell-check input[type=checkbox]")._propAttr("checked", false);
        var dc = state.dc;
        var header = dc.header1.add(dc.header2);
        header.find("input[type=checkbox]")._propAttr("checked", false);
        if (opts.idField) {
            unSelectedRowsById(state.checkedRows, opts.idField, row[opts.idField]);
        }
        opts.onUncheck.apply(target, getRowData(target, [index, row]));
    };

    function checkAll(target, selectState) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var rows = opts.finder.getRows(target);
        if (!selectState && opts.selectOnCheck) {
            selectAll(target, true);
        }
        var dc = state.dc;
        var hck = dc.header1.add(dc.header2).find("input[type=checkbox]");
        var bck = opts.finder.getTr(target, "", "allbody").addClass("datagrid-row-checked").find("div.datagrid-cell-check input[type=checkbox]");
        hck.add(bck)._propAttr("checked", true);
        if (opts.idField) {
            for (var i = 0; i < rows.length; i++) {
                addToRows(state.checkedRows, opts.idField, rows[i]);
            }
        }
        opts.onCheckAll.call(target, rows);
    };

    function uncheckAll(target, selectState) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var rows = opts.finder.getRows(target);
        if (!selectState && opts.selectOnCheck) {
            unselectAll(target, true);
        }
        var dc = state.dc;
        var hck = dc.header1.add(dc.header2).find("input[type=checkbox]");
        var bck = opts.finder.getTr(target, "", "checked").removeClass("datagrid-row-checked").find("div.datagrid-cell-check input[type=checkbox]");
        hck.add(bck)._propAttr("checked", false);
        if (opts.idField) {
            for (var i = 0; i < rows.length; i++) {
                unSelectedRowsById(state.checkedRows, opts.idField, rows[i][opts.idField]);
            }
        }
        opts.onUncheckAll.call(target, rows);
    };

    function beginEdit(target, index) {
        var opts = $.data(target, "datagrid").options;
        var tr = opts.finder.getTr(target, index);
        var row = opts.finder.getRow(target, index);
        if (tr.hasClass("datagrid-row-editing")) {
            return;
        }
        if (opts.onBeforeEdit.apply(target, getRowData(target, [index, row])) == false) {
            return;
        }
        tr.addClass("datagrid-row-editing");
        createEditor(target, index);
        fixEditorSize(target);
        tr.find("div.datagrid-editable").each(function () {
            var field = $(this).parent().attr("field");
            var ed = $.data(this, "datagrid.editor");
            ed.actions.setValue(ed.target, row[field]);
        });
        validateRow(target, index);
        opts.onBeginEdit.apply(target, getRowData(target, [index, row]));
    };

    function stopEdit(target, rowIndex, revert) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var updatedRows = state.updatedRows;
        var insertedRows = state.insertedRows;
        var tr = opts.finder.getTr(target, rowIndex);
        var row = opts.finder.getRow(target, rowIndex);
        if (!tr.hasClass("datagrid-row-editing")) {
            return;
        }
        if (!revert) {
            if (!validateRow(target, rowIndex)) {
                return;
            }
            var changed = false;
            var newValues = {};
            tr.find("div.datagrid-editable").each(function () {
                var field = $(this).parent().attr("field");
                var ed = $.data(this, "datagrid.editor");
                var t = $(ed.target);
                var textbox = t.data("textbox") ? t.textbox("textbox") : t;
                textbox.triggerHandler("blur");
                var value = ed.actions.getValue(ed.target);
                if (row[field] != value) {
                    row[field] = value;
                    changed = true;
                    newValues[field] = value;
                }
            });
            if (changed) {
                if (getObjectIndex(insertedRows, row) == -1) {
                    if (getObjectIndex(updatedRows, row) == -1) {
                        updatedRows.push(row);
                    }
                }
            }
            opts.onEndEdit.apply(target, getRowData(target, [rowIndex, row, newValues]));
        }
        tr.removeClass("datagrid-row-editing");
        destroyEditor(target, rowIndex);
        $(target).datagrid("refreshRow", rowIndex);
        if (!revert) {
            opts.onAfterEdit.apply(target, getRowData(target, [rowIndex, row, newValues]));
        } else {
            opts.onCancelEdit.apply(target, getRowData(target, [rowIndex, row]));
        }
    };

    function getEditors(target, index) {
        var opts = $.data(target, "datagrid").options;
        var tr = opts.finder.getTr(target, index);
        var editorObjs = [];
        tr.children("td").each(function () {
            var cell = $(this).find("div.datagrid-editable");
            if (cell.length) {
                var ed = $.data(cell[0], "datagrid.editor");
                editorObjs.push(ed);
            }
        });
        return editorObjs;
    };

    function getEditor(target, options) {
        var editorObjs = getEditors(target, options.index != undefined ? options.index : options.id);
        for (var i = 0; i < editorObjs.length; i++) {
            if (editorObjs[i].field == options.field) {
                return editorObjs[i];
            }
        }
        return null;
    };

    function createEditor(target, index) {
        var opts = $.data(target, "datagrid").options;
        var tr = opts.finder.getTr(target, index);
        tr.children("td").each(function () {
            var cell = $(this).find("div.datagrid-cell");
            var field = $(this).attr("field");
            var col = getColumnOption(target, field);
            if (col && col.editor) {
                var type, editorOpts;
                if (typeof col.editor == "string") {
                    type = col.editor;
                } else {
                    type = col.editor.type;
                    editorOpts = col.editor.options;
                }
                var editor = opts.editors[type];
                if (editor) {
                    var html = cell.html();
                    var width = cell._outerWidth();
                    cell.addClass("datagrid-editable");
                    cell._outerWidth(width);
                    cell.html("<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\"><tr><td></td></tr></table>");
                    cell.children("table").bind("click dblclick contextmenu", function (e) {
                        e.stopPropagation();
                    });
                    $.data(cell[0], "datagrid.editor", { actions: editor, target: editor.init(cell.find("td"), editorOpts), field: field, type: type, oldHtml: html });
                }
            }
        });
        fixRowHeight(target, index, true);
    };

    function destroyEditor(target, index) {
        var opts = $.data(target, "datagrid").options;
        var tr = opts.finder.getTr(target, index);
        tr.children("td").each(function () {
            var cell = $(this).find("div.datagrid-editable");
            if (cell.length) {
                var ed = $.data(cell[0], "datagrid.editor");
                if (ed.actions.destroy) {
                    ed.actions.destroy(ed.target);
                }
                cell.html(ed.oldHtml);
                $.removeData(cell[0], "datagrid.editor");
                cell.removeClass("datagrid-editable");
                cell.css("width", "");
            }
        });
    };

    function validateRow(target, index) {
        var tr = $.data(target, "datagrid").options.finder.getTr(target, index);
        if (!tr.hasClass("datagrid-row-editing")) {
            return true;
        }
        var vbox = tr.find(".validatebox-text");
        vbox.validatebox("validate");
        vbox.trigger("mouseleave");
        var invalid = tr.find(".validatebox-invalid");
        return invalid.length == 0;
    };

    function getChanges(target, type) {
        var insertedRows = $.data(target, "datagrid").insertedRows;
        var deletedRows = $.data(target, "datagrid").deletedRows;
        var updatedRows = $.data(target, "datagrid").updatedRows;
        if (!type) {
            var rows = [];
            rows = rows.concat(insertedRows);
            rows = rows.concat(deletedRows);
            rows = rows.concat(updatedRows);
            return rows;
        } else {
            if (type == "inserted") {
                return insertedRows;
            } else {
                if (type == "deleted") {
                    return deletedRows;
                } else {
                    if (type == "updated") {
                        return updatedRows;
                    }
                }
            }
        }
        return [];
    };

    function deleteRow(target, index) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var data = state.data;
        var insertedRows = state.insertedRows;
        var deletedRows = state.deletedRows;
        $(target).datagrid("cancelEdit", index);
        var row = opts.finder.getRow(target, index);
        if (getObjectIndex(insertedRows, row) >= 0) {
            unSelectedRowsById(insertedRows, row);
        } else {
            deletedRows.push(row);
        }
        unSelectedRowsById(state.selectedRows, opts.idField, row[opts.idField]);
        unSelectedRowsById(state.checkedRows, opts.idField, row[opts.idField]);
        opts.view.deleteRow.call(opts.view, target, index);
        if (opts.height == "auto") {
            fixRowHeight(target);
        }
        $(target).datagrid("getPager").pagination("refresh", { total: data.total });
    };

    function insertRow(target, param) {
        var data = $.data(target, "datagrid").data;
        var view = $.data(target, "datagrid").options.view;
        var insertedRows = $.data(target, "datagrid").insertedRows;
        view.insertRow.call(view, target, param.index, param.row);
        insertedRows.push(param.row);
        $(target).datagrid("getPager").pagination("refresh", { total: data.total });
    };

    function appendRow(target, row) {
        var data = $.data(target, "datagrid").data;
        var view = $.data(target, "datagrid").options.view;
        var insertedRows = $.data(target, "datagrid").insertedRows;
        view.insertRow.call(view, target, null, row);
        insertedRows.push(row);
        $(target).datagrid("getPager").pagination("refresh", { total: data.total });
    };

    function resetOperation(target) {
        var state = $.data(target, "datagrid");
        var data = state.data;
        var rows = data.rows;
        var originalRows = [];
        for (var i = 0; i < rows.length; i++) {
            originalRows.push($.extend({}, rows[i]));
        }
        state.originalRows = originalRows;
        state.updatedRows = [];
        state.insertedRows = [];
        state.deletedRows = [];
    };

    function acceptChanges(target) {
        var data = $.data(target, "datagrid").data;
        var ok = true;
        for (var i = 0, len = data.rows.length; i < len; i++) {
            if (validateRow(target, i)) {
                $(target).datagrid("endEdit", i);
            } else {
                ok = false;
            }
        }
        if (ok) {
            resetOperation(target);
        }
    };

    function rejectChanges(target) {
        var state = $.data(target, "datagrid");
        var opts = state.options;
        var originalRows = state.originalRows;
        var insertedRows = state.insertedRows;
        var deletedRows = state.deletedRows;
        var selectedRows = state.selectedRows;
        var checkedRows = state.checkedRows;
        var data = state.data;

        function getRowIds(a) {
            var ids = [];
            for (var i = 0; i < a.length; i++) {
                ids.push(a[i][opts.idField]);
            }
            return ids;
        };

        function checkOrSelect(ids, selectOrCheckState) {
            for (var i = 0; i < ids.length; i++) {
                var rowIndex = getRowIndex(target, ids[i]);
                if (rowIndex >= 0) {
                    (selectOrCheckState == "s" ? selectRow : checkRow)(target, rowIndex, true);
                }
            }
        };

        for (var i = 0; i < data.rows.length; i++) {
            $(target).datagrid("cancelEdit", i);
        }
        var selectedRowIds = getRowIds(selectedRows);
        var checkedRowIds = getRowIds(checkedRows);
        selectedRows.splice(0, selectedRows.length);
        checkedRows.splice(0, checkedRows.length);
        data.total += deletedRows.length - insertedRows.length;
        data.rows = originalRows;
        renderGrid(target, data);
        checkOrSelect(selectedRowIds, "s");
        checkOrSelect(checkedRowIds, "c");
        resetOperation(target);
    };

    function request(target, param) {
        var opts = $.data(target, "datagrid").options;
        if (param) {
            opts.queryParams = param;
        }
        var queryParams = $.extend({}, opts.queryParams);
        if (opts.pagination) {
            $.extend(queryParams, { page: opts.pageNumber || 1, rows: opts.pageSize });
        }
        if (opts.sortName) {
            $.extend(queryParams, { sort: opts.sortName, order: opts.sortOrder });
        }
        if (opts.onBeforeLoad.call(target, queryParams) == false) {
            return;
        }
        $(target).datagrid("loading");
        var loadResult = opts.loader.call(target, queryParams, function (data) {
            $(target).datagrid("loaded");
            $(target).datagrid("loadData", data);
        }, function () {
            $(target).datagrid("loaded");
            opts.onLoadError.apply(target, arguments);
        });
        if (loadResult == false) {
            $(target).datagrid("loaded");
        }
    };

    function mergeCells(target, options) {
        var opts = $.data(target, "datagrid").options;
        options.type = options.type || "body";
        options.rowspan = options.rowspan || 1;
        options.colspan = options.colspan || 1;
        if (options.rowspan == 1 && options.colspan == 1) {
            return;
        }
        var tr = opts.finder.getTr(target, (options.index != undefined ? options.index : options.id), options.type);
        if (!tr.length) {
            return;
        }
        var td = tr.find("td[field=\"" + options.field + "\"]");
        td.attr("rowspan", options.rowspan).attr("colspan", options.colspan);
        td.addClass("datagrid-td-merged");
        doMerge(td.next(), options.colspan - 1);
        for (var i = 1; i < options.rowspan; i++) {
            tr = tr.next();
            if (!tr.length) {
                break;
            }
            td = tr.find("td[field=\"" + options.field + "\"]");
            doMerge(td, options.colspan);
        }
        fixMergedCellsSize(target);

        function doMerge(td, colspans) {
            for (var i = 0; i < colspans; i++) {
                td.hide();
                td = td.next();
            }
        };
    };

    $.fn.datagrid = function (options, param) {
        if (typeof options == "string") {
            return $.fn.datagrid.methods[options](this, param);
        }
        options = options || {};
        return this.each(function () {
            var state = $.data(this, "datagrid");
            var opts;
            if (state) {
                opts = $.extend(state.options, options);
                state.options = opts;
            } else {
                opts = $.extend({}, $.extend({}, $.fn.datagrid.defaults, { queryParams: {} }), $.fn.datagrid.parseOptions(this), options);
                $(this).css("width", "").css("height", "");
                var gridWrap = wrapGrid(this, opts.rownumbers);
                if (!opts.columns) {
                    opts.columns = gridWrap.columns;
                }
                if (!opts.frozenColumns) {
                    opts.frozenColumns = gridWrap.frozenColumns;
                }
                opts.columns = $.extend(true, [], opts.columns);
                opts.frozenColumns = $.extend(true, [], opts.frozenColumns);
                opts.view = $.extend({}, opts.view);
                $.data(this, "datagrid", { options: opts, panel: gridWrap.panel, dc: gridWrap.dc, ss: null, selectedRows: [], checkedRows: [], data: { total: 0, rows: [] }, originalRows: [], updatedRows: [], insertedRows: [], deletedRows: [] });
            }
            initGrid(this);
            bindEvents(this);
            setSize(this);
            if (opts.data) {
                renderGrid(this, opts.data);
                resetOperation(this);
            } else {
                var data = $.fn.datagrid.parseData(this);
                if (data.total > 0) {
                    renderGrid(this, data);
                    resetOperation(this);
                } else {
                    opts.view.renderEmptyRow(this);
                }
            }
            request(this);
        });
    };

    function initEditors(editorNames) {
        var editorObjs = { };
        $.map(editorNames, function (name) {
            editorObjs[name] = getEditor(name);
        });
        return editorObjs;

        function getEditor(name) {
            function isA(container) {
                return $.data($(container)[0], name) != undefined;
            };

            return {
                init: function (container, param) {
                    var editor = $("<input type=\"text\" class=\"datagrid-editable-input\">").appendTo(container);
                    if (editor[name] && name != "text") {
                        return editor[name](param);
                    } else {
                        return editor;
                    }
                },
                destroy: function (target) {
                    if (isA(target, name)) {
                        $(target)[name]("destroy");
                    }
                },
                getValue: function (target) {
                    if (isA(target, name)) {
                        var opts = $(target)[name]("options");
                        if (opts.multiple) {
                            return $(target)[name]("getValues").join(opts.separator);
                        } else {
                            return $(target)[name]("getValue");
                        }
                    } else {
                        return $(target).val();
                    }
                },
                setValue: function (target, newValue) {
                    if (isA(target, name)) {
                        var opts = $(target)[name]("options");
                        if (opts.multiple) {
                            if (newValue) {
                                $(target)[name]("setValues", newValue.split(opts.separator));
                            } else {
                                $(target)[name]("clear");
                            }
                        } else {
                            $(target)[name]("setValue", newValue);
                        }
                    } else {
                        $(target).val(newValue);
                    }
                },
                resize: function (target, width) {
                    if (isA(target, name)) {
                        $(target)[name]("resize", width);
                    } else {
                        $(target)._outerWidth(width)._outerHeight(22);
                    }
                }
            };
        };
    };

    var editors = $.extend({}, initEditors(["text", "textbox", "numberbox", "numberspinner", "combobox", "combotree", "combogrid", "datebox", "datetimebox", "timespinner", "datetimespinner"]), {
        textarea: {
            init: function (container, options) {
                var editor = $("<textarea class=\"datagrid-editable-input\"></textarea>").appendTo(container);
                return editor;
            },
            getValue: function (target) {
                return $(target).val();
            },
            setValue: function (target, newValue) {
                $(target).val(newValue);
            },
            resize: function (target, width) {
                $(target)._outerWidth(width);
            }
        },
        checkbox: {
            init: function (container, options) {
                var ediotr = $("<input type=\"checkbox\">").appendTo(container);
                ediotr.val(options.on);
                ediotr.attr("offval", options.off);
                return ediotr;
            },
            getValue: function (target) {
                if ($(target).is(":checked")) {
                    return $(target).val();
                } else {
                    return $(target).attr("offval");
                }
            },
            setValue: function (target, newValue) {
                var isChange = false;
                if ($(target).val() == newValue) {
                    isChange = true;
                }
                $(target)._propAttr("checked", isChange);
            }
        },
        validatebox: {
            init: function (target, options) {
                var editor = $("<input type=\"text\" class=\"datagrid-editable-input\">").appendTo(target);
                editor.validatebox(options);
                return editor;
            },
            destroy: function (target) {
                $(target).validatebox("destroy");
            },
            getValue: function (target) {
                return $(target).val();
            },
            setValue: function (target, newValue) {
                $(target).val(newValue);
            },
            resize: function (target, width) {
                $(target)._outerWidth(width)._outerHeight(22);
            }
        }
    });
    $.fn.datagrid.methods = {
        options: function (jq) {
            var opts = $.data(jq[0], "datagrid").options;
            var panelOpts = $.data(jq[0], "datagrid").panel.panel("options");
            var opts = $.extend(opts, { width: panelOpts.width, height: panelOpts.height, closed: panelOpts.closed, collapsed: panelOpts.collapsed, minimized: panelOpts.minimized, maximized: panelOpts.maximized });
            return opts;
        },
        setSelectionState: function (jq) {
            return jq.each(function () {
                setSelectionState(this);
            });
        },
        createStyleSheet: function (jq) {
            return createStyleSheet(jq[0]);
        },
        getPanel: function (jq) {
            return $.data(jq[0], "datagrid").panel;
        },
        getPager: function (jq) {
            return $.data(jq[0], "datagrid").panel.children("div.datagrid-pager");
        },
        getColumnFields: function (jq, frozen) {
            return getColumnFields(jq[0], frozen);
        },
        getColumnOption: function (jq, field) {
            return getColumnOption(jq[0], field);
        },
        resize: function (jq, param) {
            return jq.each(function () {
                setSize(this, param);
            });
        },
        load: function (jq, param) {
            return jq.each(function () {
                var opts = $(this).datagrid("options");
                if (typeof param == "string") {
                    opts.url = param;
                    param = null;
                }
                opts.pageNumber = 1;
                var pager = $(this).datagrid("getPager");
                pager.pagination("refresh", { pageNumber: 1 });
                request(this, param);
            });
        },
        reload: function (jq, param) {
            return jq.each(function () {
                var opts = $(this).datagrid("options");
                if (typeof param == "string") {
                    opts.url = param;
                    param = null;
                }
                request(this, param);
            });
        },
        reloadFooter: function (jq, footer) {
            return jq.each(function () {
                var opts = $.data(this, "datagrid").options;
                var dc = $.data(this, "datagrid").dc;
                if (footer) {
                    $.data(this, "datagrid").footer = footer;
                }
                if (opts.showFooter) {
                    opts.view.renderFooter.call(opts.view, this, dc.footer2, false);
                    opts.view.renderFooter.call(opts.view, this, dc.footer1, true);
                    if (opts.view.onAfterRender) {
                        opts.view.onAfterRender.call(opts.view, this);
                    }
                    $(this).datagrid("fixRowHeight");
                }
            });
        },
        loading: function (jq) {
            return jq.each(function () {
                var opts = $.data(this, "datagrid").options;
                $(this).datagrid("getPager").pagination("loading");
                if (opts.loadMsg) {
                    var panel = $(this).datagrid("getPanel");
                    if (!panel.children("div.datagrid-mask").length) {
                        $("<div class=\"datagrid-mask\" style=\"display:block\"></div>").appendTo(panel);
                        var msg = $("<div class=\"datagrid-mask-msg\" style=\"display:block;left:50%\"></div>").html(opts.loadMsg).appendTo(panel);
                        msg._outerHeight(40);
                        msg.css({ marginLeft: (-msg.outerWidth() / 2), lineHeight: (msg.height() + "px") });
                    }
                }
            });
        },
        loaded: function (jq) {
            return jq.each(function () {
                $(this).datagrid("getPager").pagination("loaded");
                var panel = $(this).datagrid("getPanel");
                panel.children("div.datagrid-mask-msg").remove();
                panel.children("div.datagrid-mask").remove();
            });
        },
        fitColumns: function (jq) {
            return jq.each(function () {
                fitColumns(this);
            });
        },
        fixColumnSize: function (jq, field) {
            return jq.each(function () {
                fixColumnSize(this, field);
            });
        },
        fixRowHeight: function (jq, index) {
            return jq.each(function () {
                fixRowHeight(this, index);
            });
        },
        freezeRow: function (jq, index) {
            return jq.each(function () {
                freezeRow(this, index);
            });
        },
        autoSizeColumn: function (jq, index) {
            return jq.each(function () {
                autoSizeColumn(this, index);
            });
        },
        loadData: function (jq, data) {
            return jq.each(function () {
                renderGrid(this, data);
                resetOperation(this);
            });
        },
        getData: function (jq) {
            return $.data(jq[0], "datagrid").data;
        },
        getRows: function (jq) {
            return $.data(jq[0], "datagrid").data.rows;
        },
        getFooterRows: function (jq) {
            return $.data(jq[0], "datagrid").footer;
        },
        getRowIndex: function (jq, id) {
            return getRowIndex(jq[0], id);
        },
        getChecked: function (jq) {
            return getChecked(jq[0]);
        },
        getSelected: function (jq) {
            var rows = getSelected(jq[0]);
            return rows.length > 0 ? rows[0] : null;
        },
        getSelections: function (jq) {
            return getSelected(jq[0]);
        },
        clearSelections: function (jq) {
            return jq.each(function () {
                var state = $.data(this, "datagrid");
                var selectedRows = state.selectedRows;
                var checkedRows = state.checkedRows;
                selectedRows.splice(0, selectedRows.length);
                unselectAll(this);
                if (state.options.checkOnSelect) {
                    checkedRows.splice(0, checkedRows.length);
                }
            });
        },
        clearChecked: function (jq) {
            return jq.each(function () {
                var state = $.data(this, "datagrid");
                var selectedRows = state.selectedRows;
                var checkedRows = state.checkedRows;
                checkedRows.splice(0, checkedRows.length);
                uncheckAll(this);
                if (state.options.selectOnCheck) {
                    selectedRows.splice(0, selectedRows.length);
                }
            });
        },
        scrollTo: function (jq, index) {
            return jq.each(function () {
                scrollToRow(this, index);
            });
        },
        highlightRow: function (jq, index) {
            return jq.each(function () {
                highlightRow(this, index);
                scrollToRow(this, index);
            });
        },
        selectAll: function (jq) {
            return jq.each(function () {
                selectAll(this);
            });
        },
        unselectAll: function (jq) {
            return jq.each(function () {
                unselectAll(this);
            });
        },
        selectRow: function (jq, index) {
            return jq.each(function () {
                selectRow(this, index);
            });
        },
        selectRecord: function (jq, id) {
            return jq.each(function () {
                var opts = $.data(this, "datagrid").options;
                if (opts.idField) {
                    var index = getRowIndex(this, id);
                    if (index >= 0) {
                        $(this).datagrid("selectRow", index);
                    }
                }
            });
        },
        unselectRow: function (jq, index) {
            return jq.each(function () {
                unselectRow(this, index);
            });
        },
        checkRow: function (jq, index) {
            return jq.each(function () {
                checkRow(this, index);
            });
        },
        uncheckRow: function (jq, index) {
            return jq.each(function () {
                uncheckRow(this, index);
            });
        },
        checkAll: function (jq) {
            return jq.each(function () {
                checkAll(this);
            });
        },
        uncheckAll: function (jq) {
            return jq.each(function () {
                uncheckAll(this);
            });
        },
        beginEdit: function (jq, index) {
            return jq.each(function () {
                beginEdit(this, index);
            });
        },
        endEdit: function (jq, index) {
            return jq.each(function () {
                stopEdit(this, index, false);
            });
        },
        cancelEdit: function (jq, index) {
            return jq.each(function () {
                stopEdit(this, index, true);
            });
        },
        getEditors: function (jq, index) {
            return getEditors(jq[0], index);
        },
        getEditor: function (jq, options) {
            return getEditor(jq[0], options);
        },
        refreshRow: function (jq, index) {
            return jq.each(function () {
                var opts = $.data(this, "datagrid").options;
                opts.view.refreshRow.call(opts.view, this, index);
            });
        },
        validateRow: function (jq, index) {
            return validateRow(jq[0], index);
        },
        updateRow: function (jq, param) {
            return jq.each(function () {
                var opts = $.data(this, "datagrid").options;
                opts.view.updateRow.call(opts.view, this, param.index, param.row);
            });
        },
        appendRow: function (jq, row) {
            return jq.each(function () {
                appendRow(this, row);
            });
        },
        insertRow: function (jq, param) {
            return jq.each(function () {
                insertRow(this, param);
            });
        },
        deleteRow: function (jq, index) {
            return jq.each(function () {
                deleteRow(this, index);
            });
        },
        getChanges: function (jq, type) {
            return getChanges(jq[0], type);
        },
        acceptChanges: function (jq) {
            return jq.each(function () {
                acceptChanges(this);
            });
        },
        rejectChanges: function (jq) {
            return jq.each(function () {
                rejectChanges(this);
            });
        },
        mergeCells: function (jq, options) {
            return jq.each(function () {
                mergeCells(this, options);
            });
        },
        showColumn: function (jq, field) {
            return jq.each(function () {
                var panel = $(this).datagrid("getPanel");
                panel.find("td[field=\"" + field + "\"]").show();
                $(this).datagrid("getColumnOption", field).hidden = false;
                $(this).datagrid("fitColumns");
            });
        },
        hideColumn: function (jq, field) {
            return jq.each(function () {
                var panel = $(this).datagrid("getPanel");
                panel.find("td[field=\"" + field + "\"]").hide();
                $(this).datagrid("getColumnOption", field).hidden = true;
                $(this).datagrid("fitColumns");
            });
        },
        sort: function (jq, param) {
            return jq.each(function () {
                sort(this, param);
            });
        }
    };
    $.fn.datagrid.parseOptions = function (target) {
        var t = $(target);
        return $.extend({}, $.fn.panel.parseOptions(target), $.parser.parseOptions(target, ["url", "toolbar", "idField", "sortName", "sortOrder", "pagePosition", "resizeHandle", { sharedStyleSheet: "boolean", fitColumns: "boolean", autoRowHeight: "boolean", striped: "boolean", nowrap: "boolean" }, { rownumbers: "boolean", singleSelect: "boolean", ctrlSelect: "boolean", checkOnSelect: "boolean", selectOnCheck: "boolean" }, { pagination: "boolean", pageSize: "number", pageNumber: "number" }, { multiSort: "boolean", remoteSort: "boolean", showHeader: "boolean", showFooter: "boolean" }, { scrollbarSize: "number" }]), { pageList: (t.attr("pageList") ? eval(t.attr("pageList")) : undefined), loadMsg: (t.attr("loadMsg") != undefined ? t.attr("loadMsg") : undefined), rowStyler: (t.attr("rowStyler") ? eval(t.attr("rowStyler")) : undefined) });
    };
    $.fn.datagrid.parseData = function (target) {
        var t = $(target);
        var data = { total: 0, rows: [] };
        var fields = t.datagrid("getColumnFields", true).concat(t.datagrid("getColumnFields", false));
        t.find("tbody tr").each(function () {
            data.total++;
            var row = {};
            $.extend(row, $.parser.parseOptions(this, ["iconCls", "state"]));
            for (var i = 0; i < fields.length; i++) {
                row[fields[i]] = $(this).find("td:eq(" + i + ")").html();
            }
            data.rows.push(row);
        });
        return data;
    };
    var view = {
        render: function (target, container, frozen) {
            var rows = $(target).datagrid("getRows");
            $(container).html(this.renderTable(target, 0, rows, frozen));
        },
        renderFooter: function (target, container, frozen) {
            var opts = $.data(target, "datagrid").options;
            var rows = $.data(target, "datagrid").footer || [];
            var fields = $(target).datagrid("getColumnFields", frozen);
            var html = ["<table class=\"datagrid-ftable\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"];
            for (var i = 0; i < rows.length; i++) {
                html.push("<tr class=\"datagrid-row\" datagrid-row-index=\"" + i + "\">");
                html.push(this.renderRow.call(this, target, fields, frozen, i, rows[i]));
                html.push("</tr>");
            }
            html.push("</tbody></table>");
            $(container).html(html.join(""));
        },
        renderTable: function (target, field, rows, frozen) {
            var state = $.data(target, "datagrid");
            var opts = state.options;
            if (frozen) {
                if (!(opts.rownumbers || (opts.frozenColumns && opts.frozenColumns.length))) {
                    return "";
                }
            }
            var fields = $(target).datagrid("getColumnFields", frozen);
            var html = ["<table class=\"datagrid-btable\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"];
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                var css = opts.rowStyler ? opts.rowStyler.call(target, field, row) : "";
                var rowClass = "";
                var rowStyle = "";
                if (typeof css == "string") {
                    rowStyle = css;
                } else {
                    if (css) {
                        rowClass = css["class"] || "";
                        rowStyle = css["style"] || "";
                    }
                }
                var cls = "class=\"datagrid-row " + (field % 2 && opts.striped ? "datagrid-row-alt " : " ") + rowClass + "\"";
                var style = rowStyle ? "style=\"" + rowStyle + "\"" : "";
                var rowIdFieldName = state.rowIdPrefix + "-" + (frozen ? 1 : 2) + "-" + field;
                html.push("<tr id=\"" + rowIdFieldName + "\" datagrid-row-index=\"" + field + "\" " + cls + " " + style + ">");
                html.push(this.renderRow.call(this, target, fields, frozen, field, row));
                html.push("</tr>");
                field++;
            }
            html.push("</tbody></table>");
            return html.join("");
        },
        renderRow: function (target, fields, frozen, rowIndex, rowData) {
            var opts = $.data(target, "datagrid").options;
            var cc = [];
            if (frozen && opts.rownumbers) {
                var rowNumber = rowIndex + 1;
                if (opts.pagination) {
                    rowNumber += (opts.pageNumber - 1) * opts.pageSize;
                }
                cc.push("<td class=\"datagrid-td-rownumber\"><div class=\"datagrid-cell-rownumber\">" + rowNumber + "</div></td>");
            }
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
                var col = $(target).datagrid("getColumnOption", field);
                if (col) {
                    var fieldValue = rowData[field];
                    var css = col.styler ? (col.styler(fieldValue, rowData, rowIndex) || "") : "";
                    var rowClass = "";
                    var rowStyle = "";
                    if (typeof css == "string") {
                        rowStyle = css;
                    } else {
                        if (css) {
                            rowClass = css["class"] || "";
                            rowStyle = css["style"] || "";
                        }
                    }
                    var cls = rowClass ? "class=\"" + rowClass + "\"" : "";
                    var style = col.hidden ? "style=\"display:none;" + rowStyle + "\"" : (rowStyle ? "style=\"" + rowStyle + "\"" : "");
                    cc.push("<td field=\"" + field + "\" " + cls + " " + style + ">");
                    var style = "";
                    if (!col.checkbox) {
                        if (col.align) {
                            style += "text-align:" + col.align + ";";
                        }
                        if (!opts.nowrap) {
                            style += "white-space:normal;height:auto;";
                        } else {
                            if (opts.autoRowHeight) {
                                style += "height:auto;";
                            }
                        }
                    }
                    cc.push("<div style=\"" + style + "\" ");
                    cc.push(col.checkbox ? "class=\"datagrid-cell-check\"" : "class=\"datagrid-cell " + col.cellClass + "\"");
                    cc.push(">");
                    if (col.checkbox) {
                        cc.push("<input type=\"checkbox\" " + (rowData.checked ? "checked=\"checked\"" : ""));
                        cc.push(" name=\"" + field + "\" value=\"" + (fieldValue != undefined ? fieldValue : "") + "\">");
                    } else {
                        if (col.formatter) {
                            cc.push(col.formatter(fieldValue, rowData, rowIndex));
                        } else {
                            cc.push(fieldValue);
                        }
                    }
                    cc.push("</div>");
                    cc.push("</td>");
                }
            }
            return cc.join("");
        },
        refreshRow: function (target, index) {
            this.updateRow.call(this, target, index, {});
        },
        updateRow: function (target, index, row) {
            var opts = $.data(target, "datagrid").options;
            var rows = $(target).datagrid("getRows");
            var rowStyleData = getRowStyle(index);
            $.extend(rows[index], row);
            var newRowStyleData = getRowStyle(index);
            var oldRowClass = rowStyleData.c;
            var newRowStyle = newRowStyleData.s;
            var classText = "datagrid-row " + (index % 2 && opts.striped ? "datagrid-row-alt " : " ") + newRowStyleData.c;

            function getRowStyle(index) {
                var css = opts.rowStyler ? opts.rowStyler.call(target, index, rows[index]) : "";
                var rowClass = "";
                var rowStyle = "";
                if (typeof css == "string") {
                    rowStyle = css;
                } else {
                    if (css) {
                        rowClass = css["class"] || "";
                        rowStyle = css["style"] || "";
                    }
                }
                return { c: rowClass, s: rowStyle };
            };

            function updateColumns(frozen) {
                var fields = $(target).datagrid("getColumnFields", frozen);
                var tr = opts.finder.getTr(target, index, "body", (frozen ? 1 : 2));
                var isCheckedRow = tr.find("div.datagrid-cell-check input[type=checkbox]").is(":checked");
                tr.html(this.renderRow.call(this, target, fields, frozen, index, rows[index]));
                tr.attr("style", newRowStyle).removeClass(oldRowClass).addClass(classText);
                if (isCheckedRow) {
                    tr.find("div.datagrid-cell-check input[type=checkbox]")._propAttr("checked", true);
                }
            };

            updateColumns.call(this, true);
            updateColumns.call(this, false);
            $(target).datagrid("fixRowHeight", index);
        },
        insertRow: function (target, index, row) {
            var state = $.data(target, "datagrid");
            var opts = state.options;
            var dc = state.dc;
            var data = state.data;
            if (index == undefined || index == null) {
                index = data.rows.length;
            }
            if (index > data.rows.length) {
                index = data.rows.length;
            }

            function updateRownumber(frozen) {
                var frozenText = frozen ? 1 : 2;
                for (var i = data.rows.length - 1; i >= index; i--) {
                    var tr = opts.finder.getTr(target, i, "body", frozenText);
                    tr.attr("datagrid-row-index", i + 1);
                    tr.attr("id", state.rowIdPrefix + "-" + frozenText + "-" + (i + 1));
                    if (frozen && opts.rownumbers) {
                        var rownumber = i + 2;
                        if (opts.pagination) {
                            rownumber += (opts.pageNumber - 1) * opts.pageSize;
                        }
                        tr.find("div.datagrid-cell-rownumber").html(rownumber);
                    }
                    if (opts.striped) {
                        tr.removeClass("datagrid-row-alt").addClass((i + 1) % 2 ? "datagrid-row-alt" : "");
                    }
                }
            };

            function insertRowId(frozen) {
                var frozenText = frozen ? 1 : 2;
                var fields = $(target).datagrid("getColumnFields", frozen);
                var rowId = state.rowIdPrefix + "-" + frozenText + "-" + index;
                var tr = "<tr id=\"" + rowId + "\" class=\"datagrid-row\" datagrid-row-index=\"" + index + "\"></tr>";
                if (index >= data.rows.length) {
                    if (data.rows.length) {
                        opts.finder.getTr(target, "", "last", frozenText).after(tr);
                    } else {
                        var cc = frozen ? dc.body1 : dc.body2;
                        cc.html("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>" + tr + "</tbody></table>");
                    }
                } else {
                    opts.finder.getTr(target, index + 1, "body", frozenText).before(tr);
                }
            };

            updateRownumber.call(this, true);
            updateRownumber.call(this, false);
            insertRowId.call(this, true);
            insertRowId.call(this, false);
            data.total += 1;
            data.rows.splice(index, 0, row);
            this.refreshRow.call(this, target, index);
        },
        deleteRow: function (target, rowIndex) {
            var state = $.data(target, "datagrid");
            var opts = state.options;
            var data = state.data;

            function updateRownumber(frozen) {
                var frozenText = frozen ? 1 : 2;
                for (var i = rowIndex + 1; i < data.rows.length; i++) {
                    var tr = opts.finder.getTr(target, i, "body", frozenText);
                    tr.attr("datagrid-row-index", i - 1);
                    tr.attr("id", state.rowIdPrefix + "-" + frozenText + "-" + (i - 1));
                    if (frozen && opts.rownumbers) {
                        var rownumber = i;
                        if (opts.pagination) {
                            rownumber += (opts.pageNumber - 1) * opts.pageSize;
                        }
                        tr.find("div.datagrid-cell-rownumber").html(rownumber);
                    }
                    if (opts.striped) {
                        tr.removeClass("datagrid-row-alt").addClass((i - 1) % 2 ? "datagrid-row-alt" : "");
                    }
                }
            };

            opts.finder.getTr(target, rowIndex).remove();
            updateRownumber.call(this, true);
            updateRownumber.call(this, false);
            data.total -= 1;
            data.rows.splice(rowIndex, 1);
        },
        onBeforeRender: function (target, rows) {
        },
        onAfterRender: function (target) {
            var state = $.data(target, "datagrid");
            var opts = state.options;
            if (opts.showFooter) {
                var footer = $(target).datagrid("getPanel").find("div.datagrid-footer");
                footer.find("div.datagrid-cell-rownumber, div.datagrid-cell-check").css("visibility", "hidden");
            }
            if (opts.finder.getRows(target).length == 0) {
                this.renderEmptyRow(target);
            }
        },
        renderEmptyRow: function (target) {
            var dc = $.data(target, "datagrid").dc;
            dc.body2.html(this.renderTable(target, 0, [{}], false));
            dc.body2.find(".datagrid-row").removeClass("datagrid-row").removeAttr("datagrid-row-index");
            dc.body2.find("tbody *").css({ height: 1, borderColor: "transparent", background: "transparent" });
        }
    };
    $.fn.datagrid.defaults = $.extend({}, $.fn.panel.defaults, {
        sharedStyleSheet: false,
        frozenColumns: undefined,
        columns: undefined,
        fitColumns: false,
        resizeHandle: "right",
        autoRowHeight: true,
        toolbar: null,
        striped: false,
        method: "post",
        nowrap: true,
        idField: null,
        url: null,
        data: null,
        loadMsg: "Processing, please wait ...",
        rownumbers: false,
        singleSelect: false,
        ctrlSelect: false,
        selectOnCheck: true,
        checkOnSelect: true,
        pagination: false,
        pagePosition: "bottom",
        pageNumber: 1,
        pageSize: 10,
        pageList: [10, 20, 30, 40, 50],
        queryParams: {},
        sortName: null,
        sortOrder: "asc",
        multiSort: false,
        remoteSort: true,
        showHeader: true,
        showFooter: false,
        scrollbarSize: 18,
        rowEvents: { mouseover: handleOnmouseover(true), mouseout: handleOnmouseover(false), click: handlerOnclick, dblclick: handlerOndblclick, contextmenu: handlerContextmenu },
        rowStyler: function (rowIndex, rowData) {
        },
        loader: function (param, success, error) {
            var opts = $(this).datagrid("options");
            if (!opts.url) {
                return false;
            }
            $.ajax({
                type: opts.method,
                url: opts.url,
                data: param,
                dataType: "json",
                success: function (data) {
                    success(data);
                },
                error: function () {
                    error.apply(this, arguments);
                }
            });
        },
        loadFilter: function (data) {
            if (typeof data.length == "number" && typeof data.splice == "function") {
                return { total: data.length, rows: data };
            } else {
                return data;
            }
        },
        editors: editors,
        finder: {
            getTr: function (target, rowIndex, type, step) {
                type = type || "body";
                step = step || 0;
                var state = $.data(target, "datagrid");
                var dc = state.dc;
                var opts = state.options;
                if (step == 0) {
                    var tr1 = opts.finder.getTr(target, rowIndex, type, 1);
                    var tr2 = opts.finder.getTr(target, rowIndex, type, 2);
                    return tr1.add(tr2);
                } else {
                    if (type == "body") {
                        var tr = $("#" + state.rowIdPrefix + "-" + step + "-" + rowIndex);
                        if (!tr.length) {
                            tr = (step == 1 ? dc.body1 : dc.body2).find(">table>tbody>tr[datagrid-row-index=" + rowIndex + "]");
                        }
                        return tr;
                    } else {
                        if (type == "footer") {
                            return (step == 1 ? dc.footer1 : dc.footer2).find(">table>tbody>tr[datagrid-row-index=" + rowIndex + "]");
                        } else {
                            if (type == "selected") {
                                return (step == 1 ? dc.body1 : dc.body2).find("* > table > tbody > tr.datagrid-row-selected");
                            } else {
                                if (type == "highlight") {
                                    return (step == 1 ? dc.body1 : dc.body2).find("* > table > tbody > tr.datagrid-row-over");
                                } else {
                                    if (type == "checked") {
                                        return (step == 1 ? dc.body1 : dc.body2).find("* > table > tbody > tr.datagrid-row-checked");
                                    } else {
                                        if (type == "editing") {
                                            return (step == 1 ? dc.body1 : dc.body2).find("* > table > tbody > tr.datagrid-row-editing");
                                        } else {
                                            if (type == "last") {
                                                return (step == 1 ? dc.body1 : dc.body2).find("* > table > tbody > tr[datagrid-row-index]:last");
                                            } else {
                                                if (type == "allbody") {
                                                    return (step == 1 ? dc.body1 : dc.body2).find("* > table > tbody > tr[datagrid-row-index]");
                                                } else {
                                                    if (type == "allfooter") {
                                                        return (step == 1 ? dc.footer1 : dc.footer2).find("* > table > tbody > tr[datagrid-row-index]");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            getRow: function (target, p) {
                var rowIndex = (typeof p == "object") ? p.attr("datagrid-row-index") : p;
                return $.data(target, "datagrid").data.rows[parseInt(rowIndex)];
            },
            getRows: function (target) {
                return $(target).datagrid("getRows");
            }
        },
        view: view,
        onBeforeLoad: function (param) {
        },
        onLoadSuccess: function () {
        },
        onLoadError: function () {
        },
        onClickRow: function (rowIndex, rowData) {
        },
        onDblClickRow: function (rowIndex, rowData) {
        },
        onClickCell: function (rowIndex, field, value) {
        },
        onDblClickCell: function (rowIndex, field, value) {
        },
        onBeforeSortColumn: function (sort, order) {
        },
        onSortColumn: function (sort, order) {
        },
        onResizeColumn: function (rowIndex, rowData) {
        },
        onBeforeSelect: function (rowIndex, rowData) {
        },
        onSelect: function (rowIndex, rowData) {
        },
        onBeforeUnselect: function (rowIndex, rowData) {
        },
        onUnselect: function (rowIndex, rowData) {
        },
        onSelectAll: function (rows) {
        },
        onUnselectAll: function (rows) {
        },
        onBeforeCheck: function (rowIndex, rowData) {
        },
        onCheck: function (rowIndex, rowData) {
        },
        onBeforeUncheck: function (rowIndex, rowData) {
        },
        onUncheck: function (rowIndex, rowData) {
        },
        onCheckAll: function (rows) {
        },
        onUncheckAll: function (rows) {
        },
        onBeforeEdit: function (rowIndex, rowData) {
        },
        onBeginEdit: function (rowIndex, rowData) {
        },
        onEndEdit: function (rowIndex, rowData, changes) {
        },
        onAfterEdit: function (rowIndex, rowData, changes) {
        },
        onCancelEdit: function (rowIndex, rowData) {
        },
        onHeaderContextMenu: function (e, field) {
        },
        onRowContextMenu: function (e, rowIndex, rowData) {
        }
    });
})(jQuery);