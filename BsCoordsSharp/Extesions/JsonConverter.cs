using Newtonsoft.Json;
using System;

namespace BsCoordsSharp.Extesions
{
    internal static class JsonConverter
    {
        private static JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static T FromJson<T>(this string json) where T : class
            => JsonConvert.DeserializeObject<T>(json);

        public static T FromJson<T>(this string json, Type specific) where T : class
        {
            var res = JsonConvert.DeserializeObject(json, specific, Settings);
            var t = res as T;
            if (res != null && t == null)
                throw new ArgumentException($"Type [{specific.Name}] is not a subclass of [{typeof(T).Name}]");
            return t;
        }

        public static string ToJson(this object obj)
            => JsonConvert.SerializeObject(obj, Settings);
    }
}