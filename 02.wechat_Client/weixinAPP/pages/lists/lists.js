//lists.js
const app = getApp()

//var urlStr='https://yuanshengqi.top/web/forWeChat.aspx/getData';

Page({
  data: {
   uslStr : 'https://localhost:44373/web/forWeChat.aspx/getData',
    newsList: [
    ]
  },
  //事件处理函数
  bindViewTap: function () {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
  onLoad: function () {
    var that = this
    setInterval(function(){
     
      wx.request({
        url: 'https://yuanshengqi.top/forWeChat.aspx/getData',
        data: { tb: 'all', startTime: null, endTime: null, seriesNames: ['PH', 'COD', 'NH3N', 'TP', 'TN', 'datetimee', 'PH1', 'COD1', 'NH3N1', 'TP1', 'TN1', 'LL','LL1']},
        method:"POST",
        header: {
          'Content-Type': 'application/json'
        },
        success(res) {
          that.setData({
            newsList: JSON.parse(res.data.d) 
          })
        }
      })
    },60000)
    
  },
  onShow:function(){
    var that = this
    wx.request({
      url: 'https://yuanshengqi.top/forWeChat.aspx/getData',
      data: { tb: 'all', startTime: null, endTime: null, seriesNames: ['PH', 'COD', 'NH3N', 'TP', 'TN', 'datetimee', 'LL',] },//'PH1', 'COD1', 'NH3N1', 'TP1', 'TN1', 'LL', 'LL1'
      method: "POST",
      header: {
        'Content-Type': 'application/json'
      },
      success(res) {
        that.setData({
          newsList: JSON.parse(res.data.d) 
        })
        console.log(res.data.d)
      }
    })
  }
 })
 



