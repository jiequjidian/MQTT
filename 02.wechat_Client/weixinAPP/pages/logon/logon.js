Page({
    data: {
        phone: "user",
        password: "123",
        passwordAgain: ""
    },
    phoneInput: function(s) {
        this.setData({
            phone: s.detail.value
        });
    },
    passwordInput: function(s) {
        this.setData({
            password: s.detail.value
        });
    },
    passwordInputAgain: function(s) {
        this.setData({
            passwordAgain: s.detail.value
        });
    },
    logon: function(s) {
        this.phone = this.data.phone, this.password = this.data.password, this.passwordAgain = this.data.passwordAgain, 
        null == this.phone || null == this.password || null == this.passwordAgain ? wx.showToast({
            title: "用户名和密码不能为空",
            icon: "loading",
            duration: 2e3
        }) : this.password != this.passwordAgain ? wx.showToast({
            title: "两次密码不一致，请确认后重新输入",
            icon: "loading",
            duration: 2e3
        }) : wx.request({
            url: "https://localhost:44363/login.aspx?method=logon",
            header: {
                "Content-Type": "application/json"
            },
            data: {
                userName: this.phone,
                pwd: this.password
            },
            success: function(s) {
                console.log(s), "True" == s.data ? wx.showToast({
                    title: "注册成功",
                    icon: "success",
                    duration: 2e3,
                    success: function() {
                        setTimeout(function() {
                            wx.switchTab({
                                url: "../../pages/login/login"
                            });
                        }, 2e3);
                    }
                }) : wx.showToast({
                    title: "用户已存在"
                });
            }
        });
    }
});