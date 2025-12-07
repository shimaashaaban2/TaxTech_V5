 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tax_Tech.Areas.Configuration.Helpers
{
    public class methods
    {
        private methods() { }
        private static methods _instance;
        public static methods Getmethods()
        {
            if (_instance == null)
            {
                _instance = new methods();
            }
            return _instance;
        }

        public string GetRand()
        {
            Random random = new Random();
            char[] keys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890".ToCharArray();
            string rand = Enumerable
                       .Range(1, 5) // for(i.. ) 
                       .Select(k => keys[random.Next(0, keys.Length - 1)])  // generate a new random char 
                       .Aggregate("", (e, c) => e + c); // join into a string
            return rand;
        }
        public string GetRandCode(int lengthOfCode)
        {
            Random random = new Random();
            char[] keys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890".ToCharArray();
            string rand = Enumerable
                       .Range(1, lengthOfCode) // for(i.. ) 
                       .Select(k => keys[random.Next(0, keys.Length - 1)])  // generate a new random char 
                       .Aggregate("", (e, c) => e + c); // join into a string
            return rand;
        }
        public bool NullString(string ToCheck)
        {
            if (ToCheck!=null)
            {
                if (ToCheck.Trim().Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }
            else
            {
                return true;
            }

        }
        public string SinceDays(DateTime date)
        {
            try
            {
                DateTime today = DateTime.Now.Date;
                int NumberOFDay = (today - date).Days;
                return "(Since " + NumberOFDay + " days)";
            }
            catch (Exception ex)
            {
                return ex.InnerException.ToString();
            }
        }
        public bool isNullOrEmptyOrWhiteSpace(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                return true;
            }
            else
                return false;
        }
        public bool ValidEmail(string ToCheck)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(ToCheck);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        public DateTime ConvertDate(string date)
        {
            try
            {
                DateTime Date = Convert.ToDateTime(date);
                return Date;
            }
            catch (Exception ex)
            {
                string val1 = date.Substring(0, 2);
                string val2 = date.Substring(date.IndexOf("/")+1,2 );
                string val3 = date.Substring(date.LastIndexOf("/") + 1, 4);

                string dateNewFormat = val2+"/"+val1+"/"+val3;
                return DateTime.Parse(dateNewFormat);
            }
        }
        public bool IsChecked(string Value)
        {
            bool ValueChecked = false;
            if (Value == "on")
            {
                ValueChecked = true;
            }
            return ValueChecked;
        }
        public string IsCheckedString(string Value)
        {
            string ValueChecked = "0";
            if (Value == "on")
            {
                ValueChecked = "1";
            }
            return ValueChecked;
        }
        
        public bool IsLong(string Value)
        {
            long temp;
            if (long.TryParse(Value, out temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsDecimal(string Value)
        {
            decimal temp;
            if (decimal.TryParse(Value, out temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsDate(string Value)
        {
            DateTime temp;
            if (DateTime.TryParse(Value, out temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string CurrentDateFormat()
        {
            string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            return "The required Date Format is (" + sysFormat + ")";
        }
        public string CurrentDateInputFormat()
        {
            string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            return sysFormat.ToUpper();
        }
        public string GetDateFormat()
        {
            string sysFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            return sysFormat;
        }
        public SelectList OrdersList(int? id)
        {
            Dictionary<string, string> Orders = new Dictionary<string, string>();
            for (int i = 1; i <= 100; i++)
            {
                Orders.Add(i.ToString(), i.ToString());
            }
            return new SelectList(Orders, "Key", "Value", id);

        }

        public string GetFileContentResult(string ext)
        {
            if (ext.ToLower() == ".jpeg" || ext.ToLower() == ".jpg"
                || ext.ToLower() == ".png" || ext.ToLower() == ".gif"
                || ext.ToLower() == ".bmp" || ext.ToLower() == ".tiff")
                return "image/png";
            else if (ext.ToLower() == ".doc")
                return "application/ms-word";
            else if (ext.ToLower() == ".docx")
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            else if (ext.ToLower() == ".pdf")
                return "application/pdf";
            else if (ext.ToLower() == ".txt")
                return "text/plain";
            else if (ext.ToLower() == ".csv")
                return "text/csv";
            else if (ext.ToLower() == ".zip")
                return "application/zip";
            else if (ext.ToLower() == ".rar")
                return "application/rar";
            else if (ext.ToLower() == ".xls")
                return "application/vnd.ms-excel";
            else if (ext.ToLower() == ".xlsx")
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            else if (ext.ToLower() == ".mp3")
                return "audio/mpeg";
            else if (ext.ToLower() == ".wma")
                return "audio/x-ms-wma";
            else if (ext.ToLower() == ".wav")
                return "audio/x-wav";
            else if (ext.ToLower() == ".flac")
                return "audio/flac";
            else if (ext.ToLower() == ".aac")
                return "audio/aac";

            return null;

        }
         
        public string FormatFileSize(string FileSize)
        {
            long FileSizeInt=Convert.ToInt64(FileSize);

            return ((FileSizeInt / 1024) / 1024) + "MB"; 
        }
        public bool ValidStrongPassword(string password)
        {
            bool res = false; 

            if (password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit) && password.Length > 7)
                res = true;
            if (res)
                if (HasSymbols(password))
                    res = true;
            if (res)
                if (HasPreventSymbols(password))
                    res = false;

            return res;
             
        }
        private bool HasSymbols(string word)
        {
            String Symbols = "@ $ % ^ & * !";

            String[] token = Symbols.Split();
            foreach (var L in token)
	        {
                 if(word.Contains(L))
                return true;
	        }
            return false;
        }
        private bool HasPreventSymbols(string word)
        {
            String Symbols = ". #";

            String[] token = Symbols.Split();
            foreach (var L in token)
            {
                if (word.Contains(L))
                    return true;
            }
            return false;
        }
        public bool ValidComparePassword(string password,string Re_enterPassword)
        {
            if (string.Compare(password, Re_enterPassword)==0)
                return true;

            else return false;

        }

        public bool DOB(string Val)
        {
            bool IsValidDOB = false;
            int MinAge = 10;
            bool IsValidDate = IsDate(Val);
            if (IsValidDate)
            {
                DateTime ToDay = DateTime.Now;
                DateTime DOB = Convert.ToDateTime(Val);
                if (DOB.AddYears(MinAge) < ToDay)
                    IsValidDOB = true;
            }

            return IsValidDOB;

        }

        public bool HasNumbers(string txt)
        {
            for(int i = 0; i < txt.Length; i++)
            {
                if (!char.IsDigit(txt[i]))
                    return false;
            }
            return true;
        }
    }
}