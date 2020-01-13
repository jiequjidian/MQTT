using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqttServer_win
{
    class commData
    {
    }
    public class updata
    {
        public DateTime time;      
        public List<sonData> Data;
    }
    public class sonData
    {
        public string name;
        public string value;
    }

}
