using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public partial class Helper
    {
        public class Data
        {
            public static string GetSafeStr(object value, string defaultStr="")
            {
                return value != null ? value.ToString() : defaultStr;
            }


            public static int GetSafeInt(object pValue,int pDefault =0)
            {
                try
                {
                    int result = 0;
                    Int32.TryParse(pValue.ToString(), out result);
                    return result == 0 ? pDefault : result;
                }
                catch
                {
                    return pDefault;
                }
            }
            public static string ConvertObjectToJson(object pSource)
            {
                try
                {
                    string result = JsonConvert.SerializeObject(pSource, new Newtonsoft.Json.Converters.StringEnumConverter());
                    return result;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
            public static T Get<T>(object pValue, T pDefault)
            {
                var t = typeof(T);
                t = Nullable.GetUnderlyingType(t) ?? t;
                try
                {
                    if(pValue == DBNull.Value || pValue == null)
                    {
                        return pDefault;
                    }
                    else
                    {
                        return (T)Convert.ChangeType(pValue, t);
                    }
                }
                catch
                {
                    return pDefault;
                }
            }

            public static T Get<T>(object pValue)
            {

                T defaultValue;
                if (Checker.IsNumType<T>())
                {
                    defaultValue = (T)Convert.ChangeType(0, typeof(T));
                }
                else
                {
                    if (Checker.IsString<T>())
                    {
                        defaultValue = (T)Convert.ChangeType(string.Empty, typeof(T));
                    }
                    else
                    {
                        defaultValue = default(T);
                    }
                }
                return Get<T>(pValue, defaultValue);
            }
        }
    }
}
