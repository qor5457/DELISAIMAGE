using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DELISAIMAGE.Model;

namespace DELISAIMAGE.Converter
{
    public static class ListToDataTable
    {
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
        
        public static DataTable MergeTablesByIndex(DataTable First, DataTable second )
        {
            var firstClone = First.Clone();
            foreach (DataColumn col in second.Columns)
            {
                var newColumnName = col.ColumnName;
                var colNum = 1;
                while (firstClone.Columns.Contains(newColumnName))
                {
                    newColumnName = $"{col.ColumnName}_{++colNum}";
                }
                firstClone.Columns.Add(newColumnName, col.DataType);
            }
            var mergedRows = First.AsEnumerable().Zip(second.AsEnumerable(),
                (r1, r2) => r1.ItemArray.Concat(r2.ItemArray).ToArray());
            foreach (object[] rowFields in mergedRows)
                firstClone.Rows.Add(rowFields);
 
            return firstClone;
        }
    }
}