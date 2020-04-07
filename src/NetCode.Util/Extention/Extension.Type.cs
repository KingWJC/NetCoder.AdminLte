using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCode.Util
{
    public static partial class Extension
    {
        /// <summary>
        /// 转换为对应的Nullable'T'类型
        /// </summary>
        /// <param name="type">值类型</param>
        /// <returns></returns>
        public static Type ToNullableType(this Type type)
        {
            if (type.IsValueType)
            {
                string fullName = $"System.Nullable`1[[{type.FullName}, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]";

                return Type.GetType(fullName);
            }
            else
                return type;
        }

        /// <summary>
        /// 可空值类型（Nullable<T>）的转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="convertibleValue">任意继承转换接口的基础类型</param>
        /// <returns></returns>
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (null == convertibleValue)
            {
                return default(T);
            }

            Type t = typeof(T);
            if (t.IsGenericType)
            {
                Type gedericTypeDefinition = t.GetGenericTypeDefinition();
                if (gedericTypeDefinition == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(t));
                }
            }
            else
            {
                return (T)Convert.ChangeType(convertibleValue, t);
            }

            throw new InvalidCastException(string.Format("Invalid cast from type \"{0}\" to type \"{1}\".", convertibleValue.GetType().FullName, typeof(T).FullName));
        }
    }
}
