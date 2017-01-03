using System;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace stSqlite
{
    public partial class Wrapper
    {
        //
        // For cross platform support (Windows/Mono) use ADO.NET 2.0 Provider for SQLite
        // Url: https://sourceforge.net/projects/sqlite-dotnet2/files/
        //
        private const string thisClass = "[SQLiteDB]: ";
        private SQLiteConnection _dbConn = null;
        private string _dbPath = null;
        private string _connUri = @"Data Source={0};Version=3;UTF8Encoding=True;DateTimeFormat=ISO8601;Cache Size=3000;";
        private string _fmtClear = "SELECT NAME FROM SQLITE_MASTER WHERE type='table' order by NAME;";
        private string _fmtUpdate = "UPDATE {0} SET {1} WHERE {2};";
        private string _fmtInsert = "INSERT INTO {0}({1}) VALUES({2});";
        private string _fmtDelete = "DELETE FROM {0} WHERE {1};";
        private string _fmtDeleteAll = "DELETE FROM {0};";
        private bool _isnewdb = false;
        public bool isNewDB
        {
            get { return this._isnewdb; }
        }

        public Wrapper(string dbname)
        {
            this._Wrapper(dbname, null);
        }
        public Wrapper(string dbname, string connuri)
        {
            this._Wrapper(dbname, connuri);
        }
        ~Wrapper()
        {
            this._Disconnect();
        }

        #region INIT METHOD

        private void _Wrapper(string dbname, string connuri)
        {
            if (string.IsNullOrWhiteSpace(dbname))
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNameEmpty);
            }
            if (!string.IsNullOrWhiteSpace(connuri))
            {
                this._connUri = connuri;
            }
            this._CreatePath(dbname);
            if (string.IsNullOrWhiteSpace(this._dbPath))
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBStringEmpty);
            }
            try
            {
                this._Connect();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        private void _CreatePath(string dbname)
        {
            if (Path.GetFileName(dbname).Length == dbname.Length)
            {
                this._dbPath = Path.Combine(
                    stCore.IOBaseAssembly.BaseDataDir(),
                    dbname
                );
            }
            else
            {
                this._dbPath = dbname;
            }
        }
        private void _Connect()
        {
            try
            {
                if (!File.Exists(this._dbPath))
                {
                    SQLiteConnection.CreateFile(this._dbPath);
                    this._isnewdb = true;
                }
                this._dbConn = new SQLiteConnection(
                    string.Format(this._connUri, _dbPath)
                );
                this._dbConn.Open();
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        private void _Disconnect()
        {
            if (this._Check())
            {
                this._dbConn.Close();
                this._dbConn.Dispose();
                this._dbConn = null;
            }
        }
        private bool _Check()
        {
            return (
                (
                  (this._dbConn == null) ||
                  (
                    (this._dbConn.State == ConnectionState.Closed) ||
                    (this._dbConn.State == ConnectionState.Broken)
                  )
                ) ? false : true
            );
        }

        #endregion

        #region PUBLIC METHOD (SQLiteDataAdapter/DataTable)

        // TODO: upgrade System.Data.SQLite version
        // is support Bind Function (SQLiteFunction.BindFunction)
        //
        public void RegisterFunction(SQLiteFunction sqlfunc)
        {
            var attributes = sqlfunc.GetType().GetCustomAttributes(typeof(SQLiteFunctionAttribute), true);//.Cast<SQLiteFunctionAttribute>().ToArray();
            if (attributes.Length == 0)
            {
                throw new InvalidOperationException(Properties.Resources.DBFunctionEmptyAttribute);
            }
            /*
            SQLiteFunction.BindFunction(this.dbConn, attributes, sqlfunc, null);
             */
        }
        public void RegisterFunction(Type functype)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }
            try
            {
                SQLiteFunction.RegisterFunction(functype);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public DataTable Query(string query)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }

            SQLiteDataAdapter adp = null;

            try
            {
                DataTable dTable = new DataTable();
                adp = new SQLiteDataAdapter(query, this._dbConn);
                adp.Fill(dTable);
                return dTable;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + query + "]"
                );
#else
                throw e;
#endif
            }
            finally
            {
                if (adp != null) { adp.Dispose(); }
            }
        }
        public void QueryNoReturn(string query)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }

            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand(query, this._dbConn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + query + "]"
                );
#else
                throw e;
#endif
            }
            finally
            {
                if (cmd != null) { cmd.Dispose(); }
            }
        }
        public T? QueryOneReturnNum<T>(string query) where T : struct
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }

            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand(query, this._dbConn);
                T? t = (T)cmd.ExecuteScalar();
                if (t != null)
                {
                    return (T)System.Convert.ChangeType(t, typeof(T));
                }
                return null;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + query + "]"
                );
#else
                throw e;
