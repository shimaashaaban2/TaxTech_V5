using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.IO;

namespace Tax_Tech.Areas.Configuration.Helpers
{
    public class ExeclValidator
    {
        private readonly methods _methods;

        public ExeclValidator()
        {
            _methods = methods.Getmethods();
        }

        public DataTable ReadExeclFile(string pathToExcelFile)
        {
            var excelFile = new ExcelQueryFactory(pathToExcelFile);
            var connectionString = string.Format("Provider='Microsoft.ACE.OLEDB.12.0';Data Source='{0}';Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';", pathToExcelFile);

            if(Path.GetExtension(pathToExcelFile) == ".xls")
            {
                connectionString = string.Format("Provider='Microsoft.ACE.OLEDB.12.0';Data Source='{0}';Extended Properties='Excel 8.0;HDR=YES;IMEX=1';", pathToExcelFile);
            }
            else if (Path.GetExtension(pathToExcelFile) == ".xlsm")
            {
                connectionString = string.Format("Provider='Microsoft.ACE.OLEDB.12.0';Data Source='{0}';Extended Properties='Excel 12.0 Macro;HDR=YES;IMEX=1';", pathToExcelFile);
            }
            else if (Path.GetExtension(pathToExcelFile) == ".xlsb")
            {
                connectionString = string.Format("Provider='Microsoft.ACE.OLEDB.12.0';Data Source='{0}';Extended Properties='Excel 12.0;HDR=YES;IMEX=1';", pathToExcelFile);
            }


            string SheetName = excelFile.GetWorksheetNames().FirstOrDefault();

            var adapter = new OleDbDataAdapter("SELECT * FROM [" + SheetName + "$]", connectionString);
            var ds = new DataSet();

            adapter.Fill(ds, "ExcelTable");
            return ds.Tables["ExcelTable"];
        }

        //public ExeclValidtorResult ValidateExeclFile(string pathToExcelFile, int requiredColCount)
        //{
        //    try
        //    {
        //        DataTable dtable = ReadExeclFile(pathToExcelFile);
        //        int columns = dtable.Columns.Count;
        //        int rows = dtable.Rows.Count;
        //        List<string> errors = new List<string>();

        //        #region check columns and rows
        //        if (columns != requiredColCount)
        //        {
        //            errors.Add(MiddlewareExternalWeb.Resources.Resource.ColNumErrors);
        //        }
        //        if (rows <= 1)
        //        {
        //            errors.Add(MiddlewareExternalWeb.Resources.Resource.ExeclRowsErrMsg);
        //        }
        //        #endregion

        //        #region Validating columns
        //        for (int j = 0; j < dtable.Rows.Count; j++)
        //        {
        //            if (_validationMethods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][0])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][0])))
        //            {
        //                errors.Add($"{Resources.Resource.PlsEnterValidMsisdn} {MiddlewareExternalWeb.Resources.Resource.oncolumn} {1} {MiddlewareExternalWeb.Resources.Resource.onrow} {j + 1}");
        //            }

        //            if (_validationMethods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][1])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][1])))
        //            {
        //                errors.Add($"{Resources.Resource.AmountExeclErr} {MiddlewareExternalWeb.Resources.Resource.oncolumn} {2} {MiddlewareExternalWeb.Resources.Resource.onrow} {j + 1}");
        //            }

        //            if (columns == 3)
        //            {
        //                if (_validationMethods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][2])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][2])))
        //                {
        //                    errors.Add($"{Resources.Resource.PlsEnterAccID} {MiddlewareExternalWeb.Resources.Resource.oncolumn} {3} {MiddlewareExternalWeb.Resources.Resource.onrow} {j + 1}");
        //                }
        //            }
        //        }
        //        #endregion

        //        return new ExeclValidtorResult
        //        {
        //            Errors = errors,
        //            Data = dtable
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.InnerException != null)
        //        {
        //            //ErrorLog.GetErrorLog().Insert("Execl Validator", "Validating the execl file", ex.InnerException, ex.InnerException.Message);
        //            return new ExeclValidtorResult
        //            {
        //                Errors = new List<string>{ $"Error: {ex.InnerException.Message}" }
        //            };
        //        }
        //        else
        //        {
        //            string stackTrace = ex.StackTrace;
        //            //ErrorLog.GetErrorLog().Insert("Execl Validator", "Validating the execl file", ex, ex.Message);
        //            return new ExeclValidtorResult
        //            {
        //                Errors = new List<string> {$"Error: {ex.Message}"}
        //            };
        //        }
        //    }
        //}
    }
}