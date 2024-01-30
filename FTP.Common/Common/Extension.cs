using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace FTP.Common
{
    public static class Extension
    {
        /// <summary>
        /// get list object T from data reader
        /// </summary>
        /// <typeparam name="T">typeof object to get from data reader</typeparam>
        /// <param name="dr">data reader to query</param>
        /// <returns>list object T</returns>
        public static List<T> ToList<T>(this IDataReader dr)
        {
            var list = new List<T>();

            var columns = (from prop in typeof(T).GetProperties() where dr.HasColumn(prop.Name) select prop.Name).ToList();

            while (dr.Read())
            {
                var obj = Activator.CreateInstance<T>();
                try
                {
                    foreach (var column in columns)
                    {
                        if (!Equals(dr[column], DBNull.Value))
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(column);
                            propertyInfo.SetValue(obj, dr[column], null);
                        }
                    }
                    list.Add(obj);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return list;
        }
        /// <summary>
        /// check if data reader has column
        /// </summary>
        /// <param name="dr">data reader to query</param>
        /// <param name="columnName">name of column to check exist in data reader</param>
        private static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// convert object to Json string
        /// </summary>
        /// <param name="obj">object to convert</param>
        /// <returns></returns>
        public static string ToJsonString(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static TColumnType GetValue<TColumnType>(IDataReader dataReader, int columnIndex)
        {
            // validate the data reader is not null
            if (dataReader == null)
                throw new ArgumentNullException("dataReader");

            // get the value
            var value = dataReader[columnIndex];

            // if the value is dbnull, return the default value of the given type
            if (value == DBNull.Value)
                return default(TColumnType);

            var targetType = typeof(TColumnType);

            // account for nullable types
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                targetType = targetType.GetGenericArguments()[0];

            // account for enumerations
            if (targetType.IsEnum)
                return (TColumnType)Enum.Parse(targetType, value.ToString());

            // return the value cast as the given type
            return (TColumnType)value;
        }

        public static TColumnType GetValue<TColumnType>(IDataReader dataReader, string columnName)
        {
            // validate the data reader is not null
            if (dataReader == null)
                throw new ArgumentNullException("dataReader");

            // get the value
            var value = dataReader[columnName];

            // if the value is dbnull, return the default value of the given type
            if (value == DBNull.Value)
                return default(TColumnType);

            var targetType = typeof(TColumnType);

            // account for nullable types
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                targetType = targetType.GetGenericArguments()[0];

            // account for enumerations
            if (targetType.IsEnum)
                return (TColumnType)Enum.Parse(targetType, value.ToString());

            // return the value cast as the given type
            return (TColumnType)value;
        }
    }
}