#endif
            }
            finally
            {
                if (cmd != null) { cmd.Dispose(); }
            }
        }
        public string QueryOneReturnString(string query)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }

            SQLiteCommand cmd = null;

            try
            {
                cmd = new SQLiteCommand(query, this._dbConn);
                return (string)cmd.ExecuteScalar();
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + query + "]"
                );
#else
                throw e;
#endif
            }
            finally
            {
                if (cmd != null) { cmd.Dispose(); }
            }
        }
        /// <summary>
        ///     Allows the programmer to easily update DataTable fill and set to DB.
        /// </summary>
        /// <param name="query">SQL query string, get another data similar DataTable</param>
        /// <param name="dTable">A DataTable source to set new values.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Update(string query, DataTable dTable)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }
            if (
                ((dTable == null) || (dTable.Rows.Count == 0)) ||
                (string.IsNullOrWhiteSpace(query))
               )
            {
                return false;
            }

            SQLiteDataAdapter adp = null;
            SQLiteCommandBuilder bcmd = null;

            try
            {
                adp = new SQLiteDataAdapter(query, this._dbConn);
                bcmd = new SQLiteCommandBuilder(adp);
                adp.Update(dTable);
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + query + "]"
                );
#else
                throw e;
#endif
            }
            finally
            {
                if (adp != null)  { adp.Dispose(); }
                if (bcmd != null) { bcmd.Dispose(); }
            }
        }
        /// <summary>
        ///     Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Update(string tableName, Dictionary<string, string> data, string where)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }
            if (
                ((data == null) || (data.Count == 0)) ||
                (string.IsNullOrWhiteSpace(tableName)) ||
                (string.IsNullOrWhiteSpace(where))
               )
            {
                return false;
            }

            int i = 1;
            StringBuilder sb = new StringBuilder();

            if (data.Count > 0)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    sb.AppendFormat(
                        " {0} = '{1}'{2}",
                        val.Key.ToString(),
                        val.Value.ToString(),
                        (((data.Count - 1) > i) ? "," : "")
                    );
                }
            }
            else
            {
                return false;
            }
            try
            {
                this.QueryNoReturn(
                    string.Format(
                        _fmtUpdate, tableName, sb.ToString(), where
                    )
                );
                sb.Clear();
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + tableName + "]"
                );
#else
                throw e;
#endif
            }
        }
        /// <summary>
        ///     Allows the programmer to easily insert into the DB
        /// </summary>
        /// <param name="tableName">The table into which we insert the data.</param>
        /// <param name="data">A dictionary containing the column names and data for the insert.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Insert(string tableName, Dictionary<String, String> data)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }
            if (
                ((data == null) || (data.Count == 0)) ||
                (string.IsNullOrWhiteSpace(tableName))
               )
            {
                return false;
            }

            int i = 1;
            StringBuilder sbcol = new StringBuilder(),
                          sbval = new StringBuilder();
            
            foreach (KeyValuePair<String, String> val in data)
            {
                sbcol.AppendFormat(" {0},", val.Key.ToString());
                sbval.AppendFormat(" '{0}',", val.Value);
                if (data.Count > i++)
                {
                    sbcol.Append(",");
                    sbval.Append(",");
                }
            }
            try
            {
                this.QueryNoReturn(
                    string.Format(
                        _fmtInsert, tableName, sbcol.ToString(), sbval.ToString()
                    )
                );
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + tableName + "]"
                );
#else
                throw e;
#endif
            }
            finally
            {
                sbval.Clear();
                sbcol.Clear();
            }
        }
        /// <summary>
        ///     Allows the programmer to easily delete rows from the DB.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Delete(string tableName, string where)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }
            if (
                (string.IsNullOrWhiteSpace(tableName)) ||
                (string.IsNullOrWhiteSpace(where))
               )
            {
                return false;
            }

            try
            {
                this.QueryNoReturn(
                    string.Format(
                        _fmtDelete, tableName, where
                    )
                );
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + tableName + " & " + where + "]"
                );
#else
                throw e;
#endif
            }
        }
        /// <summary>
        ///     Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="tableName">The name of the table to clear.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearTable(string tableName)
        {
            if (!this._Check())
            {
                throw new ArgumentException(thisClass + Properties.Resources.DBNotOpen);
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                return false;
            }
            try
            {
                this.QueryNoReturn(
                    string.Format(
                        _fmtDeleteAll, tableName
                    )
                );
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(
                    thisClass +
                    e.Message +
                    Environment.NewLine +
                    stCore.stConsole.GetTabString(3, 0) +
                    "[" + tableName + "]"
                );
#else
                throw e;
#endif
            }
        }
        /// <summary>
        ///     Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearDB()
        {
            DataTable tables = null;
            try
            {
                tables = this.Query(_fmtClear);
                if ((tables == null) || (tables.Columns.Count == 0))
                {
                    return false;
                }
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                throw new ArgumentException(thisClass + e.Message);
#else
                throw e;
#endif
            }
        }
        public bool Check()
        {
            return this._Check();
        }
        public void Close()
        {
            this._Disconnect();
        }

        #endregion

    }
}
