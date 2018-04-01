using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CqsDataFoundation
{
    [Serializable]
    public class DescribedEnumItem<T> where T : struct
    {
        [NonSerialized]
        private readonly string _description;

        public DescribedEnumItem(T value)
        {
            Value = value;
            FieldInfo fi = typeof(T).GetField(Enum.GetName(typeof(T), value), BindingFlags.Static | BindingFlags.Public);
            _description = GetMemberDescription(fi, value);
        }

        private static string GetMemberDescription(FieldInfo fi, T val)
        {
            var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));
            return attr == null ? val.ToString() : attr.Description;
        }

        public T Value
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (obj is DescribedEnumItem<T>)            
                return ((DescribedEnumItem<T>)obj).Value.Equals(Value);            
            
           return false;            
        }
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        public override string ToString()
        {
            return _description;
        }

        public static DescribedEnumItem<T>[] GetItems()
        {
            return (from T val in Enum.GetValues(typeof (T)) 
                        select new DescribedEnumItem<T>(val)).ToArray();
        }

    }

}
