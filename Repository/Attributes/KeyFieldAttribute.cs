using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Reflection;

namespace Repository
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true)]
    public class KeyFieldAttribute : Attribute
    {
        //...
    }

    public class KeyExtractor<T> 
    {
        public object[] KeyValues(T item)
        {
            List<Object> keys = new List<Object>();
            foreach (PropertyInfo fi in KeyFields())
            {
                keys.Add(fi.GetValue(item));
            }
            return keys.ToArray();
        }

        public List<PropertyInfo> KeyFields()
        {
            return FindFields(typeof(T), typeof(KeyFieldAttribute));
        }

        public void InitKeyValue(T item)
        {
            foreach (PropertyInfo propertyInfo in KeyFields())
            {
                Object value = KeyGenerator.next(); 

                if (propertyInfo.PropertyType.Equals(typeof(String)))
                {
                    value = value.ToString();
                }

                propertyInfo.SetValue(item,value);
            }
        }

        private List<PropertyInfo> FindFields(Type type, Type attribute)
        {
            List<PropertyInfo> fields = new List<PropertyInfo>();
            foreach (PropertyInfo field in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if ((field.PropertyType.Equals(typeof(String)) ||
                    field.PropertyType.Equals(typeof(int))) &&
                    field.IsDefined(attribute, true))
                {
                    fields.Add(field);
                }
            }
            return fields;
        }      
    }


    public class KeyGenerator
    {
        private static int value;
        private static object valueLock = new object();

        private KeyGenerator() { }
        
        public static int next()
        {
            lock (valueLock)
            {
                return value++;
            }
        }

        public static void reserve(object keyValue)
        {
            int intKey = -1;

            if (typeof(int).Equals(keyValue.GetType()))
            {
                intKey = (int)keyValue;
            }
            else if (typeof(String).Equals(keyValue.GetType()))
            {
                if (!int.TryParse((String)keyValue, out intKey))
                {
                    return;
                }
            }

            lock (valueLock)
            {
                if (intKey >= value)
                {
                    value = intKey + 1;
                } 
            }
        }

        public static void reserve(params object[] keyValues)
        {
            foreach (object keyValue in keyValues)
            {
                reserve(keyValue);
            }
        }
    }
}
