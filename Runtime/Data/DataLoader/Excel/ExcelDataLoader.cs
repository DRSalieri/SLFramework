using System;
using System.IO;
using OfficeOpenXml;
using UnityEngine;


namespace SLFramework.DataLoader.Excel
{
    public static class ExcelDataLoader
    {
        public static ExcelWorksheet LoadSheet(string filePath, int index)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            ExcelPackage excelPackage = new ExcelPackage(fileInfo);

            if (excelPackage.Workbook.Worksheets.Count <= index)
            {
                Debug.LogWarning("(Excel Data Loader) 读取表单第" + index.ToString() + "页失败：index out of range for " + filePath);
                return null;
            }

            else
            {

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[index];
                Debug.Log("(Excel Data Loader) 读取表单第"+ index.ToString() +"页成功： " + filePath);
                return worksheet;
            }
        }

    }
}
