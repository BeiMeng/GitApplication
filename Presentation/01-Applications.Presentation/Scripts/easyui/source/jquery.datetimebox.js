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
    function create(target) {
        var state = $.data(target, "datetimebox");
        var opts = state.options;
        $(target).datebox($.extend({}, opts, {
            onShowPanel: function() {
                var value = $(this).datetimebox("getValue");
                setValue(this, value, true);
                opts.onShowPanel.call(this);
            },
            formatter: $.fn.datebox.defaults.formatter,
            parser: $.fn.datebox.defaults.parser
        }));
        $(target).removeClass("datebox-f").addClass("datetimebox-f");
        $(target).datebox("calendar").calendar({
            onSelect: function(date) {
                opts.onSelect.call(this.target, date);
            }
        });
        if (!state.spinner) {
            var panel = $(target).datebox("panel");
            var p = $("<div style=\"padding:2px\"><input></div>").insertAfter(panel.children("div.datebox-calendar-inner"));
            state.spinner = p.children("input");
        }
        state.spinner.timespinner({ width: opts.spinnerWidth, showSeconds: opts.showSeconds, separator: opts.timeSeparator });
        $(target).datetimebox("initValue", opts.value);
    };

    function getInitValue(target) {
        var c = $(target).datetimebox("calendar");
        var t = $(target).datetimebox("spinner");
        var current = c.calendar("options").current;
        return new Date(current.getFullYear(), current.getMonth(), current.getDate(), t.timespinner("getHours"), t.timespinner("getMinutes"), t.timespinner("getSeconds"));
    };

    function doQuery(target, value) {
        setValue(target, value, true);
    };

    function doEnter(target) {
        var opts = $.data(target, "datetimebox").options;
        var initValue = getInitValue(target);
        setValue(target, opts.formatter.call(target, initValue));
        $(target).combo("hidePanel");
    };

    function setValue(target, value, changed) {
        var opts = $.data(target, "datetimebox").options;
        $(target).combo("setValue", value);
        if (!changed) {
            if (value) {
                var parsedValue = opts.parser.call(target, value);
                $(target).combo("setText", opts.formatter.call(target, parsedValue));
                $(target).combo("setValue", opts.formatter.call(target, parsedValue));
            } else {
                $(target).combo("setText", value);
            }
        }
        var parsedValue = opts.parser.call(target, value);
        $(target).datetimebox("calendar").calendar("moveTo", parsedValue);
        $(target).datetimebox("spinner").timespinner("setValue", formatData(parsedValue));

        function formatData(date) {
            function padPreFixZero(value) {
                return (value < 10 ? "0" : "") + value;
            };

            var tt = [padPreFixZero(date.getHours()), padPreFixZero(date.getMinutes())];
            if (opts.showSeconds) {
                tt.push(padPreFixZero(date.getSeconds()));
            }
            return tt.join($(target).datetimebox("spinner").timespinner("options").separator);
        };
    };

    $.fn.datetimebox = function(options, param) {
        if (typeof options == "string") {
            var method = $.fn.datetimebox.methods[options];
            if (method) {
                return method(this, param);
            } else {
                return this.datebox(options, param);
            }
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "datetimebox");
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, "datetimebox", { options: $.extend({}, $.fn.datetimebox.defaults, $.fn.datetimebox.parseOptions(this), options) });
            }
            create(this);
        });
    };

    $.fn.datetimebox.methods = {
        options: function(jq) {
            var opts = jq.datebox("options");
            return $.extend($.data(jq[0], "datetimebox").options, { originalValue: opts.originalValue, disabled: opts.disabled, readonly: opts.readonly });
        },
        cloneFrom: function(jq, target) {
            return jq.each(function() {
                $(this).datebox("cloneFrom", target);
                $.data(this, "datetimebox", { options: $.extend(true, {}, $(target).datetimebox("options")), spinner: $(target).datetimebox("spinner") });
                $(this).removeClass("datebox-f").addClass("datetimebox-f");
            });
        },
        spinner: function(jq) {
            return $.data(jq[0], "datetimebox").spinner;
        },
        initValue: function(jq, value) {
            return jq.each(function() {
                var opts = $(this).datetimebox("options");
                var value = opts.value;
                if (value) {
                    value = opts.formatter.call(this, opts.parser.call(this, value));
                }
                $(this).combo("initValue", value).combo("setText", value);
            });
        },
        setValue: function(jq, value) {
            return jq.each(function() {
                setValue(this, value);
            });
        },
        reset: function(jq) {
            return jq.each(function() {
                var opts = $(this).datetimebox("options");
                $(this).datetimebox("setValue", opts.originalValue);
            });
        }
    };

    $.fn.datetimebox.parseOptions = function(target) {
        var t = $(target);
        return $.extend({}, $.fn.datebox.parseOptions(target), $.parser.parseOptions(target, ["timeSeparator", "spinnerWidth", { showSeconds: "boolean" }]));
    };

    $.fn.datetimebox.defaults = $.extend({}, $.fn.datebox.defaults, {
        spinnerWidth: "100%",
        showSeconds: true,
        timeSeparator: ":",
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
                doEnter(this);
            },
            query: function(q, e) {
                doQuery(this, q);
            }
        },
        buttons: [
            {
                text: function(target) {
                    return $(target).datetimebox("options").currentText;
                },
                handler: function(target) {
                    var opts = $(target).datetimebox("options");
                    setValue(target, opts.formatter.call(target, new Date()));
                    $(target).datetimebox("hidePanel");
                }
            }, {
                text: function(target) {
                    return $(target).datetimebox("options").okText;
                },
                handler: function(_2b) {
                    doEnter(_2b);
                }
            }, {
                text: function(target) {
                    return $(target).datetimebox("options").closeText;
                },
                handler: function(target) {
                    $(target).datetimebox("hidePanel");
                }
            }
        ],
        formatter: function(value) {
            var h = value.getHours();
            var M = value.getMinutes();
            var s = value.getSeconds();

            function padPreFixZero(value) {
                return (value < 10 ? "0" : "") + value;
            };

            var separator = $(this).datetimebox("spinner").timespinner("options").separator;
            var r = $.fn.datebox.defaults.formatter(value) + " " + padPreFixZero(h) + separator + padPreFixZero(M);
            if ($(this).datetimebox("options").showSeconds) {
                r += separator + padPreFixZero(s);
            }
            return r;
        },
        parser: function(s) {
            if ($.trim(s) == "") {
                return new Date();
            }
            var dt = s.split(" ");
            var d = $.fn.datebox.defaults.parser(dt[0]);
            if (dt.length < 2) {
                return d;
            }
            var separator = $(this).datetimebox("spinner").timespinner("options").separator;
            var tt = dt[1].split(separator);
            var hours = parseInt(tt[0], 10) || 0;
            var minutes = parseInt(tt[1], 10) || 0;
            var seconds = parseInt(tt[2], 10) || 0;
            return new Date(d.getFullYear(), d.getMonth(), d.getDate(), hours, minutes, seconds);
        }
    });
})(jQuery);