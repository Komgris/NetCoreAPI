using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.Model
{
    public class BaseJSONModel
    {
    }

    public static class ExtensionModel
    {
        public static string ToJson(this BaseJSONModel obj) {
            return JsonConvert.SerializeObject(obj);
        }

        public static string ToJson(this BaseJSONModel obj, JsonSerializerSettings jsonSetting)
        {
            return JsonConvert.SerializeObject(obj, jsonSetting);
        }

        public static T ToObject<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }
    }
}
