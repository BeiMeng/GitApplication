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
    function init(target) {
        var opts = $.data(target, "menubutton").options;
        var btn = $(target);
        btn.linkbutton(opts);
        btn.removeClass(opts.cls.btn1 + " " + opts.cls.btn2).addClass("m-btn");
        btn.removeClass("m-btn-small m-btn-medium m-btn-large").addClass("m-btn-" + opts.size);
        var leftBtn = btn.find(".l-btn-left");
        $("<span></span>").addClass(opts.cls.arrow).appendTo(leftBtn);
        $("<span></span>").addClass("m-btn-line").appendTo(leftBtn);
        if (opts.menu) {
            $(opts.menu).menu({ duration: opts.duration });
            var menuOpts = $(opts.menu).menu("options");
            var menuOnShow = menuOpts.onShow;
            var menuOnHide = menuOpts.onHide;
            $.extend(menuOpts, {
                onShow: function() {
                    var menuItemOpts = $(this).menu("options");
                    var sideItem = $(menuItemOpts.alignTo);
                    var sideItemOpts = sideItem.menubutton("options");
                    sideItem.addClass((sideItemOpts.plain == true) ? sideItemOpts.cls.btn2 : sideItemOpts.cls.btn1);
                    menuOnShow.call(this);
                },
                onHide: function() {
                    var itemOpts = $(this).menu("options");
                    var side = $(itemOpts.alignTo);
                    var sideOpts = side.menubutton("options");
                    side.removeClass((sideOpts.plain == true) ? sideOpts.cls.btn2 : sideOpts.cls.btn1);
                    menuOnHide.call(this);
                }
            });
        }
    };

    function bindEvents(target) {
        var opts = $.data(target, "menubutton").options;
        var btn = $(target);
        var t = btn.find("." + opts.cls.trigger);
        if (!t.length) {
            t = btn;
        }
        t.unbind(".menubutton");
        var to = null;
        t.bind("click.menubutton", function() {
            if (!isDisabled()) {
                show(target);
                return false;
            }
        }).bind("mouseenter.menubutton", function() {
            if (!isDisabled()) {
                to = setTimeout(function() {
                    show(target);
                }, opts.duration);
                return false;
            }
        }).bind("mouseleave.menubutton", function() {
            if (to) {
                clearTimeout(to);
            }
            $(opts.menu).triggerHandler("mouseleave");
        });

        function isDisabled() {
            return $(target).linkbutton("options").disabled;
        };
    };

    function show(target) {
        var opts = $(target).menubutton("options");
        if (opts.disabled || !opts.menu) {
            return;
        }
        $("body > div.menu-top").menu("hide");
        var btn = $(target);
        var mm = $(opts.menu);
        if (mm.length) {
            mm.menu("options").alignTo = btn;
            mm.menu("show", { alignTo: btn, align: opts.menuAlign });
        }
        btn.blur();
    };

    $.fn.menubutton = function(options, param) {
        if (typeof options == "string") {
            var method = $.fn.menubutton.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.linkbutton(options, param);
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "menubutton");
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, "menubutton", { options: $.extend({}, $.fn.menubutton.defaults, $.fn.menubutton.parseOptions(this), options) });
                $(this).removeAttr("disabled");
            }
            init(this);
            bindEvents(this);
        });
    };

    $.fn.menubutton.methods = {
        options: function(jq) {
            var opts = jq.linkbutton("options");
            return $.extend($.data(jq[0], "menubutton").options, { toggle: opts.toggle, selected: opts.selected, disabled: opts.disabled });
        },
        destroy: function(jq) {
            return jq.each(function() {
                var opts = $(this).menubutton("options");
                if (opts.menu) {
                    $(opts.menu).menu("destroy");
                }
                $(this).remove();
            });
        }
    };

    $.fn.menubutton.parseOptions = function(target) {
        var t = $(target);
        return $.extend({}, $.fn.linkbutton.parseOptions(target), $.parser.parseOptions(target, ["menu", { plain: "boolean", duration: "number" }]));
    };

    $.fn.menubutton.defaults = $.extend({}, $.fn.linkbutton.defaults, { plain: true, menu: null, menuAlign: "left", duration: 100, cls: { btn1: "m-btn-active", btn2: "m-btn-plain-active", arrow: "m-btn-downarrow", trigger: "m-btn" } });
})(jQuery);