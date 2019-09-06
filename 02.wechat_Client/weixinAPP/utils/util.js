function formatTime(date) {
  var year = date.getFullYear()
  var month = date.getMonth() + 1
  var day = date.getDate()

  var hour = date.getHours()
  var minute = date.getMinutes()
  var second = date.getSeconds()


  return [year, month, day].map(formatNumber).join('/') + ' ' + [hour, minute, second].map(formatNumber).join(':')
}

function formatNumber(n) {
  n = n.toString()
  return n[1] ? n : '0' + n
}


const endFormatTime = (date, x) => { //x为小时数
  var year = date.getFullYear()
  var month = date.getMonth() + 1
  var day = date.getDate()
  var hour = date.getHours() - x
  var minute = date.getMinutes()
  var second = date.getSeconds()
while(true){
  if (hour >= 24) {
    hour -= 24;
    day += 1;
  }
  else{
    if (hour < 0) {
      hour += 24;
      day -= 1;
    }else{
      break;
    }    
  }
}
  
  

  return [year, month, day].map(formatNumber).join('/') + ' ' + [hour, minute, second].map(formatNumber).join(':')
}

const endFormatDate=(date,x)=>{ //x为天数
  var year=date.getFullYear();
  var month=date.getMonth()+1;
  var day=date.getDate()-1;
  while (true) {
    if (day >= 30) {
      hour -= 30;
      month += 1;
    }
    else {
      if (day < 0) {
        hour += 30;
        month -= 1;
      } else {
        break;
      }
    }
  }
  return [year,month,day].map(formatNumber).join('/')
}

const formatDate = (date) => { //x为天数
  var year = date.getFullYear();
  var month = date.getMonth() + 1;
  var day = date.getDate();
 
  return [year, month, day].map(formatNumber).join('/')
}


module.exports = {
  formatTime: formatTime,
  endFormatTime: endFormatTime,
  endFormatDate: endFormatDate,
  formatDate: formatDate,
}
