using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace AspNetCoreStarterPack.Default
{
    public static class ExpandoObjectExtension
    {
        public static ExpandoObject ShallowClone(this ExpandoObject obj)
        {
            var clone = new ExpandoObject();

            var original = (IDictionary<string, object>)obj;
            var _clone = (IDictionary<string, object>)clone;

            for (var i = 0; i < original.Keys.Count(); i++)
            {
                var key = original.Keys.ElementAt(i);
                _clone.Add(key, original[key]);
            }

            return clone;
        }

        public static bool ContainProperty(this ExpandoObject obj, string property)
        {
            var dic = (IDictionary<string, object>)obj;
            return dic.ContainsKey(property);
        }

        public static bool IsNull(this ExpandoObject obj, string property)
        {
            var dic = (IDictionary<string, object>)obj;
            return dic[property] == DBNull.Value || dic[property] == null;
        }

        public static T GetProperty<T>(this ExpandoObject obj, string property)
        {
            var dic = (IDictionary<string, object>)obj;

            if (null == dic[property] || !(dic[property] is T)) return default(T);

            return (T)dic[property];
        }

        public static void SetProperty(this ExpandoObject obj, string property, object value)
        {
            var dic = (IDictionary<string, object>)obj;
            dic[property] = value;
        }

        public static dynamic AsDynamic(this ExpandoObject obj)
        {
            return obj as dynamic;
        }
    }
}
