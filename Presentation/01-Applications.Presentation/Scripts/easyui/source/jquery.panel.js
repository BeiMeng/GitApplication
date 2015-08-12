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
    $.fn._remove = function() {
        return this.each(function() {
            $(this).remove();
            try {
                this.outerHTML = "";
            } catch (err) {
            }
        });
    };

    function removeNode(target) {
        target._remove();
    };

    function setSize(target, options) {
        var state = $.data(target, "panel");
        var opts = state.options;
        var panel = state.panel;
        var header = panel.children(".panel-header");
        var body = panel.children(".panel-body");
        var footer = panel.children(".panel-footer");
        if (options) {
            $.extend(opts, { width: options.width, height: options.height, minWidth: options.minWidth, maxWidth: options.maxWidth, minHeight: options.minHeight, maxHeight: options.maxHeight, left: options.left, top: options.top });
        }
        panel._size(opts);
        header.add(body)._outerWidth(panel.width());
        if (!isNaN(parseInt(opts.height))) {
            body._outerHeight(panel.height() - header._outerHeight() - footer._outerHeight());
        } else {
            body.css("height", "");
            var minHeight = $.parser.parseValue("minHeight", opts.minHeight, panel.parent());
            var maxHeight = $.parser.parseValue("maxHeight", opts.maxHeight, panel.parent());
            var height = header._outerHeight() + footer._outerHeight() + panel._outerHeight() - panel.height();
            body._size("minHeight", minHeight ? (minHeight - height) : "");
            body._size("maxHeight", maxHeight ? (maxHeight - height) : "");
        }
        panel.css({ height: "", minHeight: "", maxHeight: "", left: opts.left, top: opts.top });
        opts.onResize.apply(target, [opts.width, opts.height]);
        $(target).panel("doLayout");
    };

    function movePanel(target, options) {
        var opts = $.data(target, "panel").options;
        var panel = $.data(target, "panel").panel;
        if (options) {
            if (options.left != null) {
                opts.left = options.left;
            }
            if (options.top != null) {
                opts.top = options.top;
            }
        }
        panel.css({ left: opts.left, top: opts.top });
        opts.onMove.apply(target, [opts.left, opts.top]);
    };

    function wrapPanel(target) {
        $(target).addClass("panel-body")._size("clear");
        var panel = $("<div class=\"panel\"></div>").insertBefore(target);
        panel[0].appendChild(target);
        panel.bind("_resize", function(e, options) {
            if ($(this).hasClass("easyui-fluid") || options) {
                setSize(target);
            }
            return false;
        });
        return panel;
    };

    function renderPanel(target) {
        var state = $.data(target, "panel");
        var opts = state.options;
        var panel = state.panel;
        panel.css(opts.style);
        panel.addClass(opts.cls);
        addHeader();
        addFooter();
        var header = $(target).panel("header");
        var pbody = $(target).panel("body");
        var footer = $(target).siblings(".panel-footer");
        if (opts.border) {
            header.removeClass("panel-header-noborder");
            pbody.removeClass("panel-body-noborder");
            footer.removeClass("panel-footer-noborder");
        } else {
            header.addClass("panel-header-noborder");
            pbody.addClass("panel-body-noborder");
            footer.addClass("panel-footer-noborder");
        }
        header.addClass(opts.headerCls);
        pbody.addClass(opts.bodyCls);
        $(target).attr("id", opts.id || "");
        if (opts.content) {
            $(target).panel("clear");
            $(target).html(opts.content);
            $.parser.parse($(target));
        }

        function addHeader() {
            if (opts.noheader || (!opts.title && !opts.header)) {
                removeNode(panel.children(".panel-header"));
                panel.children(".panel-body").addClass("panel-body-noheader");
            } else {
                if (opts.header) {
                    $(opts.header).addClass("panel-header").prependTo(panel);
                } else {
                    var pheader = panel.children(".panel-header");
                    if (!pheader.length) {
                        pheader = $("<div class=\"panel-header\"></div>").prependTo(panel);
                    }
                    if (!$.isArray(opts.tools)) {
                        pheader.find("div.panel-tool .panel-tool-a").appendTo(opts.tools);
                    }
                    pheader.empty();
                    var title = $("<div class=\"panel-title\"></div>").html(opts.title).appendTo(pheader);
                    if (opts.iconCls) {
                        title.addClass("panel-with-icon");
                        $("<div class=\"panel-icon\"></div>").addClass(opts.iconCls).appendTo(pheader);
                    }
                    var tool = $("<div class=\"panel-tool\"></div>").appendTo(pheader);
                    tool.bind("click", function(e) {
                        e.stopPropagation();
                    });
                    if (opts.tools) {
                        if ($.isArray(opts.tools)) {
                            $.map(opts.tools, function(t) {
                                collapsed(tool, t.iconCls, eval(t.handler));
                            });
                        } else {
                            $(opts.tools).children().each(function() {
                                $(this).addClass($(this).attr("iconCls")).addClass("panel-tool-a").appendTo(tool);
                            });
                        }
                    }
                    if (opts.collapsible) {
                        collapsed(tool, "panel-tool-collapse", function () {
                            if (opts.collapsed == true) {
                                expandPanel(target, true);
                            } else {
                                collapsePanel(target, true);
                            }
                        });
                    }
                    if (opts.minimizable) {
                        collapsed(tool, "panel-tool-min", function() {
                            minimizePanel(target);
                        });
                    }
                    if (opts.maximizable) {
                        collapsed(tool, "panel-tool-max", function() {
                            if (opts.maximized == true) {
                                restorePanel(target);
                            } else {
                                maximizePanel(target);
                            }
                        });
                    }
                    if (opts.closable) {
                        collapsed(tool, "panel-tool-close", function() {
                            closePanel(target);
                        });
                    }
                }
                panel.children("div.panel-body").removeClass("panel-body-noheader");
            }
        };

        function collapsed(c, iconcls, fn) {
            var a = $("<a href=\"javascript:void(0)\"></a>").addClass(iconcls).appendTo(c);
            a.bind("click", fn);
        };

        function addFooter() {
            if (opts.footer) {
                $(opts.footer).addClass("panel-footer").appendTo(panel);
                $(target).addClass("panel-body-nobottom");
            } else {
                panel.children(".panel-footer").remove();
                $(target).removeClass("panel-body-nobottom");
            }
        };
    };

    function loadData(target, queryParams) {
        var state = $.data(target, "panel");
        var opts = state.options;
        if (params) {
            opts.queryParams = queryParams;
        }
        if (!opts.href) {
            return;
        }
        if (!state.isLoaded || !opts.cache) {
            var params = $.extend({}, opts.queryParams);
            if (opts.onBeforeLoad.call(target, params) == false) {
                return;
            }
            state.isLoaded = false;
            $(target).panel("clear");
            if (opts.loadingMessage) {
                $(target).html($("<div class=\"panel-loading\"></div>").html(opts.loadingMessage));
            }
            opts.loader.call(target, params, function(data) {
                var result = opts.extractor.call(target, data);
                $(target).html(result);
                $.parser.parse($(target));
                opts.onLoad.apply(target, arguments);
                state.isLoaded = true;
            }, function() {
                opts.onLoadError.apply(target, arguments);
            });
        }
    };

    function clear(target) {
        var t = $(target);
        t.find(".combo-f").each(function() {
            $(this).combo("destroy");
        });
        t.find(".m-btn").each(function() {
            $(this).menubutton("destroy");
        });
        t.find(".s-btn").each(function() {
            $(this).splitbutton("destroy");
        });
        t.find(".tooltip-f").each(function() {
            $(this).tooltip("destroy");
        });
        t.children("div").each(function() {
            $(this)._size("unfit");
        });
        t.empty();
    };

    function init(target) {
        $(target).panel("doLayout", true);
    };

    function open(target, forceOpen) {
        var opts = $.data(target, "panel").options;
        var state = $.data(target, "panel").panel;
        if (forceOpen != true) {
            if (opts.onBeforeOpen.call(target) == false) {
                return;
            }
        }
        state.stop(true, true);
        if ($.isFunction(opts.openAnimation)) {
            opts.openAnimation.call(target, cb);
        } else {
            switch (opts.openAnimation) {
            case "slide":
                state.slideDown(opts.openDuration, cb);
                break;
            case "fade":
                state.fadeIn(opts.openDuration, cb);
                break;
            case "show":
                state.show(opts.openDuration, cb);
                break;
            default:
                state.show();
                cb();
            }
        }

        function cb() {
            opts.closed = false;
            opts.minimized = false;
            var header = state.children(".panel-header").find("a.panel-tool-restore");
            if (header.length) {
                opts.maximized = true;
            }
            opts.onOpen.call(target);
            if (opts.maximized == true) {
                opts.maximized = false;
                maximizePanel(target);
            }
            if (opts.collapsed == true) {
                opts.collapsed = false;
                collapsePanel(target);
            }
            if (!opts.collapsed) {
                loadData(target);
                init(target);
            }
        };
    };

    function closePanel(target, forceClose) {
        var opts = $.data(target, "panel").options;
        var panel = $.data(target, "panel").panel;
        if (forceClose != true) {
            if (opts.onBeforeClose.call(target) == false) {
                return;
            }
        }
        panel.stop(true, true);
        panel._size("unfit");
        if ($.isFunction(opts.closeAnimation)) {
            opts.closeAnimation.call(target, cb);
        } else {
            switch (opts.closeAnimation) {
            case "slide":
                panel.slideUp(opts.closeDuration, cb);
                break;
            case "fade":
                panel.fadeOut(opts.closeDuration, cb);
                break;
            case "hide":
                panel.hide(opts.closeDuration, cb);
                break;
            default:
                panel.hide();
                cb();
            }
        }

        function cb() {
            opts.closed = true;
            opts.onClose.call(target);
        };
    };

    function destroyPanel(target, forceDestroy) {
        var state = $.data(target, "panel");
        var opts = state.options;
        var panel = state.panel;
        if (forceDestroy != true) {
            if (opts.onBeforeDestroy.call(target) == false) {
                return;
            }
        }
        $(target).panel("clear").panel("clear", "footer");
        removeNode(panel);
        opts.onDestroy.call(target);
    };

    function collapsePanel(target, animate) {
        var opts = $.data(target, "panel").options;
        var panel = $.data(target, "panel").panel;
        var pbody = panel.children(".panel-body");
        var header = panel.children(".panel-header").find("a.panel-tool-collapse");
        if (opts.collapsed == true) {
            return;
        }
        pbody.stop(true, true);
        if (opts.onBeforeCollapse.call(target) == false) {
            return;
        }
        header.addClass("panel-tool-expand");
        if (animate == true) {
            pbody.slideUp("normal", function() {
                opts.collapsed = true;
                opts.onCollapse.call(target);
            });
        } else {
            pbody.hide();
            opts.collapsed = true;
            opts.onCollapse.call(target);
        }
    };

    function expandPanel(target, animate) {
        var opts = $.data(target, "panel").options;
        var panel = $.data(target, "panel").panel;
        var pbody = panel.children(".panel-body");
        var tool = panel.children(".panel-header").find("a.panel-tool-collapse");
        if (opts.collapsed == false) {
            return;
        }
        pbody.stop(true, true);
        if (opts.onBeforeExpand.call(target) == false) {
            return;
        }
        tool.removeClass("panel-tool-expand");
        if (animate == true) {
            pbody.slideDown("normal", function() {
                opts.collapsed = false;
                opts.onExpand.call(target);
                loadData(target);
                init(target);
            });
        } else {
            pbody.show();
            opts.collapsed = false;
            opts.onExpand.call(target);
            loadData(target);
            init(target);
        }
    };

    function maximizePanel(target) {
        var opts = $.data(target, "panel").options;
        var panel = $.data(target, "panel").panel;
        var tool = panel.children(".panel-header").find("a.panel-tool-max");
        if (opts.maximized == true) {
            return;
        }
        tool.addClass("panel-tool-restore");
        if (!$.data(target, "panel").original) {
            $.data(target, "panel").original = { width: opts.width, height: opts.height, left: opts.left, top: opts.top, fit: opts.fit };
        }
        opts.left = 0;
        opts.top = 0;
        opts.fit = true;
        setSize(target);
        opts.minimized = false;
        opts.maximized = true;
        opts.onMaximize.call(target);
    };

    function minimizePanel(target) {
        var opts = $.data(target, "panel").options;
        var panel = $.data(target, "panel").panel;
        panel._size("unfit");
        panel.hide();
        opts.minimized = true;
        opts.maximized = false;
        opts.onMinimize.call(target);
    };

    function restorePanel(target) {
        var opts = $.data(target, "panel").options;
        var panel = $.data(target, "panel").panel;
        var tool = panel.children(".panel-header").find("a.panel-tool-max");
        if (opts.maximized == false) {
            return;
        }
        panel.show();
        tool.removeClass("panel-tool-restore");
        $.extend(opts, $.data(target, "panel").original);
        setSize(target);
        opts.minimized = false;
        opts.maximized = false;
        $.data(target, "panel").original = null;
        opts.onRestore.call(target);
    };

    function setTitle(target, title) {
        $.data(target, "panel").options.title = title;
        $(target).panel("header").find("div.panel-title").html(title);
    };

    var to = null;
    $(window).unbind(".panel").bind("resize.panel", function() {
        if (to) {
            clearTimeout(to);
        }
        to = setTimeout(function() {
            var _65 = $("body.layout");
            if (_65.length) {
                _65.layout("resize");
                $("body").children(".easyui-fluid:visible").each(function() {
                    $(this).triggerHandler("_resize");
                });
            } else {
                $("body").panel("doLayout");
            }
            to = null;
        }, 100);
    });
    $.fn.panel = function(options, param) {
        if (typeof options == "string") {
            return $.fn.panel.methods[options](this, param);
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "panel");
            var opts;
            if (state) {
                opts = $.extend(state.options, options);
                state.isLoaded = false;
            } else {
                opts = $.extend({}, $.fn.panel.defaults, $.fn.panel.parseOptions(this), options);
                $(this).attr("title", "");
                state = $.data(this, "panel", { options: opts, panel: wrapPanel(this), isLoaded: false });
            }
            renderPanel(this);
            if (opts.doSize == true) {
                state.panel.css("display", "block");
                setSize(this);
            }
            if (opts.closed == true || opts.minimized == true) {
                state.panel.hide();
            } else {
                open(this);
            }
        });
    };
    $.fn.panel.methods = {
        options: function(jq) {
            return $.data(jq[0], "panel").options;
        },
        panel: function(jq) {
            return $.data(jq[0], "panel").panel;
        },
        header: function(jq) {
            return $.data(jq[0], "panel").panel.children(".panel-header");
        },
        footer: function(jq) {
            return jq.panel("panel").children(".panel-footer");
        },
        body: function(jq) {
            return $.data(jq[0], "panel").panel.children(".panel-body");
        },
        setTitle: function(jq, title) {
            return jq.each(function() {
                setTitle(this, title);
            });
        },
        open: function(jq, forceOpen) {
            return jq.each(function() {
                open(this, forceOpen);
            });
        },
        close: function(jq, forceClose) {
            return jq.each(function() {
                closePanel(this, forceClose);
            });
        },
        destroy: function(jq, forceDestroy) {
            return jq.each(function() {
                destroyPanel(this, forceDestroy);
            });
        },
        clear: function(jq, selector) {
            return jq.each(function() {
                clear(selector == "footer" ? $(this).panel("footer") : this);
            });
        },
        refresh: function(jq, href) {
            return jq.each(function() {
                var state = $.data(this, "panel");
                state.isLoaded = false;
                if (href) {
                    if (typeof href == "string") {
                        state.options.href = href;
                    } else {
                        state.options.queryParams = href;
                    }
                }
                loadData(this);
            });
        },
        resize: function(jq, options) {
            return jq.each(function() {
                setSize(this, options);
            });
        },
        doLayout: function(jq, all) {
            return jq.each(function() {
                setChildSize(this, "body");
                setChildSize($(this).siblings(".panel-footer")[0], "footer");

                function setChildSize(target, child) {
                    if (!target) {
                        return;
                    }
                    var isBody = target == $("body")[0];
                    var s = $(target).find("div.panel:visible, div.accordion:visible, div.tabs-container:visible, div.layout:visible, .easyui-fluid:visible").filter(function(_76, el) {
                        var p = $(el).parents(".panel-" + child + ":first");
                        return isBody ? p.length == 0 : p[0] == target;
                    });
                    s.each(function() {
                        $(this).triggerHandler("_resize", [all || false]);
                    });
                };
            });
        },
        move: function(jq, options) {
            return jq.each(function() {
                movePanel(this, options);
            });
        },
        maximize: function(jq) {
            return jq.each(function() {
                maximizePanel(this);
            });
        },
        minimize: function(jq) {
            return jq.each(function() {
                minimizePanel(this);
            });
        },
        restore: function(jq) {
            return jq.each(function() {
                restorePanel(this);
            });
        },
        collapse: function(jq, animate) {
            return jq.each(function() {
                collapsePanel(this, animate);
            });
        },
        expand: function(jq, animate) {
            return jq.each(function() {
                expandPanel(this, animate);
            });
        }
    };
    $.fn.panel.parseOptions = function(target) {
        var t = $(target);
        var hh = t.children(".panel-header, header");
        var ff = t.children(".panel-footer, footer");
        return $.extend({}, $.parser.parseOptions(target, ["id", "width", "height", "left", "top", "title", "iconCls", "cls", "headerCls", "bodyCls", "tools", "href", "method", "header", "footer", { cache: "boolean", fit: "boolean", border: "boolean", noheader: "boolean" }, { collapsible: "boolean", minimizable: "boolean", maximizable: "boolean" }, { closable: "boolean", collapsed: "boolean", minimized: "boolean", maximized: "boolean", closed: "boolean" }, "openAnimation", "closeAnimation", { openDuration: "number", closeDuration: "number" },]), { loadingMessage: (t.attr("loadingMessage") != undefined ? t.attr("loadingMessage") : undefined), header: (hh.length ? hh.removeClass("panel-header") : undefined), footer: (ff.length ? ff.removeClass("panel-footer") : undefined) });
    };
    $.fn.panel.defaults = {
        id: null,
        title: null,
        iconCls: null,
        width: "auto",
        height: "auto",
        left: null,
        top: null,
        cls: null,
        headerCls: null,
        bodyCls: null,
        style: {},
        cache: true,
        fit: false,
        border: true,
        doSize: true,
        noheader: false,
        content: null,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        closable: false,
        collapsed: false,
        minimized: false,
        maximized: false,
        closed: false,
        openAnimation: false,
        openDuration: 400,
        closeAnimation: false,
        closeDuration: 400,
        tools: null,
        footer: null,
        header: null,
        queryParams: {},
        method: "get",
        href: null,
        loadingMessage: "Loading...",
        loader: function(param, success, error) {
            var opts = $(this).panel("options");
            if (!opts.href) {
                return false;
            }
            $.ajax({
                type: opts.method,
                url: opts.href,
                cache: false,
                data: param,
                dataType: "html",
                success: function(data) {
                    success(data);
                },
                error: function() {
                    error.apply(this, arguments);
                }
            });
        },
        extractor: function(data) {
            var pattern = /<body[^>]*>((.|[\n\r])*)<\/body>/im;
            var result = pattern.exec(data);
            if (result) {
                return result[1];
            } else {
                return data;
            }
        },
        onBeforeLoad: function (param) {
        },
        onLoad: function() {
        },
        onLoadError: function() {
        },
        onBeforeOpen: function() {
        },
        onOpen: function() {
        },
        onBeforeClose: function() {
        },
        onClose: function() {
        },
        onBeforeDestroy: function() {
        },
        onDestroy: function() {
        },
        onResize: function (width, height) {
        },
        onMove: function (left, top) {
        },
        onMaximize: function() {
        },
        onRestore: function() {
        },
        onMinimize: function() {
        },
        onBeforeCollapse: function() {
        },
        onBeforeExpand: function() {
        },
        onCollapse: function() {
        },
        onExpand: function() {
        }
    };
})(jQuery);