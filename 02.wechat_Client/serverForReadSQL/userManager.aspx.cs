
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace serverForReadSQL.web
{
    public partial class userManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
 
      
            static string uname, pswd;
            [WebMethod]
        public static string getuser(string username, string password,string meth)
            {
            bool resultStr=false;
                if (!string.IsNullOrEmpty(meth))
                {
                uname = username;
                pswd = password;
                    switch (meth)
                    {
                        case "1"://登录
                             resultStr = verify();//登录验证的方法
                        break ;
                        case "0"://注册
                            bool addResult = addUser(username, password);
                            break;
                        default:
                            break;
                    }              

                }
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(resultStr);
            }

            ///   <summary>
            ///   给一个字符串进行MD5加密
            ///   </summary>
            ///   <param   name="strText">待加密字符串</param>
            ///   <returns>加密后的字符串</returns>
            public static string md5Encode(string str)
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] data = Encoding.Default.GetBytes(str);
                byte[] md5data = md5.ComputeHash(data);
                md5.Clear();
                string re_str = "";
                for (int i = 0; i < md5data.Length; i++)
                {
                    re_str += md5data[i].ToString("x").PadLeft(2, '0');
                }
                return re_str;
            }





            /// <summary>
            /// 查询一个值
            /// </summary>
            /// <param name="userName"></param>
            /// <param name="pwd"></param>
            /// <returns></returns>

            public static bool verify()
            {
                if (pswd.Trim() == BLL.table(uname).Trim())
                {

                    return true;
                }
                else
                {
                    return false;
                }

            }

            /// <summary>
            /// 向数据库添加新用户
            /// </summary>
            /// <param name="userStr"></param>
            /// <param name="pwdStr"></param>
            /// <returns></returns>
            public static bool addUser(string userStr, string pwdStr)
            {

                return BLL.addAccount(userStr, pwdStr);

            }



    }
}



