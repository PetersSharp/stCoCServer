﻿using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

#if STCLIENTBUILD
using stBase = stClient;
using stConvertExt = stClient.ClientConvertExtension;

namespace stClient

#else
using stBase = stSqlite;
using stConvertExt = stSqlite.SqliteConvertExtension;

namespace stSqlite
#endif
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Method |AttributeTargets.Property | AttributeTargets.Parameter)]
    public class TablePropertyMapAttribute : Attribute
    {
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable<T>"/> map to Columns field name
        /// </summary>
        public string FieldName { get; private set; }
        /// <summary>
        /// Use to map SQlite attribute for query
        /// </summary>
        public string FieldSQL  { get; private set; }
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable{T}"/> map to Columns field type
        /// </summary>
        public Type   FieldType { get; private set; }
        /* TODO: arguments!!
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable{T}"/> map field to Key
        /// </summary>
        public bool FieldKey { get; private set; }
        */
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable{T}"/> map field to Primary Key
        /// </summary>
        public bool FieldPrimaryKey { get; private set; }
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable{T}"/> map field to Unique Key
        /// </summary>
        public bool FieldUnique { get; private set; }
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable{T}"/> map field to Primary AUTOINCREMENT Key
        /// </summary>
        public bool FieldAutoIncrement { get; private set; }
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable{T}"/> map field to function filter name
        /// </summary>
        public string FieldFilterName { get; private set; }
        /// <summary>
        /// Converter <see cref="stSqlite.SqliteConvert.JsonToDataTable{T}"/> map field to function filter <see cref="Systm.Reflection.MethodInfo"/>
        /// </summary>
        public MethodInfo FieldFilterMi { get; private set; }

        //(ispkey, isuniq, type, name, sql, nfilter, mfilter)
        /// <summary>
        /// Init Atribute Map to data class
        /// <example>
        /// <code>
        ///     TablePropertyMapAttribute(
        ///         is primary key,
        ///         is auto increment key,
        ///         is unique,
        ///         System Type,
        ///         name field to DataTable,
        ///         sql type,
        ///         name filter function in data class,
        ///         MethodInfo instance for name filter
        ///     )
        /// </code>
        /// Example data map class:
        /// <code>
        ///         public class MyMmapClass : TablePropertyMapMethod
        ///         {
        ///             [TablePropertyMapAttribute("KeyInJsonName", typeof(System.String), true, true, "MapFilterMy")]
        ///             public string keys { get; set; }
        ///             [TablePropertyMapAttribute("TagInJsonName", typeof(System.String), true, "MapFilterMy")]
        ///             public string tag { get; set; }
        ///             ....
        ///         }
        /// </code>
        /// Example filter function in you data class:
        /// <code>
        ///         public static string MapFilterMy(string src)
        ///         {
        ///             return src.Substring(1);
        ///         }
        /// </code>
        /// public static string NormalizeJsonTable(string jString)
        /// Example use normalize Json source to flat Json
        /// Create function hardcoded name NormalizeTable() in you data class:
        /// <code>
        ///         public static string NormalizeJsonTable(string JsonString)
        ///         {
        ///             ... any replace, delete or/and insert ...
        ///             return JsonString;
        ///         }
        /// </code>
        /// </example>
        /// And more owerwrite this method..
        /// </summary>
        /// <remarks>
        /// Use to create or edit pattern in regex http://regexstorm.net/tester
        /// </remarks>
        /// <param name="name"></param>
        public TablePropertyMapAttribute(string name)
        {
            this._TablePropertyMapAttribute(false, false, false, null, name, null, null, null);
        }
        public TablePropertyMapAttribute(string name, string sql)
        {
            this._TablePropertyMapAttribute(false, false, false, null, name, sql, null, null);
        }
        public TablePropertyMapAttribute(string name, Type type)
        {
            this._TablePropertyMapAttribute(false, false, false, type, name, null, null, null);
        }
        public TablePropertyMapAttribute(string name, Type type, string filter)
        {
            this._TablePropertyMapAttribute(false, false, false, type, name, null, filter, null);
        }
        //
        public TablePropertyMapAttribute(string name, string sql, bool isuniq)
        {
            this._TablePropertyMapAttribute(false, false, isuniq, null, name, sql, null, null);
        }
        public TablePropertyMapAttribute(string name, string sql, bool ispkey, bool isauto)
        {
            this._TablePropertyMapAttribute(ispkey, isauto, false, null, name, sql, null, null);
        }
        //
        public TablePropertyMapAttribute(string name, string sql, bool isuniq, string filter)
        {
            this._TablePropertyMapAttribute(false, false, isuniq, null, name, sql, filter, null);
        }
        public TablePropertyMapAttribute(string name, string sql, bool ispkey, bool isauto, string filter)
        {
            this._TablePropertyMapAttribute(ispkey, isauto, false, null, name, sql, filter, null);
        }
        //
        public TablePropertyMapAttribute(string name, Type type, bool isuniq, string filter)
        {
            this._TablePropertyMapAttribute(false, false, isuniq, type, name, null, filter, null);
        }
        public TablePropertyMapAttribute(string name, Type type, bool ispkey, bool isauto, string filter)
        {
            this._TablePropertyMapAttribute(ispkey, isauto, false, type, name, null, filter, null);
        }
        //
        public TablePropertyMapAttribute(string name, Type type, bool ispkey, bool isauto, bool isuniq)
        {
            this._TablePropertyMapAttribute(ispkey, isauto, isuniq, type, name, null, null, null);
        }
        public TablePropertyMapAttribute(string name, Type type, bool ispkey, bool isauto, bool isuniq, string filter)
        {
            this._TablePropertyMapAttribute(ispkey, isauto, isuniq, type, name, null, filter, null);
        }
        public TablePropertyMapAttribute(string name, Type type, string sql, bool ispkey, bool isauto, bool isuniq, string nfilter)
        {
            this._TablePropertyMapAttribute(ispkey, isauto, isuniq, type, name, sql, nfilter, null);
        }
        // base full constructor
        public TablePropertyMapAttribute(string name, Type type, string sql, bool ispkey, bool isauto, bool isuniq, string nfilter, MethodInfo mfilter)
        {
            this._TablePropertyMapAttribute(ispkey, isauto, isuniq, type, name, sql, nfilter, mfilter);
        }
        
        private void _TablePropertyMapAttribute(
            bool ispkey,
            bool isauto,
            bool isuniq,
            Type type,
            string name,
            string sql,
            string nfilter,
            MethodInfo mfilter
          )
        {
            this.FieldPrimaryKey = ispkey;
            this.FieldAutoIncrement = isauto;
            this.FieldUnique = isuniq;
            this.FieldName = name;
            this.FieldType = ((type == null) ? null : type);
            this.FieldSQL  = ((string.IsNullOrWhiteSpace(sql)) ? null : sql);
            this.FieldFilterName = ((string.IsNullOrWhiteSpace(nfilter)) ? null : nfilter);
            this.FieldFilterMi = mfilter;
        }
        public string ToString(bool isFormat = true)
        {
            return string.Format(
                "\tPrimary key: {}" + ((isFormat) ? Environment.NewLine : ", ") +
                "\tAuto invrement: {}" + ((isFormat) ? Environment.NewLine : ", ") +
                "\tUnique: {}" + ((isFormat) ? Environment.NewLine : ", ") +
                "\tName: {}" + ((isFormat) ? Environment.NewLine : ", ") +
                "\tType: {}" + ((isFormat) ? Environment.NewLine : ", ") +
                "\tSQL tupe: {}" + ((isFormat) ? Environment.NewLine : ", ") +
                "\tFilter name: {}" + ((isFormat) ? Environment.NewLine : ", ") +
                "\tFilter instance: {}" + ((isFormat) ? Environment.NewLine : " "),
                this.FieldPrimaryKey,
                this.FieldAutoIncrement,
                this.FieldUnique,
                ((string.IsNullOrWhiteSpace(this.FieldName)) ? "null" : this.FieldName),
                ((this.FieldType == null) ? "null" : this.FieldType.ToString()),
                ((string.IsNullOrWhiteSpace(this.FieldSQL)) ? "null" : this.FieldSQL),
                ((string.IsNullOrWhiteSpace(this.FieldFilterName)) ? "null" : this.FieldFilterName),
                ((this.FieldFilterMi == null) ? "null" : this.FieldFilterMi.ToString())
            );
        }
    }
    public abstract class TablePropertyMapMethod
    {
        public virtual string InvokeMethod(string methodName, string args)
        {
            return (string)GetType().GetMethod(methodName).Invoke(this, new[] { args });
        }
        public virtual string getAlias(string name, bool isNull = false)
        {
            var attr = this.findAttr(name);
            return ((attr == null) ?
                ((isNull) ? null : name) :
                attr.FieldName
            );
        }
        public virtual string getSQL(string name, bool isNull = false)
        {
            var attr = this.findAttr(name);
            return ((attr == null) ?
                ((isNull) ? null : String.Empty) :
                attr.FieldSQL
            );
        }
        public virtual Type getType(string name, bool isNull = false)
        {
            var attr = this.findAttr(name);
            return ((attr == null) ?
                ((isNull) ? null : typeof(System.String)) :
                attr.FieldType
            );
        }
        public virtual MethodInfo getNormalizeTableFunc()
        {
            try
            {
                return this.GetType().GetMethod("NormalizeJsonTable");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual TablePropertyMapAttribute findAttr(string name, bool isNull = false)
        {
            PropertyInfo[] Props = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                TablePropertyMapAttribute attr = prop.GetCustomAttributes(true)
                    .Where(c => c.GetType() == typeof(TablePropertyMapAttribute))
                    .Cast<TablePropertyMapAttribute>()
                    .FirstOrDefault();
                if (
                    (attr != null) &&
                    (!string.IsNullOrWhiteSpace(attr.FieldName)) &&
                    (attr.FieldName.Equals(name))
                   )
                {
                    Type ptype = ((attr.FieldType != null) ? attr.FieldType :
                            ((prop.PropertyType != null) ? prop.PropertyType : typeof(System.String))
                    );
                    MethodInfo mi = null;
                    if (!string.IsNullOrWhiteSpace(attr.FieldFilterName))
                    {
                        try
                        {
                            mi = this.GetType().GetMethod(attr.FieldFilterName);
                        }
                        catch (Exception)
                        {
                            mi = null;
                        }
#if DEBUGTPM
                        Console.WriteLine("TablePropertyMapAttribute mi: " + 
                            (mi != null).ToString() + " : " + 
                            this.GetType().GetMethod(attr.FieldFilterName).ToString()
                        );
#endif
                    }
                    return new TablePropertyMapAttribute(
                        ((prop.Name != null) ? prop.Name : name),
                        ptype,
                        ((string.IsNullOrWhiteSpace(attr.FieldSQL)) ? stConvertExt.TablePropertyMapSqlType(ptype) : attr.FieldSQL),
                        attr.FieldPrimaryKey,
                        attr.FieldAutoIncrement,
                        attr.FieldUnique,
                        attr.FieldFilterName,
                        mi
                    );
                }
            }
            return ((isNull) ?
                null :
                new TablePropertyMapAttribute(name, typeof(System.String))
            );
        }
    }

}
