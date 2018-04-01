using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ClientPagerProto.DataSource.Viking
{
    public static class JsonHelperNs
    {
        public static T ParseJson<T>(string input)
        {
            return ParseJson<T>(input, false, false);
        }

        public static T ParseJson<T>(string input, bool includeType, bool camelCase)
        {            
            return JsonConvert.DeserializeObject<T>(input, new JsonSerializerSettings
            {
                ContractResolver = camelCase ? new CamelCasePropertyNamesContractResolver():new DefaultContractResolver(),
                TypeNameHandling = includeType ? TypeNameHandling.All : TypeNameHandling.None
            });
        }

        public static string ToJson(object obj)
        {
            return ToJson(obj, false, false);
        }

        public static string ToJson(object obj, bool includeType, bool camelCase)
        {
            var json =
              JsonConvert.SerializeObject(
                obj,
                Formatting.None,
                settings: new JsonSerializerSettings
                {
                    ContractResolver = camelCase ? new CamelCasePropertyNamesContractResolver() : new DefaultContractResolver(),
                    TypeNameHandling = includeType ? TypeNameHandling.All:TypeNameHandling.None
                }
              );

            return json.Replace("“", "'").Replace("”", "'").Replace("„", "'");            
        }
    }
}
