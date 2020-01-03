function t(t) {
    for (var e = 0; e < t.touches.length; ++e) {
        var a = t.touches[e];
        a.offsetX = a.x, a.offsetY = a.y;
    }
    return t;
}

var e = function(t) {
    return t && t.__esModule ? t : {
        default: t
    };
}(require("./wx-canvas")), a = function(t) {
    if (t && t.__esModule) return t;
    var e = {};
    if (null != t) for (var a in t) Object.prototype.hasOwnProperty.call(t, a) && (e[a] = t[a]);
    return e.default = t, e;
}(require("./echarts")), c = void 0;

Component({
    properties: {
        canvasId: {
            type: String,
            value: "ec-canvas"
        },
        ec: {
            type: Object
        }
    },
    data: {},
    ready: function() {
        this.data.ec ? this.data.ec.lazyLoad || this.init() : console.warn('组件需绑定 ec 变量，例：<ec-canvas id="mychart-dom-bar" canvas-id="mychart-bar" ec="{{ ec }}"></ec-canvas>');
    },
    methods: {
        init: function(t) {
            var n = this, r = wx.version.version.split(".").map(function(t) {
                return parseInt(t, 10);
            });
            if (r[0] > 1 || 1 === r[0] && r[1] > 9 || 1 === r[0] && 9 === r[1] && r[2] >= 91) {
                c = wx.createCanvasContext(this.data.canvasId, this);
                var s = new e.default(c, this.data.canvasId);
                a.setCanvasCreator(function() {
                    return s;
                }), wx.createSelectorQuery().in(this).select(".ec-canvas").boundingClientRect(function(e) {
                    "function" == typeof t ? n.chart = t(s, e.width, e.height) : n.data.ec && "function" == typeof n.data.ec.onInit ? n.chart = n.data.ec.onInit(s, e.width, e.height) : n.triggerEvent("init", {
                        canvas: s,
                        width: e.width,
                        height: e.height
                    });
                }).exec();
            } else console.error("微信基础库版本过低，需大于等于 1.9.91。参见：https://github.com/ecomfe/echarts-for-weixin#%E5%BE%AE%E4%BF%A1%E7%89%88%E6%9C%AC%E8%A6%81%E6%B1%82");
        },
        canvasToTempFilePath: function(t) {
            var e = this;
            t.canvasId || (t.canvasId = this.data.canvasId), c.draw(!0, function() {
                wx.canvasToTempFilePath(t, e);
            });
        },
        touchStart: function(e) {
            if (this.chart && e.touches.length > 0) {
                var a = e.touches[0], c = this.chart.getZr().handler;
                c.dispatch("mousedown", {
                    zrX: a.x,
                    zrY: a.y
                }), c.dispatch("mousemove", {
                    zrX: a.x,
                    zrY: a.y
                }), c.processGesture(t(e), "start");
            }
        },
        touchMove: function(e) {
            if (this.chart && e.touches.length > 0) {
                var a = e.touches[0], c = this.chart.getZr().handler;
                c.dispatch("mousemove", {
                    zrX: a.x,
                    zrY: a.y
                }), c.processGesture(t(e), "change");
            }
        },
        touchEnd: function(e) {
            if (this.chart) {
                var a = e.changedTouches ? e.changedTouches[0] : {}, c = this.chart.getZr().handler;
                c.dispatch("mouseup", {
                    zrX: a.x,
                    zrY: a.y
                }), c.dispatch("click", {
                    zrX: a.x,
                    zrY: a.y
                }), c.processGesture(t(e), "end");
            }
        }
    }
});