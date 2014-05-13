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

}