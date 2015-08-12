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
    function wrapTree(jq) {
        var tree = $(jq);
        tree.addClass("tree");
        return tree;
    };

    function bindEvent(jq) {
        var opts = $.data(jq, "tree").options;
        $(jq).unbind().bind("mouseover", function(e) {
            var tt = $(e.target);
            var treeNode = tt.closest("div.tree-node");
            if (!treeNode.length) {
                return;
            }
            treeNode.addClass("tree-node-hover");
            if (tt.hasClass("tree-hit")) {
                if (tt.hasClass("tree-expanded")) {
                    tt.addClass("tree-expanded-hover");
                } else {
                    tt.addClass("tree-collapsed-hover");
                }
            }
            e.stopPropagation();
        }).bind("mouseout", function(e) {
            var tt = $(e.target);
            var treeNode = tt.closest("div.tree-node");
            if (!treeNode.length) {
                return;
            }
            treeNode.removeClass("tree-node-hover");
            if (tt.hasClass("tree-hit")) {
                if (tt.hasClass("tree-expanded")) {
                    tt.removeClass("tree-expanded-hover");
                } else {
                    tt.removeClass("tree-collapsed-hover");
                }
            }
            e.stopPropagation();
        }).bind("click", function(e) {
            var tt = $(e.target);
            var treeNode = tt.closest("div.tree-node");
            if (!treeNode.length) {
                return;
            }
            if (tt.hasClass("tree-hit")) {
                toggle(jq, treeNode[0]);
                return false;
            } else {
                if (tt.hasClass("tree-checkbox")) {
                    check(jq, treeNode[0]);
                    return false;
                } else {
                    select(jq, treeNode[0]);
                    opts.onClick.call(jq, getNode(jq, treeNode[0]));
                }
            }
            e.stopPropagation();
        }).bind("dblclick", function(e) {
            var treeNode = $(e.target).closest("div.tree-node");
            if (!treeNode.length) {
                return;
            }
            select(jq, treeNode[0]);
            opts.onDblClick.call(jq, getNode(jq, treeNode[0]));
            e.stopPropagation();
        }).bind("contextmenu", function(e) {
            var treeNode = $(e.target).closest("div.tree-node");
            if (!treeNode.length) {
                return;
            }
            opts.onContextMenu.call(jq, e, getNode(jq, treeNode[0]));
            e.stopPropagation();
        });
    };

    // 禁用拖放功能
    function disableDnd(jq) {
        var opts = $.data(jq, "tree").options;
        opts.dnd = false;
        var node = $(jq).find("div.tree-node");
        node.draggable("disable");
        node.css("cursor", "pointer");
    };

    //启用拖放功能
    function enableDnd(jq) {
        var state = $.data(jq, "tree");
        var opts = state.options;
        var tree = state.tree;
        state.disabledNodes = [];
        opts.dnd = true;
        tree.find("div.tree-node").draggable({
            disabled: false,
            revert: true,
            cursor: "pointer",
            proxy: function(node) {
                var p = $("<div class=\"tree-node-proxy\"></div>").appendTo("body");
                p.html("<span class=\"tree-dnd-icon tree-dnd-no\">&nbsp;</span>" + $(node).find(".tree-title").html());
                p.hide();
                return p;
            },
            deltaX: 15,
            deltaY: 15,
            onBeforeDrag: function(e) {
                if (opts.onBeforeDrag.call(jq, getNode(jq, this)) == false) {
                    return false;
                }
                if ($(e.target).hasClass("tree-hit") || $(e.target).hasClass("tree-checkbox")) {
                    return false;
                }
                if (e.which != 1) {
                    return false;
                }
                $(this).next("ul").find("div.tree-node").droppable({ accept: "no-accept" });
                var nodes = $(this).find("span.tree-indent");
                if (nodes.length) {
                    e.data.offsetWidth -= nodes.length * nodes.width();
                }
            },
            onStartDrag: function() {
                $(this).draggable("proxy").css({ left: -10000, top: -10000 });
                opts.onStartDrag.call(jq, getNode(jq, this));
                var sNode = getNode(jq, this);
                if (sNode.id == undefined) {
                    sNode.id = "easyui_tree_node_id_temp";
                    update(jq, sNode);
                }
                state.draggingNodeId = sNode.id;
            },
            onDrag: function(e) {
                var x1 = e.pageX, y1 = e.pageY, x2 = e.data.startX, y2 = e.data.startY;
                var d = Math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
                if (d > 3) {
                    $(this).draggable("proxy").show();
                }
                this.pageY = e.pageY;
            },
            onStopDrag: function() {
                $(this).next("ul").find("div.tree-node").droppable({ accept: "div.tree-node" });
                for (var i = 0; i < state.disabledNodes.length; i++) {
                    $(state.disabledNodes[i]).droppable("enable");
                }
                state.disabledNodes = [];
                var endNode = find(jq, state.draggingNodeId);
                if (endNode && endNode.id == "easyui_tree_node_id_temp") {
                    endNode.id = "";
                    update(jq, endNode);
                }
                opts.onStopDrag.call(jq, endNode);
            }
        }).droppable({
            accept: "div.tree-node",
            onDragEnter: function(e, source) {
                if (opts.onDragEnter.call(jq, this, changeNode(source)) == false) {
                    doDnd(source, false);
                    $(this).removeClass("tree-node-append tree-node-top tree-node-bottom");
                    $(this).droppable("disable");
                    state.disabledNodes.push(this);
                }
            },
            onDragOver: function(e, source) {
                if ($(this).droppable("options").disabled) {
                    return;
                }
                var pageY = source.pageY;
                var top = $(this).offset().top;
                var height = top + $(this).outerHeight();
                doDnd(source, true);
                $(this).removeClass("tree-node-append tree-node-top tree-node-bottom");
                if (pageY > top + (height - top) / 2) {
                    if (height - pageY < 5) {
                        $(this).addClass("tree-node-bottom");
                    } else {
                        $(this).addClass("tree-node-append");
                    }
                } else {
                    if (pageY - top < 5) {
                        $(this).addClass("tree-node-top");
                    } else {
                        $(this).addClass("tree-node-append");
                    }
                }
                if (opts.onDragOver.call(jq, this, changeNode(source)) == false) {
                    doDnd(source, false);
                    $(this).removeClass("tree-node-append tree-node-top tree-node-bottom");
                    $(this).droppable("disable");
                    state.disabledNodes.push(this);
                }
            },
            onDragLeave: function(e, source) {
                doDnd(source, false);
                $(this).removeClass("tree-node-append tree-node-top tree-node-bottom");
                opts.onDragLeave.call(jq, this, changeNode(source));
            },
            onDrop: function(e, source) {
                var target = this;
                var action, point;
                if ($(this).hasClass("tree-node-append")) {
                    action = moveNode;
                    point = "append";
                } else {
                    action = insertNode;
                    point = $(this).hasClass("tree-node-top") ? "top" : "bottom";
                }
                if (opts.onBeforeDrop.call(jq, target, changeNode(source), point) == false) {
                    $(this).removeClass("tree-node-append tree-node-top tree-node-bottom");
                    return;
                }
                action(source, target, point);
                $(this).removeClass("tree-node-append tree-node-top tree-node-bottom");
            }
        });

        function changeNode(target, pop) {
            return $(target).closest("ul.tree").tree(pop ? "pop" : "getData", target);
        };

        function doDnd(target, isEnableDnd) {
            var node = $(target).draggable("proxy").find("span.tree-dnd-icon");
            node.removeClass("tree-dnd-yes tree-dnd-no").addClass(isEnableDnd ? "tree-dnd-yes" : "tree-dnd-no");
        };

        function moveNode(nodeDom, parent) {
            if (getNode(jq, parent).state == "closed") {
                expand(jq, parent, function () {
                    doMoveNode();
                });
            } else {
                doMoveNode();
            }

            function doMoveNode() {
                var node = changeNode(nodeDom, true);
                $(jq).tree("append", { parent: parent, data: [node] });
                opts.onDrop.call(jq, parent, node, "append");
            };
        };

        function insertNode(nodeDom, dest, point) {
            var param = {};
            if (point == "top") {
                param.before = dest;
            } else {
                param.after = dest;
            }
            var node = changeNode(nodeDom, true);
            param.data = node;
            $(jq).tree("insert", param);
            opts.onDrop.call(jq, dest, node, point);
        };
    };

    function check(jq, target, isChecked) {
        var state = $.data(jq, "tree");
        var opts = state.options;
        if (!opts.checkbox) {
            return;
        }
        var node = getNode(jq, target);
        if (isChecked == undefined) {
            var ck = $(target).find(".tree-checkbox");
            if (ck.hasClass("tree-checkbox1")) {
                isChecked = false;
            } else {
                if (ck.hasClass("tree-checkbox0")) {
                    isChecked = true;
                } else {
                    if (node._checked == undefined) {
                        node._checked = $(target).find(".tree-checkbox").hasClass("tree-checkbox1");
                    }
                    isChecked = !node._checked;
                }
            }
        }
        node._checked = isChecked;
        if (opts.onBeforeCheck.call(jq, node, isChecked) == false) {
            return;
        }
        //设置级联选中
        if (opts.cascadeCheck) {
            setChildrenChecked(node, isChecked);
            setParentsChecked(node, isChecked);
        } else {
            setCheckStyle($(node.target), isChecked ? "1" : "0");
        }
        opts.onCheck.call(jq, node, isChecked);

        function setCheckStyle(node, isCheckValue) {
            var ck = node.find(".tree-checkbox");
            ck.removeClass("tree-checkbox0 tree-checkbox1 tree-checkbox2");
            ck.addClass("tree-checkbox" + isCheckValue);
        };

        function setChildrenChecked(node, isChecked) {
            if (opts.deepCheck) {
                var $node = $("#" + node.domId);
                var checkValue = isChecked ? "1" : "0";
                setCheckStyle($node, checkValue);
                setCheckStyle($node.next(), checkValue);
            } else {
                checkChild(node, isChecked);
                getChildrenOfNextNode(node.children || [], function(n) {
                    checkChild(n, isChecked);
                });
            }
        };

        function checkChild(node, isChecked) {
            if (node.hidden) {
                return;
            }
            var cls = "tree-checkbox" + (isChecked ? "1" : "0");
            var $node = $("#" + node.domId);
            setCheckStyle($node, isChecked ? "1" : "0");
            if (node.children) {
                for (var i = 0; i < node.children.length; i++) {
                    if (node.children[i].hidden) {
                        if (!$("#" + node.children[i].domId).find("." + cls).length) {
                            setCheckStyle($node, "2");
                            var parentNode = getParent(jq, $node[0]);
                            while (parentNode) {
                                setCheckStyle($(parentNode.target), "2");
                                parentNode = getParent(jq, parentNode[0]);
                            }
                            return;
                        }
                    }
                }
            }
        };

        function setParentsChecked(n, ischecked) {
            var $node = $("#" + n.domId);
            var parentNode = getParent(jq, $node[0]);
            if (parentNode) {
                var checkValue = "";
                if (isAllNull($node, true)) {
                    checkValue = "1";
                } else {
                    if (isAllNull($node, false)) {
                        checkValue = "0";
                    } else {
                        checkValue = "2";
                    }
                }
                setCheckStyle($(parentNode.target), checkValue);
                setParentsChecked(parentNode, ischecked);
            }
        };

        //是否全未勾选
        function isAllNull(n, isChecked) {
            var cls = "tree-checkbox" + (isChecked ? "1" : "0");
            var ck = n.find(".tree-checkbox");
            if (!ck.hasClass(cls)) {
                return false;
            }
            var b = true;
            n.parent().siblings().each(function() {
                var ck = $(this).children("div.tree-node").children(".tree-checkbox");
                if (ck.length && !ck.hasClass(cls)) {
                    b = false;
                    return false;
                }
            });
            return b;
        };
    };

    function setCheckBoxValue(jq, target) {
        var opts = $.data(jq, "tree").options;
        if (!opts.checkbox) {
            return;
        }
        var node = $(target);
        if (isLeaf(jq, target)) {//叶子结点
            var ck = node.find(".tree-checkbox");
            if (ck.length) {
                if (ck.hasClass("tree-checkbox1")) {
                    check(jq, target, true);
                } else {
                    check(jq, target, false);
                }
            } else {
                if (opts.onlyLeafCheck) {
                    $("<span class=\"tree-checkbox tree-checkbox0\"></span>").insertBefore(node.find(".tree-title"));
                }
            }
        } else {
            var ck = node.find(".tree-checkbox");
            if (opts.onlyLeafCheck) {
                ck.remove();
            } else {
                if (ck.hasClass("tree-checkbox1")) {
                    check(jq, target, true);
                } else {
                    if (ck.hasClass("tree-checkbox2")) {
                        var checked = true;
                        var unchecked = true;
                        var children = getChildren(jq, target);
                        for (var i = 0; i < children.length; i++) {
                            if (children[i].checked) {
                                unchecked = false;
                            } else {
                                checked = false;
                            }
                        }
                        if (checked) {
                            check(jq, target, true);
                        }
                        if (unchecked) {
                            check(jq, target, false);
                        }
                    }
                }
            }
        }
    };

    function loadData(jq, ul, data, isAppend) {
        var state = $.data(jq, "tree");
        var opts = state.options;
        var prevNode = $(ul).prevAll("div.tree-node:first");
        data = opts.loadFilter.call(jq, data, prevNode[0]);
        var root = getNodeByAttr(jq, "domId", prevNode.attr("id"));
        if (!isAppend) {
            root ? root.children = data : state.data = data;
            $(ul).empty();
        } else {
            if (root) {
                root.children ? root.children = root.children.concat(data) : root.children = data;
            } else {
                state.data = state.data.concat(data);
            }
        }
        opts.view.render.call(opts.view, jq, ul, data);
        if (opts.dnd) {
            enableDnd(jq);
        }
        if (root) {
            update(jq, root);
        }
        var uncheckedNodes = [];
        var checkedNodes = [];
        for (var i = 0; i < data.length; i++) {
            var node = data[i];
            if (!node.checked) {
                uncheckedNodes.push(node);
            }
        }
        getChildrenOfNextNode(data, function(node) {
            if (node.checked) {
                checkedNodes.push(node);
            }
        });
        var checkEventHandle = opts.onCheck;
        opts.onCheck = function() {
        };
        if (uncheckedNodes.length) {
            check(jq, $("#" + uncheckedNodes[0].domId)[0], false);
        }
        for (var i = 0; i < checkedNodes.length; i++) {
            check(jq, $("#" + checkedNodes[i].domId)[0], true);
        }
        opts.onCheck = checkEventHandle;
        setTimeout(function() {
            loadTreeCss(jq, jq);
        }, 0);
        opts.onLoadSuccess.call(jq, root, data);
    };

    function loadTreeCss(jq, ul, _6d) {
        var opts = $.data(jq, "tree").options;
        if (opts.lines) {
            $(jq).addClass("tree-lines");
        } else {
            $(jq).removeClass("tree-lines");
            return;
        }
        if (!_6d) {
            _6d = true;
            $(jq).find("span.tree-indent").removeClass("tree-line tree-join tree-joinbottom");
            $(jq).find("div.tree-node").removeClass("tree-node-last tree-root-first tree-root-one");
            var roots = $(jq).tree("getRoots");
            if (roots.length > 1) {
                $(roots[0].target).addClass("tree-root-first");
            } else {
                if (roots.length == 1) {
                    $(roots[0].target).addClass("tree-root-one");
                }
            }
        }
        $(ul).children("li").each(function() {
            var node = $(this).children("div.tree-node");
            var ul = node.next("ul");
            if (ul.length) {
                if ($(this).next().length) {
                    addTreeLine(node);
                }
                loadTreeCss(jq, ul, _6d);
            } else {
                addIcon(node);
            }
        });
        var lastNode = $(ul).children("li:last").children("div.tree-node").addClass("tree-node-last");
        lastNode.children("span.tree-join").removeClass("tree-join").addClass("tree-joinbottom");

        function addIcon(node, s75) {
            var icon = node.find("span.tree-icon");
            icon.prev("span.tree-indent").addClass("tree-join");
        };

        function addTreeLine(node) {
            var depth = node.find("span.tree-indent, span.tree-hit").length;
            node.next().find("div.tree-node").each(function() {
                $(this).children("span:eq(" + (depth - 1) + ")").addClass("tree-line");
            });
        };
    };

    function request(jq, ul, params, callBack) {
        var opts = $.data(jq, "tree").options;
        params = $.extend({}, opts.queryParams, params || {});
        var node = null;
        if (jq != ul) {
            var prevNodes = $(ul).prev();
            node = getNode(jq, prevNodes[0]);
        }
        if (opts.onBeforeLoad.call(jq, node, params) == false) {
            return;
        }
        var folder = $(ul).prev().children("span.tree-folder");
        folder.addClass("tree-loading");
        var result = opts.loader.call(jq, params, function(data) {
            folder.removeClass("tree-loading");
            loadData(jq, ul, data);
            if (callBack) {
                callBack();
            }
        }, function() {
            folder.removeClass("tree-loading");
            opts.onLoadError.apply(jq, arguments);
            if (callBack) {
                callBack();
            }
        });
        if (result == false) {
            folder.removeClass("tree-loading");
        }
    };

    function expand(jq, target, callBack) {
        var opts = $.data(jq, "tree").options;
        var hit = $(target).children("span.tree-hit");
        if (hit.length == 0) {
            return;
        }
        if (hit.hasClass("tree-expanded")) {
            return;
        }
        var node = getNode(jq, target);
        if (opts.onBeforeExpand.call(jq, node) == false) {
            return;//若onBeforeExpand返回false则直接返回
        }
        //修改样式
        hit.removeClass("tree-collapsed tree-collapsed-hover").addClass("tree-expanded");
        hit.next().addClass("tree-folder-open");
        var ul = $(target).next();//获取同辈后面的结点
        if (ul.length) {
            if (opts.animate) {
                ul.slideDown("normal", function() {
                    node.state = "open";
                    opts.onExpand.call(jq, node);
                    if (callBack) {
                        callBack();
                    }
                });
            } else {
                ul.css("display", "block");
                node.state = "open";
                opts.onExpand.call(jq, node);
                if (callBack) {
                    callBack();
                }
            }
        } else {
            var child = $("<ul style=\"display:none\"></ul>").insertAfter(target);
            request(jq, child[0], { id: node.id }, function () {
                if (child.is(":empty")) {
                    child.remove();
                }
                if (opts.animate) {
                    child.slideDown("normal", function() {
                        node.state = "open";
                        opts.onExpand.call(jq, node);
                        if (callBack) {
                            callBack();
                        }
                    });
                } else {
                    child.css("display", "block");
                    node.state = "open";
                    opts.onExpand.call(jq, node);
                    if (callBack) {
                        callBack();
                    }
                }
            });
        }
    };

    function collapse(jq, nodeDom) {
        var opts = $.data(jq, "tree").options;
        var hit = $(nodeDom).children("span.tree-hit");
        if (hit.length == 0) {
            return;
        }
        if (hit.hasClass("tree-collapsed")) {
            return;
        }
        var node = getNode(jq, nodeDom);
        if (opts.onBeforeCollapse.call(jq, node) == false) {
            return;
        }
        hit.removeClass("tree-expanded tree-expanded-hover").addClass("tree-collapsed");
        hit.next().removeClass("tree-folder-open");
        var ul = $(nodeDom).next();
        if (opts.animate) {
            ul.slideUp("normal", function() {
                node.state = "closed";
                opts.onCollapse.call(jq, node);
            });
        } else {
            ul.css("display", "none");
            node.state = "closed";
            opts.onCollapse.call(jq, node);
        }
    };

    function toggle(jq, nodeDom) {
        var hit = $(nodeDom).children("span.tree-hit");
        if (hit.length == 0) {
            return;
        }
        if (hit.hasClass("tree-expanded")) {
            collapse(jq, nodeDom);
        } else {
            expand(jq, nodeDom);
        }
    };

    function expandAll(jq, target) {
        var children = getChildren(jq, target);
        if (target) {
            children.unshift(getNode(jq, target));
        }
        for (var i = 0; i < children.length; i++) {
            expand(jq, children[i].target);
        }
    };

    function expandTo(jq, target) {
        var ancestors = [];
        var p = getParent(jq, target);
        while (p) {
            ancestors.unshift(p);
            p = getParent(jq, p.target);
        }
        for (var i = 0; i < ancestors.length; i++) {
            expand(jq, ancestors[i].target);
        }
    };

    function scrollTo(jq, target) {
        var c = $(jq).parent();
        while (c[0].tagName != "BODY" && c.css("overflow-y") != "auto") {
            c = c.parent();
        }
        var n = $(target);
        var ntop = n.offset().top;
        if (c[0].tagName != "BODY") {
            var ctop = c.offset().top;
            if (ntop < ctop) {
                c.scrollTop(c.scrollTop() + ntop - ctop);
            } else {
                if (ntop + n.outerHeight() > ctop + c.outerHeight() - 18) {
                    c.scrollTop(c.scrollTop() + ntop + n.outerHeight() - ctop - c.outerHeight() + 18);
                }
            }
        } else {
            c.scrollTop(ntop);
        }
    };

    function collapseAll(jq, target) {
        var children = getChildren(jq, target);
        if (target) {
            children.unshift(getNode(jq, target));
        }
        for (var i = 0; i < children.length; i++) {
            collapse(jq, children[i].target);
        }
    };

    function append(jq, param) {
        var node = $(param.parent);
        var data = param.data;
        if (!data) {
            return;
        }
        data = $.isArray(data) ? data : [data];
        if (!data.length) {
            return;
        }
        var ul;
        if (node.length == 0) {
            ul = $(jq);
        } else {
            if (isLeaf(jq, node[0])) {
                var icon = node.find("span.tree-icon");
                icon.removeClass("tree-file").addClass("tree-folder tree-folder-open");
                var hit = $("<span class=\"tree-hit tree-expanded\"></span>").insertBefore(icon);
                if (hit.prev().length) {
                    hit.prev().remove();
                }
            }
            ul = node.next();
            if (!ul.length) {
                ul = $("<ul></ul>").insertAfter(node);
            }
        }
        loadData(jq, ul[0], data, true);
        setCheckBoxValue(jq, ul.prev());
    };

    function insert(jq, param) {
        var ref = param.before || param.after;
        var parentNode = getParent(jq, ref);
        var data = param.data;
        if (!data) {
            return;
        }
        data = $.isArray(data) ? data : [data];
        if (!data.length) {
            return;
        }
        append(jq, { parent: (parentNode ? parentNode.target : null), data: data });
        var childrenNodes = parentNode ? parentNode.children : $(jq).tree("getRoots");
        for (var i = 0; i < childrenNodes.length; i++) {
            if (childrenNodes[i].domId == $(ref).attr("id")) {
                for (var j = data.length - 1; j >= 0; j--) {
                    childrenNodes.splice((param.before ? i : (i + 1)), 0, data[j]);
                }
                childrenNodes.splice(childrenNodes.length - data.length, data.length);
                break;
            }
        }
        var li = $();
        for (var i = 0; i < data.length; i++) {
            li = li.add($("#" + data[i].domId).parent());
        }
        if (param.before) {
            li.insertBefore($(ref).parent());
        } else {
            li.insertAfter($(ref).parent());
        }
    };

    /**
	 * 删除一个节点和它的子节点
	 * @param {Object} jq
	 * @param {Object} target 需要删除的结点DOM对象
	 */
    function remove(jq, target) {
        var targetParentNode = del(target);
        $(target).parent().remove();
        if (targetParentNode) {
            if (!targetParentNode.children || !targetParentNode.children.length) {
                var node = $(targetParentNode.target);
                node.find(".tree-icon").removeClass("tree-folder").addClass("tree-file");
                node.find(".tree-hit").remove();
                $("<span class=\"tree-indent\"></span>").prependTo(node);
                node.next().remove();
            }
            update(jq, targetParentNode);
            setCheckBoxValue(jq, targetParentNode.target);
        }
        loadTreeCss(jq, jq);

        function del(targetNode) {
            var id = $(targetNode).attr("id");
            var parentNode = getParent(jq, targetNode);
            var cc = parentNode ? parentNode.children : $.data(jq, "tree").data;
            for (var i = 0; i < cc.length; i++) {
                if (cc[i].domId == id) {
                    cc.splice(i, 1);
                    break;
                }
            }
            return parentNode;
        };
    };

    function update(jq, target) {
        var opts = $.data(jq, "tree").options;
        var node = $(target.target);
        var data = getNode(jq, target.target);
        var isChecked = data.checked;
        if (data.iconCls) {
            node.find(".tree-icon").removeClass(data.iconCls);
        }
        $.extend(data, target);
        node.find(".tree-title").html(opts.formatter.call(jq, data));
        if (data.iconCls) {
            node.find(".tree-icon").addClass(data.iconCls);
        }
        if (isChecked != data.checked) {
            check(jq, target.target, data.checked);
        }
    };

    function getRoot(jq, target) {
        if (target) {
            var p = getParent(jq, target);
            while (p) {
                target = p.target;
                p = getParent(jq, target);
            }
            return getNode(jq, target);
        } else {
            var roots = getRoots(jq);
            return roots.length ? roots[0] : null;
        }
    };

    function getRoots(jq) {
        var data = $.data(jq, "tree").data;
        for (var i = 0; i < data.length; i++) {
            getNodeInfo(data[i]);
        }
        return data;
    };

    function getChildren(jq, target) {
        var nodes = [];
        var n = getNode(jq, target);
        var data = n ? (n.children || []) : $.data(jq, "tree").data;
        getChildrenOfNextNode(data, function (node) {
            nodes.push(getNodeInfo(node));
        });
        return nodes;
    };

    function getParent(jq, target) {
        var p = $(target).closest("ul").prevAll("div.tree-node:first");
        return getNode(jq, p[0]);
    };

    function getChecked(jq, state) {
        state = state || "checked";
        if (!$.isArray(state)) {
            state = [state];
        }
        var styles = [];
        for (var i = 0; i < state.length; i++) {
            var s = state[i];
            if (s == "checked") {
                styles.push("span.tree-checkbox1");
            } else {
                if (s == "unchecked") {
                    styles.push("span.tree-checkbox0");
                } else {
                    if (s == "indeterminate") {
                        styles.push("span.tree-checkbox2");
                    }
                }
            }
        }
        var nodes = [];
        $(jq).find(styles.join(",")).each(function() {
            var node = $(this).parent();
            nodes.push(getNode(jq, node[0]));
        });
        return nodes;
    };

    function getSelected(jq) {
        var node = $(jq).find("div.tree-node-selected");
        return node.length ? getNode(jq, node[0]) : null;
    };

    function getData(jq, target) {
        var data = getNode(jq, target);
        if (data && data.children) {
            getChildrenOfNextNode(data.children, function(node) {
                getNodeInfo(node);
            });
        }
        return data;
    };

    function getNode(jq, target) {
        return getNodeByAttr(jq, "domId", $(target).attr("id"));
    };

    function find(jq, id) {
        return getNodeByAttr(jq, "id", id);
    };

    function getNodeByAttr(jq, nodeAttr, nodeAttrValue) {
        var data = $.data(jq, "tree").data;
        var nodeToFind = null;
        getChildrenOfNextNode(data, function(node) {
            if (node[nodeAttr] == nodeAttrValue) {
                nodeToFind = getNodeInfo(node);
                return false;
            }
        });
        return nodeToFind;
    };

    function getNodeInfo(node) {
        var d = $("#" + node.domId);
        node.target = d[0];
        node.checked = d.find(".tree-checkbox").hasClass("tree-checkbox1");
        return node;
    };

    function getChildrenOfNextNode(data, callback) {
        var nodes = [];
        for (var i = 0; i < data.length; i++) {
            nodes.push(data[i]);
        }
        while (nodes.length) {
            var node = nodes.shift();
            if (callback(node) == false) {
                return;
            }
            if (node.children) {
                for (var i = node.children.length - 1; i >= 0; i--) {
                    nodes.unshift(node.children[i]);
                }
            }
        }
    };

    function select(jq, target) {
        var opts = $.data(jq, "tree").options;
        var node = getNode(jq, target);
        if (opts.onBeforeSelect.call(jq, node) == false) {
            return;
        }
        $(jq).find("div.tree-node-selected").removeClass("tree-node-selected");
        $(target).addClass("tree-node-selected");
        opts.onSelect.call(jq, node);
    };

    function isLeaf(jq, target) {
        return $(target).children("span.tree-hit").length == 0;
    };

    function beginEdit(jq, target) {
        var opts = $.data(jq, "tree").options;
        var node = getNode(jq, target);
        if (opts.onBeforeEdit.call(jq, node) == false) {
            return;
        }
        $(target).css("position", "relative");
        var nt = $(target).find(".tree-title");
        var titleWidth = nt.outerWidth();
        nt.empty();
        //编辑器
        var editor = $("<input class=\"tree-editor\">").appendTo(nt);
        editor.val(node.text).focus();
        editor.width(titleWidth + 20);
        editor.height(document.compatMode == "CSS1Compat" ? (18 - (editor.outerHeight() - editor.height())) : 18);
        editor.bind("click", function(e) {
            return false;
        }).bind("mousedown", function(e) {
            e.stopPropagation();
        }).bind("mousemove", function(e) {
            e.stopPropagation();
        }).bind("keydown", function(e) {
            if (e.keyCode == 13) {
                endEdit(jq, target);
                return false;
            } else {
                if (e.keyCode == 27) {
                    cancelEdit(jq, target);
                    return false;
                }
            }
        }).bind("blur", function(e) {
            e.stopPropagation();
            endEdit(jq, target);
        });
    };

    function endEdit(jq, target) {
        var opts = $.data(jq, "tree").options;
        $(target).css("position", "");
        var editor = $(target).find("input.tree-editor");
        var val = editor.val();
        editor.remove();
        var node = getNode(jq, target);
        node.text = val;
        update(jq, node);
        opts.onAfterEdit.call(jq, node);
    };

    function cancelEdit(jq, target) {
        var opts = $.data(jq, "tree").options;
        $(target).css("position", "");
        $(target).find("input.tree-editor").remove();
        var node = getNode(jq, target);
        update(jq, node);
        opts.onCancelEdit.call(jq, node);
    };

    function doFilter(jq, q) {
        var state = $.data(jq, "tree");
        var opts = state.options;
        var ids = {};
        getChildrenOfNextNode(state.data, function(node) {
            if (opts.filter.call(jq, q, node)) {
                $("#" + node.domId).removeClass("tree-node-hidden");
                ids[node.domId] = 1;
                node.hidden = false;
            } else {
                $("#" + node.domId).addClass("tree-node-hidden");
                node.hidden = true;
            }
        });
        for (var id in ids) {
            show(id);
        }

        function show(domId) {
            var p = $(jq).tree("getParent", $("#" + domId)[0]);
            while (p) {
                $(p.target).removeClass("tree-node-hidden");
                p.hidden = false;
                p = $(jq).tree("getParent", p.target);
            }
        };
    };

    $.fn.tree = function(options, param) {
        if (typeof options == "string") {
            return $.fn.tree.methods[options](this, param);
        }
        var options = options || {};
        return this.each(function() {
            var state = $.data(this, "tree");
            var opts;
            if (state) {
                opts = $.extend(state.options, options);
                state.options = opts;
            } else {
                opts = $.extend({}, $.fn.tree.defaults, $.fn.tree.parseOptions(this), options);
                $.data(this, "tree", { options: opts, tree: wrapTree(this), data: [] });
                var data = $.fn.tree.parseData(this);
                if (data.length) {
                    loadData(this, this, data);
                }
            }
            bindEvent(this);
            if (opts.data) {
                loadData(this, this, $.extend(true, [], opts.data));
            }
            request(this, this);
        });
    };
    
    $.fn.tree.methods = {
        options: function(jq) {
            return $.data(jq[0], "tree").options;
        },
        loadData: function(jq, data) {
            return jq.each(function() {
                loadData(this, this, data);
            });
        },
        getNode: function(jq, target) {
            return getNode(jq[0], target);
        },
        getData: function(jq, target) {
            return getData(jq[0], target);
        },
        reload: function(jq, target) {
            return jq.each(function() {
                if (target) {
                    var node = $(target);
                    var hit = node.children("span.tree-hit");
                    hit.removeClass("tree-expanded tree-expanded-hover").addClass("tree-collapsed");
                    node.next().remove();
                    expand(this, target);
                } else {
                    $(this).empty();
                    request(this, this);
                }
            });
        },
        getRoot: function(jq, target) {
            return getRoot(jq[0], target);
        },
        getRoots: function(jq) {
            return getRoots(jq[0]);
        },
        getParent: function(jq, target) {
            return getParent(jq[0], target);
        },
        getChildren: function(jq, target) {
            return getChildren(jq[0], target);
        },
        getChecked: function(jq, target) {
            return getChecked(jq[0], target);
        },
        getSelected: function(jq) {
            return getSelected(jq[0]);
        },
        isLeaf: function(jq, target) {
            return isLeaf(jq[0], target);
        },
        find: function(jq, id) {
            return find(jq[0], id);
        },
        select: function(jq, target) {
            return jq.each(function() {
                select(this, target);
            });
        },
        check: function(jq, target) {
            return jq.each(function() {
                check(this, target, true);
            });
        },
        uncheck: function(jq, target) {
            return jq.each(function() {
                check(this, target, false);
            });
        },
        collapse: function(jq, target) {
            return jq.each(function() {
                collapse(this, target);
            });
        },
        expand: function(jq, target) {
            return jq.each(function() {
                expand(this, target);
            });
        },
        collapseAll: function(jq, target) {
            return jq.each(function() {
                collapseAll(this, target);
            });
        },
        expandAll: function(jq, target) {
            return jq.each(function() {
                expandAll(this, target);
            });
        },
        expandTo: function(jq, target) {
            return jq.each(function() {
                expandTo(this, target);
            });
        },
        scrollTo: function(jq, target) {
            return jq.each(function() {
                scrollTo(this, target);
            });
        },
        toggle: function(jq, target) {
            return jq.each(function() {
                toggle(this, target);
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
        remove: function(jq, target) {
            return jq.each(function() {
                remove(this, target);
            });
        },
        pop: function(jq, target) {
            var node = jq.tree("getData", target);
            jq.tree("remove", target);
            return node;
        },
        update: function(jq, param) {
            return jq.each(function() {
                update(this, param);
            });
        },
        enableDnd: function(jq) {
            return jq.each(function() {
                enableDnd(this);
            });
        },
        disableDnd: function(jq) {
            return jq.each(function() {
                disableDnd(this);
            });
        },
        beginEdit: function(jq, target) {
            return jq.each(function() {
                beginEdit(this, target);
            });
        },
        endEdit: function(jq, target) {
            return jq.each(function() {
                endEdit(this, target);
            });
        },
        cancelEdit: function(jq, target) {
            return jq.each(function() {
                cancelEdit(this, target);
            });
        },
        doFilter: function(jq, q) {
            return jq.each(function() {
                doFilter(this, q);
            });
        }
    };

    $.fn.tree.parseOptions = function(target) {
        var t = $(target);
        return $.extend({}, $.parser.parseOptions(target, ["url", "method", { checkbox: "boolean", cascadeCheck: "boolean", onlyLeafCheck: "boolean" }, { animate: "boolean", lines: "boolean", dnd: "boolean" }]));
    };

    $.fn.tree.parseData = function(target) {
        var data = [];
        collectTreeData(data, $(target));
        return data;

        function collectTreeData(aa, tree) {
            tree.children("li").each(function() {
                var node = $(this);
                var item = $.extend({}, $.parser.parseOptions(this, ["id", "iconCls", "state"]), { checked: (node.attr("checked") ? true : undefined) });
                item.text = node.children("span").html();
                if (!item.text) {
                    item.text = node.html();
                }
                var children = node.children("ul");
                if (children.length) {
                    item.children = [];
                    collectTreeData(item.children, children);
                }
                aa.push(item);
            });
        };
    };
    var itemIndex = 1;
    var view = {
        render: function(jq, ul, data) {
            var opts = $.data(jq, "tree").options;
            var level = $(ul).prev("div.tree-node").find("span.tree-indent, span.tree-hit").length;
            var cc = getTreeHtml(level, data);
            $(ul).append(cc.join(""));

            function getTreeHtml(treeLevel, treeData) {
                var cc = [];
                for (var i = 0; i < treeData.length; i++) {
                    var item = treeData[i];
                    if (item.state != "open" && item.state != "closed") {
                        item.state = "open";
                    }
                    item.domId = "_easyui_tree_" + itemIndex++;
                    cc.push("<li>");
                    cc.push("<div id=\"" + item.domId + "\" class=\"tree-node\">");
                    for (var j = 0; j < treeLevel; j++) {
                        cc.push("<span class=\"tree-indent\"></span>");
                    }
                    var isLeaf = false;
                    if (item.state == "closed") {
                        cc.push("<span class=\"tree-hit tree-collapsed\"></span>");
                        cc.push("<span class=\"tree-icon tree-folder " + (item.iconCls ? item.iconCls : "") + "\"></span>");
                    } else {
                        if (item.children && item.children.length) {
                            cc.push("<span class=\"tree-hit tree-expanded\"></span>");
                            cc.push("<span class=\"tree-icon tree-folder tree-folder-open " + (item.iconCls ? item.iconCls : "") + "\"></span>");
                        } else {
                            cc.push("<span class=\"tree-indent\"></span>");
                            cc.push("<span class=\"tree-icon tree-file " + (item.iconCls ? item.iconCls : "") + "\"></span>");
                            isLeaf = true;
                        }
                    }
                    if (opts.checkbox) {
                        if ((!opts.onlyLeafCheck) || isLeaf) {
                            cc.push("<span class=\"tree-checkbox tree-checkbox0\"></span>");
                        }
                    }
                    cc.push("<span class=\"tree-title\">" + opts.formatter.call(jq, item) + "</span>");
                    cc.push("</div>");
                    if (item.children && item.children.length) {
                        var tmp = getTreeHtml(treeLevel + 1, item.children);
                        cc.push("<ul style=\"display:" + (item.state == "closed" ? "none" : "block") + "\">");
                        cc = cc.concat(tmp);
                        cc.push("</ul>");
                    }
                    cc.push("</li>");
                }
                return cc;
            };
        }
    };
    
    // 默认参数设置
    $.fn.tree.defaults = {
        url: null,
        method: "post",
        animate: false,
        checkbox: false,
        cascadeCheck: true,
        onlyLeafCheck: false,
        lines: false,
        dnd: false,
        data: null,
        queryParams: {},
        formatter: function(node) {
            return node.text;
        },
        filter: function(q, node) {
            return node.text.toLowerCase().indexOf(q.toLowerCase()) >= 0;
        },
        loader: function(param, success, error) {
            var opts = $(this).tree("options");
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
        loadFilter: function(data, parent) {
            return data;
        },
        view: view,
        onBeforeLoad: function(node, param) {},
        onLoadSuccess: function(node, data) {},
        onLoadError: function() {},
        onClick: function(node) {},
        onDblClick: function(node) {},
        onBeforeExpand: function(node) {},
        onExpand: function(node) {},
        onBeforeCollapse: function(node) {},
        onCollapse: function(node) {},
        onBeforeCheck: function (node, checked) {},
        onCheck: function (node, checked) {},
        onBeforeSelect: function(node) {},
        onSelect: function(node) {},
        onContextMenu: function(e, node) {},
        onBeforeDrag: function(node) {},
        onStartDrag: function(node) {},
        onStopDrag: function(node) {},
        onDragEnter: function (target, source) {},
        onDragOver: function (target, source) {},
        onDragLeave: function (target, source) {},
        onBeforeDrop: function (target, source, point) {},
        onDrop: function (target, source, point) {},
        onBeforeEdit: function(node) {},
        onAfterEdit: function(node) {},
        onCancelEdit: function(node) {}
    };
})(jQuery);