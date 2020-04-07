using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace NetCode.Util
{
    public static partial class Extension
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static Extension()
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                return setting;
            });
        }

        /// <summary>
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>
        /// <returns></returns> 
        public static byte[] ToBytes(this object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        /// <summary>
        /// 判断是否为Null或者空
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object obj)
        {
            if (obj == null)
                return true;
            else
            {
                string objStr = obj.ToString();
                return string.IsNullOrEmpty(objStr);
            }
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var serializerSettings = new JsonSerializerSettings();
            //小驼峰命名字段
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 实体类转json数据，速度快（但是字段类型都按字符串处理）
        /// </summary>
        /// <param name="t">实体类</param>
        /// <returns></returns>
        public static string EntityToJson(this object t)
        {
            if (t == null)
                return null;
            string jsonStr = "";
            jsonStr += "{";
            PropertyInfo[] infos = t.GetType().GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                jsonStr = jsonStr + "\"" + infos[i].Name + "\":\"" + infos[i].GetValue(t).ToString() + "\"";
                if (i != infos.Length - 1)
                    jsonStr += ",";
            }
            jsonStr += "}";
            return jsonStr;
        }

        /// <summary>
        /// 深复制
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
                return null;

            return obj.ToJson().ToObject<T>();
        }

        /// <summary>
        /// 改变实体类型
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static object ChangeType(this object obj, Type targetType)
        {
            return obj.ToJson().ToObject(targetType);
        }

        /// <summary>
        /// 改变实体类型
        /// </summary>
        /// <typeparam name="T">目标泛型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T ChangeType<T>(this object obj)
        {
            return obj.ToJson().ToObject<T>();
        }

        /// <summary>
        /// 初始化时间默认值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static T InitDateTime<T>(this T obj) where T : class
        {
            obj.GetType().GetProperties().ForEach(aProperty =>
            {
                if (aProperty.PropertyType == typeof(DateTime))
                {
                    aProperty.SetValue(obj, DateTime.Now);
                }
            });

            return obj;
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr);
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }

        /// <summary>
        /// 将对象序列化为XML字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <param name="rootNodeName">根节点名(建议设为xml)</param>
        /// <returns></returns>
        public static string ToXmlStr<T>(this T obj, string rootNodeName)
        {
            var jsonStr = obj.ToJson();
            var xmlDoc = JsonConvert.DeserializeXmlNode(jsonStr, rootNodeName);
            string xmlDocStr = xmlDoc.InnerXml;

            return xmlDocStr;
        }

        /// <summary>
        /// 获取某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).GetValue(obj);
        }

        /// <summary>
        /// 设置某属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).SetValue(obj, value);
        }

        #region 数值转换
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int ToInt(this object data)
        {
            if (data == null)
                return 0;
            int result;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
                return result;
            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为可空整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int? ToIntOrNull(this object data)
        {
            if (data == null)
                return null;
            int result;
            bool isValid = int.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double ToDouble(this object data)
        {
            if (data == null)
                return 0;
            double result;
            return double.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为双精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static double ToDouble(this object data, int digits)
        {
            return Math.Round(ToDouble(data), digits);
        }

        /// <summary>
        /// 转换为可空双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double? ToDoubleOrNull(this object data)
        {
            if (data == null)
                return null;
            double result;
            bool isValid = double.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal ToDecimal(this object data)
        {
            if (data == null)
                return 0;
            decimal result;
            return decimal.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        /// 转换为高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal ToDecimal(this object data, int digits)
        {
            return Math.Round(ToDecimal(data), digits);
        }

        /// <summary>
        /// 转换为可空高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal? ToDecimalOrNull(this object data)
        {
            if (data == null)
                return null;
            decimal result;
            bool isValid = decimal.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为可空高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal? ToDecimalOrNull(this object data, int digits)
        {
            var result = ToDecimalOrNull(data);
            if (result == null)
                return null;
            return Math.Round(result.Value, digits);
        }

        /// <summary>
        /// 转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime ToDate(this object data)
        {
            if (data == null)
                return DateTime.MinValue;
            DateTime result;
            return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
        }

        /// <summary>
        /// 转换为可空日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime? ToDateOrNull(this object data)
        {
            if (data == null)
                return null;
            DateTime result;
            bool isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
                return result;
            return null;
        }

        /// <summary>
        /// 转换为可空布尔值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Boolean? ToBool(this object data)
        {
            if (data == null)
                return null;

            bool flag;
            if (bool.TryParse(data.ToString(), out flag))
                return flag;
            else
                return null;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <param name="data">数据</param>
        public static string ToStringOrEmpty(this object data)
        {
            return data == null ? string.Empty : data.ToString().Trim();
        }
        #endregion


    }
}
