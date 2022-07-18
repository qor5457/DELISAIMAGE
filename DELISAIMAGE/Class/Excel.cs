using System;
using System.Data;
using System.Diagnostics;
using Syncfusion.XlsIO;

namespace DELISAIMAGE.Class
{
    public class Excel
    {
        public  Excel()
        {

        }

        public void Excel_Save(DataTable dt)
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
                    workbook.SaveAs(Folder.Filepath + "Data1.xls");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}