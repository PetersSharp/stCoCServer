using System;
using System.Text;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using stCore;
using System.Diagnostics;

namespace stSqlite
{
    public static class SqliteConvertExtension
    {
        private const string _emptySource = "{\"error\":1,\"msg\":\"request return empty table\"}";
        private const string _addSource = "{{\"error\":0,\"msg\":\"\",\"recordsTotal\":{0},\"recordsFiltered\":{0},\"displayLength\":{1},\"nextupdate\":{2},\"data\":[";

        /// <summary>
        /// Minify JSON string
        /// </summary>
        /// <param name="jString">JSON string</param>
        /// <returns></returns>
        public static string JsonToMinify(this string jString)
        {
            return ((string.IsNullOrWhiteSpace(jString)) ? 
                _emptySource : 
                Regex.Replace(jString, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1")
            );
        }
        ///<summary>
        ///Query DB to get sum from field
        ///</summary>
        ///<param name="dtbl">DataTable DB request data</param>
        ///<param name="fld">Field name</param>
        public static Int32 ToSUM(this DataTable dtbl, string fld)
        {
            return (Int32)dtbl.AsEnumerable().Select(x => x.Field<decimal>(fld)).Sum();
        }
        /// <summary>
        /// Convert Map <see cref="TablePropertyMapAttribute"/> to <see cref="DataTable"/>
        /// </summary>
        /// <typeparam name="T">Data Class instance</typeparam>
        /// <returns></returns>
        public static DataTable MapToDataTable<T>() where T : class, new()
        {
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            List<DataColumn> keyTbl = new List<DataColumn>();
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            dt.TableName = typeof(T).Name;

            foreach (PropertyInfo prop in Props)
            {
                try
                {
                    var attr = prop.GetCustomAttributes(true).Where(c => c.GetType() == typeof(TablePropertyMapAttribute)).Cast<TablePropertyMapAttribute>().FirstOrDefault();
                    if (attr != null)
                    {
                        Type propType = ((attr.FieldType != null) ? attr.FieldType :
                            ((prop.PropertyType == null) ? typeof(System.String) : prop.PropertyType));

                        dt.Columns.Add(prop.Name, propType);
                        dt.Columns[prop.Name].Unique = attr.FieldUnique;
                        if (attr.FieldPrimaryKey)
                        {
                            keyTbl.Add(dt.Columns[prop.Name]);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new ConvertExtensionException(
                        stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                        "Columns",
                        e.Message
                    );
                }
            }
            if (keyTbl.Count > 0)
            {
                try
                {
                    dt.PrimaryKey = keyTbl.ToArray();
                    keyTbl.Clear();
                }
                catch (Exception e)
                {
                    throw new ConvertExtensionException(
                        stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                        "PrimaryKey",
                        e.Message
                    );
                }
            }
            dt.AcceptChanges();
            return dt;
        }
        /// <summary>
        /// Convert Map <see cref="TablePropertyMapAttribute"/> to SQL Create Table string
        /// </summary>
        /// <typeparam name="T">Data Class instance</typeparam>
        /// <returns></returns>
        public static string MapToSQLCreateTable<T>() where T : class, new()
        {
            int cnt = 0;
            StringBuilder sb = new StringBuilder();
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            sb.AppendFormat("CREATE TABLE IF NOT EXISTS {0} (", typeof(T).Name);

            foreach (PropertyInfo prop in Props)
            {
                try
                {
                    var attr = prop.GetCustomAttributes(true).Where(c => c.GetType() == typeof(TablePropertyMapAttribute)).Cast<TablePropertyMapAttribute>().FirstOrDefault();
                    if (attr != null)
                    {
                        sb.AppendFormat(
                            "{0} {1}{2}{3}{4}{5}",
                            prop.Name,
                            ((attr.FieldSQL != null) ? attr.FieldSQL :
                                ((attr.FieldType != null) ? SqliteConvertExtension.TablePropertyMapSqlType(attr.FieldType) :
                                    SqliteConvertExtension.TablePropertyMapSqlType(prop.PropertyType))),
                            ((attr.FieldUnique) ? " UNIQUE" : ""),
                            ((attr.FieldPrimaryKey) ? " PRIMARY KEY" : ""),
                            ((attr.FieldAutoIncrement) ? " AUTOINCREMENT" : ""),
                            (((Props.Length - 1) > cnt) ? ", " : "")
                        );
                    }
                }
                catch (Exception e)
                {
                    throw new ConvertExtensionException(
                        stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                        "SQL Create Table",
                        e.Message
                    );
                }
                cnt++;
            }
            sb.AppendFormat(" )");
            return sb.ToString();
        }
        ///<summary>
        ///Print debug DataTable source to console screen
        ///</summary>
        [Conditional("DEBUG")]
        public static void DataTableToPrint(this DataTable dtbl)
        {
            if ((dtbl == null) || (dtbl.Rows.Count == 0))
            {
                stConsole.WriteLine("--- Table is empty ---");
            }
            stConsole.WriteLine(string.Format("--- Table {0} / {1} ---", dtbl.TableName, dtbl.Rows.Count));
            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                stConsole.WriteLine(string.Format("--- Row {0}/{1} ---", (i + 1), dtbl.Rows.Count));
                for (int j = 0; j < dtbl.Columns.Count; j++)
                {
                    stConsole.WriteLine(
                        string.Format(
                            "\tCol: '{0}' - [{1}]:\t{2}",
                            dtbl.Columns[j].ColumnName,
                            dtbl.Rows[i][j].GetType(),
                            dtbl.Rows[i][j].ToString()
                        )
                    );
                }
            }
        }
        ///<summary>
        ///JSON format to <see cref="System.Data.DateDatable"/> .JsonToDataTable(string JsonSource)
        ///Used attribute <see cref="stSqlite.TablePropertyMapAttribute"/> for more details.
        ///<example>
        ///Examle:
        ///<code>[TablePropertyMapAttribute("Schema", typeof(System.String)), "STRING UNIQUE"]</code>
        ///"Schema" is name field in Json string, type string
        ///Full mapping example <see cref="stSqlite.TablePropertyMapAttribute.TablePropertyMapAttribute"/>
        ///</example>
        ///</summary>
        ///<param name="jString">JSON serializing source string</param>
        ///<param name="skipNoMap">Skiping no Map field</param>
        ///<returns>DataTable DB parse data</returns>
        ///<remarks>Warning: support only flat Json array, not object in object</remarks>
        public static DataTable JsonToDataTable<T>(this string jString, bool skipNoMap = false, bool scanAllRows = true) where T : class, new()
        {
            T dataclass = new T();
            stSqlite.TablePropertyMapMethod tpmm = dataclass as stSqlite.TablePropertyMapMethod;
            if (tpmm == null)
            {
                throw new ConvertExtensionException(
                    stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorBadInheritClass,
                    dataclass.ToString()
                );
            }
            MethodInfo mi = tpmm.getNormalizeTableFunc();
            if (mi != null)
            {
                jString = mi.Invoke(dataclass, new[] { jString }) as string;
            }
            string[] jsonParts = Regex.Split(jString.Replace("[", "").Replace("]", ""), "},{");
            if (jsonParts.Length == 0)
            {
                throw new ConvertExtensionException(
                    stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorUndefined,
                    Properties.Resources.ConvertErrorJsonArray + " : " + dataclass.ToString()
                );
            }
            string[] jsonColumns = ((scanAllRows) ? jsonParts : new string [] { jsonParts[0]});
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            List<DataColumn> colKey = new List<DataColumn>();
            dt.TableName = typeof(T).Name;

            foreach (string jp in jsonColumns)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",\"");
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        if (!dt.Columns.Contains(n))
                        {
                            stSqlite.TablePropertyMapAttribute res = tpmm.findAttr(n, true);
                            if (res != null)
                            {
                                dt.Columns.Add(res.FieldName, res.FieldType);
                                dt.Columns[res.FieldName].Unique = res.FieldUnique;
                                dt.Columns[res.FieldName].DefaultValue =
                                    SqliteConvertExtension.ColumnsPropertyTypeDefaultValue(res.FieldType);
                                if (res.FieldPrimaryKey)
                                {
                                    colKey.Add(dt.Columns[res.FieldName]);
                                }
                            }
                            else if (!skipNoMap)
                            {
                                dt.Columns.Add(n);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        throw new ConvertExtensionException(
                            stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                            rowData,
                            e.Message
                        );
                    }
                }
                break;
            }
            if (colKey.Count > 0)
            {
                try
                {
                    dt.PrimaryKey = colKey.ToArray();
                    colKey.Clear();
                }
                catch (Exception e)
                {
                    throw new ConvertExtensionException(
                        stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                        "PrimaryKey",
                        e.Message
                    );
                }
            }
            foreach (string jp in jsonParts)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",\"");
                DataRow nr = dt.NewRow();
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string v = rowData.Substring(idx + 1).Replace("\"", "");
                        stSqlite.TablePropertyMapAttribute res = tpmm.findAttr(n, true);
                        if (res != null)
                        {
                            if ((!scanAllRows) && (!dt.Columns.Contains(res.FieldName)))
                            {
                                continue;
                            }
                            if (res.FieldFilterMi != null)
                            {
                                nr[res.FieldName] = res.FieldFilterMi.Invoke(dataclass, new[] { v }) as string;
                            }
                            else
                            {
                                nr[res.FieldName] = v;
                            }
                        }
                        else if (!skipNoMap)
                        {
                            if ((!scanAllRows) && (!dt.Columns.Contains(n)))
                            {
                                continue;
                            }
                            nr[n] = v;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                try
                {
                    dt.Rows.Add(nr);
                }
                catch (Exception e)
                {
                    throw new ConvertExtensionException(
                        stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                        "Add Rows",
                        e.Message
                    );
                }
            }
            return dt;
        }
        ///<summary>
        ///JSON format to <see cref="System.Data.DateDatable"/> .JsonToDataTable(string JsonSource)
        ///</summary>
        ///<param name="jString">JSON serializing source string</param>
        ///<returns>DataTable DB parse data</returns>
        ///<remarks>Warning: support only flat Json array, not object in object</remarks>
        public static DataTable JsonToDataTable(this string jString, bool scanAllRows = true)
        {
            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            string[] jsonParts = Regex.Split(jString.Replace("[", "").Replace("]", ""), "},{");
            if (jsonParts.Length == 0)
            {
                throw new ConvertExtensionException(
                    stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorUndefined,
                    Properties.Resources.ConvertErrorJsonArray
                );
            }
            string[] jsonColumns = ((scanAllRows) ? jsonParts : new string[] { jsonParts[0] });

            foreach (string jp in jsonColumns)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",\"");
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string v = rowData.Substring(idx + 1);
                        if (!dt.Columns.Contains(n))
                        {
                            Type t = SqliteConvertExtension._jsonType(v);
                            dt.Columns.Add(n, t);
                            dt.Columns[n].DefaultValue = SqliteConvertExtension.ColumnsPropertyTypeDefaultValue(t);

                        }
                    }
                    catch (Exception e)
                    {
                        throw new ConvertExtensionException(
                            stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                            rowData,
                            e.Message
                        );
                    }
                }
                break;
            }
            foreach (string jp in jsonParts)
            {
                string[] propData = Regex.Split(jp.Replace("{", "").Replace("}", ""), ",\"");
                DataRow nr = dt.NewRow();
                foreach (string rowData in propData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string n = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string v = rowData.Substring(idx + 1).Replace("\"", "");
                        if ((!scanAllRows) && (!dt.Columns.Contains(n)))
                        {
                            continue;
                        }
                        nr[n] = v;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                try
                {
                    dt.Rows.Add(nr);
                }
                catch (Exception e)
                {
                    throw new ConvertExtensionException(
                        stSqlite.ConvertExtensionException.ConvertErrorType.ConvertErrorToDataTable,
                        "Add Rows",
                        e.Message
                    );
                }
            }
            return dt;
        }
        ///<summary>
        ///<see cref="System.Data.DateDatable"/> to Query DateDatables JSON format .ToJson(DataTable, bool)
        ///Compatible response to jQuery.DateDatables plugin (http://www.datatables.net/)
        ///</summary>
        ///<param name="dtbl">DataTable DB request data</param>
        ///<param name="isStruct">return full web response JSON structure</param>
        ///<param name="isDispose">free instance for end, default true</param>
        ///<param name="updateNextMilliseconds">update Data period in milliseconds, default 0</param>
        public static string ToJson(this DataTable dtbl, bool isStruct = false, bool isDispose = true, int updateNextMilliseconds = 0)
        {
            StringBuilder jString = new StringBuilder();
            if ((dtbl != null) && (dtbl.Rows.Count > 0))
            {
                dtbl.Locale = CultureInfo.InvariantCulture;

                if (isStruct)
                {
                    jString.AppendFormat(
                        SqliteConvertExtension._addSource,
                        dtbl.Rows.Count,
                        ((dtbl.Rows.Count > 50) ? 50 : dtbl.Rows.Count),
                        updateNextMilliseconds
                    );
                }
                else
                {
                    jString.Append("[");
                }
                for (int i = 0; i < dtbl.Rows.Count; i++)
                {
                    jString.Append("{");
                    for (int j = 0; j < dtbl.Columns.Count; j++)
                    {
                        SqliteConvertExtension._jsonFormat(
                            ref jString,
                            dtbl.Columns[j].ColumnName,
                            Convert.ToString(dtbl.Rows[i][j], CultureInfo.InvariantCulture),
                            dtbl.Rows[i][j].GetType(),
                            ((j == (dtbl.Columns.Count - 1)) ? false : true)
                        );
                    }
                    if (i == (dtbl.Rows.Count - 1))
                    {
                        jString.Append("}");
                    }
                    else
                    {
                        jString.Append("},");
                    }
                }
                if (isStruct)
                {
                    jString.Append("]}");
                }
                else
                {
                    jString.Append("]");
                }
            }
            else
            {
                jString.Append(SqliteConvertExtension._emptySource);
            }
            if ((isDispose) && (dtbl != null))
            {
                dtbl.Dispose();
            }
            return jString.ToString();
        }
        ///<summary>
        ///<see cref="System.Data.DataRow"/> to Query DateRow JSON format .ToJson(DateRow)
        ///Compatible response to jQuery.DateDatables plugin (http://www.datatables.net/)
        ///</summary>
        ///<param name="dr">DataRow DB request data</param>
        public static string ToJson(this DataRow dr)
        {
            StringBuilder jString = new StringBuilder();
            if (dr != null)
            {
                jString.Append("{");
                for (int j = 0; j < dr.Table.Columns.Count; j++)
                {
                    SqliteConvertExtension._jsonFormat(
                        ref jString,
                        dr.Table.Columns[j].ColumnName,
                        Convert.ToString(dr[j], CultureInfo.InvariantCulture),
                        dr[j].GetType(),
                        ((j == (dr.Table.Columns.Count - 1)) ? false : true)
                    );
                }
                jString.Append("}");
            }
            else
            {
                jString.Append(SqliteConvertExtension._emptySource);
            }
            return jString.ToString();
        }
        ///<summary>
        ///<see cref="System.Data.DataSet"/> to JSON format .ToJson(DataSet, bool)
        ///Compatible response to jQuery.DateDatables plugin (http://www.datatables.net/)
        ///</summary>
        ///<param name="dset">DataSet DB request data</param>
        ///<param name="isDispose">free instance for end, default true</param>
        public static string ToJson(this DataSet dset, bool isDispose = true)
        {
            int i = 1;
            StringBuilder jString = new StringBuilder();
            if (dset != null)
            {
                jString.Append("{");
                foreach (DataTable table in dset.Tables)
                {
                    jString.Append("\"" + table.TableName + "\":" + SqliteConvertExtension.ToJson(table, false, false));
                    if (dset.Tables.Count > i++)
                    {
                        jString.Append(",");
                    }
                }
                if (isDispose)
                {
                    dset.Dispose();
                }
                jString.Append("}");
            }
            else
            {
                jString.Append(SqliteConvertExtension._emptySource);
            }
            return jString.ToString();
        }
        ///<summary>
        ///<see cref="System.Collections.Generic.IList<T>"/> to JSON format .ToJson(IList<T>, string, bool)
        ///</summary>
        ///<param name="list">IList request data</param>
        ///<param name="JsonName">Json object name</param>
        ///<param name="isDispose">free instance for end, default true</param>
        public static string ToJson<T>(this IList<T> list, string JsonName = null, bool isDispose = true)
        {
            StringBuilder jString = new StringBuilder();
            if ((list != null) && (list.Count > 0))
            {

                if ((list != null) && (string.IsNullOrEmpty(JsonName)))
                {
                    JsonName = list[0].GetType().Name;
                }

                jString.Append("{\"" + JsonName + "\":[");
                
                for (int i = 0; i < list.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    PropertyInfo[] pi = obj.GetType().GetProperties();
                    jString.Append("{");

                    for (int j = 0; j < pi.Length; j++)
                    {
                        SqliteConvertExtension._jsonFormat(
                            ref jString,
                            pi[j].Name.ToString(),
                            Convert.ToString(pi[j].GetValue(list[i], null), CultureInfo.InvariantCulture),
                            pi[j].GetValue(list[i], null).GetType(),
                            ((j < (pi.Length - 1)) ? true : false)
                        );
                    }
                    jString.Append("}");
                    if ((list.Count - 1) < i)
                    {
                        jString.Append(",");
                    }
                }
                if (isDispose)
                {
                    list.Clear();
                }
                jString.Append("]}");
            }
            else
            {
                jString.Append(SqliteConvertExtension._emptySource);
            }
            return jString.ToString();
        }
        ///<summary>
        ///<see cref="System.Object"/> to JSON format .ToJson(IList<T>, string, bool)
        ///</summary>
        ///<param name="jObject">Object request data</param>
        ///<param name="isDispose">free instance for end, default true</param>
        public static string ToJson(object jObject, bool isDispose = true)
        {
            StringBuilder jString = new StringBuilder();
            if (jObject != null)
            {
                jString.Append("{");
                PropertyInfo[] propertyInfo = jObject.GetType().GetProperties();
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    jString.Append("\"" + SqliteConvertExtension._jsonS2J(propertyInfo[i].Name) + "\":");
                    object objectValue = propertyInfo[i].GetGetMethod().Invoke(jObject, null);

                    if (objectValue is DateTime || objectValue is Guid || objectValue is TimeSpan)
                    {
                        jString.Append("'" + objectValue.ToString() + "'");
                    }
                    else if (objectValue is string)
                    {
                        jString.Append("'" + SqliteConvertExtension.ToJson(objectValue.ToString(), false) + "'");
                    }
                    else if (objectValue is IEnumerable)
                    {
                        jString.Append(SqliteConvertExtension.ToJson((IEnumerable)objectValue, false));
                    }
                    else
                    {
                        jString.Append(SqliteConvertExtension.ToJson(objectValue.ToString(), false));
                    }
                    if ((propertyInfo.Length - 1) > i)
                    {
                        jString.Append(",");
                    }
                }
                if (isDispose)
                {
                    jObject = null;
                }
                jString.Append("{");
            }
            else
            {
                jString.Append(SqliteConvertExtension._emptySource);
            }
            return jString.ToString();
        }
        ///<summary>
        ///<see cref="System.Collections.IEnumerable"/> to JSON format .ToJson(IList<T>, string, bool)
        ///</summary>
        ///<param name="jObject">IEnumerable Object request data</param>
        ///<param name="isArray">return Json as array format</param>
        ///<param name="isDispose">free instance for end, default true</param>
        //public static string ToJson(IEnumerable ijObject)
        public static string ToJson(IEnumerable ijObject, bool isArray = false, bool isDispose = true)
        {
            StringBuilder jString = new StringBuilder();
            if (ijObject != null)
            {
                jString.Append(
                    ((isArray) ? "[" : "{")
                );
                foreach (object item in ijObject)
                {
                    IEnumerator enumerator = ijObject.GetEnumerator();
                    jString.Append(SqliteConvertExtension.ToJson(item, false));
                    if (enumerator.MoveNext())
                    {
                        jString.Append(",");
                    }
                }
                jString.Append(
                    ((isArray) ? "]" : "}")
                );
                if (isDispose)
                {
                    ijObject = null;
                }
            }
            else
            {
                jString.Append(SqliteConvertExtension._emptySource);
            }
            return jString.ToString();
        }
        /// <summary>
        /// Convert System.Type to SQLite type field string
        /// </summary>
        /// <param name="type">System.Type input</param>
        /// <returns>return string SQLite type field</returns>
        public static string TablePropertyMapSqlType(Type type)
        {
            switch (type.ToString())
            {
                case "System.Object": { return "BLOB"; }
                case "System.Void":
                case "System.DBNull": { return "NULL"; }

                case "System.String":
                case "System.DateTime": { return "TEXT"; }

                case "System.Char":
                case "System.Byte":
                case "System.SByte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64": { return "INTEGER"; }

                case "System.Boolean":
                case "System.Decimal": { return "NUMERIC"; }

                case "System.Double":
                case "System.Single": { return "REAL"; }

                default: { return "STRING"; }
            }
        }
        /// <summary>
        /// Convert System.Type to SQLite type field string
        /// </summary>
        /// <param name="type">System.Type input</param>
        /// <returns>return string SQLite type field</returns>
        public static object ColumnsPropertyTypeDefaultValue(Type type)
        {
            switch (type.ToString())
            {
                default:
                case "System.Object":
                case "System.Void":
                case "System.DateTime":
                case "System.DBNull": { return null; }

                case "System.String": { return String.Empty; }

                case "System.Char":
                case "System.Byte":
                case "System.SByte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.UInt16":
                case "System.UInt32":
                case "System.UInt64":
                case "System.Boolean":
                case "System.Decimal": { return 0; }

                case "System.Double":
                case "System.Single": { return 0.0; }
            }
        }

        #region private method

        private static void _jsonFormat(ref StringBuilder sb, string name, object val, Type type, bool isNext = false)
        {
            bool isQuoted = true;
            string str = String.Empty;

            if (type == typeof(string))
            {
                str = SqliteConvertExtension._jsonS2J(val as String);
            }
            else if (type == typeof(bool))
            {
                str = val as String;
                str = str.ToLower();
                isQuoted = false;
            }
            else if (type == typeof(System.Single))
            {
                if (val == null)
                {
                    str = "0.0";
                }
                isQuoted = false;
            }
            else if (
                (type == typeof(System.Int64)) ||
                (type == typeof(System.Int32)) ||
                (type == typeof(System.Int16))
               )
            {
                if (val == null)
                {
                    str = "0";
                }
                isQuoted = false;
            }
            else if ((type == typeof(DateTime)) || (type != typeof(string)))
            {
                str = val.ToString();
            }
            sb.AppendFormat(
                CultureInfo.InvariantCulture,
                ((isQuoted) ? "\"{0}\":\"{1}\"{2}" : "\"{0}\":{1}{2}"),
                name,
                ((string.IsNullOrWhiteSpace(str)) ? val : str),
                ((isNext) ? "," : "")
            );
        }
        private static string _jsonS2J(String s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        {
                            sb.Append("\\\""); break;
                        }
                    case '\\':
                        {
                        sb.Append("\\\\"); break;
                        }
                    case '/':
                        {
                        sb.Append("\\/"); break;
                        }
                    case '\b':
                        {
                        sb.Append("\\b"); break;
                        }
                    case '\f':
                        {
                        sb.Append("\\f"); break;
                        }
                    case '\n':
                        {
                        sb.Append("\\n"); break;
                        }
                    case '\r':
                        {
                        sb.Append("\\r"); break;
                        }
                    case '\t':
                        {
                        sb.Append("\\t"); break;
                        }
                    default:
                        {
                        sb.Append(c); break;
                        }
                }
            }
            return sb.ToString();
        }
        private static Type _jsonType(string src)
        {
            if (src[0] == '"')
            {
                System.DateTime dDate;
                if (DateTime.TryParse(src.Replace("\"", ""), out dDate))
                {
                    return typeof(System.DateTime);
                }
                return typeof(System.String);
            }
            else if (src.Contains("null"))
            {
                return typeof(System.DBNull);
            }
            else if ((src.Contains("true")) || (src.Contains("false")))
            {
                return typeof(System.Boolean);
            }
            else if (Char.IsNumber(src[0]))
            {
                int isDot = 0;
                for (int i = 0; i < src.Length; i++)
                {
                    if (!Char.IsNumber(src[i]))
                    {
                        if ((src[i] == '.') || (src[i] == ','))
                        {
                            isDot++;
                        }
                        else
                        {
                            System.DateTime dDate;
                            if (DateTime.TryParse(src, out dDate))
                            {
                                return typeof(System.DateTime);
                            }
                            else
                            {
                                return typeof(System.String);
                            }
                        }
                    }
                }
                if (isDot == 1)
                {
                    return typeof(System.Double);
                }
                else if (isDot == 0)
                {
                    return typeof(System.Decimal);
                }
                else
                {
                    return typeof(System.String);
                }
            }
            return typeof(System.String);
        }

        #endregion
    }
}
