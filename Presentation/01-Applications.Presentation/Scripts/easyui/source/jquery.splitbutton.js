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
    /**
	 * 初始化组件
	 * @param {Object} jq
	 */
    function initSplitButton(target) {
        var opts = $.data(target, "splitbutton").options;
        $(target).menubutton(opts);
        $(target).addClass("s-btn");
    };

    $.fn.splitbutton = function(options, param) {
        if (typeof options == "string") {
            var method = $.fn.splitbutton.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.menubutton(options, param);
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "splitbutton");
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, "splitbutton", { options: $.extend({}, $.fn.splitbutton.defaults, $.fn.splitbutton.parseOptions(this), options) });
                $(this).removeAttr("disabled");
            }
            initSplitButton(this);
        });
    };

    $.fn.splitbutton.methods = {
        options: function(jq) {
            var menubuttonOpts = jq.menubutton("options");
            var opts = $.data(jq[0], "splitbutton").options;
            $.extend(opts, {
                disabled: menubuttonOpts.disabled,
                toggle: menubuttonOpts.toggle,
                selected: menubuttonOpts.selected
            });
            return opts;
        }
    };

    $.fn.splitbutton.parseOptions = function(target) {
        var t = $(target);
        return $.extend({},
            $.fn.linkbutton.parseOptions(target),
            $.parser.parseOptions(target, ["menu", { plain: "boolean", duration: "number" }]));
    };

    $.fn.splitbutton.defaults = $.extend({},
        $.fn.linkbutton.defaults,
        {
            plain: true,
            menu: null,
            duration: 100,
            cls: {
                btn1: "m-btn-active s-btn-active",
                btn2: "m-btn-plain-active s-btn-plain-active",
                arrow: "m-btn-downarrow",
                trigger: "m-btn-line"
            }
        });
})(jQuery);