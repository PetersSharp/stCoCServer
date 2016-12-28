using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stCoCAPI
{
    public partial class CoCAPI
    {
        ///<summary>
        /// Clash of Clan season selector
        ///</summary>
        public static class CoCSeason
        {
            public static int[] GetSeasonDateInt()
            {
                return CoCSeason._GetSeasonDate();
            }
            public static string[] GetSeasonDateString()
            {
                int[] dt = CoCSeason._GetSeasonDate();
                return new string[] {
                dt[0].ToString(),
                dt[1].ToString()
            };
            }
            public static string GetSeasonDateDB()
            {
                int[] dt = CoCSeason._GetSeasonDate();
                return string.Format(
                    Properties.Settings.Default.DBWhereDate,
                    dt[0],
                    dt[1]
                );
            }
            public static string GetSeasonDateDB(int idx, string[] cmd)
            {
                int[] dt = CoCSeason._GetDate(idx, cmd);
                return string.Format(
                    Properties.Settings.Default.DBWhereDate,
                    dt[0],
                    dt[1]
                );
            }

            #region private method

            private static int[] _GetSeasonDate()
            {
                DateTime ctm = DateTime.Now;
                int[] dt = { 0, 0 };

                if (
                    (ctm.Day <= 7) &&
                    ((ctm.DayOfWeek - ctm.Day) > 0)
                   )
                {
                    dt[0] = ((ctm.Month == 1) ? (ctm.Year - 1) : ctm.Year);
                    dt[1] = ((ctm.Month == 1) ? 12 : (ctm.Month - 1));
                }
                else
                {
                    dt[0] = ctm.Year;
                    dt[1] = ctm.Month;
                }
                return dt;
            }
            private static void _ParseDateContains(string src, char contains, ref int[] dt)
            {
                string[] digits = src.Split(contains);
                if (digits.Length > 0)
                {
                    for (int i = 0; i < digits.Length; i++)
                    {
                        if (digits[i].All(char.IsDigit))
                        {
                            dt = CoCSeason._ParseDateDefault(digits[i], dt);
                        }
                    }
                }
            }
            private static int[] _ParseDateDefault(string src, int[] dt)
            {
                int[] dtx = CoCSeason._ParseDate(src);
                dt[0] = (((dt[0] == 0) && (dtx[0] > 0)) ? dtx[0] : dt[0]);
                dt[1] = (((dt[1] == 0) && (dtx[1] > 0)) ? dtx[1] : dt[1]);
                return dt;
            }
            private static int[] _ParseDate(string src)
            {
                int[] dt = { 0, 0 };
                int tint;

                if (src.All(char.IsDigit))
                {
                    try
                    {
                        tint = Convert.ToInt32(src);
                    }
                    catch (Exception)
                    {
                        return dt;
                    }
                    if ((tint > 2000) && (tint <= DateTime.Now.Year))
                    {
                        dt[0] = tint;
                    }
                    else if ((tint > 0) && (tint <= DateTime.Now.Month))
                    {
                        dt[1] = tint;
                    }
                }
                return dt;
            }
            private static int[] _GetDate(int idx, string[] cmd)
            {
                int[] dt = { 0, 0 };

                if ((cmd.Length - 1) > idx)
                {
                    int max = ((cmd.Length > (idx + 2)) ? (idx + 2) : (idx + 1));
                    for (int i = max; i > idx; i--)
                    {
                        if (string.IsNullOrWhiteSpace(cmd[i]))
                        {
                            continue;
                        }
                        if (cmd[i].All(char.IsDigit))
                        {
                            dt = CoCSeason._ParseDateDefault(cmd[i], dt);
                        }
                        else if (cmd[i].Contains("-"))
                        {
                            CoCSeason._ParseDateContains(cmd[i], '-', ref dt);
                        }
                        else if (cmd[i].Contains("."))
                        {
                            CoCSeason._ParseDateContains(cmd[i], '.', ref dt);
                        }
                        if ((dt[0] > 0) && (dt[1] > 0))
                        {
                            break;
                        }
                    }
                }
                if ((dt[0] == 0) || (dt[1] == 0))
                {
                    int[] dtc = CoCSeason._GetSeasonDate();
                    dt[0] = ((dt[0] > 0) ? dt[0] : dtc[0]);
                    dt[1] = ((dt[1] > 0) ? dt[1] : dtc[1]);
                }
                return dt;
            }

            #endregion
        }
    }
}