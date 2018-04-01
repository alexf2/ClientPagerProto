using System;
using System.Globalization;
using System.Text;
using System.Web.Script.Serialization;

namespace ClientPagerProto.DataSource.Viking
{
    public class JsonHelper
    {
        public static T ParseJson<T>(string input)
        {
            return ParseJson<T>(input, false);
        }

        public static T ParseJson<T>(string input, bool includeType)
        {
            return (includeType ? new JavaScriptSerializer(new SimpleTypeResolver()) : JSonSerializer).Deserialize<T>(input);
        }

        public static string ToJson(object input)
        {
            return ToJson(input, false);
        }

        public static string ToJson(object input, bool includeType)
        {
            var ser = includeType ? new JavaScriptSerializer(new SimpleTypeResolver()) : JSonSerializer;
            var json = ser.Serialize(input);

            json = json.Replace("“", "'").Replace("”", "'").Replace("„", "'");

            return json;
        }

        private static JavaScriptSerializer _jSonSerializer;
        public static JavaScriptSerializer JSonSerializer
        {
            get { return _jSonSerializer ?? (_jSonSerializer = new JavaScriptSerializer { MaxJsonLength = int.MaxValue }); }
        }

        public static string EnumToJson(Type type)
        {
            var sb = new StringBuilder("{");
            string[] typeNames = Enum.GetNames(type);
            int i = 0;
            foreach (string name in typeNames)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(name + " : " + i.ToString(CultureInfo.InvariantCulture));
                ++i;
            }
            sb.Append("};");

            return sb.ToString();
        }
    }
}