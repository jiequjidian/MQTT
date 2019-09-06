Page({
  data: {
    phone:'',
    password:''
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

  // 登录
  login: function () {
    if (this.data.phone.length == 0 || this.data.password.length == 0) {
      wx.showToast({
        title: '用户名和密码不能为空',
        icon: 'loading',
        duration: 2000
      })
    } else {
      //若用户名和密码不为空，则向服务器提交数据，等待服务器发送验证成功或失败响应
      this.phone = this.data.phone,
        this.password = this.data.password,
        wx.request({
        url: 'https://yuanshengqi.top/Default.aspx?method=getuser',

          header: {
            "Content-Type": "application/json"
          },
          data: {
            username: this.phone,
            password: this.password
          },


          success: function (res) {           
            if (res.data == "True") {
              
             // if (res.data!=null) {
               
              // 这里修改成跳转的页面
              wx.showToast({
                title: '登录成功',
                icon: 'success',
                duration: 2000,
                success: function () {
                  setTimeout(function () {
                    wx.switchTab({
                      url: '../../pages/lists/lists',
                    })
                  }, 2000)
                }
              })
            }
            else {
              wx.showToast({
                icon:"error",
                title: '用户名或密码错误',
              })
            }
          }

        })


    }
  },

  logon:function(){
    wx.navigateTo({
      url: '../../pages/logon/logon'
    })
  }
})
