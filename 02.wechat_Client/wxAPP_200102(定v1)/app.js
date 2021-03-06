App({
    onLaunch: function() {
        var n = wx.getStorageSync("logs") || [];
        n.unshift(Date.now()), wx.setStorageSync("logs", n);
    },
    getUserInfo: function(n) {
        var o = this;
        this.globalData.userInfo ? "function" == typeof n && n(this.globalData.userInfo) : wx.login({
            success: function() {
                wx.getUserInfo({
                    success: function(t) {
                        o.globalData.userInfo = t.userInfo, "function" == typeof n && n(o.globalData.userInfo);
                    }
                });
            }
        });
    },
    globalData: {
        userInfo: null
    }
});