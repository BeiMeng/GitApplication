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
    $(function() {
        $(document).unbind(".combo").bind("mousedown.combo mousewheel.combo", function(e) {
            var p = $(e.target).closest("span.combo, div.combo-p, div.menu");
            if(p.length) {
                onBeforeClose(p);
                return;
            }
            $("body > div.combo-p > div.combo-panel:visible").panel("close");
        });
    });

    /**
	 * 渲染组件
	 * @param {Object} target
	 * @memberOf {TypeName} 
	 * @return {TypeName} 
	 */
    function renderCombo(target) {
        var state = $.data(target, "combo");
        var opts = state.options;
        if(!state.panel) {
            state.panel = $("<div class=\"combo-panel\"></div>").appendTo("body");
            state.panel.panel({
                minWidth: opts.panelMinWidth,
                maxWidth: opts.panelMaxWidth,
                minHeight: opts.panelMinHeight,
                maxHeight: opts.panelMaxHeight,
                doSize: false,
                closed: true,
                cls: "combo-p",
                style: { position: "absolute", zIndex: 10 },
                onOpen: function() {
                    var comboTarget = $(this).panel("options").comboTarget;
                    var comboTargetState = $.data(comboTarget, "combo");
                    if(comboTargetState) {
                        comboTargetState.options.onShowPanel.call(comboTarget);
                    }
                },
                onBeforeClose: function() {
                    onBeforeClose(this);
                },
                onClose: function() {
                    var comboTarget = $(this).panel("options").comboTarget;
                    var comboTargetSate = $(comboTarget).data("combo");
                    if(comboTargetSate) {
                        comboTargetSate.options.onHidePanel.call(comboTarget);
                    }
                }
            });
        }
        var icons = $.extend(true, [], opts.icons);
        if(opts.hasDownArrow) {
            icons.push({
                iconCls: "combo-arrow",//下拉箭头
                handler: function(e) {
                    togglePanel(e.data.target);
                }
            });
        }
        $(target).addClass("combo-f").textbox($.extend({}, opts, {
            icons: icons,
            onChange: function() {
            }
        }));
        $(target).attr("comboName", $(target).attr("textboxName"));
        state.combo = $(target).next();
        state.combo.addClass("combo");
    };
    /**
	 * 销毁Combo
	 * @param {Object} target
	 */
    function destroyCombo(target) {
        var state = $.data(target, "combo");
        var opts = state.options;
        var p = state.panel;
        if(p.is(":visible")) {
            p.panel("close");
        }
        if(!opts.cloned) {
            p.panel("destroy");
        }
        $(target).textbox("destroy");
    };

    function togglePanel(target) {
        var panel = $.data(target, "combo").panel;
        if(panel.is(":visible")) {
            hidePanel(target);
        } else {
            var p = $(target).closest("div.combo-panel");
            $("div.combo-panel:visible").not(panel).not(p).panel("close");
            $(target).combo("showPanel");
        }
        $(target).combo("textbox").focus();
    };

    function onBeforeClose(target) {
        $(target).find(".combo-f").each(function() {
            var p = $(this).combo("panel");
            if(p.is(":visible")) {
                p.panel("close");
            }
        });
    };

    function handleOnClick(e) {
        var target = e.data.target;
        var state = $.data(target, "combo");
        var opts = state.options;
        var panel = state.panel;
        if(!opts.editable) {
            togglePanel(target);
        } else {
            var p = $(target).closest("div.combo-panel");
            $("div.combo-panel:visible").not(panel).not(p).panel("close");
        }
    };

    /**
	 * 绑定事件
	 * @param {Object} e
	 * @memberOf {TypeName} 
	 * @return {TypeName} 
	 */
    function bindEvents(e) {
        var target = e.data.target;
        var $target = $(target);
        var state = $target.data("combo");
        var opts = $target.combo("options");
        switch(e.keyCode) {
        case 38:
            opts.keyHandler.up.call(target, e);
            break;
        case 40:
            opts.keyHandler.down.call(target, e);
            break;
        case 37:
            opts.keyHandler.left.call(target, e);
            break;
        case 39:
            opts.keyHandler.right.call(target, e);
            break;
        case 13:
            e.preventDefault();
            opts.keyHandler.enter.call(target, e);
            return false;
        case 9:
        case 27:
            hidePanel(target);
            break;
        default:
            if(opts.editable) {
                if(state.timer) {
                    clearTimeout(state.timer);
                }
                state.timer = setTimeout(function() {
                    var q = $target.combo("getText");
                    if(state.previousText != q) {
                        state.previousText = q;
                        $target.combo("showPanel");
                        opts.keyHandler.query.call(target, q, e);
                        $target.combo("validate");
                    }
                }, opts.delay);
            }
        }
    };

    function showPanel(target) {
        var state = $.data(target, "combo");
        var combo = state.combo;
        var panel = state.panel;
        var opts = $(target).combo("options");
        var panelOpts = panel.panel("options");
        panelOpts.comboTarget = target;
        if(panelOpts.closed) {
            panel.panel("panel").show().css({ zIndex: ($.fn.menu ? $.fn.menu.defaults.zIndex++ : $.fn.window.defaults.zIndex++), left: -999999 });
            panel.panel("resize", { width: (opts.panelWidth ? opts.panelWidth : combo._outerWidth()), height: opts.panelHeight });
            panel.panel("panel").hide();
            panel.panel("open");
        }
        (function() {
            if(panel.is(":visible")) {
                panel.panel("move", {
                    left: getOffsetLeft(),
                    top: getOffsetTop()
                });
                setTimeout(arguments.callee, 200);
            }
        })();

        /**
		 * 获取Left位置
		 * @return {TypeName} 
		 */
        function getOffsetLeft() {
            var left = combo.offset().left;
            if(opts.panelAlign == "right") {
                left += combo._outerWidth() - panel._outerWidth();
            }
            if(left + panel._outerWidth() > $(window)._outerWidth() + $(document).scrollLeft()) {
                left = $(window)._outerWidth() + $(document).scrollLeft() - panel._outerWidth();
            }
            if(left < 0) {
                left = 0;
            }
            return left;
        };

        /**
		 * 获取TOP位置
		 * @return {TypeName} 
		 */
        function getOffsetTop() {
            var top = combo.offset().top + combo._outerHeight();
            if(top + panel._outerHeight() > $(window)._outerHeight() + $(document).scrollTop()) {
                top = combo.offset().top - panel._outerHeight();
            }
            if(top < $(document).scrollTop()) {
                top = combo.offset().top + combo._outerHeight();
            }
            return top;
        };
    };

    /**
	 * 隐藏下拉选择面板
	 * @param {Object} target
	 */
    function hidePanel(target) {
        var panel = $.data(target, "combo").panel;
        panel.panel("close");
    };

    /**
	 * 设置text值
	 * @param {Object} target
	 * @param {Object} text
	 */
    function setText(target, text) {
        var state = $.data(target, "combo");
        var oldText = $(target).textbox("getText");
        if(oldText != text) {
            $(target).textbox("setText", text);
            state.previousText = text;
        }
    };

    /**
	 * 获取值（多选）
	 * @param {Object} target
	 * @memberOf {TypeName} 
	 * @return {TypeName} 
	 */
    function getValues(target) {
        var values = [];
        var combo = $.data(target, "combo").combo;
        combo.find(".textbox-value").each(function() {
            values.push($(this).val());
        });
        return values;
    };

    /**
	 * 设置值（多选）
	 * @param {Object} target
	 * @param {Object} values
	 */
    function setValues(target, values) {
        var state = $.data(target, "combo");
        var opts = state.options;
        var combo = state.combo;
        if(!$.isArray(values)) {
            values = values.split(opts.separator);
        }
        var oldValues = getValues(target);
        combo.find(".textbox-value").remove();
        var textboxName = $(target).attr("textboxName") || "";
        for(var i = 0; i < values.length; i++) {
            var $textboxHidden = $("<input type=\"hidden\" class=\"textbox-value\">").appendTo(combo);
            $textboxHidden.attr("name", textboxName);
            if(opts.disabled) {
                $textboxHidden.attr("disabled", "disabled");
            }
            $textboxHidden.val(values[i]);
        }
        var isChanged = (function () {
            if(oldValues.length != values.length) {
                return true;
            }
            var a1 = $.extend(true, [], oldValues);
            var a2 = $.extend(true, [], values);
            a1.sort();
            a2.sort();
            for(var i = 0; i < a1.length; i++) {
                if(a1[i] != a2[i]) {
                    return true;
                }
            }
            return false;
        })();
        if(isChanged) {
            if(opts.multiple) {
                opts.onChange.call(target, values, oldValues);
            } else {
                opts.onChange.call(target, values[0], oldValues[0]);
            }
            $(target).closest("form").trigger("_change", [target]);
        }
    };
    /**
	 * 获取值（单选）
	 * @param {Object} target
	 * @return {TypeName} 
	 */
    function getValue(target) {
        var values = getValues(target);
        return values[0];
    };

    /**
	 * 设置值（单选）
	 * @param {Object} target
	 * @param {Object} value
	 */
    function setValue(target, value) {
        setValues(target, [value]);
    };

    /**
	 * 初始化combo值
	 * @param {Object} jq
	 */
    function initValue(target) {
        var opts = $.data(target, "combo").options;
        var onChangeHandler = opts.onChange;
        opts.onChange = function() {
        };
        if(opts.multiple) {
            setValues(target, opts.value ? opts.value : []);
        } else {
            setValue(target, opts.value);
        }
        opts.onChange = onChangeHandler;
    };

    $.fn.combo = function(options, param) {
        if(typeof options == "string") {
            var methods = $.fn.combo.methods[options];
            if(methods) {
                return methods(this, param);
            } else {
                return this.textbox(options, param);
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "combo");
            if(state) {
                $.extend(state.options, options);
                if(options.value != undefined) {
                    state.options.originalValue = options.value;
                }
            } else {
                state = $.data(this, "combo", { options: $.extend({}, $.fn.combo.defaults, $.fn.combo.parseOptions(this), options), previousText: "" });
                state.options.originalValue = state.options.value;
            }
            renderCombo(this);
            initValue(this);
        });
    };

    $.fn.combo.methods = {
        options: function(jq) {
            var opts = jq.textbox("options");
            return $.extend($.data(jq[0], "combo").options, { width: opts.width, height: opts.height, disabled: opts.disabled, readonly: opts.readonly });
        },
        cloneFrom: function(jq, target) {
            return jq.each(function() {
                $(this).textbox("cloneFrom", target);
                $.data(this, "combo", { options: $.extend(true, { cloned: true }, $(target).combo("options")), combo: $(this).next(), panel: $(target).combo("panel") });
                $(this).addClass("combo-f").attr("comboName", $(this).attr("textboxName"));
            });
        },
        panel: function(jq) {
            return $.data(jq[0], "combo").panel;
        },
        destroy: function(jq) {
            return jq.each(function() {
                destroyCombo(this);
            });
        },
        showPanel: function(jq) {
            return jq.each(function() {
                showPanel(this);
            });
        },
        hidePanel: function(jq) {
            return jq.each(function() {
                hidePanel(this);
            });
        },
        clear: function(jq) {
            return jq.each(function() {
                $(this).textbox("setText", "");
                var opts = $.data(this, "combo").options;
                if(opts.multiple) {
                    $(this).combo("setValues", []);
                } else {
                    $(this).combo("setValue", "");
                }
            });
        },
        reset: function(jq) {
            return jq.each(function() {
                var opts = $.data(this, "combo").options;
                if(opts.multiple) {
                    $(this).combo("setValues", opts.originalValue);
                } else {
                    $(this).combo("setValue", opts.originalValue);
                }
            });
        },
        setText: function(jq, text) {
            return jq.each(function() {
                setText(this, text);
            });
        },
        getValues: function(jq) {
            return getValues(jq[0]);
        },
        setValues: function(jq, values) {
            return jq.each(function() {
                setValues(this, values);
            });
        },
        getValue: function(jq) {
            return getValue(jq[0]);
        },
        setValue: function(jq, value) {
            return jq.each(function() {
                setValue(this, value);
            });
        }
    };

    $.fn.combo.parseOptions = function(target) {
        var t = $(target);
        return $.extend({}, $.fn.textbox.parseOptions(target), $.parser.parseOptions(target, ["separator", "panelAlign", { panelWidth: "number", hasDownArrow: "boolean", delay: "number", selectOnNavigation: "boolean" }, { panelMinWidth: "number", panelMaxWidth: "number", panelMinHeight: "number", panelMaxHeight: "number" }]), { panelHeight: (t.attr("panelHeight") == "auto" ? "auto" : parseInt(t.attr("panelHeight")) || undefined), multiple: (t.attr("multiple") ? true : undefined) });
    };

    $.fn.combo.defaults = $.extend({}, $.fn.textbox.defaults, {
        inputEvents: { click: handleOnClick, keydown: bindEvents, paste: bindEvents, drop: bindEvents },
        panelWidth: null,
        panelHeight: 200,
        panelMinWidth: null,
        panelMaxWidth: null,
        panelMinHeight: null,
        panelMaxHeight: null,
        panelAlign: "left",
        multiple: false,
        selectOnNavigation: true,
        separator: ",",
        hasDownArrow: true,
        delay: 200,
        keyHandler: {
            up: function(e) {
            },
            down: function(e) {
            },
            left: function(e) {
            },
            right: function(e) {
            },
            enter: function(e) {
            },
            query: function(q, e) {
            }
        },
        onShowPanel: function() {
        },
        onHidePanel: function() {
        },
        onChange: function (newValue, oldValue) {
        }
    });
})(jQuery);