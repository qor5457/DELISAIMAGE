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
            foreach (var prop in Props)
            {
               var psd = prop.CustomAttributes.ToList().Find(x => x.AttributeType.Name == "NotDataTableColumnAttribute");
                if (psd == null)
                {
                    dataTable.Columns.Add(prop.Name);
                }
            }

            foreach (T item in items)
            {
                DataRow values = dataTable.NewRow();
                for (int i = 0; i < Props.Length; i++)
                {
                    var psd = Props[i].CustomAttributes.ToList().Find(x => x.AttributeType.Name == "NotDataTableColumnAttribute");
                    if (psd == null)
                    {
                        values[Props[i].Name] = (Props[i].GetValue(item, null)).ToString();
                    }
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static DataTable MergeTablesByIndex(DataTable first, DataTable second )
        {
            var firstClone = first.Clone();
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

            for (int i = 0; i < first.Rows.Count; i++)
            {
                var BlankCount = firstClone.Columns.Count - second.Columns.Count;
                for (int j = 0; j < second.Rows.Count; j++)
                {
                    var row = firstClone.NewRow();
                    if (BlankCount != 0)
                    {
                        row.ItemArray = first.Rows[i].ItemArray.Concat(second.Rows[j].ItemArray).ToArray();
                        BlankCount--;
                    }
                    else
                    {
                        var secondList = second.Rows[j].ItemArray.ToList();
                        for (int k = 0; k < firstClone.Columns.Count - second.Columns.Count; k++)
                        {
                            secondList.Insert(0, "");
                        }
                        row.ItemArray = secondList.ToArray();
                    }
                    firstClone.Rows.Add(row);
                }
            }
            return firstClone;
        }

    }
}