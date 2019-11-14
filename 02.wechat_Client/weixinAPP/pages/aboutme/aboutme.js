var a = getApp();

Page({
    data: {
        img: "../../images/chemic.png",
        title: "凯秘克（上海）环保科技有限公司",
      intro: "这是一个关于凯秘克云平台数据查看的模块，通过该模块可以进行查看相应的泵站运行数据，以及数据趋势整理。",
        contab: "联系方式",
        address: "上海市普陀区金沙江路1340弄D区",
        mobile: "13918379739",
        email: "2465941545@qq.com"
    },
    bindViewTap: function() {
        wx.navigateTo({
            url: "../logs/logs"
        });
    },
    onLoad: function() {
        var e = this;
        a.globalData.userInfo ? this.setData({
            userInfo: a.globalData.userInfo,
            hasUserInfo: !0
        }) : this.data.canIUse ? a.userInfoReadyCallback = function(a) {
            e.setData({
                userInfo: a.userInfo,
                hasUserInfo: !0
            });
        } : wx.getUserInfo({
            success: function(s) {
                a.globalData.userInfo = s.userInfo, e.setData({
                    userInfo: s.userInfo,
                    hasUserInfo: !0
                });
            }
        });
    },
    getUserInfo: function(e) {
        console.log(e), a.globalData.userInfo = e.detail.userInfo, this.setData({
            userInfo: e.detail.userInfo,
            hasUserInfo: !0
        });
    }
});