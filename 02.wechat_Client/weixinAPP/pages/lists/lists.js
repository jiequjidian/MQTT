//lists.js
const app = getApp()



Page({
  data: {
   

  listData_yw:[
    {id:0,name:'进水流量',value:'',unit:'m³/h',color:'black'},
    { id: 1, name: '出水流量', value: '', unit: 'm³/h', color: 'black' },
    { id: 2, name: '进水液位', value: '', unit: 'm', color: 'black' },
    { id: 3, name: 'datetimee', value: '', unit: '', color: 'black'},
  ],
    listData_JS: [
      { id: 0, name: 'PH', value: '', unit: '', color: 'black' },
      { id: 1, name: 'COD', value: '', unit: 'mg/L', color: 'black'},
      { id: 2, name: 'NH3N', value: '', unit: 'mg/L', color: 'black'},
      { id: 3, name: 'TP', value: '', unit: 'mg/L', color: 'black'},
      { id: 4, name: 'TN', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'datetimee', value: '', unit: '', color: 'black'},
    ],
     listData_CS: [
       { id: 0, name: 'PH', value: '', unit: '', color: 'black'},
       { id: 1, name: 'COD', value: '', unit: 'mg/L', color: 'black'},
       { id: 2, name: 'NH3N', value: '', unit: 'mg/L', color: 'black' },
       { id: 3, name: 'TP', value: '', unit: 'mg/L', color: 'black'},
       { id: 4, name: 'TN', value: '', unit: 'mg/L', color: 'black'},
       { id: 5, name: 'datetimee', value: '', unit: '', color: 'black'},
    ],
    listData_LLday:[
      { id: 0, name: '一期日进水', value: '', unit: 'm³', color: 'black' },
      { id: 1, name: '二期日进水', value: '', unit: 'm³', color: 'black' },
      { id: 2, name: '日进水流量', value: '', unit: 'm³', color: 'black' },
      { id: 3, name: '日出水流量', value: '', unit: 'm³', color: 'black' },
      { id: 4, name: 'datetimee', value: '', unit: '', color: 'black' },
    ],
    listData_YQMLSS: [
      { id: 0, name: 'MLSS1101F', value: '', unit: 'mg/L', color: 'black'},
      { id: 1, name: 'MLSS1201F', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'MLSS1301F', value: '', unit: 'mg/L', color: 'black'},
      { id: 3, name: 'MLSS1401F', value: '', unit: 'mg/L', color: 'black'},
      { id: 4, name: 'datetimee', value: '', unit: '', color: 'black'},
    ],
    
     listData_YQDO: [
       { id: 0, name: 'DO1101F', value: '', unit: 'mg/L', color: 'black'},
       { id: 1, name: 'DO1102F', value: '', unit: 'mg/L', color: 'black' },
       { id: 2, name: 'DO1201F', value: '', unit: 'mg/L', color: 'black' },
       { id: 3, name: 'DO1202F', value: '', unit: 'mg/L', color: 'black' },
       { id: 4, name: 'DO1301F', value: '', unit: 'mg/L', color: 'black' },
       { id: 5, name: 'DO1302F', value: '', unit: 'mg/L', color: 'black' },
       { id: 6, name: 'DO1401F', value: '', unit: 'mg/L', color: 'black' },
       { id: 7, name: 'DO1402F', value: '', unit: 'mg/L', color: 'black'},
       { id: 8, name: 'datetimee', value: '', unit: '', color: 'black'},
    ],
    listData_EQ: [
      { id: 0, name: 'MLSS1100S', value: '', unit: 'mg/L', color: 'black' },
      { id: 1, name: 'MLSS1101S', value: '', unit: 'mg/L', color: 'black'},
      { id: 2, name: 'MLSS1200S', value: '', unit: 'mg/L', color: 'black'},
      { id: 3, name: 'MLSS1201S', value: '', unit: 'mg/L', color: 'black'},
      { id: 4, name: 'DO1100S', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'DO1101S', value: '', unit: 'mg/L', color: 'black' },
      { id: 6, name: 'DO1200S', value: '', unit: 'mg/L', color: 'black' },
      { id: 7, name: 'DO1201S', value: '', unit: 'mg/L', color: 'black'},
      { id: 8, name: 'datetimee', value: '', unit: '', color: 'black'},
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
    var list_YW1 = [
      { id: 0, name: '进水流量', value: '', unit: 'm³/h', color: 'black' },
      { id: 1, name: '出水流量', value: '', unit: 'm³/h', color: 'black' },
      { id: 2, name: '进水液位', value: '', unit: 'm', color: 'black' },
      { id: 3, name: 'datetimee', value: '', unit: '', color: 'black' },
    ];
    var list_JS1 = [
      { id: 0, name: 'PH', value: '', unit: '', color: 'black' },
      { id: 1, name: 'COD', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'NH3N', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'TP', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'TN', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_CS1 = [
      { id: 0, name: 'PH', value: '', unit: '', color: 'black' },
      { id: 1, name: 'COD', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'NH3N', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'TP', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'TN', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_LLday1= [
      { id: 0, name: '一期日进水', value: '', unit: 'm³', color: 'black' },
      { id: 1, name: '二期日进水', value: '', unit: 'm³', color: 'black' },
      { id: 2, name: '日进水流量', value: '', unit: 'm³', color: 'black' },
      { id: 3, name: '日出水流量', value: '', unit: 'm³', color: 'black' },
      { id: 4, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_YQMLSS1 = [
      { id: 0, name: 'MLSS1101F', value: '', unit: 'mg/L', color: 'black' },
      { id: 1, name: 'MLSS1201F', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'MLSS1301F', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'MLSS1401F', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_YQDO1 = [
      { id: 0, name: 'DO1101F', value: '', unit: 'mg/L', color: 'black' },
      { id: 1, name: 'DO1102F', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'DO1201F', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'DO1202F', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'DO1301F', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'DO1302F', value: '', unit: 'mg/L', color: 'black' },
      { id: 6, name: 'DO1401F', value: '', unit: 'mg/L', color: 'black' },
      { id: 7, name: 'DO1402F', value: '', unit: 'mg/L', color: 'black' },
      { id: 8, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_EQ1 = [
      { id: 0, name: 'MLSS1100S', value: '', unit: 'mg/L', color: 'black' },
      { id: 1, name: 'MLSS1101S', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'MLSS1200S', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'MLSS1201S', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'DO1100S', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'DO1101S', value: '', unit: 'mg/L', color: 'black' },
      { id: 6, name: 'DO1200S', value: '', unit: 'mg/L', color: 'black' },
      { id: 7, name: 'DO1201S', value: '', unit: 'mg/L', color: 'black' },
      { id: 8, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]


    setTimeout(function(){     
      wx.request({
        url: 'https://yuanshengqi.top/forWeChat.aspx/getData',
        //url: "https://localhost:44373/forWeChat.aspx/getData",
        data: { tb: 'all', startTime: null, endTime: null, seriesNames: ['PH', 'COD', 'NH3N', 'TP', 'TN','LL', 'datetimee', 'JSLL_ALL','JSLL_YQ','JSLL_EQ','CSLL_ALL','warm','MLSS','DO']},
        method:"POST",
        header: {
          'Content-Type': 'application/json'
        },
        success(res) {
          var listFake = JSON.parse(res.data.d) 
          // that.setData({
          //   newsList: listFake
          // })
          //console.log(listFake[0])

          var list_YW = ['JS_LL', 'CS_LL', 'JS_YW','datetimee']
          var list_JS = ['PH1', 'COD1', 'NH3N1', 'TP1', 'TN1', 'datetimee1']
          var list_CS = ['PH2', 'COD2', 'NH3N2', 'TP2', 'TN2', 'datetimee2']
          var list_LLday = ['JSLL_YQ_LLday','JSLL_EQ_LLday','JSLL_ALL_LLday', 'CSLL_ALL_LLday', 'datetimee_LLday']
          var list_YQMLSS = ['MLSS1101F3', 'MLSS1201F3', 'MLSS1301F3', 'MLSS1401F3', 'datetimee3']
          var list_YQDO = ['DO1101F3', 'DO1102F3', 'DO1201F3', 'DO1202F3', 'DO1301F3', 'DO1302F3', 'DO1401F3', 'DO1402F3', 'datetimee3']
          var list_EQ = ['MLSS1100S4', 'MLSS1101S4', 'MLSS1200S4', 'MLSS1201S4', 'DO1100S4', 'DO1101S4', 'DO1200S4', 'DO1201S4', 'datetimee4']
         
        //   var len = listFake[0].keys()
        var listFree=[]
          for (let i in listFake[0] ){           
            listFree.push(i)
            var ccc = listFake[0][i]
         }


          
          listFree.forEach(function(item,index){
            
            var w=item
            var valueA = listFake[0][w]
            for (var v in list_YW) {
              if (w == list_YW[v]) {
                list_YW1[v].value = listFake[0][w]
              }
            }
            for (var v in list_JS) {
              if (w == list_JS[v]) {
                list_JS1[v].value = valueA
              }
            }
            for (var v in list_CS) {
              if (w == list_CS[v]) {
                list_CS1[v].value = valueA
              }
            }
            for (var v in list_LLday) {
              if (w == list_LLday[v]) {
                list_LLday1[v].value = valueA
              }
            }
            for (var v in list_YQMLSS) {
              if (w == list_YQMLSS[v]) {
                list_YQMLSS1[v].value = valueA
              }
            }
            for (var v in list_YQDO) {
              if (w == list_YQDO[v]) {
                list_YQDO1[v].value = valueA
              }
            }
            for (var v in list_EQ) {
              if (w == list_EQ[v]) {
                list_EQ1[v].value = valueA
              }
            }
          })
         
          if (listFake[0].warning1!=null){
            var warLen1 = listFake[0].warning1.length
            if (warLen1>1){
              var numS=[];
               numS=listFake[0].warning1.split('.');
               numS.forEach(function(item,index){
                 list_JS1[item].color='red'
               })
            }
            else {
              var num = listFake[0].warning1;
              list_JS1[num].color = 'red'
            }
          }
          
          if (listFake[0].warning2 != null) {
            var warLen2 = listFake[0].warning2.length
            if (warLen2 > 1) {
              var numS = [];
              numS = listFake[0].warning2.split('.');
              numS.forEach(function (item, index) {
                list_CS1[item].color = 'red'
              })
            }
            else{
              var num = listFake[0].warning2;
              list_CS1[num].color = 'red'
            }
          }

          that.setData({
            listData_yw: list_YW1,
            listData_JS: list_JS1,
            listData_CS: list_CS1,
            listData_LLday: list_LLday1,
            listData_YQMLSS: list_YQMLSS1,
            listData_YQDO: list_YQDO1,
            listData_EQ: list_EQ1,
          })
          
        }
      })
    },1000)
    
  },
  onShow: function(){
    var that = this
    var list_YW1 = [
      { id: 0, name: '进水流量', value: '', unit: 'm³/h', color: 'black' },
      { id: 1, name: '出水流量', value: '', unit: 'm³/h', color: 'black' },
      { id: 2, name: '进水液位', value: '', unit: 'm', color: 'black' },
      { id: 3, name: 'datetimee', value: '', unit: '', color: 'black' },
    ];
    var list_JS1 = [
      { id: 0, name: 'PH', value: '', unit: '', color: 'black' },
      { id: 1, name: 'COD', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'NH3N', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'TP', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'TN', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_CS1 = [
      { id: 0, name: 'PH', value: '', unit: '', color: 'black' },
      { id: 1, name: 'COD', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'NH3N', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'TP', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'TN', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_LLday = [
      { id: 0, name: '一期日进水', value: '', unit: 'm³/h', color: 'black' },
      { id: 1, name: '二期日进水', value: '', unit: 'm³/h', color: 'black' },
      { id: 2, name: '日进水流量', value: '', unit: 'm³/h', color: 'black' },
      { id: 3, name: '日出水流量', value: '', unit: 'm³/h', color: 'black' },
      { id: 4, name: 'datetimee', value: '', unit: '', color: 'black' },
    ];
    var list_YQMLSS1 = [
      { id: 0, name: 'MLSS1101F', value: '', unit: 'mg/L', color: 'black' },
      { id: 1, name: 'MLSS1201F', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'MLSS1301F', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'MLSS1401F', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_YQDO1 = [
      { id: 0, name: 'DO1101F', value: '', unit: 'mg/L', color: 'black' },
      { id: 1, name: 'DO1102F', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'DO1201F', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'DO1202F', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'DO1301F', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'DO1302F', value: '', unit: 'mg/L', color: 'black' },
      { id: 6, name: 'DO1401F', value: '', unit: 'mg/L', color: 'black' },
      { id: 7, name: 'DO1402F', value: '', unit: 'mg/L', color: 'black' },
      { id: 8, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]
    var list_EQ1 = [
      { id: 0, name: 'MLSS1100S', value: '', unit: 'mg/L', color: 'black' },
      { id: 1, name: 'MLSS1101S', value: '', unit: 'mg/L', color: 'black' },
      { id: 2, name: 'MLSS1200S', value: '', unit: 'mg/L', color: 'black' },
      { id: 3, name: 'MLSS1201S', value: '', unit: 'mg/L', color: 'black' },
      { id: 4, name: 'DO1100S', value: '', unit: 'mg/L', color: 'black' },
      { id: 5, name: 'DO1101S', value: '', unit: 'mg/L', color: 'black' },
      { id: 6, name: 'DO1200S', value: '', unit: 'mg/L', color: 'black' },
      { id: 7, name: 'DO1201S', value: '', unit: 'mg/L', color: 'black' },
      { id: 8, name: 'datetimee', value: '', unit: '', color: 'black' },
    ]


    setInterval(function () {
      wx.request({
        url: 'https://yuanshengqi.top/forWeChat.aspx/getData',
        //url: "https://localhost:44373/forWeChat.aspx/getData",
        data: { tb: 'all', startTime: null, endTime: null, seriesNames: ['PH', 'COD', 'NH3N', 'TP', 'TN', 'LL', 'datetimee', 'JSLL_YQ','JSLL_EQ','JSLL_ALL','CSLL_ALL', 'warm', 'MLSS', 'DO'] },
        method: "POST",
        header: {
          'Content-Type': 'application/json'
        },
        success(res) {
          var listFake = JSON.parse(res.data.d)
          // that.setData({
          //   newsList: listFake
          // })
          //console.log(listFake[0])

          var list_YW = ['JS_LL', 'CS_LL', 'JS_YW', 'datetimee']
          var list_JS = ['PH1', 'COD1', 'NH3N1', 'TP1', 'TN1', 'datetimee1']
          var list_CS = ['PH2', 'COD2', 'NH3N2', 'TP2', 'TN2', 'datetimee2']
          var list_LLday = ['JSLL_YQ_LLday','JSLL_EQ_LLday','JSLL_ALL_LLday', 'CSLL_ALL_LLday', 'datetimee_LLday']
          var list_YQMLSS = ['MLSS1101F3', 'MLSS1201F3', 'MLSS1301F3', 'MLSS1401F3', 'datetimee3']
          var list_YQDO = ['DO1101F3', 'DO1102F3', 'DO1201F3', 'DO1202F3', 'DO1301F3', 'DO1302F3', 'DO1401F3', 'DO1402F3', 'datetimee3']
          var list_EQ = ['MLSS1100S4', 'MLSS1101S4', 'MLSS1200S4', 'MLSS1201S4', 'DO1100S4', 'DO1101S4', 'DO1200S4', 'DO1201S4', 'datetimee4']

          //   var len = listFake[0].keys()
          var listFree = []
          for (let i in listFake[0]) {
            listFree.push(i)
            var ccc = listFake[0][i]
          }



          listFree.forEach(function (item, index) {

            var w = item
            var valueA = listFake[0][w]
            for (var v in list_YW) {
              if (w == list_YW[v]) {
                list_YW1[v].value = listFake[0][w]
              }
            }
            for (var v in list_JS) {
              if (w == list_JS[v]) {
                list_JS1[v].value = valueA
              }
            }
            for (var v in list_CS) {
              if (w == list_CS[v]) {
                list_CS1[v].value = valueA
              }
            }
            for (var v in list_YQMLSS) {
              if (w == list_YQMLSS[v]) {
                list_YQMLSS1[v].value = valueA
              }
            }
            for (var v in list_YQDO) {
              if (w == list_YQDO[v]) {
                list_YQDO1[v].value = valueA
              }
            }
            for (var v in list_EQ) {
              if (w == list_EQ[v]) {
                list_EQ1[v].value = valueA
              }
            }
          })

          if (listFake[0].warning1 != null) {
            var warLen1 = listFake[0].warning1.length
            if (warLen1 > 1) {
              var numS = [];
              numS = listFake[0].warning1.split('.');
              numS.forEach(function (item, index) {
                list_JS1[item].color = 'red'
              })
            }
            else {
              var num = listFake[0].warning1;
              list_JS1[num].color = 'red'
            }
          }

          if (listFake[0].warning2 != null) {
            var warLen2 = listFake[0].warning2.length
            if (warLen2 > 1) {
              var numS = [];
              numS = listFake[0].warning2.split('.');
              numS.forEach(function (item, index) {
                list_CS1[item].color = 'red'
              })
            }
            else {
              var num = listFake[0].warning2;
              list_CS1[num].color = 'red'
            }
          }

          that.setData({
            listData_yw: list_YW1,
            listData_JS: list_JS1,
            listData_CS: list_CS1,
            listData_YQMLSS: list_YQMLSS1,
            listData_YQDO: list_YQDO1,
            listData_EQ: list_EQ1,
          })

        }
      })
    }, 60000)

  }
  
 })
 



