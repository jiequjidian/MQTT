Page({
  data: {
    phone: 'user',
    password: '123',
    passwordAgain:''
  },
  // 获取输入账号
  phoneInput: function (e) {
    this.setData({
      phone: e.detail.value
    })
  },

  // 获取输入密码
  passwordInput: function (e) {
    this.setData({
      password: e.detail.value
    })
  },
  //获取确认密码
  passwordInputAgain: function (e) {
    this.setData({
      passwordAgain: e.detail.value
    })
  },
  
  logon:function(e){
    this.phone=this.data.phone;
    this.password=this.data.password;
    this.passwordAgain=this.data.passwordAgain;
    if (this.phone == null || this.password==null || this.passwordAgain==null){
      wx.showToast({
        title: '用户名和密码不能为空',
        icon: 'loading',
        duration: 2000
      })
    }
    else{
      if (this.password != this.passwordAgain){
        wx.showToast({
          title: '两次密码不一致，请确认后重新输入',
          icon: 'loading',
          duration: 2000
        })
      }
      else{
        // this.phone=this.data.phone,
        // this.password = this.data.password,
          wx.request({
            url: 'https://localhost:44363/login.aspx?method=logon',

            header: {
              "Content-Type": "application/json"
            },
            data: {
              userName: this.phone,
              pwd: this.password
            },

            success: function (res) {
              console.log(res);
              if (res.data == "True") {
                // 这里修改成跳转的页面
                wx.showToast({
                  title: '注册成功',
                  icon: 'success',
                  duration: 2000,
                  success: function () {
                    setTimeout(function () {
                      wx.switchTab({
                        url: '../../pages/login/login',
                      })
                    }, 2000)
                  }
                })
              }
              else {
                wx.showToast({
                  title: '用户已存在',
                })
              }
            }

          })
      }
    }
  }
})