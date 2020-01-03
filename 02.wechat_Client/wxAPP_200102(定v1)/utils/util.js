function e(e) {
    return (e = e.toString())[1] ? e : "0" + e;
}

module.exports = {
    formatTime: function(t) {
        var n = t.getFullYear(), r = t.getMonth() + 1, o = t.getDate(), a = t.getHours(), i = t.getMinutes(), u = t.getSeconds();
        return [ n, r, o ].map(e).join("/") + " " + [ a, i, u ].map(e).join(":");
    },
    endFormatTime: function(t, n) {
        for (var r = t.getFullYear(), o = t.getMonth() + 1, a = t.getDate(), i = t.getHours() - n, u = t.getMinutes(), g = t.getSeconds(); ;) if (i >= 24) i -= 24, 
        a += 1; else {
            if (!(i < 0)) break;
            i += 24, a -= 1;
        }
        return [ r, o, a ].map(e).join("/") + " " + [ i, u, g ].map(e).join(":");
    },

    //日期t-n（day）
  endFormatDate: function (t, n) {
    for (var r = t.getFullYear(), o = t.getMonth() + 1, a = t.getDate() - n; ;){
      if (a >= 30) {
        a -= 28,
        o += 1;
      }
     
      if (a <=0){
        a += 28, o -= 1;
      }
       
      if (o > 12) {
        o = 1;
        a = 1;
        r += 1;
      }
      if (o == 0) {
        o = 12;
        r -= 1;
      }
      return [r, o, a].map(e).join("/");
    }
      
  },
    formatDate: function(t) {
        return [ t.getFullYear(), t.getMonth() + 1, t.getDate() ].map(e).join("/");
    }
};