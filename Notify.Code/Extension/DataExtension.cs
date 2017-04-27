using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Notify.Code.Extension
{
    /// <summary>
    /// The data extension.
    /// </summary>
    public static class DataExtension
    {
        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            var result = new List<T>();

            DataTable dt = reader.GetSchemaTable();
            try
            {
                while (reader.Read())
                {
                    var t = new T();

                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            // 当前列名&属性名
                            string columnName = dr[0].ToString();
                            PropertyInfo pro = typeof(T).GetProperty(columnName);

                            if (pro == null)
                            {
                                continue;
                            }

                            if (!pro.CanWrite)
                            {
                                continue;
                            }

                            pro.SetValue(t, ConvertExtension.ConvertHelper(reader[columnName], pro.PropertyType), null);
                        }
                    }

                    result.Add(t);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Dispose();
                }
            }

            return result;
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">dt</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            var result = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                var t = new T();
                try
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        // 当前列名&属性名
                        string columnName = column.ColumnName;
                        PropertyInfo pro = typeof(T).GetProperty(columnName);

                        if (pro == null)
                        {
                            continue;
                        }

                        if (!pro.CanWrite)
                        {
                            continue;
                        }

                        pro.SetValue(t, ConvertExtension.ConvertHelper(dr[columnName], pro.PropertyType), null);
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }

                result.Add(t);
            }

            return result;
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataSet ds) where T : class, new()
        {
            return ds.Tables[0].ToList<T>();
        }

        /// <summary>
        /// ToList
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <param name="dataTableIndex">dataTableIndex</param>
        /// <returns>T</returns>
        public static List<T> ToList<T>(this DataSet ds, int dataTableIndex) where T : class, new()
        {
            return ds.Tables[dataTableIndex].ToList<T>();
        }

        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="reader">reader</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this IDataReader reader) where T : class, new()
        {
            var t = new T();
            DataTable dt = reader.GetSchemaTable();
            try
            {
                while (reader.Read())
                {
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            // 当前列名&属性名
                            string columnName = dr[0].ToString();
                            PropertyInfo pro = typeof(T).GetProperty(columnName);

                            if (pro == null)
                            {
                                continue;
                            }

                            if (!pro.CanWrite)
                            {
                                continue;
                            }

                            pro.SetValue(t, ConvertExtension.ConvertHelper(reader[columnName], pro.PropertyType), null);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Dispose();
                }
            }

            return t;
        }

        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="dt">dt</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this DataTable dt) where T : class, new()
        {
            var t = new T();
            if (dt.Rows.Count <= 0)
            {
                return t;
            }

            try
            {
                foreach (DataColumn column in dt.Columns)
                {
                    // 当前列名&属性名
                    string columnName = column.ColumnName;
                    PropertyInfo pro = typeof(T).GetProperty(columnName);
                    if (pro == null)
                    {
                        continue;
                    }

                    if (!pro.CanWrite)
                    {
                        continue;
                    }

                    pro.SetValue(t, ConvertExtension.ConvertHelper(dt.Rows[0][columnName], pro.PropertyType), null);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return t;
        }

        /// <summary>
        /// ToModel
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="ds">ds</param>
        /// <param name="dataTableIndex">dataTableIndex</param>
        /// <returns>T</returns>
        public static T ToModel<T>(this DataSet ds, int dataTableIndex = 0) where T : class, new()
        {
            return ds.Tables[0].ToModel<T>();
        }
    }
}
