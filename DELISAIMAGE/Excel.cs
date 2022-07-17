using System;
using System.Data;
using System.Diagnostics;
using Syncfusion.XlsIO;

namespace DELISAIMAGE
{
    public static class Excel
    {
        public static void Excel_Save(string filepath, DataTable dt)
        {
            try
            {
                using (ExcelEngine excelEngine = new ExcelEngine())
                {
                    IApplication application = excelEngine.Excel;
                    application.DefaultVersion = ExcelVersion.Xlsx;
                    IWorkbook workbook = application.Workbooks.Create(1);
                    workbook.Worksheets[0].Name = "OutputData";
                    workbook.Worksheets[0].InsertRow(1, 1, ExcelInsertOptions.FormatAsBefore);

                    DataView view = dt.DefaultView;
                    workbook.Worksheets[0].ImportDataView(view, true, 1, 1);
                    workbook.Worksheets[0].UsedRange.AutofitColumns();
                    workbook.SaveAs(filepath);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}