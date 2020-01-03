Page({
    data: {
        phone: "",
        password: ""
    },
    phoneInput: function(t) {
        this.setData({
            phone: t.detail.value
        });
    },
    passwordInput: function(t) {
        this.setData({
            password: t.detail.value
        });
    },
    login: function() {
        0 == this.data.phone.length || 0 == this.data.password.length ? wx.showToast({
            title: "用户名和密码不能为空",
            icon: "loading",
            duration: 2e3
        }) : (this.phone = this.data.phone, this.password = this.data.password, wx.request({
            url: "https://yuanshengqi.top/userManager.aspx/getuser",
            method: "POST",
            header: {
                "Content-Type": "application/json"
            },
            data: {
                username: this.phone,
                password: this.password,
                meth: "1"
            },
            success: function(t) {
                "true" == t.data.d ? wx.showToast({
                    title: "登录成功",
                    icon: "success",
                    duration: 2e3,
                    success: function() {
                        setTimeout(function() {
                            wx.switchTab({
                                url: "../../pages/lists/lists"
                            });
                        }, 2e3);
                    }
                }) : wx.showToast({
                    icon: "error",
                    title: "用户名或密码错误"
                });
            }
        }));
    },
    logon: function() {
        wx.navigateTo({
            url: "../../pages/logon/logon"
        });
    }
});