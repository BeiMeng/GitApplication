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
        $(target).addClass("validatebox-text");
    };

    function destroyBox(target) {
        var state = $.data(target, "validatebox");
        state.validating = false;
        if (state.timer) {
            clearTimeout(state.timer);
        }
        $(target).tooltip("destroy");
        $(target).unbind();
        $(target).remove();
    };

    function bindEvents(target) {
        var opts = $.data(target, "validatebox").options;
        var $target = $(target);
        $target.unbind(".validatebox");
        if (opts.novalidate || $target.is(":disabled")) {
            return;
        }
        for (var _a in opts.events) {
            $(target).bind(_a + ".validatebox", { target: target }, opts.events[_a]);
        }
    };

    function handleOnfocus(e) {
        var target = e.data.target;
        var state = $.data(target, "validatebox");
        var $target = $(target);
        if ($(target).attr("readonly")) {
            return;
        }
        state.validating = true;
        state.value = undefined;
        (function() {
            if (state.validating) {
                if (state.value != $target.val()) {
                    state.value = $target.val();
                    if (state.timer) {
                        clearTimeout(state.timer);
                    }
                    state.timer = setTimeout(function() {
                        $(target).validatebox("validate");
                    }, state.options.delay);
                } else {
                    repositionTooltip(target);
                }
                setTimeout(arguments.callee, 200);
            }
        })();
    };

    function handleOnblur(e) {
        var target = e.data.target;
        var state = $.data(target, "validatebox");
        if (state.timer) {
            clearTimeout(state.timer);
            state.timer = undefined;
        }
        state.validating = false;
        hideTip(target);
    };

    function handleOnmouseenter(e) {
        var target = e.data.target;
        if ($(target).hasClass("validatebox-invalid")) {
            showTip(target);
        }
    };

    function handleOnmouseleave(e) {
        var target = e.data.target;
        var state = $.data(target, "validatebox");
        if (!state.validating) {
            hideTip(target);
        }
    };

    function showTip(target) {
        var state = $.data(target, "validatebox");
        var opts = state.options;
        $(target).tooltip($.extend({}, opts.tipOptions, { content: state.message, position: opts.tipPosition, deltaX: opts.deltaX })).tooltip("show");
        state.tip = true;
    };

    function repositionTooltip(target) {
        var state = $.data(target, "validatebox");
        if (state && state.tip) {
            $(target).tooltip("reposition");
        }
    };

    function hideTip(target) {
        var state = $.data(target, "validatebox");
        state.tip = false;
        $(target).tooltip("hide");
    };

    function validate(target) {
        var state = $.data(target, "validatebox");
        var opts = state.options;
        var box = $(target);
        opts.onBeforeValidate.call(target);
        var isValid = doAllValidate();
        opts.onValidate.call(target, isValid);
        return isValid;

        function setMessage(msg) {
            state.message = msg;
        };

        function doValidate(validType, params) {
            var value = box.val();
            var types = /([a-zA-Z_]+)(.*)/.exec(validType);
            var rules = opts.rules[types[1]];
            if (rules && value) {
                var validParams = params || opts.validParams || eval(types[2]);
                if (!rules["validator"].call(target, value, validParams)) {
                    box.addClass("validatebox-invalid");
                    var message = rules["message"];
                    if (validParams) {
                        for (var i = 0; i < validParams.length; i++) {
                            message = message.replace(new RegExp("\\{" + i + "\\}", "g"), validParams[i]);
                        }
                    }
                    setMessage(opts.invalidMessage || message);
                    if (state.validating) {
                        showTip(target);
                    }
                    return false;
                }
            }
            return true;
        };

        function doAllValidate() {
            box.removeClass("validatebox-invalid");
            hideTip(target);
            if (opts.novalidate || box.is(":disabled")) {
                return true;
            }
            if (opts.required) {
                if (box.val() == "") {
                    box.addClass("validatebox-invalid");
                    setMessage(opts.missingMessage);
                    if (state.validating) {
                        showTip(target);
                    }
                    return false;
                }
            }
            if (opts.validType) {
                if ($.isArray(opts.validType)) {
                    for (var i = 0; i < opts.validType.length; i++) {
                        if (!doValidate(opts.validType[i])) {
                            return false;
                        }
                    }
                } else {
                    if (typeof opts.validType == "string") {
                        if (!doValidate(opts.validType)) {
                            return false;
                        }
                    } else {
                        for (var type in opts.validType) {
                            var params = opts.validType[type];
                            if (!doValidate(type, params)) {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        };
    };

    function enableValidation(target, isCloseValidation) {
        var opts = $.data(target, "validatebox").options;
        if (isCloseValidation != undefined) {
            opts.novalidate = isCloseValidation;
        }
        if (opts.novalidate) {
            $(target).removeClass("validatebox-invalid");
            hideTip(target);
        }
        validate(target);
        bindEvents(target);
    };

    $.fn.validatebox = function(options, param) {
        if (typeof options == "string") {
            return $.fn.validatebox.methods[options](this, param);
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "validatebox");
            if (state) {
                $.extend(state.options, options);
            } else {
                init(this);
                $.data(this, "validatebox", { options: $.extend({}, $.fn.validatebox.defaults, $.fn.validatebox.parseOptions(this), options) });
            }
            enableValidation(this);
            validate(this);
        });
    };
    $.fn.validatebox.methods = {
        options: function(jq) {
            return $.data(jq[0], "validatebox").options;
        },
        destroy: function(jq) {
            return jq.each(function() {
                destroyBox(this);
            });
        },
        validate: function(jq) {
            return jq.each(function() {
                validate(this);
            });
        },
        isValid: function(jq) {
            return validate(jq[0]);
        },
        enableValidation: function(jq) {
            return jq.each(function() {
                enableValidation(this, false);
            });
        },
        disableValidation: function(jq) {
            return jq.each(function() {
                enableValidation(this, true);
            });
        }
    };

    $.fn.validatebox.parseOptions = function(target) {
        var t = $(target);
        return $.extend({}, $.parser.parseOptions(target, ["validType", "missingMessage", "invalidMessage", "tipPosition", { delay: "number", deltaX: "number" }]), { required: (t.attr("required") ? true : undefined), novalidate: (t.attr("novalidate") != undefined ? true : undefined) });
    };

    $.fn.validatebox.defaults = {
        required: false,
        validType: null,
        validParams: null,
        delay: 200,
        missingMessage: "This field is required.",
        invalidMessage: null,
        tipPosition: "right",
        deltaX: 0,
        novalidate: false,
        events: {
            focus: handleOnfocus,
            blur: handleOnblur,
            mouseenter: handleOnmouseenter,
            mouseleave: handleOnmouseleave,
            click: function(e) {
                var t = $(e.data.target);
                if (!t.is(":focus")) {
                    t.trigger("focus");
                }
            }
        },
        tipOptions: {
            showEvent: "none",
            hideEvent: "none",
            showDelay: 0,
            hideDelay: 0,
            zIndex: "",
            onShow: function() {
                $(this).tooltip("tip").css({ color: "#000", borderColor: "#CC9933", backgroundColor: "#FFFFCC" });
            },
            onHide: function() {
                $(this).tooltip("destroy");
            }
        },
        rules: {
            email: {
                validator: function(value) {
                    return /^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i.test(value);
                },
                message: "Please enter a valid email address."
            },
            url: {
                validator: function(value) {
                    return /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value);
                },
                message: "Please enter a valid URL."
            },
            length: {
                validator: function(value, param) {
                    var len = $.trim(value).length;
                    return len >= param[0] && len <= param[1];
                },
                message: "Please enter a value between {0} and {1}."
            },
            remote: {
                validator: function(value, param) {
                    var params = {};
                    params[param[1]] = value;
                    var result = $.ajax({ url: param[0], dataType: "json", data: params, async: false, cache: false, type: "post" }).responseText;
                    return result == "true";
                },
                message: "Please fix this field."
            }
        },
        onBeforeValidate: function() {
        },
        onValidate: function(valid) {
        }
    };
})(jQuery);