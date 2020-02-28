using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048
{
    public static class Json
    {
        public static string ConvertJsonString(string str)
        {
            //格式化json字符串
            JsonSerializer serializer = new JsonSerializer();
            TextReader tr = new StringReader(str);
            JsonTextReader jtr = new JsonTextReader(tr);
            object obj = serializer.Deserialize(jtr);
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return str;
            }
        }

        static readonly object ObTOJsonLock = new object();
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string ObjectToJson(object o)
        {
            try
            {
                lock (ObTOJsonLock)
                {
                    return JsonConvert.SerializeObject(o, Formatting.None);
                }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return "";
            }
        }

        static readonly object jsonLock = new object();
        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T JsonToObject<T>(string json, T anonymousTypeObject)
        {
            try
            {
                lock (jsonLock) { return JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject); }
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                throw ex;
            }
        }



    }
}
