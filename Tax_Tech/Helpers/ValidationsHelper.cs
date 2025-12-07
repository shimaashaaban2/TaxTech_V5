using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Helpers;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Helpers
{
    public class ValidationsHelper
    {
        private static ValidationsHelper _instance;
        public static ValidationsHelper GetValidations()
        {
            if (_instance == null)
            {
                _instance = new ValidationsHelper();
            }
            return _instance;
        }

        public List<String> LoginValidation(FormCollection frm)
        {
            List<String> errorsListOfStrings = new List<String>();
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["username"]))
                errorsListOfStrings.Add(Resources.Resource.EnterUsername);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["EntityID"]))
                errorsListOfStrings.Add(Resources.Resource.PlsSelectEntity);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["password"]))
                errorsListOfStrings.Add(Resources.Resource.EnterYourPassword);

            return errorsListOfStrings;
        }

        public List<String> RegisterValidation(FormCollection frm)
        {
            bool CheckPassword = true;
            List<String> errorsListOfStrings = new List<String>();
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["username"]))
                errorsListOfStrings.Add(Resources.Resource.EnterUsername);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["EntityID"]))
                errorsListOfStrings.Add(Resources.Resource.PlsSelectEntity);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["DisplayName"]))
                errorsListOfStrings.Add(Resources.Resource.EnterDisplayName);
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["email"]))
                errorsListOfStrings.Add(Resources.Resource.EnterYourEmail);
            else if (!methods.Getmethods().ValidEmail(frm["email"]))
                errorsListOfStrings.Add(Resources.Resource.EnterValidEmail);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["password"]))
            {
                CheckPassword = false;
                errorsListOfStrings.Add(Resources.Resource.EnterYourPassword);
            }
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["Confirmpassword"]))
            {
                CheckPassword = false;
                errorsListOfStrings.Add(Resources.Resource.EnterYourConfirmPassword);
            }
            if (CheckPassword)
            {
                if (!methods.Getmethods().ValidStrongPassword(frm["password"]))
                    errorsListOfStrings.Add(Resources.Resource.PasswordShouldbeStrong);

                if (!methods.Getmethods().ValidComparePassword(frm["password"], frm["Confirmpassword"])) 
                    errorsListOfStrings.Add(Resources.Resource.PasswordDoesnotMatch);

            }
            return errorsListOfStrings;
        }
        public List<String> CheckEmail(FormCollection frm)
        {
             List<String> errorsListOfStrings = new List<String>();
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["ent"]))
                errorsListOfStrings.Add(Resources.Resource.PlsSelectEntity);
            if (!methods.Getmethods().ValidEmail(frm["email"]))
                errorsListOfStrings.Add(Resources.Resource.EnterValidEmail);
 
           
            return errorsListOfStrings;
        }
        public List<String> CheckUserEmail(FormCollection frm)
        {
            List<String> errorsListOfStrings = new List<String>();
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["EntityId"]))
                errorsListOfStrings.Add(Resources.Resource.PlsSelectEntity);
            if (!methods.Getmethods().ValidEmail(frm["email"]))
                errorsListOfStrings.Add(Resources.Resource.EnterValidEmail);
            if(errorsListOfStrings.Count()==0)
            {
                CustomViewModel ResultModel = AccountsApiRepository.GetAccounts().CheckUserEmailExists(Convert.ToInt16(frm["EntityId"]), frm["email"]);
                if (ResultModel != null)
                {
                    string Result = "";
                    string IsExists = "false";
                    if (ResultModel.CustomeRespons.ContainsKey("Response ID"))
                    {
                        Result = ResultModel.CustomeRespons["Response ID"];
                        if (Result == "0")
                        {
                            if (ResultModel.CustomeRespons.ContainsKey("IsExists"))
                                  IsExists = ResultModel.CustomeRespons["IsExists"];
                            if (IsExists.ToLower()== "false")
                                errorsListOfStrings.Add(Resources.Resource.AccountNotRegistered);
                        }
                        else if (Result == "-1")
                            errorsListOfStrings.Add(Resources.Resource.CouldNotCheckYouAccount);
                    }
 
                }

            }

            return errorsListOfStrings;
        }
        public List<String> ChangePasswordValidation(FormCollection frm)
        {
            bool CheckPassword = true;
            List<String> errorsListOfStrings = new List<String>();

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["password"]))
            {
                CheckPassword = false;
                errorsListOfStrings.Add(Resources.Resource.EnterYourPassword);
            }
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["Confirmpassword"]))
            {
                CheckPassword = false;
                errorsListOfStrings.Add(Resources.Resource.EnterYourConfirmPassword);
            }
            
            if (CheckPassword)
            {
                
                if (!methods.Getmethods().ValidStrongPassword(frm["password"]))
                    errorsListOfStrings.Add(Resources.Resource.PasswordShouldbeStrong);

                if (!methods.Getmethods().ValidComparePassword(frm["password"], frm["Confirmpassword"]))
                    errorsListOfStrings.Add(Resources.Resource.PasswordDoesnotMatch);
            }

            return errorsListOfStrings;
        }
    }
}