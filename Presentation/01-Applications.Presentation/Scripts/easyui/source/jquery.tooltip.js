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
    function initTooltio(target) {
        $(target).addClass("tooltip-f");
    };

    function bindEvents(target) {
        var opts = $.data(target, "tooltip").options;

        $(target).unbind(".tooltip").bind(opts.showEvent + ".tooltip", function(e) {
            $(target).tooltip("show", e);
        }).bind(opts.hideEvent + ".tooltip", function(e) {
            $(target).tooltip("hide", e);
        }).bind("mousemove.tooltip", function(e) {
            if (opts.trackMouse) {
                opts.trackMouseX = e.pageX;
                opts.trackMouseY = e.pageY;
                $(target).tooltip("reposition");
            }
        });
    };

    function setTimer(target) {
        var state = $.data(target, "tooltip");

        if (state.showTimer) {
            clearTimeout(state.showTimer);
            state.showTimer = null;
        }

        if (state.hideTimer) {
            clearTimeout(state.hideTimer);
            state.hideTimer = null;
        }
    };

    function reposition(target) {
        var state = $.data(target, "tooltip");
        if (!state || !state.tip) {
            return;
        }
        var opts = state.options;
        var tip = state.tip;
        var initPosition = { left: -100000, top: -100000 };
        if ($(target).is(":visible")) {
            initPosition = setPosition(opts.position);
            if (opts.position == "top" && initPosition.top < 0) {
                initPosition = setPosition("bottom");
            } else {
                if ((opts.position == "bottom") && (initPosition.top + tip._outerHeight() > $(window)._outerHeight() + $(document).scrollTop())) {
                    initPosition = setPosition("top");
                }
            }
            if (initPosition.left < 0) {
                if (opts.position == "left") {
                    initPosition = setPosition("right");
                } else {
                    $(target).tooltip("arrow").css("left", tip._outerWidth() / 2 + initPosition.left);
                    initPosition.left = 0;
                }
            } else {
                if (initPosition.left + tip._outerWidth() > $(window)._outerWidth() + $(document)._scrollLeft()) {
                    if (opts.position == "right") {
                        initPosition = setPosition("left");
                    } else {
                        var left1 = initPosition.left;
                        initPosition.left = $(window)._outerWidth() + $(document)._scrollLeft() - tip._outerWidth();
                        $(target).tooltip("arrow").css("left", tip._outerWidth() / 2 - (initPosition.left - left1));
                    }
                }
            }
        }

        tip.css({
            left: initPosition.left,
            top: initPosition.top,
            zIndex: (opts.zIndex != undefined ? opts.zIndex : ($.fn.window ? $.fn.window.defaults.zIndex++ : ""))
        });

        opts.onPosition.call(target, initPosition.left, initPosition.top);

        function setPosition(position) {
            opts.position = position || "bottom";
            tip.removeClass("tooltip-top tooltip-bottom tooltip-left tooltip-right").addClass("tooltip-" + opts.position);
            var left, top;
            if (opts.trackMouse) {
                t = $();
                left = opts.trackMouseX + opts.deltaX;
                top = opts.trackMouseY + opts.deltaY;
            } else {
                var t = $(target);
                left = t.offset().left + opts.deltaX;
                top = t.offset().top + opts.deltaY;
            }
            switch (opts.position) {
            case "right":
                left += t._outerWidth() + 12 + (opts.trackMouse ? 12 : 0);
                top -= (tip._outerHeight() - t._outerHeight()) / 2;
                break;
            case "left":
                left -= tip._outerWidth() + 12 + (opts.trackMouse ? 12 : 0);
                top -= (tip._outerHeight() - t._outerHeight()) / 2;
                break;
            case "top":
                left -= (tip._outerWidth() - t._outerWidth()) / 2;
                top -= tip._outerHeight() + 12 + (opts.trackMouse ? 12 : 0);
                break;
            case "bottom":
                left -= (tip._outerWidth() - t._outerWidth()) / 2;
                top += t._outerHeight() + 12 + (opts.trackMouse ? 12 : 0);
                break;
            }
            return { left: left, top: top };
        };
    };

    function show(target, e) {
        var state = $.data(target, "tooltip");
        var opts = state.options;
        var tip = state.tip;
        if (!tip) {
            tip = $("<div tabindex=\"-1\" class=\"tooltip\">" + "<div class=\"tooltip-content\"></div>" + "<div class=\"tooltip-arrow-outer\"></div>" + "<div class=\"tooltip-arrow\"></div>" + "</div>").appendTo("body");
            state.tip = tip;
            updateContent(target);
        }
        setTimer(target);
        state.showTimer = setTimeout(function() {
            $(target).tooltip("reposition");
            tip.show();
            opts.onShow.call(target, e);
            var arrowOuter = tip.children(".tooltip-arrow-outer");
            var arrow = tip.children(".tooltip-arrow");
            var bc = "border-" + opts.position + "-color";
            arrowOuter.add(arrow).css({ borderTopColor: "", borderBottomColor: "", borderLeftColor: "", borderRightColor: "" });
            arrowOuter.css(bc, tip.css(bc));
            arrow.css(bc, tip.css("backgroundColor"));
        }, opts.showDelay);
    };

    function hide(target, e) {
        var state = $.data(target, "tooltip");
        if (state && state.tip) {
            setTimer(target);
            state.hideTimer = setTimeout(function() {
                state.tip.hide();
                state.options.onHide.call(target, e);
            }, state.options.hideDelay);
        }
    };

    function updateContent(target, content) {
        var state = $.data(target, "tooltip");
        var opts = state.options;
        if (content) {
            opts.content = content;
        }
        if (!state.tip) {
            return;
        }
        var cc = typeof opts.content == "function" ? opts.content.call(target) : opts.content;
        state.tip.children(".tooltip-content").html(cc);
        opts.onUpdate.call(target, cc);
    };

    function destroy(target) {
        var state = $.data(target, "tooltip");
        if (state) {
            setTimer(target);
            var opts = state.options;
            if (state.tip) {
                state.tip.remove();
            }
            if (opts._title) {
                $(target).attr("title", opts._title);
            }
            $.removeData(target, "tooltip");
            $(target).unbind(".tooltip").removeClass("tooltip-f");
            opts.onDestroy.call(target);
        }
    };

    $.fn.tooltip = function(options, param) {
        if (typeof options == "string") {
            return $.fn.tooltip.methods[options](this, param);
        }
        options = options || {};
        return this.each(function() {
            var state = $.data(this, "tooltip");
            if (state) {
                $.extend(state.options, options);
            } else {
                $.data(this, "tooltip", { options: $.extend({}, $.fn.tooltip.defaults, $.fn.tooltip.parseOptions(this), options) });
                initTooltio(this);
            }
            bindEvents(this);
            updateContent(this);
        });
    };

    $.fn.tooltip.methods = {
        options: function(jq) {
            return $.data(jq[0], "tooltip").options;
        },
        tip: function(jq) {
            return $.data(jq[0], "tooltip").tip;
        },
        arrow: function(jq) {
            return jq.tooltip("tip").children(".tooltip-arrow-outer, .tooltip-arrow");
        },
        show: function(jq, e) {
            return jq.each(function() {
                show(this, e);
            });
        },
        hide: function(jq, e) {
            return jq.each(function() {
                hide(this, e);
            });
        },
        update: function(jq, content) {
            return jq.each(function() {
                updateContent(this, content);
            });
        },
        reposition: function(jq) {
            return jq.each(function() {
                reposition(this);
            });
        },
        destroy: function(jq) {
            return jq.each(function() {
                destroy(this);
            });
        }
    };

    $.fn.tooltip.parseOptions = function(target) {
        var t = $(target);
        var opts = $.extend({}, $.parser.parseOptions(target,
            [
                "position", "showEvent", "hideEvent", "content",
                {
                    trackMouse: "boolean",
                    deltaX: "number",
                    deltaY: "number",
                    showDelay: "number",
                    hideDelay: "number"
                }
            ]),
            { _title: t.attr("title") });

        t.attr("title", "");
        if (!opts.content) {
            opts.content = opts._title;
        }
        return opts;
    };
    
    $.fn.tooltip.defaults = {
        position: "bottom",
        content: null,
        trackMouse: false,
        deltaX: 0,
        deltaY: 0,
        showEvent: "mouseenter",
        hideEvent: "mouseleave",
        showDelay: 200,
        hideDelay: 100,
        onShow: function(e) {
        },
        onHide: function(e) {
        },
        onUpdate: function(content) {
        },
        onPosition: function(left, top) {
        },
        onDestroy: function() {
        }
    };
})(jQuery);