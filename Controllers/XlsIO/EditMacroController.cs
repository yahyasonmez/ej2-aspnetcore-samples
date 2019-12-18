﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.XlsIO;
using Syncfusion.Drawing;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.Office;

namespace EJ2CoreSampleBrowser.Controllers.XlsIO
{
    public partial class XlsIOController : Controller
    {
        #region Edit Macro
        public ActionResult EditMacro(string SaveOption)
        {
            if (SaveOption == null)
                return View();

            string basePath = _hostingEnvironment.WebRootPath;

            //Step 1 : Instantiate the spreadsheet creation engine.
            ExcelEngine excelEngine = new ExcelEngine();
            //Step 2 : Instantiate the excel application object.
            IApplication application = excelEngine.Excel;

            // Opening workbook
            FileStream inputStream = new FileStream(basePath + @"/XlsIO/EditMacroTemplate.xltm", FileMode.Open, FileAccess.Read);
            IWorkbook workbook = application.Workbooks.Open(inputStream);
            IWorksheet sheet = workbook.Worksheets[0];


            #region VbaProject
            IVbaProject vbaProject = workbook.VbaProject;
            IVbaModule vbaModule = vbaProject.Modules["Module1"];
            vbaModule.Code = vbaModule.Code.Replace("xlAreaStacked", "xlLine");
            #endregion

            string ContentType = null;
            string fileName = null;

            MemoryStream ms = new MemoryStream();

            if (SaveOption == "ExcelXls")
            {
                ContentType = "Application/vnd.ms-excel";
                fileName = "Sample.xls";
                workbook.Version = ExcelVersion.Excel97to2003;
                workbook.SaveAs(ms);
            }
            else if(SaveOption == "ExcelXlsm")
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12";
                fileName = "Sample.xlsm";
                workbook.SaveAs(ms, ExcelSaveType.SaveAsMacro);
            }
            else
            {
                workbook.Version = ExcelVersion.Excel2013;
                ContentType = "application/vnd.ms-excel.template.macroEnabled.12";
                fileName = "Sample.xltm";
                workbook.SaveAs(ms, ExcelSaveType.SaveAsMacroTemplate);
            }

            ms.Position = 0;

            return File(ms, ContentType, fileName);

        }
        #endregion
    }
}