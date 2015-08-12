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
    function initStart(target) {
        var start = 0;
        if (target.selectionStart) {
            start = target.selectionStart;
        } else {
            if (target.createTextRange) {
                var textRange = target.createTextRange();
                var s = document.selection.createRange();
                s.setEndPoint("StartToStart", textRange);
                start = s.text.length;
            }
        }
        return start;
    };

    function setRange(target, start, end) {
        if (target.selectionStart) {
            target.setSelectionRange(start, end);
        } else {
            if (target.createTextRange) {
                var textRange = target.createTextRange();
                textRange.collapse();
                textRange.moveEnd("character", end);
                textRange.moveStart("character", start);
                textRange.select();
            }
        }
    };

    function parseAndSetInitTime(target) {
        var opts = $.data(target, "timespinner").options;
        $(target).addClass("timespinner-f").spinner(opts);
        var value = opts.formatter.call(target, opts.parser.call(target, opts.value));
        $(target).timespinner("initValue", value);
    };

    function handleOnClick(e) {
        var target = e.data.target;
        var opts = $.data(target, "timespinner").options;
        var start = initStart(this);
        for (var i = 0; i < opts.selections.length; i++) {
            var values = opts.selections[i];
            if (start >= values[0] && start <= values[1]) {
                highlight(target, i);
                return;
            }
        }
    };

    function highlight(target, number) {
        var opts = $.data(target, "timespinner").options;
        if (number != undefined) {
            opts.highlight = number;
        }
        var values = opts.selections[opts.highlight];
        if (values) {
            var tb = $(target).timespinner("textbox");
            setRange(tb[0], values[0], values[1]);
            tb.focus();
        }
    };

    function setValue(target, value) {
        var opts = $.data(target, "timespinner").options;
        var value = opts.parser.call(target, value);
        var newValue = opts.formatter.call(target, value);
        $(target).spinner("setValue", newValue);
    };

    /**
	 * 当用户点击spinner按钮触发一个方法
	 * @param {Object} jq
	 * @param {Object} down
	 */
    function clickSpinner(target, down) {
        var opts = $.data(target, "timespinner").options;
        var s = $(target).timespinner("getValue");
        var values = opts.selections[opts.highlight];
        var s1 = s.substring(0, values[0]);
        var s2 = s.substring(values[0], values[1]);
        var s3 = s.substring(values[1]);
        var v = s1 + ((parseInt(s2) || 0) + opts.increment * (down ? -1 : 1)) + s3;
        $(target).timespinner("setValue", v);
        highlight(target);
    };

    $.fn.timespinner = function(options, param) {
        if (typeof options == "string") {
            var method = $.fn.timespinner.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.spinner(options, param);
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "timespinner");
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, "timespinner", { options: $.extend({}, $.fn.timespinner.defaults, $.fn.timespinner.parseOptions(this), options) });
            }
            parseAndSetInitTime(this);
        });
    };
    $.fn.timespinner.methods = {
        options: function(jq) {
            var spinnerOptions = jq.data("spinner") ? jq.spinner("options") : {};
            return $.extend($.data(jq[0], "timespinner").options, { width: spinnerOptions.width, value: spinnerOptions.value, originalValue: spinnerOptions.originalValue, disabled: spinnerOptions.disabled, readonly: spinnerOptions.readonly });
        },
        setValue: function(jq, value) {
            return jq.each(function() {
                setValue(this, value);
            });
        },
        getHours: function(jq) {
            var opts = $.data(jq[0], "timespinner").options;
            var vv = jq.timespinner("getValue").split(opts.separator);
            return parseInt(vv[0], 10);
        },
        getMinutes: function(jq) {
            var opts = $.data(jq[0], "timespinner").options;
            var vv = jq.timespinner("getValue").split(opts.separator);
            return parseInt(vv[1], 10);
        },
        getSeconds: function(jq) {
            var opts = $.data(jq[0], "timespinner").options;
            var vv = jq.timespinner("getValue").split(opts.separator);
            return parseInt(vv[2], 10) || 0;
        }
    };
    $.fn.timespinner.parseOptions = function(target) {
        return $.extend({}, $.fn.spinner.parseOptions(target), $.parser.parseOptions(target, ["separator", { showSeconds: "boolean", highlight: "number" }]));
    };

    $.fn.timespinner.defaults = $.extend({}, $.fn.spinner.defaults, {
        inputEvents: $.extend({}, $.fn.spinner.defaults.inputEvents, {
            click: function(e) {
                handleOnClick.call(this, e);
            },
            blur: function(e) {
                var t = $(e.data.target);
                t.timespinner("setValue", t.timespinner("getText"));
            },
            keydown: function(e) {
                if (e.keyCode == 13) {
                    var t = $(e.data.target);
                    t.timespinner("setValue", t.timespinner("getText"));
                }
            }
        }),
        formatter: function(value) {
            if (!value) {
                return "";
            }
            var opts = $(this).timespinner("options");
            var tt = [padPrefixZero(value.getHours()), padPrefixZero(value.getMinutes())];
            if (opts.showSeconds) {
                tt.push(padPrefixZero(value.getSeconds()));
            }
            return tt.join(opts.separator);

            function padPrefixZero(value) {
                return (value < 10 ? "0" : "") + value;
            };
        },
        parser: function(s) {
            var opts = $(this).timespinner("options");
            var date = parseDate(s);
            if (date) {
                var min = parseDate(opts.min);
                var max = parseDate(opts.max);
                if (min && min > date) {
                    date = min;
                }
                if (max && max < date) {
                    date = max;
                }
            }
            return date;

            function parseDate(s) {
                if (!s) {
                    return null;
                }
                var tt = s.split(opts.separator);
                return new Date(1900, 0, 0, parseInt(tt[0], 10) || 0, parseInt(tt[1], 10) || 0, parseInt(tt[2], 10) || 0);
            };

            if (!s) {
                return null;
            }
            var tt = s.split(opts.separator);
            return new Date(1900, 0, 0, parseInt(tt[0], 10) || 0, parseInt(tt[1], 10) || 0, parseInt(tt[2], 10) || 0);
        },
        selections: [[0, 2], [3, 5], [6, 8]],
        separator: ":",
        showSeconds: false,
        highlight: 0,
        spin: function(down) {
            clickSpinner(this, down);
        }
    });
})(jQuery);