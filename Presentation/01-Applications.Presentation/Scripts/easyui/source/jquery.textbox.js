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
    function wrapTextbox(target) {
        $(target).addClass("textbox-f").hide();
        var tb = $("<span class=\"textbox\">" + "<input class=\"textbox-text\" autocomplete=\"off\">" + "<input type=\"hidden\" class=\"textbox-value\">" + "</span>").insertAfter(target);
        var inputName = $(target).attr("name");
        if (inputName) {
            tb.find("input.textbox-value").attr("name", inputName);
            $(target).removeAttr("name").attr("textboxName", inputName);
        }
        return tb;
    };

    function init(target) {
        var state = $.data(target, "textbox");
        var opts = state.options;
        var tb = state.textbox;
        tb.find(".textbox-text").remove();
        if (opts.multiline) {
            $("<textarea class=\"textbox-text\" autocomplete=\"off\"></textarea>").prependTo(tb);
        } else {
            $("<input type=\"" + opts.type + "\" class=\"textbox-text\" autocomplete=\"off\">").prependTo(tb);
        }
        tb.find(".textbox-addon").remove();
        var bb = opts.icons ? $.extend(true, [], opts.icons) : [];
        if (opts.iconCls) {
            bb.push({ iconCls: opts.iconCls, disabled: true });
        }
        if (bb.length) {
            var bc = $("<span class=\"textbox-addon\"></span>").prependTo(tb);
            bc.addClass("textbox-addon-" + opts.iconAlign);
            for (var i = 0; i < bb.length; i++) {
                bc.append("<a href=\"javascript:void(0)\" class=\"textbox-icon " + bb[i].iconCls + "\" icon-index=\"" + i + "\" tabindex=\"-1\"></a>");
            }
        }
        tb.find(".textbox-button").remove();
        if (opts.buttonText || opts.buttonIcon) {
            var btn = $("<a href=\"javascript:void(0)\" class=\"textbox-button\"></a>").prependTo(tb);
            btn.addClass("textbox-button-" + opts.buttonAlign).linkbutton({ text: opts.buttonText, iconCls: opts.buttonIcon });
        }
        setDisable(target, opts.disabled);
        setReadonly(target, opts.readonly);
    };

    function destroy(target) {
        var tb = $.data(target, "textbox").textbox;
        tb.find(".textbox-text").validatebox("destroy");
        tb.remove();
        $(target).remove();
    };

    function setTextboxSize(target, width) {
        var state = $.data(target, "textbox");
        var opts = state.options;
        var tb = state.textbox;
        var tbParent = tb.parent();
        if (width) {
            opts.width = width;
        }
        if (isNaN(parseInt(opts.width))) {
            var c = $(target).clone();
            c.css("visibility", "hidden");
            c.insertAfter(target);
            opts.width = c.outerWidth();
            c.remove();
        }
        var isVisible = tb.is(":visible");
        if (!isVisible) {
            tb.appendTo("body");
        }
        var tbText = tb.find(".textbox-text");
        var tbButton = tb.find(".textbox-button");
        var tbaddon = tb.find(".textbox-addon");
        var tbicon = tbaddon.find(".textbox-icon");
        tb._size(opts, tbParent);
        tbButton.linkbutton("resize", { height: tb.height() });
        tbButton.css({
            left: (opts.buttonAlign == "left" ? 0 : ""),
            right: (opts.buttonAlign == "right" ? 0 : "")
        });

        tbaddon.css({
            left: (opts.iconAlign == "left" ? (opts.buttonAlign == "left" ? tbButton._outerWidth() : 0) : ""),
            right: (opts.iconAlign == "right" ? (opts.buttonAlign == "right" ? tbButton._outerWidth() : 0) : "")
        });

        tbicon.css({
            width: opts.iconWidth + "px",
            height: tb.height() + "px"
        });

        tbText.css({
            paddingLeft: (target.style.paddingLeft || ""),
            paddingRight: (target.style.paddingRight || ""), marginLeft: getMargin("left"), marginRight: getMargin("right")
        });

        if (opts.multiline) {
            tbText.css({
                paddingTop: (target.style.paddingTop || ""),
                paddingBottom: (target.style.paddingBottom || "")
            });
            tbText._outerHeight(tb.height());
        } else {
            var tbpadding = Math.floor((tb.height() - tbText.height()) / 2);
            tbText.css({ paddingTop: tbpadding + "px", paddingBottom: tbpadding + "px" });
        }
        tbText._outerWidth(tb.width() - tbicon.length * opts.iconWidth - tbButton._outerWidth());
        if (!isVisible) {
            tb.insertAfter(target);
        }
        opts.onResize.call(target, opts.width, opts.height);

        function getMargin(position) {
            return (opts.iconAlign == position ? tbaddon._outerWidth() : 0) + (opts.buttonAlign == position ? tbButton._outerWidth() : 0);
        };
    };

    function setValidateTip(target) {
        var opts = $(target).textbox("options");
        var tb = $(target).textbox("textbox");
        tb.validatebox($.extend({}, opts, {
            deltaX: $(target).textbox("getTipX"),
            onBeforeValidate: function() {
                var box = $(this);
                if (!box.is(":focus")) {
                    opts.oldInputValue = box.val();
                    box.val(opts.value);
                }
            },
            onValidate: function(isValid) {
                var box = $(this);
                if (opts.oldInputValue != undefined) {
                    box.val(opts.oldInputValue);
                    opts.oldInputValue = undefined;
                }
                var tb = box.parent();
                if (isValid) {
                    tb.removeClass("textbox-invalid");
                } else {
                    tb.addClass("textbox-invalid");
                }
            }
        }));
    };

    function bindEvents(target) {
        var state = $.data(target, "textbox");
        var opts = state.options;
        var tb = state.textbox;
        var tbtext = tb.find(".textbox-text");
        tbtext.attr("placeholder", opts.prompt);
        tbtext.unbind(".textbox");
        if (!opts.disabled && !opts.readonly) {
            tbtext.bind("blur.textbox", function(e) {
                if (!tb.hasClass("textbox-focused")) {
                    return;
                }
                opts.value = $(this).val();
                if (opts.value == "") {
                    $(this).val(opts.prompt).addClass("textbox-prompt");
                } else {
                    $(this).removeClass("textbox-prompt");
                }
                tb.removeClass("textbox-focused");
            }).bind("focus.textbox", function(e) {
                if (tb.hasClass("textbox-focused")) {
                    return;
                }
                if ($(this).val() != opts.value) {
                    $(this).val(opts.value);
                }
                $(this).removeClass("textbox-prompt");
                tb.addClass("textbox-focused");
            });
            for (var inputEvent in opts.inputEvents) {
                tbtext.bind(inputEvent + ".textbox", { target: target }, opts.inputEvents[inputEvent]);
            }
        }
        var tbaddon = tb.find(".textbox-addon");
        tbaddon.unbind().bind("click", { target: target }, function(e) {
            var tbicon = $(e.target).closest("a.textbox-icon:not(.textbox-icon-disabled)");
            if (tbicon.length) {
                var iconindex = parseInt(tbicon.attr("icon-index"));
                var icon = opts.icons[iconindex];
                if (icon && icon.handler) {
                    icon.handler.call(tbicon[0], e);
                    opts.onClickIcon.call(target, iconindex);
                }
            }
        });
        tbaddon.find(".textbox-icon").each(function(addonIconItem) {
            var item = opts.icons[addonIconItem];
            var $item = $(this);
            if (!item || item.disabled || opts.disabled || opts.readonly) {
                $item.addClass("textbox-icon-disabled");
            } else {
                $item.removeClass("textbox-icon-disabled");
            }
        });
        var btn = tb.find(".textbox-button");
        btn.unbind(".textbox").bind("click.textbox", function() {
            if (!btn.linkbutton("options").disabled) {
                opts.onClickButton.call(target);
            }
        });
        btn.linkbutton((opts.disabled || opts.readonly) ? "disable" : "enable");
        tb.unbind(".textbox").bind("_resize.textbox", function(e, width) {
            if ($(this).hasClass("easyui-fluid") || width) {
                setTextboxSize(target);
            }
            return false;
        });
    };

    function setDisable(target, isDisabled) {
        var state = $.data(target, "textbox");
        var opts = state.options;
        var tb = state.textbox;
        if (isDisabled) {
            opts.disabled = true;
            $(target).attr("disabled", "disabled");
            tb.addClass("textbox-disabled");
            tb.find(".textbox-text, .textbox-value").attr("disabled", "disabled");
        } else {
            opts.disabled = false;
            tb.removeClass("textbox-disabled");
            $(target).removeAttr("disabled");
            tb.find(".textbox-text, .textbox-value").removeAttr("disabled");
        }
    };

    function setReadonly(target, isReadonly) {
        var state = $.data(target, "textbox");
        var opts = state.options;
        opts.readonly = isReadonly == undefined ? true : isReadonly;
        state.textbox.removeClass("textbox-readonly").addClass(opts.readonly ? "textbox-readonly" : "");
        var tbtext = state.textbox.find(".textbox-text");
        tbtext.removeAttr("readonly");
        if (opts.readonly || !opts.editable) {
            tbtext.attr("readonly", "readonly");
        }
    };

    $.fn.textbox = function(options, param) {
        if (typeof options == "string") {
            var method = $.fn.textbox.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.each(function() {
                    var state = $(this).textbox("textbox");
                    state.validatebox(options, param);
                });
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "textbox");
            if (state) {
                $.extend(state.options, options);
                if (options.value != undefined) {
                    state.options.originalValue = options.value;
                }
            } else {
                state = $.data(this, "textbox", {
                    options: $.extend({}, $.fn.textbox.defaults, $.fn.textbox.parseOptions(this), options),
                    textbox: wrapTextbox(this)
                });
                state.options.originalValue = state.options.value;
            }
            init(this);
            bindEvents(this);
            setTextboxSize(this);
            setValidateTip(this);
            $(this).textbox("initValue", state.options.value);
        });
    };

    $.fn.textbox.methods = {
        options: function(jq) {
            return $.data(jq[0], "textbox").options;
        },
        cloneFrom: function(jq, target) {
            return jq.each(function() {
                var t = $(this);
                if (t.data("textbox")) {
                    return;
                }
                if (!$(target).data("textbox")) {
                    $(target).textbox();
                }
                var textboxName = t.attr("name") || "";
                t.addClass("textbox-f").hide();
                t.removeAttr("name").attr("textboxName", textboxName);
                var textbox = $(target).next().clone().insertAfter(t);
                textbox.find("input.textbox-value").attr("name", textboxName);
                $.data(this, "textbox", { options: $.extend(true, {}, $(target).textbox("options")), textbox: textbox });
                var button = $(target).textbox("button");
                if (button.length) {
                    t.textbox("button").linkbutton($.extend(true, {}, button.linkbutton("options")));
                }
                bindEvents(this);
                setValidateTip(this);
            });
        },
        textbox: function(jq) {
            return $.data(jq[0], "textbox").textbox.find(".textbox-text");
        },
        button: function(jq) {
            return $.data(jq[0], "textbox").textbox.find(".textbox-button");
        },
        destroy: function(jq) {
            return jq.each(function() {
                destroy(this);
            });
        },
        resize: function(jq, width) {
            return jq.each(function() {
                setTextboxSize(this, width);
            });
        },
        disable: function(jq) {
            return jq.each(function() {
                setDisable(this, true);
                bindEvents(this);
            });
        },
        enable: function(jq) {
            return jq.each(function() {
                setDisable(this, false);
                bindEvents(this);
            });
        },
        readonly: function(jq, _40) {
            return jq.each(function() {
                setReadonly(this, _40);
                bindEvents(this);
            });
        },
        isValid: function(jq) {
            return jq.textbox("textbox").validatebox("isValid");
        },
        clear: function(jq) {
            return jq.each(function() {
                $(this).textbox("setValue", "");
            });
        },
        setText: function(jq, text) {
            return jq.each(function() {
                var opts = $(this).textbox("options");
                var textbox = $(this).textbox("textbox");
                if ($(this).textbox("getText") != text) {
                    opts.value = text;
                    textbox.val(text);
                }
                if (!textbox.is(":focus")) {
                    if (text) {
                        textbox.removeClass("textbox-prompt");
                    } else {
                        textbox.val(opts.prompt).addClass("textbox-prompt");
                    }
                }
                $(this).textbox("validate");
            });
        },
        initValue: function(jq, text) {
            return jq.each(function() {
                var state = $.data(this, "textbox");
                state.options.value = "";
                $(this).textbox("setText", text);
                state.textbox.find(".textbox-value").val(text);
                $(this).val(text);
            });
        },
        setValue: function(jq, value) {
            return jq.each(function() {
                var opts = $.data(this, "textbox").options;
                var oldValue = $(this).textbox("getValue");
                $(this).textbox("initValue", value);
                if (oldValue != value) {
                    opts.onChange.call(this, value, oldValue);
                    $(this).closest("form").trigger("_change", [this]);
                }
            });
        },
        getText: function(jq) {
            var textbox = jq.textbox("textbox");
            if (textbox.is(":focus")) {
                return textbox.val();
            } else {
                return jq.textbox("options").value;
            }
        },
        getValue: function(jq) {
            return jq.data("textbox").textbox.find(".textbox-value").val();
        },
        reset: function(jq) {
            return jq.each(function() {
                var opts = $(this).textbox("options");
                $(this).textbox("setValue", opts.originalValue);
            });
        },
        getIcon: function(jq, index) {
            return jq.data("textbox").textbox.find(".textbox-icon:eq(" + index + ")");
        },
        getTipX: function(jq) {
            var state = jq.data("textbox");
            var opts = state.options;
            var tb = state.textbox;
            var text = tb.find(".textbox-text");
            var addonOuterWidth = tb.find(".textbox-addon")._outerWidth();
            var buttonOuterWidth = tb.find(".textbox-button")._outerWidth();
            if (opts.tipPosition == "right") {
                return (opts.iconAlign == "right" ? addonOuterWidth : 0) + (opts.buttonAlign == "right" ? buttonOuterWidth : 0) + 1;
            } else {
                if (opts.tipPosition == "left") {
                    return (opts.iconAlign == "left" ? -addonOuterWidth : 0) + (opts.buttonAlign == "left" ? -buttonOuterWidth : 0) - 1;
                } else {
                    return addonOuterWidth / 2 * (opts.iconAlign == "right" ? 1 : -1);
                }
            }
        }
    };

    $.fn.textbox.parseOptions = function(target) {
        var t = $(target);
        return $.extend({}, $.fn.validatebox.parseOptions(target), $.parser.parseOptions(target, ["prompt", "iconCls", "iconAlign", "buttonText", "buttonIcon", "buttonAlign", { multiline: "boolean", editable: "boolean", iconWidth: "number" }]), { value: (t.val() || undefined), type: (t.attr("type") ? t.attr("type") : undefined), disabled: (t.attr("disabled") ? true : undefined), readonly: (t.attr("readonly") ? true : undefined) });
    };

    $.fn.textbox.defaults = $.extend({}, $.fn.validatebox.defaults, {
        width: "auto",
        height: 22,
        prompt: "",
        value: "",
        type: "text",
        multiline: false,
        editable: true,
        disabled: false,
        readonly: false,
        icons: [],
        iconCls: null,
        iconAlign: "right",
        iconWidth: 18,
        buttonText: "",
        buttonIcon: null,
        buttonAlign: "right",
        inputEvents: {
            blur: function(e) {
                var t = $(e.data.target);
                var opts = t.textbox("options");
                t.textbox("setValue", opts.value);
            },
            keydown: function(e) {
                if (e.keyCode == 13) {
                    var t = $(e.data.target);
                    t.textbox("setValue", t.textbox("getText"));
                }
            }
        },
        onChange: function (newValue, oldValue) {
        },
        onResize: function (width, height) {
        },
        onClickButton: function() {
        },
        onClickIcon: function (index) {
        }
    });
})(jQuery);