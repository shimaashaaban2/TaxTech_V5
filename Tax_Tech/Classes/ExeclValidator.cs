using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using Tax_Tech.ViewModels;
using System.IO;
using Tax_Tech.Areas.Configuration.Helpers;

namespace Tax_Tech.Classes
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

        public ExeclValidtorResult ValidateExeclFile(string pathToExcelFile, int requiredColCount)
        {
            try
            {
                DataTable dtable = ReadExeclFile(pathToExcelFile);
                int columns = dtable.Columns.Count;
                int rows = dtable.Rows.Count;
                List<string> errors = new List<string>();

                #region check columns and rows
                //if (columns != requiredColCount)
                //{
                //    errors.Add("Please Ensure that the Column Count is " + requiredColCount);
                //    return new ExeclValidtorResult
                //    {
                //        Errors = errors
                //    };
                //}
                if (rows <= 0)
                {
                    errors.Add("Cannot import Empty Execl File");
                    return new ExeclValidtorResult
                    {
                        Errors = errors
                    };
                }
                #endregion

                #region Validating columns
                for (int j = 0; j < dtable.Rows.Count; j++)
                {
                    // checking the document id
                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][0])))
                    {
                        errors.Add($"Please Enter Document ID on column {1} on row {j + 1}");
                    }

                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][1])))
                    {
                        errors.Add($"Please Enter Item ERP Internal ID on column {2} on row {j + 1}");
                    }

                    // checking Quantity
                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][3])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][3])))
                    {
                        errors.Add($"Please Enter Valid Quantity on column {4} on row {j + 1}");
                    }

                    // checking currency
                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][4])))
                    {
                        errors.Add($"Please Enter Valid Currency on column {5} on row {j + 1}");
                    }

                    // checking Amount
                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][5])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][5])))
                    {
                        errors.Add($"Please Enter Valid Amount on column {6} on row {j + 1}");
                    }

                    // checking rate
                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][6])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][6])))
                    {
                        errors.Add($"Please Enter Valid Rate on column {7} on row {j + 1}");
                    }

                    // checking discount
                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][7])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][7])))
                    {
                        errors.Add($"Please Enter Valid Rate on column {8} on row {j + 1}");
                    }

                    // checking Action
                    if (_methods.isNullOrEmptyOrWhiteSpace(Convert.ToString(dtable.Rows[j][8])) || !_methods.HasNumbers(Convert.ToString(dtable.Rows[j][8])))
                    {
                        errors.Add($"Please Enter Valid Action on column {9} on row {j + 1}");
                    }
                }
                #endregion

                return new ExeclValidtorResult
                {
                    Errors = errors,
                    Data = dtable
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    //ErrorLog.GetErrorLog().Insert("Execl Validator", "Validating the execl file", ex.InnerException, ex.InnerException.Message);
                    return new ExeclValidtorResult
                    {
                        Errors = new List<string>{ $"Error: {ex.InnerException.Message}" }
                    };
                }
                else
                {
                    string stackTrace = ex.StackTrace;
                    //ErrorLog.GetErrorLog().Insert("Execl Validator", "Validating the execl file", ex, ex.Message);
                    return new ExeclValidtorResult
                    {
                        Errors = new List<string> {$"Error: {ex.Message}"}
                    };
                }
            }
        }
    }
}