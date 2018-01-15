using System;
using System.ComponentModel;
using System.Reflection;


namespace TianQin365.Common.Extensions
{
    public static class ObjectExtension
    {
        /// <summary>
        /// 将object转换为指定类型的对象
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ConvertTo(this object obj, Type type) { return convertObject(obj, type); }
        /// <summary>
        /// 将object类型转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(this object obj) { return (T)convertObject(obj, typeof(T)); }

        public static object GetNullObj<T>(this object obj) => typeof(T).IsValueType ? Activator.CreateInstance(typeof(T)) : null;

        /// <summary>
        /// 将一个对象转换为指定类型
        /// </summary>
        /// <param name="obj">待转换的对象</param>
        /// <param name="type">目标类型</param>
        /// <returns></returns>
        private static object convertObject(object obj, Type type)
        {
            if (type == null) return obj;
            if (obj == null || string.IsNullOrEmpty(obj.ToString())) return type.IsValueType ? Activator.CreateInstance(type) : null;

            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (type.IsAssignableFrom(obj.GetType()))
            {
                // 如果待转换对象的类型与目标类型兼容，则无需转换
                return obj;
            }
            else if ((underlyingType ?? type).IsEnum)
            {
                // 如果待转换的对象的基类型为枚举

                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString()))
                {
                    // 如果目标类型为可空枚举，并且待转换对象为null 则直接返回null值
                    return null;
                }
                else
                    return Enum.Parse(underlyingType ?? type, obj.ToString());
            }
            else if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type))
            {
                // 如果目标类型的基类型实现了IConvertible，则直接转换
                try
                {
                    return Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(obj.GetType()))
                    return converter.ConvertFrom(obj);

                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                if (constructor != null)
                {
                    object o = constructor.Invoke(null);
                    PropertyInfo[] propertys = type.GetProperties();
                    Type oldType = obj.GetType();
                    foreach (PropertyInfo property in propertys)
                    {
                        PropertyInfo p = oldType.GetProperty(property.Name);
                        if (property.CanWrite && p != null && p.CanRead)
                            property.SetValue(o, convertObject(p.GetValue(obj, null), property.PropertyType), null);
                    }
                    return o;
                }
            }
            return obj;
        }

        /// <summary>
        /// 获取默认值
        /// </summary>
        /// <returns></returns>
        public static T DefaultValue<T>()
        {
            if (typeof(T) == typeof(DateTime)) return new DateTime(1990, 1, 1).ConvertTo<T>();
            else return default(T);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DefaultValue(Type type)
        {
            if (type == typeof(DateTime)) return new DateTime(1990, 1, 1).ConvertTo(type);
            else return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}