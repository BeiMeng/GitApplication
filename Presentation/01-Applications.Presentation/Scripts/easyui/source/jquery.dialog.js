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
    function buildDialog(target) {
        var opts = $.data(target, "dialog").options;
        opts.inited = false;
        $(target).window($.extend({}, opts, {
            onResize: function(w, h) {
                if (opts.inited) {
                    setWindow(this);
                    opts.onResize.call(this, w, h);
                }
            }
        }));
        var contentWindow = $(target).window("window");
        if (opts.toolbar) {
            if ($.isArray(opts.toolbar)) {
                $(target).siblings("div.dialog-toolbar").remove();
                var toolbar = $("<div class=\"dialog-toolbar\"><table cellspacing=\"0\" cellpadding=\"0\"><tr></tr></table></div>").appendTo(contentWindow);
                var tr = toolbar.find("tr");
                for (var i = 0; i < opts.toolbar.length; i++) {
                    var toolbarItem = opts.toolbar[i];
                    if (toolbarItem == "-") {
                        $("<td><div class=\"dialog-tool-separator\"></div></td>").appendTo(tr);
                    } else {
                        var td = $("<td></td>").appendTo(tr);
                        var tool = $("<a href=\"javascript:void(0)\"></a>").appendTo(td);
                        tool[0].onclick = eval(toolbarItem.handler || function() {});
                        tool.linkbutton($.extend({}, toolbarItem, { plain: true }));
                    }
                }
            } else {
                $(opts.toolbar).addClass("dialog-toolbar").appendTo(contentWindow);
                $(opts.toolbar).show();
            }
        } else {
            $(target).siblings("div.dialog-toolbar").remove();
        }

        if (opts.buttons) {
            if ($.isArray(opts.buttons)) {
                $(target).siblings("div.dialog-button").remove();
                var buttons = $("<div class=\"dialog-button\"></div>").appendTo(contentWindow);
                for (var i = 0; i < opts.buttons.length; i++) {
                    var p = opts.buttons[i];
                    var button = $("<a href=\"javascript:void(0)\"></a>").appendTo(buttons);
                    if (p.handler) {
                        button[0].onclick = p.handler;
                    }
                    button.linkbutton(p);
                }
            } else {
                $(opts.buttons).addClass("dialog-button").appendTo(contentWindow);
                $(opts.buttons).show();
            }
        } else {
            $(target).siblings("div.dialog-button").remove();
        }

        opts.inited = true;
        var isClosed = opts.closed;
        contentWindow.show();
        $(target).window("resize");
        if (isClosed) {
            contentWindow.hide();
        }
    };

    function setWindow(target, _c) {
        var t = $(target);
        var opts = t.dialog("options");
        var isNoheader = opts.noheader;
        var tb = t.siblings(".dialog-toolbar");
        var bb = t.siblings(".dialog-button");
        tb.insertBefore(target).css({ position: "relative", borderTopWidth: (isNoheader ? 1 : 0), top: (isNoheader ? tb.length : 0) });
        bb.insertAfter(target).css({ position: "relative", top: -1 });
        tb.add(bb)._outerWidth(t._outerWidth()).find(".easyui-fluid:visible").each(function () {
            $(this).triggerHandler("_resize");
        });
        if (!isNaN(parseInt(opts.height))) {
            t._outerHeight(t._outerHeight() - tb._outerHeight() - bb._outerHeight());
        }
        
        var isShadow = $.data(target, "window").shadow;
        if (isShadow) {
            var cc = t.panel("panel");
            isShadow.css({ width: cc._outerWidth(), height: cc._outerHeight() });
        }
    };

    $.fn.dialog = function(options, param) {
        if (typeof options == "string") {
            var method = $.fn.dialog.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.window(options, param);
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "dialog");
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, "dialog", { options: $.extend({}, $.fn.dialog.defaults, $.fn.dialog.parseOptions(this), options) });
            }
            buildDialog(this);
        });
    };
    
    $.fn.dialog.methods = {
        options: function(jq) {
            var opts = $.data(jq[0], "dialog").options;
            var panelOpt = jq.panel("options");
            $.extend(opts, { width: panelOpt.width, height: panelOpt.height, left: panelOpt.left, top: panelOpt.top, closed: panelOpt.closed, collapsed: panelOpt.collapsed, minimized: panelOpt.minimized, maximized: panelOpt.maximized });
            return opts;
        },
        dialog: function(jq) {
            return jq.window("window");
        }
    };

    $.fn.dialog.parseOptions = function (target) {
        var t = $(target);
        return $.extend({}, $.fn.window.parseOptions(target), $.parser.parseOptions(target, ["toolbar", "buttons"]), { toolbar: (t.children(".dialog-toolbar").length ? t.children(".dialog-toolbar").removeClass("dialog-toolbar") : undefined), buttons: (t.children(".dialog-button").length ? t.children(".dialog-button").removeClass("dialog-button") : undefined) });
    };
    
    $.fn.dialog.defaults = $.extend({}, $.fn.window.defaults, { title: "New Dialog", collapsible: false, minimizable: false, maximizable: false, resizable: false, toolbar: null, buttons: null });
})(jQuery);