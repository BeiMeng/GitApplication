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
    var resizing = false;

    function setSize(target, param) {
        var state = $.data(target, "layout");
        var opts = state.options;
        var panels = state.panels;
        var cc = $(target);
        if (param) {
            $.extend(opts, { width: param.width, height: param.height });
        }
        if (target.tagName.toLowerCase() == "body") {
            cc._size("fit");
        } else {
            cc._size(opts);
        }
        var newSize = { top: 0, left: 0, width: cc.width(), height: cc.height() };
        setHeight(isVisible(panels.expandNorth) ? panels.expandNorth : panels.north, "n");
        setHeight(isVisible(panels.expandSouth) ? panels.expandSouth : panels.south, "s");
        setWidth(isVisible(panels.expandEast) ? panels.expandEast : panels.east, "e");
        setWidth(isVisible(panels.expandWest) ? panels.expandWest : panels.west, "w");
        panels.center.panel("resize", newSize);

        function setHeight(pp, dir) {
            if (!pp.length || !isVisible(pp)) {
                return;
            }
            var panelOpts = pp.panel("options");
            pp.panel("resize", { width: cc.width(), height: panelOpts.height });
            var height = pp.panel("panel").outerHeight();
            pp.panel("move", { left: 0, top: (dir == "n" ? 0 : cc.height() - height) });
            newSize.height -= height;
            if (dir == "n") {
                newSize.top += height;
                if (!panelOpts.split && panelOpts.border) {
                    newSize.top--;
                }
            }
            if (!panelOpts.split && panelOpts.border) {
                newSize.height++;
            }
        };

        function setWidth(pp, dir) {
            if (!pp.length || !isVisible(pp)) {
                return;
            }
            var panelOpts = pp.panel("options");
            pp.panel("resize", { width: panelOpts.width, height: newSize.height });
            var width = pp.panel("panel").outerWidth();
            pp.panel("move", { left: (dir == "e" ? cc.width() - width : 0), top: newSize.top });
            newSize.width -= width;
            if (dir == "w") {
                newSize.left += width;
                if (!panelOpts.split && panelOpts.border) {
                    newSize.left--;
                }
            }
            if (!panelOpts.split && panelOpts.border) {
                newSize.width++;
            }
        };
    };

    function init(target) {
        var cc = $(target);
        cc.addClass("layout");

        function initPanel(cc) {
            cc.children("div").each(function() {
                var opts = $.fn.layout.parsePanelOptions(this);
                if ("north,south,east,west,center".indexOf(opts.region) >= 0) {
                    createPanel(target, opts, this);
                }
            });
        };

        cc.children("form").length ? initPanel(cc.children("form")) : initPanel(cc);
        cc.append("<div class=\"layout-split-proxy-h\"></div><div class=\"layout-split-proxy-v\"></div>");
        cc.bind("_resize", function(e, param) {
            if ($(this).hasClass("easyui-fluid") || param) {
                setSize(target);
            }
            return false;
        });
    };

    function createPanel(target, options, el) {
        options.region = options.region || "center";
        var panels = $.data(target, "layout").panels;
        var cc = $(target);
        var dir = options.region;
        if (panels[dir].length) {
            return;
        }
        var pp = $(el);
        if (!pp.length) {
            pp = $("<div></div>").appendTo(cc);
        }
        var panelOpts = $.extend({}, $.fn.layout.paneldefaults, {
            width: (pp.length ? parseInt(pp[0].style.width) || pp.outerWidth() : "auto"),
            height: (pp.length ? parseInt(pp[0].style.height) || pp.outerHeight() : "auto"),
            doSize: false,
            collapsible: true,
            cls: ("layout-panel layout-panel-" + dir),
            bodyCls: "layout-body",
            onOpen: function() {
                var tools = $(this).panel("header").children("div.panel-tool");
                tools.children("a.panel-tool-collapse").hide();
                var btnDirection = { north: "up", south: "down", east: "right", west: "left" };
                if (!btnDirection[dir]) {
                    return;
                }
                var btncls = "layout-button-" + btnDirection[dir];
                var t = tools.children("a." + btncls);
                if (!t.length) {
                    t = $("<a href=\"javascript:void(0)\"></a>").addClass(btncls).appendTo(tools);
                    t.bind("click", { dir: dir }, function(e) {
                        collapsePanel(target, e.data.dir);
                        return false;
                    });
                }
                $(this).panel("options").collapsible ? t.show() : t.hide();
            }
        }, options);
        pp.panel(panelOpts);
        panels[dir] = pp;
        var shortDir = { north: "s", south: "n", east: "w", west: "e" };
        var panel = pp.panel("panel");
        if (pp.panel("options").split) {
            panel.addClass("layout-split-" + dir);
        }
        panel.resizable($.extend({}, {
            handles: (shortDir[dir] || ""),
            disabled: (!pp.panel("options").split),
            onStartResize: function(e) {
                resizing = true;
                if (dir == "north" || dir == "south") {
                    var newPanel = $(">div.layout-split-proxy-v", target);
                } else {
                    var newPanel = $(">div.layout-split-proxy-h", target);
                }
                var top = 0, left = 0, width = 0, height = 0;
                var pos = { display: "block" };
                if (dir == "north") {
                    pos.top = parseInt(panel.css("top")) + panel.outerHeight() - newPanel.height();
                    pos.left = parseInt(panel.css("left"));
                    pos.width = panel.outerWidth();
                    pos.height = newPanel.height();
                } else {
                    if (dir == "south") {
                        pos.top = parseInt(panel.css("top"));
                        pos.left = parseInt(panel.css("left"));
                        pos.width = panel.outerWidth();
                        pos.height = newPanel.height();
                    } else {
                        if (dir == "east") {
                            pos.top = parseInt(panel.css("top")) || 0;
                            pos.left = parseInt(panel.css("left")) || 0;
                            pos.width = newPanel.width();
                            pos.height = panel.outerHeight();
                        } else {
                            if (dir == "west") {
                                pos.top = parseInt(panel.css("top")) || 0;
                                pos.left = panel.outerWidth() - newPanel.width();
                                pos.width = newPanel.width();
                                pos.height = panel.outerHeight();
                            }
                        }
                    }
                }
                newPanel.css(pos);
                $("<div class=\"layout-mask\"></div>").css({ left: 0, top: 0, width: cc.width(), height: cc.height() }).appendTo(cc);
            },
            onResize: function(e) {
                if (dir == "north" || dir == "south") {
                    var resizePanel = $(">div.layout-split-proxy-v", target);
                    resizePanel.css("top", e.pageY - $(target).offset().top - resizePanel.height() / 2);
                } else {
                    var resizePanel = $(">div.layout-split-proxy-h", target);
                    resizePanel.css("left", e.pageX - $(target).offset().left - resizePanel.width() / 2);
                }
                return false;
            },
            onStopResize: function(e) {
                cc.children("div.layout-split-proxy-v, div.layout-split-proxy-h").hide();
                pp.panel("resize", e.data);
                setSize(target);
                resizing = false;
                cc.find("* > div.layout-mask").remove();
            }
        }, options));
    };

    function removePanel(target, region) {
        var panels = $.data(target, "layout").panels;
        if (panels[region].length) {
            panels[region].panel("destroy");
            panels[region] = $();
            var panelId = "expand" + region.substring(0, 1).toUpperCase() + region.substring(1);
            if (panels[panelId]) {
                panels[panelId].panel("destroy");
                panels[panelId] = undefined;
            }
        }
    };

    function collapsePanel(target, region, animate) {
        if (animate == undefined) {
            animate = "normal";
        }
        var panels = $.data(target, "layout").panels;
        var p = panels[region];
        var opts = p.panel("options");
        if (opts.onBeforeCollapse.call(p) == false) {
            return;
        }
        var regionId = "expand" + region.substring(0, 1).toUpperCase() + region.substring(1);
        if (!panels[regionId]) {
            panels[regionId] = createRegionPanel(region);
            panels[regionId].panel("panel").bind("click", function() {
                p.panel("expand", false).panel("open");
                var regionSize = getRegionSize();
                p.panel("resize", regionSize.collapse);
                p.panel("panel").animate(regionSize.expand, function() {
                    $(this).unbind(".layout").bind("mouseleave.layout", { region: region }, function(e) {
                        if (resizing == true) {
                            return;
                        }
                        if ($("body > div.combo-p > div.combo-panel:visible").length) {
                            return;
                        }
                        collapsePanel(target, e.data.region);
                    });
                });
                return false;
            });
        }
        var regionPanelSize = getRegionSize();
        if (!isVisible(panels[regionId])) {
            panels.center.panel("resize", regionPanelSize.resizeC);
        }
        p.panel("panel").animate(regionPanelSize.collapse, animate, function() {
            p.panel("collapse", false).panel("close");
            panels[regionId].panel("open").panel("resize", regionPanelSize.expandP);
            $(this).unbind(".layout");
        });

        function createRegionPanel(dir) {
            var iconClsText;
            if (dir == "east") {
                iconClsText = "layout-button-left";
            } else {
                if (dir == "west") {
                    iconClsText = "layout-button-right";
                } else {
                    if (dir == "north") {
                        iconClsText = "layout-button-down";
                    } else {
                        if (dir == "south") {
                            iconClsText = "layout-button-up";
                        }
                    }
                }
            }
            var p = $("<div></div>").appendTo(target);
            p.panel($.extend({}, $.fn.layout.paneldefaults, {
                cls: ("layout-expand layout-expand-" + dir),
                title: "&nbsp;",
                closed: true,
                minWidth: 0,
                minHeight: 0,
                doSize: false,
                tools: [
                    {
                        iconCls: iconClsText,
                        handler: function() {
                            expandPanel(target, region);
                            return false;
                        }
                    }
                ]
            }));
            p.panel("panel").hover(function() {
                $(this).addClass("layout-expand-over");
            }, function() {
                $(this).removeClass("layout-expand-over");
            });
            return p;
        };

        function getRegionSize() {
            var cc = $(target);
            var panelOpts = panels.center.panel("options");
            var collapsedSize = opts.collapsedSize;
            if (region == "east") {
                var panelWidth = p.panel("panel")._outerWidth();
                var width = panelOpts.width + panelWidth - collapsedSize;
                if (opts.split || !opts.border) {
                    width++;
                }
                return { resizeC: { width: width }, expand: { left: cc.width() - panelWidth }, expandP: { top: panelOpts.top, left: cc.width() - collapsedSize, width: collapsedSize, height: panelOpts.height }, collapse: { left: cc.width(), top: panelOpts.top, height: panelOpts.height } };
            } else {
                if (region == "west") {
                    var panelWidth = p.panel("panel")._outerWidth();
                    var width = panelOpts.width + panelWidth - collapsedSize;
                    if (opts.split || !opts.border) {
                        width++;
                    }
                    return { resizeC: { width: width, left: collapsedSize - 1 }, expand: { left: 0 }, expandP: { left: 0, top: panelOpts.top, width: collapsedSize, height: panelOpts.height }, collapse: { left: -panelWidth, top: panelOpts.top, height: panelOpts.height } };
                } else {
                    if (region == "north") {
                        var pwidth = p.panel("panel")._outerHeight();
                        var hh = panelOpts.height;
                        if (!isVisible(panels.expandNorth)) {
                            hh += pwidth - collapsedSize + ((opts.split || !opts.border) ? 1 : 0);
                        }
                        panels.east.add(panels.west).add(panels.expandEast).add(panels.expandWest).panel("resize", { top: collapsedSize - 1, height: hh });
                        return { resizeC: { top: collapsedSize - 1, height: hh }, expand: { top: 0 }, expandP: { top: 0, left: 0, width: cc.width(), height: collapsedSize }, collapse: { top: -pwidth, width: cc.width() } };
                    } else {
                        if (region == "south") {
                            var pwidth = p.panel("panel")._outerHeight();
                            var hh = panelOpts.height;
                            if (!isVisible(panels.expandSouth)) {
                                hh += pwidth - collapsedSize + ((opts.split || !opts.border) ? 1 : 0);
                            }
                            panels.east.add(panels.west).add(panels.expandEast).add(panels.expandWest).panel("resize", { height: hh });
                            return { resizeC: { height: hh }, expand: { top: cc.height() - pwidth }, expandP: { top: cc.height() - collapsedSize, left: 0, width: cc.width(), height: collapsedSize }, collapse: { top: cc.height(), width: cc.width() } };
                        }
                    }
                }
            }
        };
    };

    function expandPanel(target, region) {
        var panels = $.data(target, "layout").panels;
        var p = panels[region];
        var opts = p.panel("options");
        if (opts.onBeforeExpand.call(p) == false) {
            return;
        }
        var regionId = "expand" + region.substring(0, 1).toUpperCase() + region.substring(1);
        if (panels[regionId]) {
            panels[regionId].panel("close");
            p.panel("panel").stop(true, true);
            p.panel("expand", false).panel("open");
            var panelSize = getRegionSize();
            p.panel("resize", panelSize.collapse);
            p.panel("panel").animate(panelSize.expand, function() {
                setSize(target);
            });
        }

        function getRegionSize() {
            var cc = $(target);
            var centerPanelOpts = panels.center.panel("options");
            if (region == "east" && panels.expandEast) {
                return { collapse: { left: cc.width(), top: centerPanelOpts.top, height: centerPanelOpts.height }, expand: { left: cc.width() - p.panel("panel")._outerWidth() } };
            } else {
                if (region == "west" && panels.expandWest) {
                    return { collapse: { left: -p.panel("panel")._outerWidth(), top: centerPanelOpts.top, height: centerPanelOpts.height }, expand: { left: 0 } };
                } else {
                    if (region == "north" && panels.expandNorth) {
                        return { collapse: { top: -p.panel("panel")._outerHeight(), width: cc.width() }, expand: { top: 0 } };
                    } else {
                        if (region == "south" && panels.expandSouth) {
                            return { collapse: { top: cc.height(), width: cc.width() }, expand: { top: cc.height() - p.panel("panel")._outerHeight() } };
                        }
                    }
                }
            }
        };
    };

    function isVisible(pp) {
        if (!pp) {
            return false;
        }
        if (pp.length) {
            return pp.panel("panel").is(":visible");
        } else {
            return false;
        }
    };

    function setInitState(target) {
        var panels = $.data(target, "layout").panels;
        setState("east");
        setState("west");
        setState("north");
        setState("south");

        function setState(dir) {
            var p = panels[dir];
            if (p.length && p.panel("options").collapsed) {
                collapsePanel(target, dir, 0);
            }
        };
    };

    function doSplit(target, region, splitState) {
        var p = $(target).layout("panel", region);
        p.panel("options").split = splitState;
        var cls = "layout-split-" + region;
        var panel = p.panel("panel").removeClass(cls);
        if (splitState) {
            panel.addClass(cls);
        }
        panel.resizable({ disabled: (!splitState) });
        setSize(target);
    };

    $.fn.layout = function(options, param) {
        if (typeof options == "string") {
            return $.fn.layout.methods[options](this, param);
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "layout");
            if (state) {
                $.extend(state.options, options);
            } else {
                var opts = $.extend({}, $.fn.layout.defaults, $.fn.layout.parseOptions(this), options);
                $.data(this, "layout", { options: opts, panels: { center: $(), north: $(), south: $(), east: $(), west: $() } });
                init(this);
            }
            setSize(this);
            setInitState(this);
        });
    };

    $.fn.layout.methods = {
        options: function(jq) {
            return $.data(jq[0], "layout").options;
        },
        resize: function(jq, param) {
            return jq.each(function() {
                setSize(this, param);
            });
        },
        panel: function(jq, region) {
            return $.data(jq[0], "layout").panels[region];
        },
        collapse: function(jq, region) {
            return jq.each(function() {
                collapsePanel(this, region);
            });
        },
        expand: function(jq, region) {
            return jq.each(function() {
                expandPanel(this, region);
            });
        },
        add: function(jq, options) {
            return jq.each(function() {
                createPanel(this, options);
                setSize(this);
                if ($(this).layout("panel", options.region).panel("options").collapsed) {
                    collapsePanel(this, options.region, 0);
                }
            });
        },
        remove: function(jq, region) {
            return jq.each(function() {
                removePanel(this, region);
                setSize(this);
            });
        },
        split: function(jq, region) {
            return jq.each(function() {
                doSplit(this, region, true);
            });
        },
        unsplit: function(jq, region) {
            return jq.each(function() {
                doSplit(this, region, false);
            });
        }
    };

    $.fn.layout.parseOptions = function(target) {
        return $.extend({}, $.parser.parseOptions(target, [{ fit: "boolean" }]));
    };

    $.fn.layout.defaults = { fit: false };

    $.fn.layout.parsePanelOptions = function(target) {
        var t = $(target);
        return $.extend({}, $.fn.panel.parseOptions(target), $.parser.parseOptions(target,
        [
            "region", {
                split: "boolean",
                collpasedSize: "number",
                minWidth: "number",
                minHeight: "number",
                maxWidth: "number",
                maxHeight: "number"
            }
        ]));
    };

    $.fn.layout.paneldefaults = $.extend({}, $.fn.panel.defaults, {
        region: null,
        split: false,
        collapsedSize: 28,
        minWidth: 10,
        minHeight: 10,
        maxWidth: 10000,
        maxHeight: 10000
    });
})(jQuery);