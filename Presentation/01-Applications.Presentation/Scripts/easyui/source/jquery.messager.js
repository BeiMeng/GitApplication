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
    function bindEvent() {
        $(document).unbind(".messager").bind("keydown.messager", function(e) {
            if (e.keyCode == 27) {
                $("body").children("div.messager-window").children("div.messager-body").each(function() {
                    $(this).window("close");
                });
            } else {
                if (e.keyCode == 9) {
                    var body = $("body").children("div.messager-window").children("div.messager-body");
                    if (!body.length) {
                        return;
                    }
                    var buttons = body.find(".messager-input, .messager-button .l-btn");
                    for (var i = 0; i < buttons.length; i++) {
                        if ($(buttons[i]).is(":focus")) {
                            $(buttons[i >= buttons.length - 1 ? 0 : i + 1]).focus();
                            return false;
                        }
                    }
                }
            }
        });
    };

    function unbindMessager() {
        $(document).unbind(".messager");
    };

    function createWindow(options) {
        var opts = $.extend({}, $.messager.defaults, {
            modal: false,
            shadow: false,
            draggable: false,
            resizable: false,
            closed: true,
            style: { left: "", top: "", right: 0, zIndex: $.fn.window.defaults.zIndex++, bottom: -document.body.scrollTop - document.documentElement.scrollTop },
            title: "",
            width: 250,
            height: 100,
            showType: "slide",
            showSpeed: 600,
            msg: "",
            timeout: 4000
        }, options);

        var win = $("<div class=\"messager-body\"></div>").html(opts.msg).appendTo("body");

        win.window($.extend({}, opts, {
            openAnimation: (opts.showType),
            closeAnimation: (opts.showType == "show" ? "hide" : opts.showType),
            openDuration: opts.showSpeed,
            closeDuration: opts.showSpeed,
            onOpen: function() {
                win.window("window").hover(function() {
                    if (opts.timer) {
                        clearTimeout(opts.timer);
                    }
                }, function() {
                    close();
                });
                close();

                function close() {
                    if (opts.timeout > 0) {
                        opts.timer = setTimeout(function() {
                            if (win.length && win.data("window")) {
                                win.window("close");
                            }
                        }, opts.timeout);
                    }
                };

                if (options.onOpen) {
                    options.onOpen.call(this);
                } else {
                    opts.onOpen.call(this);
                }
            },
            onClose: function() {
                if (opts.timer) {
                    clearTimeout(opts.timer);
                }
                if (options.onClose) {
                    options.onClose.call(this);
                } else {
                    opts.onClose.call(this);
                }
                win.window("destroy");
            }
        }));
        win.window("window").css(opts.style);
        win.window("open");
        return win;
    };

    function createDialog(options) {
        bindEvent();
        var win = $("<div class=\"messager-body\"></div>").appendTo("body");
        win.window($.extend({}, options, {
            doSize: false,
            noheader: (options.title ? false : true),
            onClose: function() {
                unbindMessager();
                if (options.onClose) {
                    options.onClose.call(this);
                }
                setTimeout(function() {
                    win.window("destroy");
                }, 100);
            }
        }));
        if (options.buttons && options.buttons.length) {
            var tb = $("<div class=\"messager-button\"></div>").appendTo(win);
            $.map(options.buttons, function(button) {
                $("<a href=\"javascript:void(0)\" style=\"margin-left:10px\"></a>").appendTo(tb).linkbutton(button);
            });
        }
        win.window("window").addClass("messager-window");
        win.window("resize");
        win.children("div.messager-button").children("a:first").focus();
        return win;
    };

    $.messager = {
        show: function(options) {
            return createWindow(options);
        },
        alert: function(title, msg, icon, fn) {
            var options = typeof title == "object" ? title : { title: title, msg: msg, icon: icon, fn: fn };
            var cls = options.icon ? "messager-icon messager-" + options.icon : "";
            options = $.extend({}, $.messager.defaults, {
                content: "<div class=\"" + cls + "\"></div>" + "<div>" + options.msg + "</div>" + "<div style=\"clear:both;\"/>",
                buttons: [
                    {
                        text: $.messager.defaults.ok,
                        onClick: function() {
                            win.window("close");
                            options.fn();
                        }
                    }
                ]
            }, options);
            var win = createDialog(options);
            return win;
        },
        confirm: function(title, msg, fn) {
            var options = typeof title == "object" ? title : { title: title, msg: msg, fn: fn };
            options = $.extend({}, $.messager.defaults, {
                content: "<div class=\"messager-icon messager-question\"></div>" + "<div>" + options.msg + "</div>" + "<div style=\"clear:both;\"/>",
                buttons: [
                    {
                        text: $.messager.defaults.ok,
                        onClick: function() {
                            win.window("close");
                            options.fn(true);
                        }
                    }, {
                        text: $.messager.defaults.cancel,
                        onClick: function() {
                            win.window("close");
                            options.fn(false);
                        }
                    }
                ]
            }, options);
            var win = createDialog(options);
            return win;
        },
        prompt: function(title, msg, fn) {
            var options = typeof title == "object" ? title : { title: title, msg: msg, fn: fn };
            options = $.extend({}, $.messager.defaults, {
                content: "<div class=\"messager-icon messager-question\"></div>" + "<div>" + options.msg + "</div>" + "<br/>" + "<div style=\"clear:both;\"/>" + "<div><input class=\"messager-input\" type=\"text\"/></div>",
                buttons: [
                    {
                        text: $.messager.defaults.ok,
                        onClick: function() {
                            win.window("close");
                            options.fn(win.find(".messager-input").val());
                        }
                    }, {
                        text: $.messager.defaults.cancel,
                        onClick: function() {
                            win.window("close");
                            options.fn();
                        }
                    }
                ]
            }, options);
            var win = createDialog(options);
            win.find("input.messager-input").focus();
            return win;
        },
        progress: function(options) {
            var methods = {
                bar: function() {
                    return $("body > div.messager-window").find("div.messager-p-bar");
                },
                close: function() {
                    var win = $("body > div.messager-window > div.messager-body:has(div.messager-progress)");
                    if (win.length) {
                        win.window("close");
                    }
                }
            };
            if (typeof options == "string") {
                var method = methods[options];
                return method();
            }
            var opts = $.extend({}, { title: "", content: undefined, msg: "", text: undefined, interval: 300 }, options || {});
            var win = createDialog($.extend({}, $.messager.defaults, { content: "<div class=\"messager-progress\"><div class=\"messager-p-msg\">" + opts.msg + "</div><div class=\"messager-p-bar\"></div></div>", closable: false, doSize: false }, opts, {
                onClose: function() {
                    if (this.timer) {
                        clearInterval(this.timer);
                    }
                    if (options.onClose) {
                        options.onClose.call(this);
                    } else {
                        $.messager.defaults.onClose.call(this);
                    }
                }
            }));
            var bar = win.find("div.messager-p-bar");
            bar.progressbar({ text: opts.text });
            win.window("resize");
            if (opts.interval) {
                win[0].timer = setInterval(function() {
                    var v = bar.progressbar("getValue");
                    v += 10;
                    if (v > 100) {
                        v = 0;
                    }
                    bar.progressbar("setValue", v);
                }, opts.interval);
            }
            return win;
        }
    };

    $.messager.defaults = $.extend({}, $.fn.window.defaults, {
        ok: "Ok",
        cancel: "Cancel",
        width: 300,
        height: "auto",
        modal: true,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        resizable: false,
        fn: function() {
        }
    });
})(jQuery);