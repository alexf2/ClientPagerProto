using System;
using System.Reflection;

namespace CqsDataFoundation.Query.Providers
{
    public class PropertyValidator
    {
        public virtual void ValidatePropertyName<T>(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            string[] propsChain = name.Split('.');
            var t = typeof (T);
            foreach (var p in propsChain)            
                t = GetType(Validate(t, p));
        }

        static Type GetType(MemberInfo mi)
        {
            var pi = mi as PropertyInfo;
            if (pi != null)
                return pi.PropertyType;
            var fi = mi as FieldInfo;
            return fi.FieldType;            
        }

        static MemberInfo Validate(Type t, string name)
        {            
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            MemberInfo mi = (MemberInfo)t.GetProperty(name) ?? t.GetField(name);            

            if (mi == null)            
                throw new ArgumentException(string.Format("Unable to sort. '{0}' is not a public property of '{1}'.", name, t.FullName));

            return mi;
        }
    }
}
