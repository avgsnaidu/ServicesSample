using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace VirtusMobileService.Models
{
    public class ConverterHelper
    {


        sealed class Tuple<T1, T2>
        {
            public Tuple() { }
            public Tuple(T1 value1, T2 value2) { Value1 = value1; Value2 = value2; }
            public T1 Value1 { get; set; }
            public T2 Value2 { get; set; }
        }

        public static List<T> ConvertToList<T>(DataTable table)
        where T : class, new()
        {
            List<Tuple<DataColumn, PropertyInfo>> map =
            new List<Tuple<DataColumn, PropertyInfo>>();

            foreach (PropertyInfo pi in typeof(T).GetProperties())
            {
                if (table.Columns.Contains(pi.Name))
                {
                    map.Add(new Tuple<DataColumn, PropertyInfo>(
                    table.Columns[pi.Name], pi));
                }
            }

            List<T> list = new List<T>(table.Rows.Count);
            foreach (DataRow row in table.Rows)
            {
                if (row == null)
                {
                    list.Add(null);
                    continue;
                }
                T item = new T();
                foreach (Tuple<DataColumn, PropertyInfo> pair in map)
                {
                    object value = row[pair.Value1];
                    if (value is DBNull) value = null;
                    pair.Value2.SetValue(item, value, null);
                }
                list.Add(item);
            }
            return list;
        }


        public static DataTable ConvertToDataTable<T>(List<T> items)
        {
            if (items != null)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);

                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
            else
                return (DataTable)null;
        }


        public static string CheckSingleQuote(string str)
        {
            if (str.Equals("''"))
            {
                return string.Empty;
            }
            else
                return str;
        }




    }

  public  class Utility<T, TReturn> : IConvertible
    {
        public static TReturn Change(T arg)
        {
            return (TReturn)Convert.ChangeType(arg, typeof(TReturn));
        }



        #region IConvertible Members

        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        #endregion
    }


}