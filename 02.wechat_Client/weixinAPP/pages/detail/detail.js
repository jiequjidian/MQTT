
import * as echarts from '../../ec-canvas/echarts';
var util = require('../../utils/util.js');

const app = getApp()

var index=null;

//var Chart=null;
var seriesName=null;
var xTime = [];
var tbName=null;
var sTime=null;
var eTime = null;

var dataList = null;




Page({
  data: { 
    stime_d: (util.endFormatDate(new Date(), 3)),//默认起始时间  
    etime_d: (util.endFormatDate(new Date(), 0)),//默认结束时间 
    array:['进水','出水','流量'],
    index:0,
    ec: {
      lazyLoad: true // 延迟加载
    },

    info: {
      id: 1,
      //title: "历史曲线",
      img: "../../images/1.png",
      cTime: '2018-12-12 00:00:00',
      //content: "这是关于园区泵站趋势测试"
    }

  },

  // 时间段选择  
  bindDateChange(e) {
   // let that = this;
    console.log(e.detail.value)
    sTime=e.detail.value;
   // this.stime_d = e.detail.value;
    this.setData({
      stime_d: e.detail.value
    })
    this.getData();
  },
  bindDateChange2(e) {
    // let that = this;
    eTime=e.detail.value; 
    this.setData({
      etime_d: e.detail.value
    })   
    this.getData();
  },

  //事件处理函数

  bindPickerChange:function(e){
    this.setData({
      index:e.detail.value//事件触发后获取的值      
    } );
    switch (e.detail.value) {
      case "0":
      tbName="test";
        seriesName = ["datetimee", "PH", "NH3N", "COD", "TP", "TN"];
        break;
      case "1":
        tbName = "test1";
        seriesName = ["datetimee", "PH", "NH3N", "COD", "TP", "TN"];
        break;
      case "2":
        tbName = "two";       
        seriesName = ['datetimee','进水流量','出水流量'];
        break;
      default:
        break;
    };

    this.getData();//刷新曲线

  },
  onLoad: function (options) {
    sTime= (util.endFormatDate(new Date(), 3));//默认起始时间  
    eTime= (util.endFormatDate(new Date(), 0)),//默认结束时间 
    tbName = options.tb;
    switch (options.tb) {
      case 'test':
        this.index=0;
        seriesName = ["datetimee","PH","NH3N","COD","TP","TN"];
        break;
      case 'test1':
        this.index=1;
        seriesName = ["datetimee", "PH", "NH3N", "COD", "TP", "TN"];
        break;
      case 'two':
        this.index=2;
        seriesName = ["datetimee", "进水流量","出水流量"];  
        break;
      default:
        break;
    };
    this.setData({
      index: this.index,//事件触发后获取的值
    });

    this.echartsComponnet = this.selectComponent('#mychart');   
    //setInterval(this.getData,60000)//每隔一分钟更新一次数据
    setTimeout(this.getData, 1000);
  },
//   onShow:function(){
// setTimeout(this.getData,1000);
//   },
 

  getData:function(){
    var sTimeS = sTime;
   var eTimeS = eTime;
   
    wx.request({
      url: 'https://yuanshengqi.top/forWeChat.aspx/getData', 
      //url: "https://localhost:44373/forWeChat.aspx/getData",
      method:'POST',
      header: {
        'Content-Type': 'application/json'
        //'Content-Type':'application/x-www-form-urlencoded'
      },

      data: { tb: tbName, startTime: sTime, endTime: eTime, seriesNames: seriesName },
      success(res) {
        dataList = JSON.parse(res.data.d);   
        for (var ti in dataList){
          var nS = dataList[ti].datetimee;
         xTime[ti]=nS;
         // xTime.push(nS);//顺序是乱的
        }    
      }
    })
    setTimeout(this.initChart, 1000)  
  },

  initChart:function () {
    this.echartsComponnet.init((canvas, width, height) => {
      // 初始化图表
     const Chart = echarts.init(canvas, null, {
        width: width,
        height: height
      });
       Chart.setOption(this.getOption());
      //this.setOption(Chart);
      // 注意这里一定要返回 chart 实例，否则会影响事件处理等
      return Chart;
    })
  },


  setOption: function (Chart) {
    Chart.clear();  // 清除
    Chart.setOption(this.getOption());  //获取新数据
  },
  getOption: function () {
    // 指定图表的配置项和数据
    var option = {
      title: {
        //text: '园区泵站',
        left: 'center'
      },
      legend: {
        top: 25,
        bottom: 50,
        left: 'center',
       // data: seName,
        z: 100
      },

      tooltip: {
        show: true,
        trigger: 'axis'
      },
      dataset: {
        dimensions: seriesName,
        source: dataList,
      },
      xAxis: {
        type: 'category', 
        data: xTime,
        axisLabel: {
          show: true,
          textStyle: {
            //color: '#c3dbff',  //更改坐标轴文字颜色
            fontSize: 14      //更改坐标轴文字大小
          }
        },
        //type: 'time',
        // data: ['2019/09/08 10:18:23', '2019/09/08 11:17:04','2019/09/08 12:47:05'],
        // axisLabel:{rotate:-45},
        boundaryGap: false,
      },
      yAxis: {
        x: 'center',
        type: 'value',
        axisLabel: {
          show: true,
          textStyle: {
            //color: '#c3dbff',  //更改坐标轴文字颜色
            fontSize: 14      //更改坐标轴文字大小
          }
        },
       
        splitLine: {
          lineStyle: {
            type: 'dashed'
          }
        }
      },
      series: [{
        type: 'line',
        smooth: true,
      }, {
        type: 'line',
        smooth: true,
      }, {
        type: 'line',
        smooth: true,
      }, {
        type: 'line',
        smooth: true,
        }, {
          type: 'line',
          smooth: true,
        }]
    };
    return option;
  },
  
})