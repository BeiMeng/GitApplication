/**
 * jQuery EasyUI 1.4.2
 * 
 * Copyright (c) 2009-2015 www.jeasyui.com. All rights reserved.
 *
 * Licensed under the GPL license: http://www.gnu.org/licenses/gpl.txt
 * To use it on other terms please contact us at info@jeasyui.com
 *
 */
(function($) {
    function initGrid(target) {
        var state = $.data(target, "treegrid");
        var opts = state.options;
        $(target).datagrid($.extend({}, opts, {
            url: null,
            data: null,
            loader: function() {
                return false;
            },
            onBeforeLoad: function() {
                return false;
            },
            onLoadSuccess: function() {
            },
            onResizeColumn: function(field, width) {
                setRowHeight(target);
                opts.onResizeColumn.call(target, field, width);
            },
            onBeforeSortColumn: function(sort, order) {
                if (opts.onBeforeSortColumn.call(target, sort, order) == false) {
                    return false;
                }
            },
            onSortColumn: function(sort, order) {
                opts.sortName = sort;
                opts.sortOrder = order;
                if (opts.remoteSort) {
                    request(target);
                } else {
                    var data = $(target).treegrid("getData");
                    loadData(target, 0, data);
                }
                opts.onSortColumn.call(target, sort, order);
            },
            onClickCell: function(nodeId, field) {
                opts.onClickCell.call(target, field, find(target, nodeId));
            },
            onDblClickCell: function(nodeId, field) {
                opts.onDblClickCell.call(target, field, find(target, nodeId));
            },
            onRowContextMenu: function(e, nodeId) {
                opts.onContextMenu.call(target, e, find(target, nodeId));
            }
        }));
        var options = $.data(target, "datagrid").options;
        opts.columns = options.columns;
        opts.frozenColumns = options.frozenColumns;

        state.dc = $.data(target, "datagrid").dc;
        if (opts.pagination) {
            var pager = $(target).datagrid("getPager");
            pager.pagination({
                pageNumber: opts.pageNumber,
                pageSize: opts.pageSize,
                pageList: opts.pageList,
                onSelectPage: function(pageNumber, pageSize) {
                    opts.pageNumber = pageNumber;
                    opts.pageSize = pageSize;
                    request(target);
                }
            });
            opts.pageSize = pager.pagination("options").pageSize;
        }
    };

    function setRowHeight(target, nodeId) {
        var opts = $.data(target, "datagrid").options;
        var dc = $.data(target, "datagrid").dc;
        if (!dc.body1.is(":empty") && (!opts.nowrap || opts.autoRowHeight)) {
            if (nodeId != undefined) {
                var children = getChildren(target, nodeId);
                for (var i = 0; i < children.length; i++) {
                    setHeight(children[i][opts.idField]);
                }
            }
        }
        $(target).datagrid("fixRowHeight", nodeId);

        function setHeight(nodeid) {
            var tr1 = opts.finder.getTr(target, nodeid, "body", 1);
            var tr2 = opts.finder.getTr(target, nodeid, "body", 2);
            tr1.css("height", "");
            tr2.css("height", "");
            var height = Math.max(tr1.height(), tr2.height());
            tr1.css("height", height);
            tr2.css("height", height);
        };
    };

    function fixRowNumbers(target) {
        var dc = $.data(target, "datagrid").dc;
        var opts = $.data(target, "treegrid").options;
        if (!opts.rownumbers) {
            return;
        }
        dc.body1.find("div.datagrid-cell-rownumber").each(function(i) {
            $(this).html(i + 1);
        });
    };

    function rowOnmouseover(ismouseover) {
        return function(e) {
            $.fn.datagrid.defaults.rowEvents[ismouseover ? "mouseover" : "mouseout"](e);
            var tt = $(e.target);
            var fn = ismouseover ? "addClass" : "removeClass";
            if (tt.hasClass("tree-hit")) {
                tt.hasClass("tree-expanded") ? tt[fn]("tree-expanded-hover") : tt[fn]("tree-collapsed-hover");
            }
        };
    };

    function rowOnClick(e) {
        var tt = $(e.target);
        if (tt.hasClass("tree-hit")) {
            var tr = tt.closest("tr.datagrid-row");
            var children = tr.closest("div.datagrid-view").children(".datagrid-f")[0];
            toggle(children, tr.attr("node-id"));
        } else {
            $.fn.datagrid.defaults.rowEvents.click(e);
        }
    };

    function initSubTree(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var tr1 = opts.finder.getTr(target, nodeId, "body", 1);
        var tr2 = opts.finder.getTr(target, nodeId, "body", 2);
        var colspan1 = $(target).datagrid("getColumnFields", true).length + (opts.rownumbers ? 1 : 0);
        var colspan2 = $(target).datagrid("getColumnFields", false).length;
        createSubTree(tr1, colspan1);
        createSubTree(tr2, colspan2);

        function createSubTree(tr, colspan) {
            $("<tr class=\"treegrid-tr-tree\">" + "<td style=\"border:0px\" colspan=\"" + colspan + "\">" + "<div></div>" + "</td>" + "</tr>").insertAfter(tr);
        };
    };

    function loadData(target, nodeId, param, isAppend) {
        var state = $.data(target, "treegrid");
        var opts = state.options;
        var dc = state.dc;
        param = opts.loadFilter.call(target, param, nodeId);
        var row = find(target, nodeId);
        if (row) {
            var tr1 = opts.finder.getTr(target, nodeId, "body", 1);
            var tr2 = opts.finder.getTr(target, nodeId, "body", 2);
            var cc1 = tr1.next("tr.treegrid-tr-tree").children("td").children("div");
            var cc2 = tr2.next("tr.treegrid-tr-tree").children("td").children("div");
            if (!isAppend) {
                row.children = [];
            }
        } else {
            var cc1 = dc.body1;
            var cc2 = dc.body2;
            if (!isAppend) {
                state.data = [];
            }
        }
        if (!isAppend) {
            cc1.empty();
            cc2.empty();
        }
        if (opts.view.onBeforeRender) {
            opts.view.onBeforeRender.call(opts.view, target, nodeId, param);
        }
        opts.view.render.call(opts.view, target, cc1, true);
        opts.view.render.call(opts.view, target, cc2, false);
        if (opts.showFooter) {
            opts.view.renderFooter.call(opts.view, target, dc.footer1, true);
            opts.view.renderFooter.call(opts.view, target, dc.footer2, false);
        }
        if (opts.view.onAfterRender) {
            opts.view.onAfterRender.call(opts.view, target);
        }
        if (!nodeId && opts.pagination) {
            var total = $.data(target, "treegrid").total;
            var pager = $(target).datagrid("getPager");
            if (pager.pagination("options").total != total) {
                pager.pagination({ total: total });
            }
        }
        setRowHeight(target);
        fixRowNumbers(target);
        $(target).treegrid("showLines");
        $(target).treegrid("setSelectionState");
        $(target).treegrid("autoSizeColumn");
        opts.onLoadSuccess.call(target, row, param);
    };

    function request(target, parentId, param, isAppend, callBack) {
        var opts = $.data(target, "treegrid").options;
        var body = $(target).datagrid("getPanel").find("div.datagrid-body");
        if (param) {
            opts.queryParams = param;
        }
        var queryParams = $.extend({}, opts.queryParams);
        if (opts.pagination) {
            $.extend(queryParams, { page: opts.pageNumber, rows: opts.pageSize });
        }
        if (opts.sortName) {
            $.extend(queryParams, { sort: opts.sortName, order: opts.sortOrder });
        }
        var row = find(target, parentId);
        if (opts.onBeforeLoad.call(target, row, queryParams) == false) {
            return;
        }
        var folder = body.find("tr[node-id=\"" + parentId + "\"] span.tree-folder");
        folder.addClass("tree-loading");
        $(target).treegrid("loading");
        var loadResult = opts.loader.call(target, queryParams, function(data) {
            folder.removeClass("tree-loading");
            $(target).treegrid("loaded");
            loadData(target, parentId, data, isAppend);
            if (callBack) {
                callBack();
            }
        }, function() {
            folder.removeClass("tree-loading");
            $(target).treegrid("loaded");
            opts.onLoadError.apply(target, arguments);
            if (callBack) {
                callBack();
            }
        });
        if (loadResult == false) {
            folder.removeClass("tree-loading");
            $(target).treegrid("loaded");
        }
    };

    function getRoot(target) {
        var roots = getRoots(target);
        if (roots.length) {
            return roots[0];
        } else {
            return null;
        }
    };

    function getRoots(target) {
        return $.data(target, "treegrid").data;
    };

    function getParent(target, nodeId) {
        var row = find(target, nodeId);
        if (row._parentId) {
            return find(target, row._parentId);
        } else {
            return null;
        }
    };

    function getChildren(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var body = $(target).datagrid("getPanel").find("div.datagrid-view2 div.datagrid-body");
        var children = [];
        if (nodeId) {
            findChildren(nodeId);
        } else {
            var roots = getRoots(target);
            for (var i = 0; i < roots.length; i++) {
                children.push(roots[i]);
                findChildren(roots[i][opts.idField]);
            }
        }

        function findChildren(nodeId) {
            var node = find(target, nodeId);
            if (node && node.children) {
                for (var i = 0, len = node.children.length; i < len; i++) {
                    var child = node.children[i];
                    children.push(child);
                    findChildren(child[opts.idField]);
                }
            }
        };

        return children;
    };

    function getLevel(target, nodeId) {
        if (!nodeId) {
            return 0;
        }
        var opts = $.data(target, "treegrid").options;
        var gridView = $(target).datagrid("getPanel").children("div.datagrid-view");
        var treeNode = gridView.find("div.datagrid-body tr[node-id=\"" + nodeId + "\"]").children("td[field=\"" + opts.treeField + "\"]");
        return treeNode.find("span.tree-indent, span.tree-hit").length;
    };

    function find(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var data = $.data(target, "treegrid").data;
        var cc = [data];
        while (cc.length) {
            var c = cc.shift();
            for (var i = 0; i < c.length; i++) {
                var rowData = c[i];
                if (rowData[opts.idField] == nodeId) {
                    return rowData;
                } else {
                    if (rowData["children"]) {
                        cc.push(rowData["children"]);
                    }
                }
            }
        }
        return null;
    };

    function collapse(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var row = find(target, nodeId);
        var tr = opts.finder.getTr(target, nodeId);
        var hit = tr.find("span.tree-hit");
        if (hit.length == 0) {
            return;
        }
        if (hit.hasClass("tree-collapsed")) {
            return;
        }
        if (opts.onBeforeCollapse.call(target, row) == false) {
            return;
        }
        hit.removeClass("tree-expanded tree-expanded-hover").addClass("tree-collapsed");
        hit.next().removeClass("tree-folder-open");
        row.state = "closed";
        tr = tr.next("tr.treegrid-tr-tree");
        var cc = tr.children("td").children("div");
        if (opts.animate) {
            cc.slideUp("normal", function() {
                $(target).treegrid("autoSizeColumn");
                setRowHeight(target, nodeId);
                opts.onCollapse.call(target, row);
            });
        } else {
            cc.hide();
            $(target).treegrid("autoSizeColumn");
            setRowHeight(target, nodeId);
            opts.onCollapse.call(target, row);
        }
    };

    function expand(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var tr = opts.finder.getTr(target, nodeId);
        var hit = tr.find("span.tree-hit");
        var row = find(target, nodeId);
        if (hit.length == 0) {
            return;
        }
        if (hit.hasClass("tree-expanded")) {
            return;
        }
        if (opts.onBeforeExpand.call(target, row) == false) {
            return;
        }
        hit.removeClass("tree-collapsed tree-collapsed-hover").addClass("tree-expanded");
        hit.next().addClass("tree-folder-open");
        var subtree = tr.next("tr.treegrid-tr-tree");
        if (subtree.length) {
            var cc = subtree.children("td").children("div");
            expandSubtree(cc);
        } else {
            initSubTree(target, row[opts.idField]);
            var subtree = tr.next("tr.treegrid-tr-tree");
            var cc = subtree.children("td").children("div");
            cc.hide();
            var queryParams = $.extend({}, opts.queryParams || {});
            queryParams.id = row[opts.idField];
            request(target, row[opts.idField], queryParams, true, function() {
                if (cc.is(":empty")) {
                    subtree.remove();
                } else {
                    expandSubtree(cc);
                }
            });
        }

        function expandSubtree(cc) {
            row.state = "open";
            if (opts.animate) {
                cc.slideDown("normal", function() {
                    $(target).treegrid("autoSizeColumn");
                    setRowHeight(target, nodeId);
                    opts.onExpand.call(target, row);
                });
            } else {
                cc.show();
                $(target).treegrid("autoSizeColumn");
                setRowHeight(target, nodeId);
                opts.onExpand.call(target, row);
            }
        };
    };

    function toggle(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var tr = opts.finder.getTr(target, nodeId);
        var hit = tr.find("span.tree-hit");
        if (hit.hasClass("tree-expanded")) {
            collapse(target, nodeId);
        } else {
            expand(target, nodeId);
        }
    };

    function collapseAll(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var children = getChildren(target, nodeId);
        if (nodeId) {
            children.unshift(find(target, nodeId));
        }
        for (var i = 0; i < children.length; i++) {
            collapse(target, children[i][opts.idField]);
        }
    };

    function expandAll(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var children = getChildren(target, nodeId);
        if (nodeId) {
            children.unshift(find(target, nodeId));
        }
        for (var i = 0; i < children.length; i++) {
            expand(target, children[i][opts.idField]);
        }
    };

    function expandTo(target, nodeId) {
        var opts = $.data(target, "treegrid").options;
        var ids = [];
        var p = getParent(target, nodeId);
        while (p) {
            var id = p[opts.idField];
            ids.unshift(id);
            p = getParent(target, id);
        }
        for (var i = 0; i < ids.length; i++) {
            expand(target, ids[i]);
        }
    };

    function append(target, param) {
        var opts = $.data(target, "treegrid").options;
        if (param.parent) {
            var tr = opts.finder.getTr(target, param.parent);
            if (tr.next("tr.treegrid-tr-tree").length == 0) {
                initSubTree(target, param.parent);
            }
            var cell = tr.children("td[field=\"" + opts.treeField + "\"]").children("div.datagrid-cell");
            var icon = cell.children("span.tree-icon");
            if (icon.hasClass("tree-file")) {
                icon.removeClass("tree-file").addClass("tree-folder tree-folder-open");
                var hit = $("<span class=\"tree-hit tree-expanded\"></span>").insertBefore(icon);
                if (hit.prev().length) {
                    hit.prev().remove();
                }
            }
        }
        loadData(target, param.parent, param.data, true);
    };

    function insert(target, param) {
        var ref = param.before || param.after;
        var opts = $.data(target, "treegrid").options;
        var parent = getParent(target, ref);
        append(target, { parent: (parent ? parent[opts.idField] : null), data: [param.data] });
        var roots = parent ? parent.children : $(target).treegrid("getRoots");
        for (var i = 0; i < roots.length; i++) {
            if (roots[i][opts.idField] == ref) {
                var _9c = roots[roots.length - 1];
                roots.splice(param.before ? i : (i + 1), 0, _9c);
                roots.splice(roots.length - 1, 1);
                break;
            }
        }
        doInsert(true);
        doInsert(false);
        fixRowNumbers(target);
        $(target).treegrid("showLines");

        function doInsert(isLeft) {
            var side = isLeft ? 1 : 2;
            var tr = opts.finder.getTr(target, param.data[opts.idField], "body", side);
            var btable = tr.closest("table.datagrid-btable");
            tr = tr.parent().children();
            var rows = opts.finder.getTr(target, ref, "body", side);
            if (param.before) {
                tr.insertBefore(rows);
            } else {
                var sub = rows.next("tr.treegrid-tr-tree");
                tr.insertAfter(sub.length ? sub : rows);
            }
            btable.remove();
        };
    };

    function remove(target, nodeId) {
        var state = $.data(target, "treegrid");
        $(target).datagrid("deleteRow", nodeId);
        fixRowNumbers(target);
        state.total -= 1;
        $(target).datagrid("getPager").pagination("refresh", { total: state.total });
        $(target).treegrid("showLines");
    };

    function showLines(target) {
        var t = $(target);
        var opts = t.treegrid("options");
        if (opts.lines) {
            t.treegrid("getPanel").addClass("tree-lines");
        } else {
            t.treegrid("getPanel").removeClass("tree-lines");
            return;
        }
        t.treegrid("getPanel").find("span.tree-indent").removeClass("tree-line tree-join tree-joinbottom");
        t.treegrid("getPanel").find("div.datagrid-cell").removeClass("tree-node-last tree-root-first tree-root-one");
        var roots = t.treegrid("getRoots");
        if (roots.length > 1) {
            getFirstRootNode(roots[0]).addClass("tree-root-first");
        } else {
            if (roots.length == 1) {
                getFirstRootNode(roots[0]).addClass("tree-root-one");
            }
        }
        initTreeNodes(roots);
        addTreeLines(roots);

        function initTreeNodes(nodes) {
            $.map(nodes, function(node) {
                if (node.children && node.children.length) {
                    initTreeNodes(node.children);
                } else {
                    var firstRootNode = getFirstRootNode(node);
                    firstRootNode.find(".tree-icon").prev().addClass("tree-join");
                }
            });
            if (nodes.length) {
                var node = getFirstRootNode(nodes[nodes.length - 1]);
                node.addClass("tree-node-last");
                node.find(".tree-join").removeClass("tree-join").addClass("tree-joinbottom");
            }
        };

        function addTreeLines(nodes) {
            $.map(nodes, function(item) {
                if (item.children && item.children.length) {
                    addTreeLines(item.children);
                }
            });
            for (var i = 0; i < nodes.length - 1; i++) {
                var node = nodes[i];
                var nodelevel = t.treegrid("getLevel", node[opts.idField]);
                var tr = opts.finder.getTr(target, node[opts.idField]);
                var cc = tr.next().find("tr.datagrid-row td[field=\"" + opts.treeField + "\"] div.datagrid-cell");
                cc.find("span:eq(" + (nodelevel - 1) + ")").addClass("tree-line");
            }
        };

        function getFirstRootNode(node) {
            var tr = opts.finder.getTr(target, node[opts.idField]);
            var cell = tr.find("td[field=\"" + opts.treeField + "\"] div.datagrid-cell");
            return cell;
        };
    };

    $.fn.treegrid = function(options, param) {
        if (typeof options == "string") {
            var method = $.fn.treegrid.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.datagrid(options, param);
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "treegrid");
            if (state) {
                $.extend(state.options, options);
            } else {
                state = $.data(this, "treegrid", { options: $.extend({}, $.fn.treegrid.defaults, $.fn.treegrid.parseOptions(this), options), data: [] });
            }
            initGrid(this);
            if (state.options.data) {
                $(this).treegrid("loadData", state.options.data);
            }
            request(this);
        });
    };

    $.fn.treegrid.methods = {
        options: function(jq) {
            return $.data(jq[0], "treegrid").options;
        },
        resize: function(jq, options) {
            return jq.each(function() {
                $(this).datagrid("resize", options);
            });
        },
        fixRowHeight: function(jq, id) {
            return jq.each(function() {
                setRowHeight(this, id);
            });
        },
        loadData: function(jq, data) {
            return jq.each(function() {
                loadData(this, data.parent, data);
            });
        },
        load: function(jq, param) {
            return jq.each(function() {
                $(this).treegrid("options").pageNumber = 1;
                $(this).treegrid("getPager").pagination({ pageNumber: 1 });
                $(this).treegrid("reload", param);
            });
        },
        reload: function(jq, id) {
            return jq.each(function() {
                var opts = $(this).treegrid("options");
                var node = {};
                if (typeof id == "object") {
                    node = id;
                } else {
                    node = $.extend({}, opts.queryParams);
                    node.id = id;
                }
                if (node.id) {
                    var record = $(this).treegrid("find", node.id);
                    if (record.children) {
                        record.children.splice(0, record.children.length);
                    }
                    opts.queryParams = node;
                    var tr = opts.finder.getTr(this, node.id);
                    tr.next("tr.treegrid-tr-tree").remove();
                    tr.find("span.tree-hit").removeClass("tree-expanded tree-expanded-hover").addClass("tree-collapsed");
                    expand(this, node.id);
                } else {
                    request(this, null, node);
                }
            });
        },
        reloadFooter: function(jq, footer) {
            return jq.each(function() {
                var opts = $.data(this, "treegrid").options;
                var dc = $.data(this, "datagrid").dc;
                if (footer) {
                    $.data(this, "treegrid").footer = footer;
                }
                if (opts.showFooter) {
                    opts.view.renderFooter.call(opts.view, this, dc.footer1, true);
                    opts.view.renderFooter.call(opts.view, this, dc.footer2, false);
                    if (opts.view.onAfterRender) {
                        opts.view.onAfterRender.call(opts.view, this);
                    }
                    $(this).treegrid("fixRowHeight");
                }
            });
        },
        getData: function(jq) {
            return $.data(jq[0], "treegrid").data;
        },
        getFooterRows: function(jq) {
            return $.data(jq[0], "treegrid").footer;
        },
        getRoot: function(jq) {
            return getRoot(jq[0]);
        },
        getRoots: function(jq) {
            return getRoots(jq[0]);
        },
        getParent: function(jq, id) {
            return getParent(jq[0], id);
        },
        getChildren: function(jq, id) {
            return getChildren(jq[0], id);
        },
        getLevel: function(jq, id) {
            return getLevel(jq[0], id);
        },
        find: function(jq, id) {
            return find(jq[0], id);
        },
        isLeaf: function(jq, id) {
            var opts = $.data(jq[0], "treegrid").options;
            var tr = opts.finder.getTr(jq[0], id);
            var hit = tr.find("span.tree-hit");
            return hit.length == 0;
        },
        select: function(jq, id) {
            return jq.each(function() {
                $(this).datagrid("selectRow", id);
            });
        },
        unselect: function(jq, id) {
            return jq.each(function() {
                $(this).datagrid("unselectRow", id);
            });
        },
        collapse: function(jq, id) {
            return jq.each(function() {
                collapse(this, id);
            });
        },
        expand: function(jq, id) {
            return jq.each(function() {
                expand(this, id);
            });
        },
        toggle: function(jq, id) {
            return jq.each(function() {
                toggle(this, id);
            });
        },
        collapseAll: function(jq, id) {
            return jq.each(function() {
                collapseAll(this, id);
            });
        },
        expandAll: function(jq, id) {
            return jq.each(function() {
                expandAll(this, id);
            });
        },
        expandTo: function(jq, id) {
            return jq.each(function() {
                expandTo(this, id);
            });
        },
        append: function(jq, param) {
            return jq.each(function() {
                append(this, param);
            });
        },
        insert: function(jq, param) {
            return jq.each(function() {
                insert(this, param);
            });
        },
        remove: function(jq, id) {
            return jq.each(function() {
                remove(this, id);
            });
        },
        pop: function(jq, id) {
            var row = jq.treegrid("find", id);
            jq.treegrid("remove", id);
            return row;
        },
        refresh: function(jq, id) {
            return jq.each(function() {
                var opts = $.data(this, "treegrid").options;
                opts.view.refreshRow.call(opts.view, this, id);
            });
        },
        update: function(jq, param) {
            return jq.each(function() {
                var opts = $.data(this, "treegrid").options;
                opts.view.updateRow.call(opts.view, this, param.id, param.row);
            });
        },
        beginEdit: function(jq, id) {
            return jq.each(function() {
                $(this).datagrid("beginEdit", id);
                $(this).treegrid("fixRowHeight", id);
            });
        },
        endEdit: function(jq, id) {
            return jq.each(function() {
                $(this).datagrid("endEdit", id);
            });
        },
        cancelEdit: function(jq, id) {
            return jq.each(function() {
                $(this).datagrid("cancelEdit", id);
            });
        },
        showLines: function(jq) {
            return jq.each(function() {
                showLines(this);
            });
        }
    };
    $.fn.treegrid.parseOptions = function(target) {
        return $.extend({}, $.fn.datagrid.parseOptions(target), $.parser.parseOptions(target, ["treeField", { animate: "boolean" }]));
    };
    var view = $.extend({}, $.fn.datagrid.defaults.view, {
        render: function(target, container, frozen) {
            var opts = $.data(target, "treegrid").options;
            var fields = $(target).datagrid("getColumnFields", frozen);
            var rowIdPrefix = $.data(target, "datagrid").rowIdPrefix;
            if (frozen) {
                if (!(opts.rownumbers || (opts.frozenColumns && opts.frozenColumns.length))) {
                    return;
                }
            }
            var grid = this;
            if (this.treeNodes && this.treeNodes.length) {
                var nodes = buildTreeNodes(frozen, this.treeLevel, this.treeNodes);
                $(container).append(nodes.join(""));
            }

            function buildTreeNodes(frozen, treeLevel, rows) {
                var parent = $(target).treegrid("getParent", rows[0][opts.idField]);
                var length = (parent ? parent.children.length : $(target).treegrid("getRoots").length) - rows.length;
                var html = ["<table class=\"datagrid-btable\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"];
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    if (row.state != "open" && row.state != "closed") {
                        row.state = "open";
                    }
                    var css = opts.rowStyler ? opts.rowStyler.call(target, row) : "";
                    var rowclass = "";
                    var rowwstyle = "";
                    if (typeof css == "string") {
                        rowwstyle = css;
                    } else {
                        if (css) {
                            rowclass = css["class"] || "";
                            rowwstyle = css["style"] || "";
                        }
                    }
                    var cls = "class=\"datagrid-row " + (length++ % 2 && opts.striped ? "datagrid-row-alt " : " ") + rowclass + "\"";
                    var attr = rowwstyle ? "style=\"" + rowwstyle + "\"" : "";
                    var rowId = rowIdPrefix + "-" + (frozen ? 1 : 2) + "-" + row[opts.idField];
                    html.push("<tr id=\"" + rowId + "\" node-id=\"" + row[opts.idField] + "\" " + cls + " " + attr + ">");
                    html = html.concat(grid.renderRow.call(grid, target, fields, frozen, treeLevel, row));
                    html.push("</tr>");
                    if (row.children && row.children.length) {
                        var tt = buildTreeNodes(frozen, treeLevel + 1, row.children);
                        var v = row.state == "closed" ? "none" : "block";
                        html.push("<tr class=\"treegrid-tr-tree\"><td style=\"border:0px\" colspan=" + (fields.length + (opts.rownumbers ? 1 : 0)) + "><div style=\"display:" + v + "\">");
                        html = html.concat(tt);
                        html.push("</div></td></tr>");
                    }
                }
                html.push("</tbody></table>");
                return html;
            };
        },
        renderFooter: function(target, grid, frozen) {
            var opts = $.data(target, "treegrid").options;
            var footer = $.data(target, "treegrid").footer || [];
            var fields = $(target).datagrid("getColumnFields", frozen);
            var html = ["<table class=\"datagrid-ftable\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\"><tbody>"];
            for (var i = 0; i < footer.length; i++) {
                var row = footer[i];
                row[opts.idField] = row[opts.idField] || ("foot-row-id" + i);
                html.push("<tr class=\"datagrid-row\" node-id=\"" + row[opts.idField] + "\">");
                html.push(this.renderRow.call(this, target, fields, frozen, 0, row));
                html.push("</tr>");
            }
            html.push("</tbody></table>");
            $(grid).html(html.join(""));
        },
        renderRow: function(target, fields, frozen, deepth, row) {
            var opts = $.data(target, "treegrid").options;
            var cc = [];
            if (frozen && opts.rownumbers) {
                cc.push("<td class=\"datagrid-td-rownumber\"><div class=\"datagrid-cell-rownumber\">0</div></td>");
            }
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
                var col = $(target).datagrid("getColumnOption", field);
                if (col) {
                    var css = col.styler ? (col.styler(row[field], row) || "") : "";
                    var rowclass = "";
                    var rowstyle = "";
                    if (typeof css == "string") {
                        rowstyle = css;
                    } else {
                        if (cc) {
                            rowclass = css["class"] || "";
                            rowstyle = css["style"] || "";
                        }
                    }
                    var cls = rowclass ? "class=\"" + rowclass + "\"" : "";
                    var style = col.hidden ? "style=\"display:none;" + rowstyle + "\"" : (rowstyle ? "style=\"" + rowstyle + "\"" : "");
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
                    if (col.checkbox) {
                        cc.push("class=\"datagrid-cell-check ");
                    } else {
                        cc.push("class=\"datagrid-cell " + col.cellClass);
                    }
                    cc.push("\">");
                    if (col.checkbox) {
                        if (row.checked) {
                            cc.push("<input type=\"checkbox\" checked=\"checked\"");
                        } else {
                            cc.push("<input type=\"checkbox\"");
                        }
                        cc.push(" name=\"" + field + "\" value=\"" + (row[field] != undefined ? row[field] : "") + "\">");
                    } else {
                        var val = null;
                        if (col.formatter) {
                            val = col.formatter(row[field], row);
                        } else {
                            val = row[field];
                        }
                        if (field == opts.treeField) {
                            for (var j = 0; j < deepth; j++) {
                                cc.push("<span class=\"tree-indent\"></span>");
                            }
                            if (row.state == "closed") {
                                cc.push("<span class=\"tree-hit tree-collapsed\"></span>");
                                cc.push("<span class=\"tree-icon tree-folder " + (row.iconCls ? row.iconCls : "") + "\"></span>");
                            } else {
                                if (row.children && row.children.length) {
                                    cc.push("<span class=\"tree-hit tree-expanded\"></span>");
                                    cc.push("<span class=\"tree-icon tree-folder tree-folder-open " + (row.iconCls ? row.iconCls : "") + "\"></span>");
                                } else {
                                    cc.push("<span class=\"tree-indent\"></span>");
                                    cc.push("<span class=\"tree-icon tree-file " + (row.iconCls ? row.iconCls : "") + "\"></span>");
                                }
                            }
                            cc.push("<span class=\"tree-title\">" + val + "</span>");
                        } else {
                            cc.push(val);
                        }
                    }
                    cc.push("</div>");
                    cc.push("</td>");
                }
            }
            return cc.join("");
        },
        refreshRow: function(target, id) {
            this.updateRow.call(this, target, id, {});
        },
        updateRow: function(target, id, row) {
            var opts = $.data(target, "treegrid").options;
            var node = $(target).treegrid("find", id);
            $.extend(node, row);
            var level = $(target).treegrid("getLevel", id) - 1;
            var rowstyle = opts.rowStyler ? opts.rowStyler.call(target, node) : "";
            var rowIdPrefix = $.data(target, "datagrid").rowIdPrefix;
            var rowId = node[opts.idField];

            function updateColumns(frozen) {
                var columnsFrozen = $(target).treegrid("getColumnFields", frozen);
                var tr = opts.finder.getTr(target, id, "body", (frozen ? 1 : 2));
                var rownumber = tr.find("div.datagrid-cell-rownumber").html();
                var isCheckedRow = tr.find("div.datagrid-cell-check input[type=checkbox]").is(":checked");
                tr.html(this.renderRow(target, columnsFrozen, frozen, level, node));
                tr.attr("style", rowstyle || "");
                tr.find("div.datagrid-cell-rownumber").html(rownumber);
                if (isCheckedRow) {
                    tr.find("div.datagrid-cell-check input[type=checkbox]")._propAttr("checked", true);
                }
                if (rowId != id) {
                    tr.attr("id", rowIdPrefix + "-" + (frozen ? 1 : 2) + "-" + rowId);
                    tr.attr("node-id", rowId);
                }
            };

            updateColumns.call(this, true);
            updateColumns.call(this, false);
            $(target).treegrid("fixRowHeight", id);
        },
        deleteRow: function(target, id) {
            var opts = $.data(target, "treegrid").options;
            var tr = opts.finder.getTr(target, id);
            tr.next("tr.treegrid-tr-tree").remove();
            tr.remove();
            var parent = del(id);
            if (parent) {
                if (parent.children.length == 0) {
                    tr = opts.finder.getTr(target, parent[opts.idField]);
                    tr.next("tr.treegrid-tr-tree").remove();
                    var cells = tr.children("td[field=\"" + opts.treeField + "\"]").children("div.datagrid-cell");
                    cells.find(".tree-icon").removeClass("tree-folder").addClass("tree-file");
                    cells.find(".tree-hit").remove();
                    $("<span class=\"tree-indent\"></span>").prependTo(cells);
                }
            }

            function del(id) {
                var cc;
                var parent = $(target).treegrid("getParent", id);
                if (parent) {
                    cc = parent.children;
                } else {
                    cc = $(target).treegrid("getData");
                }
                for (var i = 0; i < cc.length; i++) {
                    if (cc[i][opts.idField] == id) {
                        cc.splice(i, 1);
                        break;
                    }
                }
                return parent;
            };
        },
        onBeforeRender: function(target, nodeId, data) {
            if ($.isArray(nodeId)) {
                data = { total: nodeId.length, rows: nodeId };
                nodeId = null;
            }
            if (!data) {
                return false;
            }
            var state = $.data(target, "treegrid");
            var opts = state.options;
            if (data.length == undefined) {
                if (data.footer) {
                    state.footer = data.footer;
                }
                if (data.total) {
                    state.total = data.total;
                }
                data = this.transfer(target, nodeId, data.rows);
            } else {
                function setParent(param, nodeId) {
                    for (var i = 0; i < param.length; i++) {
                        var row = param[i];
                        row._parentId = nodeId;
                        if (row.children && row.children.length) {
                            setParent(row.children, row[opts.idField]);
                        }
                    }
                };
                setParent(data, nodeId);
            }

            var node = find(target, nodeId);
            if (node) {
                if (node.children) {
                    node.children = node.children.concat(data);
                } else {
                    node.children = data;
                }
            } else {
                state.data = state.data.concat(data);
            }
            this.sort(target, data);
            this.treeNodes = data;
            this.treeLevel = $(target).treegrid("getLevel", nodeId);
        },
        sort: function(target, data) {
            var opts = $.data(target, "treegrid").options;
            if (!opts.remoteSort && opts.sortName) {
                var sortNames = opts.sortName.split(",");
                var orders = opts.sortOrder.split(",");
                sort(data);
            }

            function sort(rows) {
                rows.sort(function(r1, r2) {
                    var r = 0;
                    for (var i = 0; i < sortNames.length; i++) {
                        var sn = sortNames[i];
                        var so = orders[i];
                        var col = $(target).treegrid("getColumnOption", sn);
                        var sorter = col.sorter || function(a, b) {
                            return a == b ? 0 : (a > b ? 1 : -1);
                        };
                        r = sorter(r1[sn], r2[sn]) * (so == "asc" ? 1 : -1);
                        if (r != 0) {
                            return r;
                        }
                    }
                    return r;
                });
                for (var i = 0; i < rows.length; i++) {
                    var children = rows[i].children;
                    if (children && children.length) {
                        sort(children);
                    }
                }
            };
        },
        transfer: function(target, nodeId, data) {
            var opts = $.data(target, "treegrid").options;
            var rows = [];
            for (var i = 0; i < data.length; i++) {
                rows.push(data[i]);
            }
            var children = [];
            for (var i = 0; i < rows.length; i++) {
                var row = rows[i];
                if (!nodeId) {
                    if (!row._parentId) {
                        children.push(row);
                        rows.splice(i, 1);
                        i--;
                    }
                } else {
                    if (row._parentId == nodeId) {
                        children.push(row);
                        rows.splice(i, 1);
                        i--;
                    }
                }
            }
            var toDo = [];
            for (var i = 0; i < children.length; i++) {
                toDo.push(children[i]);
            }
            while (toDo.length) {
                var node = toDo.shift();
                for (var i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    if (row._parentId == node[opts.idField]) {
                        if (node.children) {
                            node.children.push(row);
                        } else {
                            node.children = [row];
                        }
                        toDo.push(row);
                        rows.splice(i, 1);
                        i--;
                    }
                }
            }
            return children;
        }
    });

    $.fn.treegrid.defaults = $.extend({}, $.fn.datagrid.defaults, {
        treeField: null,
        lines: false,
        animate: false,
        singleSelect: true,
        view: view,
        rowEvents: $.extend({}, $.fn.datagrid.defaults.rowEvents, { mouseover: rowOnmouseover(true), mouseout: rowOnmouseover(false), click: rowOnClick }),
        loader: function(param, success, error) {
            var opts = $(this).treegrid("options");
            if (!opts.url) {
                return false;
            }
            $.ajax({
                type: opts.method,
                url: opts.url,
                data: param,
                dataType: "json",
                success: function(data) {
                    success(data);
                },
                error: function() {
                    error.apply(this, arguments);
                }
            });
        },
        loadFilter: function(data, parentId) {
            return data;
        },
        finder: {
            getTr: function(target, id, type, rowIndex) {
                type = type || "body";
                rowIndex = rowIndex || 0;
                var dc = $.data(target, "datagrid").dc;
                if (rowIndex == 0) {
                    var opts = $.data(target, "treegrid").options;
                    var tr1 = opts.finder.getTr(target, id, type, 1);
                    var tr2 = opts.finder.getTr(target, id, type, 2);
                    return tr1.add(tr2);
                } else {
                    if (type == "body") {
                        var tr = $("#" + $.data(target, "datagrid").rowIdPrefix + "-" + rowIndex + "-" + id);
                        if (!tr.length) {
                            tr = (rowIndex == 1 ? dc.body1 : dc.body2).find("tr[node-id=\"" + id + "\"]");
                        }
                        return tr;
                    } else {
                        if (type == "footer") {
                            return (rowIndex == 1 ? dc.footer1 : dc.footer2).find("tr[node-id=\"" + id + "\"]");
                        } else {
                            if (type == "selected") {
                                return (rowIndex == 1 ? dc.body1 : dc.body2).find("tr.datagrid-row-selected");
                            } else {
                                if (type == "highlight") {
                                    return (rowIndex == 1 ? dc.body1 : dc.body2).find("tr.datagrid-row-over");
                                } else {
                                    if (type == "checked") {
                                        return (rowIndex == 1 ? dc.body1 : dc.body2).find("tr.datagrid-row-checked");
                                    } else {
                                        if (type == "last") {
                                            return (rowIndex == 1 ? dc.body1 : dc.body2).find("tr:last[node-id]");
                                        } else {
                                            if (type == "allbody") {
                                                return (rowIndex == 1 ? dc.body1 : dc.body2).find("tr[node-id]");
                                            } else {
                                                if (type == "allfooter") {
                                                    return (rowIndex == 1 ? dc.footer1 : dc.footer2).find("tr[node-id]");
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
            getRow: function(target, p) {
                var id = (typeof p == "object") ? p.attr("node-id") : p;
                return $(target).treegrid("find", id);
            },
            getRows: function(target) {
                return $(target).treegrid("getChildren");
            }
        },
        onBeforeLoad: function(row, param) {
        },
        onLoadSuccess: function(row, data) {
        },
        onLoadError: function() {
        },
        onBeforeCollapse: function(row) {
        },
        onCollapse: function(row) {
        },
        onBeforeExpand: function(row) {
        },
        onExpand: function(row) {
        },
        onClickRow: function(row) {
        },
        onDblClickRow: function(row) {
        },
        onClickCell: function(field, row) {
        },
        onDblClickCell: function(field, row) {
        },
        onContextMenu: function(e, row) {
        },
        onBeforeEdit: function(row) {
        },
        onAfterEdit: function(row, changes) {
        },
        onCancelEdit: function(row) {
        }
    });
})(jQuery);