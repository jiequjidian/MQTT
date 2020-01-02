
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace serverForReadSQL.web
{
    public partial class userManager : System.Web.UI.Page
    {
        static string uname, pswd;
        [WebMethod]
        public static string getuser(string username, string password, string meth)
        {
            //bool resultStr = false;
            string resultStr = string.Empty;
            if (!string.IsNullOrEmpty(meth))
            {
                uname = username;
                pswd = password;
                switch (meth)
                {
                    case "1"://登录
                        resultStr = verify();//登录验证的方法
                        break;
                    //case "0"://注册
                    //    bool addResult = addUser(username, password);
                    //    break;
                    //default:
                    //    break;
                }

            }
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            return resultStr;
        }

        //static Dictionary<string, usertable> dicClass = new Dictionary<string, usertable>();
        //static Dictionary<string, System.Timers.Timer> dicThread = new Dictionary<string, System.Timers.Timer>();
        static List<usertable> dicClass = new List<usertable>();//存储用户信息

        static int userNum;//当前在线的用户数
        static int timeLimit=10;//每个账号的登录信息保存时常（单位：分钟），超时则移除
        static int userTime;
        //static int userTime = (userNum + 1) * timeLimit - timeLimit;
        /// <summary>
        /// 判断登陆账号是否正确
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        /// 
        public static string verify()
        {
            string Result = string.Empty;

            if (pswd.Trim() == BLL.table(uname).Trim())//首先确认数据库中存在该账号/密码
            {
                //Result = IsRole();//判断账户在线数量是否已达上限，若否，将其添加到dicClass
                //if (Result == "")
                //{
                //    if (dicClass.Count == 1)
                //    {//若之前没有账户登录过，启动定时器
                //        userNum = 1;
                //        userTime = timeLimit;
                //        System.Timers.Timer t = new System.Timers.Timer();
                //        t.Enabled = true;
                //        t.Interval = 60000; //执行间隔时间,单位为毫秒; 这里实际间隔为1分钟  
                //        t.Start();
                //        t.Elapsed += new System.Timers.ElapsedEventHandler(test);
                //    }
                //    if (dicClass.Count >1)
                //    {//如果已经有用户在线
                //        userNum ++;
                //        userTime+= timeLimit;

                //        //dicThread.Add(uname, t);
                //    }
                //}
                return Result="true";
            }
            else
            {
                return Result = "账号密码错误";
            }

            return Result;
        }

        /// <summary>
        /// 获取登陆用户信息，判断登陆是否上限
        /// </summary>
        /// <returns></returns>
        //public static string IsRole()
        //{
        //    string Result = string.Empty;

        //    string sql = string.Format("SELECT username,password,LoginSize,role FROM usertable WHERE username='{0}' AND password='{1}';", uname, pswd);

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        dt = SQLserver.QueryTable(sql);//获取  用户名/密码  与uname/pswd  匹配的该条数据记录（username,password,LoginSize,role）

        //        if (dt.Rows.Count == 0)
        //        {
        //            return Result;
        //        }

        //        usertable ut = new usertable()
        //        {
        //            username = dt.Rows[0]["username"].ToString(),
        //            password = dt.Rows[0]["password"].ToString(),
        //            LoginSize = Convert.ToInt32(dt.Rows[0]["LoginSize"]),
        //            role = Convert.ToInt32(dt.Rows[0]["role"])
        //        };

        //        if (ut.role == 0)
        //        {
        //            if (ut.LoginSize >= 0 && ut.LoginSize < 10)
        //            {
        //                dicClass.Add(ut);//本地保存用户信息
        //                //数据库中标识+1
        //                string sqlstr = string.Format("UPDATE usertable set LoginSize = LoginSize+1 WHERE username='{0}' AND password='{1}';", uname, pswd);
        //                try
        //                {
        //                    int i = SQLserver.ExecuteSql(sqlstr);
        //                }
        //                catch (Exception)
        //                {

        //                    throw;
        //                }

        //                return Result;
        //            }
        //            else
        //            {
        //                Result = "登陆超过上限";
        //            }
        //        }
        //        else if (ut.role == 1)
        //        {
        //            if (ut.LoginSize >= 0 && ut.LoginSize < 2)
        //            {
        //                dicClass.Add(ut);//本地保存用户信息
        //                //数据库中标识+1
        //                string sqlstr = string.Format("UPDATE usertable set LoginSize = LoginSize+1 WHERE username='{0}' AND password='{1}';", uname, pswd);
        //                try
        //                {
        //                    int i = SQLserver.ExecuteSql(sqlstr);
        //                }
        //                catch (Exception)
        //                {

        //                    throw;
        //                }
        //                return Result;
        //            }
        //            else
        //            {
        //                Result = "登陆超过上限";
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //    return Result;
        //}

        /// <summary>
        /// 用户登录过期，删除登陆
        /// </summary>
        public static void UpdLoginSize(usertable us)
        {
            if (dicClass.Count == 0)
            {
                return;
            }
            string sql = string.Format("UPDATE usertable set LoginSize = LoginSize-1 WHERE username='{0}' AND password='{1}';", us.username, us.password);

            try
            {
                int i = SQLserver.ExecuteSql(sql);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void Logout(int index)
        {
            if (dicClass != null)
            {
                //for (int i = 0; i < dicClass.Count; i++)
                {
                    if (dicClass.Count > 0)
                    {
                        userNum--;
                        UpdLoginSize(dicClass[index]);//将数据库用于判断在线用户数量的标识减1
                        dicClass.Remove(dicClass[index]);//将本地缓存中的第一个用户移除
                    }
                }
            }
                
        }


       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private static void test(object source, ElapsedEventArgs e)
        {
            if (userNum >0)
            {
                userTime--;  //有用户登录时+timeLimit，每t.Interval分钟-1
                if (userTime % timeLimit == 0)//当有用户登录时间到期
                {
                    Logout(0);
                }
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

        #region 工具

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

        #endregion
    }

    public class usertable
    {
        public string username { get; set; }
        public string password { get; set; }
        public int LoginSize { get; set; }
        public int role { get; set; }
    }
}



