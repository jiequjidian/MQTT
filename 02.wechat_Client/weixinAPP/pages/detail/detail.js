

import * as echarts from '../../ec-canvas/echarts';
var util = require('../../utils/util.js');

const app = getApp()

var index = null;

var seriesName = null;
var xTime = null;
var tbName = null;
var lUnit=null;
var sTime = null;
var eTime = null;

var dataList = null;
var l = [];

Page({
    data: {
        stime_d: util.endFormatDate(new Date(), 2),
        etime_d: util.endFormatDate(new Date(), -1),
        stimeL: util.endFormatDate(new Date(), 1),
        etimeL: util.endFormatDate(new Date(), 0),
      array: ["液位(m)/流量(m³/h)", "进水水质(mg/L)", "出水水质(mg/L)", "日进水流量(m³)","一期MLSS(mg/L)", "一期DO(mg/L)", "二期MLSS/DO(mg/L)" ],
        index: 0,
        ec: {
            lazyLoad: !0
        },
        info: {
            id: 1,
            img: "../../images/1.png",
            cTime: "2018-12-12 00:00:00"
        }
    },
    bindDateChange: function(e) {
        console.log(e.detail.value), sTime = e.detail.value, 
        this.setData({
            stime_d: e.detail.value,
          etime_d: util.endFormatDate(new Date(e.detail.value), -3)
        }), 
        this.setData({
            stimeL: util.endFormatDate(new Date(e.detail.value), -1)
        }), setTimeout(this.getData, 1e3);
    },
    bindDateChange2: function(e) {
        eTime = e.detail.value, 
        this.setData({
          stime_d: util.endFormatDate(new Date(e.detail.value), 3),
            etime_d: e.detail.value
        }), this.setData({
            etimeL: util.endFormatDate(new Date(e.detail.value), 1)
        }), this.getData();
    },
    bindPickerChange: function(e) {
        switch (this.setData({
            index: e.detail.value
        }), e.detail.value) {
          case "0":
            tbName = "YW", seriesName = ["datetimee", "进水流量", "出水流量", "进水液位"], lUnit = ["", "(m³/h)", "(m³/h)", "(m)"];
            break;

          case "1":
            tbName = "JS", seriesName = ["datetimee", "PH", "NH3N", "COD", "TP", "TN"], lUnit = ["", "", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
            break;

          case "2":
            tbName = "CS", seriesName = ["datetimee", "PH", "NH3N", "COD", "TP", "TN"], lUnit = ["", "", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];

          case "3":
            tbName = "LLday", seriesName = ["datetimee", "一期日进水", "二期日进水", "日进水流量", "日出水流量"], lUnit = ["", "(m³)", "(m³)", "(m³)", "(m³)"];

          case "4":
            tbName = "YQMLSS", seriesName = ["datetimee", "MLSS1101F", "MLSS1201F", "MLSS1301F", "MLSS1401F"], lUnit = ["", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
            break;

          case "5":
            tbName = "YQDO", seriesName = ["datetimee", "DO1101F", "DO1102F", "DO1201F", "DO1202F", "DO1301F", "DO1302F", "DO1401F", "DO1402F"], lUnit = ["", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
            break;

          case "6":
            tbName = "EQ", seriesName = ["datetimee", "MLSS1100S", "MLSS1200S", "MLSS1101S", "MLSS1201S", "DO1101S", "DO1200S", "DO1201S", "DO1100S"], lUnit = ["", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
        }
        this.getData();
    },
    onLoad: function(e) {
        switch (this.echartsComponnet = this.selectComponent("#mychart"), sTime = util.endFormatDate(new Date(), 2), 
        eTime = util.endFormatDate(new Date(), -1), tbName = e.tb, e.tb) {
          case "YW":
            this.index = 0, seriesName = ["datetimee", "进水流量", "出水流量", "进水液位"], lUnit = ["","(m³/h)","(m³/h)","(m)"];
            break;

          case "JS":
            this.index = 1, seriesName = ["datetimee", "PH", "NH3N", "COD", "TP", "TN"], lUnit = ["", "", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
            break;

          case "CS":
            this.index = 2, seriesName = ["datetimee", "PH", "NH3N", "COD", "TP", "TN"], lUnit = ["", "", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
            break;
          case "LLday":
            this.index = 3, seriesName = ["datetimee", "一期日进水", "二期日进水", "日进水流量", "日出水流量"], lUnit = ["", "(m³)", "(m³)", "(m³)", "(m³)"];
            break;
          case "YQMLSS":
            this.index = 4, seriesName = ["datetimee", "MLSS1101F", "MLSS1201F", "MLSS1301F", "MLSS1401F"], lUnit = ["", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
            break;

          case "YQDO":
            this.index = 5, seriesName = ["datetimee", "DO1101F", "DO1102F", "DO1201F", "DO1202F", "DO1301F", "DO1302F", "DO1401F", "DO1402F"], lUnit = ["", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
            break;

          case "EQ":
            this.index = 6, seriesName = ["datetimee", "MLSS1100S", "MLSS1200S", "MLSS1101S", "MLSS1201S", "DO1101S", "DO1200S", "DO1201S", "DO1100S"], lUnit = ["", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)", "(mg/L)"];
        }
        this.setData({
            index: this.index
        }), this.getData();
    },
    getData: function() {
        var e = new Date(sTime), t = new Date(eTime);
        parseInt(t.getTime() - e.getTime());
        wx.request({
           url: "https://yuanshengqi.top/forWeChat.aspx/getData",
          //url: "https://localhost:44373/forWeChat.aspx/getData",
            method: "POST",
            header: {
                "Content-Type": "application/json"
            },
            data: {
              tb: tbName,
                startTime: sTime,
                endTime: eTime,
                seriesNames: seriesName
            },
            success: function(res) {
              if (res.data.d != null){
               {
                  dataList = JSON.parse(res.data.d);
                  if (dataList.length != 0){
                    var t = 0;
                    xTime = [dataList[0].datetimee, dataList[1].datetimee];
                    for (var a in dataList) {//删掉不在范围内的数据
                      var n = dataList[a].datetimee.replace(/-/g, "/"), l = n.slice(5, n.length);
                      Date.parse(n) < Date.parse(sTime) || Date.parse(n) > Date.parse(eTime) ? (dataList.splice(a, 1),
                        xTime.splice(a, 1)) : (dataList[a].datetimee = l, xTime[a] = l), t = a;
                    }
                    xTime.slice(0, t), dataList.slice(0, t);
                  }
                  
                }
                
              }
              
            }
        }), setTimeout(this.initChart, 2e3);
    },
    initChart: function() {
        var that = this;
      this.echartsComponnet.init(function (canvas, width, height) {
        var chart = echarts.init(canvas, null, {
          width: width,
          height: height
            });
            l = [];
            for (var d = 0; d < seriesName.length - 1; d++) {
                var D = {
                    type: "line",
                    smooth: !0
                };
                l.push(D);
            }
        return chart.setOption(that.getOption()), chart;
        });
    },
    getOption: function() {
      var serieNN=[]
      for (let b in seriesName){
        serieNN.push(seriesName[b] );
      }
        return {
            title: {
                left: "center"
            },
            legend: {//图例属性
                top: 15,
                bottom: 30,
                left: "center",
                z: 100
            },
            tooltip: {
                show: !0,
              trigger: "axis", 
              axisPointer: {
                type: 'cross'
              },
              // backgroundColor: 'rgba(245, 245, 245, 0.8)',
              // borderWidth: 1,
              // borderColor: '#cc1',
              // textStyle: {
              //   color: '#000'
              // },
              position: function (pos, params, el, elRect, size) {
                var obj = { top: 45 };
                obj[['left', 'right'][+(pos[0] < size.viewSize[0] / 2)]] = 30;
                return obj;
              },
            },
          axisPointer: {
            link: { xAxisIndex: 'all' },
            label: {
              backgroundColor: '#777'
            }
          },
          grid:{
            left:50,
            top:50,
          },
            dataset: {
              dimensions: serieNN,
              source: dataList
            },
            xAxis: {
                type: "category",
                data: xTime,
                axisLabel: {
                    show: !0,
                    textStyle: {
                        fontSize: 14
                    }
                },
              axisPointer:{
                show:!0,
              },
                boundaryGap: !1
            },
            yAxis: {
                x: "center",
                type: "value",
                axisLabel: {
                    show: !0,
                    textStyle: {
                        fontSize: 14
                    }
                },
                splitLine: {
                    lineStyle: {
                        type: "dashed"
                    }
                }
            },
            series: l
        };
    }
});