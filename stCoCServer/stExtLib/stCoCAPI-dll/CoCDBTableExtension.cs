using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using stSqlite;
using System.Data.SQLite;

namespace stCoCAPI
{
    [SQLiteFunctionAttribute(Name = "COCSEASON", Arguments = 2, FuncType = System.Data.SQLite.FunctionType.Scalar)]
    public class WhereSeasonFunction : SQLiteFunction
    {
        private int[] ss = stCoCAPI.CoCAPI.CoCSeason.GetSeasonDateInt();

        public static SQLiteFunctionAttribute GetAttribute()
        {
            return (SQLiteFunctionAttribute)typeof(WhereSeasonFunction)
                .GetCustomAttributes(typeof(SQLiteFunctionAttribute), false)
                .Single();
        }
        public override object Invoke(object[] args)
        {
            return ((
                (Convert.ToInt32(args[0]) == ss[0]) && 
                (Convert.ToInt32(args[1]) == ss[1])
                    ) ? 1 : 0);
        }
    }
    [SQLiteFunctionAttribute(Name = "COCLEAGUEICO", Arguments = 1, FuncType = System.Data.SQLite.FunctionType.Scalar)]
    public class SelectLeagueIcoFunction : SQLiteFunction
    {
        public static SQLiteFunctionAttribute GetAttribute()
        {
            return (SQLiteFunctionAttribute)typeof(WhereSeasonFunction)
                .GetCustomAttributes(typeof(SQLiteFunctionAttribute), false)
                .Single();
        }
        public override object Invoke(object[] args)
        {
            return CoCAPI.GetLeagueUrl(args[0] as string);
        }
    }
    [SQLiteFunctionAttribute(Name = "COCBADGEICO", Arguments = 1, FuncType = System.Data.SQLite.FunctionType.Scalar)]
    public class SelectBadgeIcoFunction : SQLiteFunction
    {
        public static SQLiteFunctionAttribute GetAttribute()
        {
            return (SQLiteFunctionAttribute)typeof(WhereSeasonFunction)
                .GetCustomAttributes(typeof(SQLiteFunctionAttribute), false)
                .Single();
        }
        public override object Invoke(object[] args)
        {
            return CoCAPI.GetBadgeUrl(args[0] as string);
        }
    }
    [SQLiteFunctionAttribute(Name = "COCFLAGICO", Arguments = 2, FuncType = System.Data.SQLite.FunctionType.Scalar)]
    public class SelectFlagIcoFunction : SQLiteFunction
    {
        public static SQLiteFunctionAttribute GetAttribute()
        {
            return (SQLiteFunctionAttribute)typeof(WhereSeasonFunction)
                .GetCustomAttributes(typeof(SQLiteFunctionAttribute), false)
                .Single();
        }
        public override object Invoke(object[] args)
        {
            return CoCAPI.GetFlagUrl(args[0] as string, ((args[1] != null) ? (((Int64)args[1] == 1) ? true : false) : false));
        }
    }
}
