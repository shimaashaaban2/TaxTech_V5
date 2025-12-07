using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.ApiModels;
using Tax_Tech.Areas.Configuration.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Helpers
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

        public List<string> CreateValidation(FormCollection frm)
        {
            bool CheckPassword = true;
            List<string> errorsListOfStrings = new List<string>();
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["username"]))
                errorsListOfStrings.Add(Resources.Resource.EnterUsername);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["EntityID"]))
                errorsListOfStrings.Add(Resources.Resource.PlsSelectEntity);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["DisplayName"]))
                errorsListOfStrings.Add(Resources.Resource.EnterDisplayName);

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["RoleID"]))
                errorsListOfStrings.Add(Resources.Resource.ChooseRole);
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

        public List<string> EditValidation(FormCollection frm)
        {
            List<string> errorsListOfStrings = new List<string>();

            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["DisplayName"]))
                errorsListOfStrings.Add(Resources.Resource.EnterDisplayName);
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["RoleID"]))
                errorsListOfStrings.Add(Resources.Resource.ChooseRole);
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(frm["UserMail"]))
                errorsListOfStrings.Add(Resources.Resource.EnterYourEmail);
            else if (!methods.Getmethods().ValidEmail(frm["UserMail"]))
                errorsListOfStrings.Add(Resources.Resource.EnterValidEmail);

            return errorsListOfStrings;
        }

        public List<string> MOFValidation(byte card, long Entityid, string ClientID, string ClientSecret, string DefaultActivityCode,
                                    string DocumentVersion, string EnviromentBool)
        {
            List<string> errorsListOfStrings = new List<string>();
            if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(Convert.ToString(Entityid)))
                errorsListOfStrings.Add(Resources.Resource.PlsSelectEntity);

            if (card == 1)
            {
                if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(Convert.ToString(ClientID)))
                    errorsListOfStrings.Add(Resources.Resource.Enter + " Client ID");
                if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(Convert.ToString(ClientSecret)))
                    errorsListOfStrings.Add(Resources.Resource.Enter + " Client Secret");
            }
            else if (card == 2)
            {
                if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(Convert.ToString(DefaultActivityCode)))
                    errorsListOfStrings.Add(Resources.Resource.Enter + " Default Activity Code");
                if (methods.Getmethods().isNullOrEmptyOrWhiteSpace(Convert.ToString(DocumentVersion)))
                    errorsListOfStrings.Add(Resources.Resource.Enter + " Document Version");
            }

            return errorsListOfStrings;
        }

        public List<string> ModelValidation(ICollection<ModelState> modelStates)
        {
            List<string> errors = new List<string>();

            foreach (var item in modelStates)
            {
                if (item.Errors.Count >= 1)
                {
                    errors.Add(item.Errors[0].ErrorMessage);
                }
            }
            return errors;
        }

        public List<string> ValidateVendor(VendorViewModel model)
        {
            List<string> errors = new List<string>();
            if (model.VendorAPIType == 1)
            {
                // Business
                if (model.PersonalID.Length != 9 || !methods.Getmethods().IsLong(model.PersonalID))
                {
                    errors.Add(Resources.Resource.BusinessRegIDValidation);
                }

                if(model.CountryID != "65")
                {
                    errors.Add(Resources.Resource.CountryMustBeEgyptIfCustTypeISBOrP);
                }
            }
            else if (model.VendorAPIType == 2)
            {
                // Person
                if (model.PersonalID.Length != 14 || !methods.Getmethods().IsLong(model.PersonalID))
                {
                    errors.Add(Resources.Resource.PersonalRegIDValidation);
                }

                if (model.CountryID != "65")
                {
                    errors.Add(Resources.Resource.CountryMustBeEgyptIfCustTypeISBOrP);
                }
            }
            else if (model.VendorAPIType == 3)
            {
                // Foreign
                if (model.PersonalID != "0")
                {
                    errors.Add(Resources.Resource.RegIDValidation);
                }

                if (model.CountryID == "65")
                {
                    errors.Add(Resources.Resource.CountryCannotBeEgyptWhenCustTypeISF);
                }
            }
            #region CheckExists
            ApiResponse res = ValidationApiRepository.Get().CheckVendorExists(model.ERPInternalID, model.PersonalID);
            if (res.CustomeRespons.ResponseID == "1")
            {
                if (Convert.ToInt32(res.CustomeRespons.ERPInternalID) > 0)
                {
                    errors.Add(Resources.Resource.ERPInternalIDExists);
                }
                //if (Convert.ToInt32(res.CustomeRespons.PersonalID) > 0)
                //{
                //    errors.Add(Resources.Resource.PersonalIDExists);
                //}
            }
            #endregion

            return errors;
        }

        public List<string> ValidateVendorForUpdate(VendorViewModel model)
        {
            List<string> errors = new List<string>();
            if (model.VendorAPIType == 1)
            {
                // Business
                if (model.PersonalID.Length != 9 || !methods.Getmethods().IsLong(model.PersonalID))
                {
                    errors.Add(Resources.Resource.BusinessRegIDValidation);
                }

                if (model.CountryID != "65")
                {
                    errors.Add(Resources.Resource.CountryMustBeEgyptIfCustTypeISBOrP);
                }
            }
            else if (model.VendorAPIType == 2)
            {
                // Person
                if (model.PersonalID.Length != 14 || !methods.Getmethods().IsLong(model.PersonalID))
                {
                    errors.Add(Resources.Resource.PersonalRegIDValidation);
                }

                if (model.CountryID != "65")
                {
                    errors.Add(Resources.Resource.CountryMustBeEgyptIfCustTypeISBOrP);
                }
            }
            else if (model.VendorAPIType == 3)
            {
                // Foreign
                if (model.PersonalID != "0")
                {
                    errors.Add(Resources.Resource.RegIDValidation);
                }

                if (model.CountryID == "65")
                {
                    errors.Add(Resources.Resource.CountryCannotBeEgyptWhenCustTypeISF);
                }
            }
            return errors;
        }

        public List<string> ValidateBranch(BranchViewModel model)
        {
            List<string> errors = new List<string>();
            if (model.BranchAPIType == 1)
            {
                if (model.PersonalID.Length != 9 || !methods.Getmethods().IsLong(model.PersonalID))
                {
                    errors.Add(Resources.Resource.BusinessRegIDValidation);
                }

            }
            else if (model.BranchAPIType == 2)
            {
                if (model.PersonalID.Length != 14 || !methods.Getmethods().IsLong(model.PersonalID))
                {
                    errors.Add(Resources.Resource.PersonalRegIDValidation);
                }
            }
            else if (model.BranchAPIType == 3)
            {
                if (model.PersonalID != "0")
                {
                    errors.Add(Resources.Resource.RegIDValidation);
                }
            }
            #region CheckExists

            ApiResponse res = ValidationApiRepository.Get().CheckVendorExists(model.ERPInternalID, model.PersonalID);
            if (res.CustomeRespons.ResponseID == "1")
            {
                if (Convert.ToInt32(res.CustomeRespons.ERPInternalID) > 0)
                {
                    errors.Add(Resources.Resource.ERPInternalIDExists);
                }
                if (Convert.ToInt32(res.CustomeRespons.PersonalID) > 0)
                {
                    errors.Add(Resources.Resource.PersonalIDExists);
                }
            }
            #endregion

            return errors;
        }

        public List<string> ValidateItems(ItemViewModel model)
        {
            List<string> errors = new List<string>();

            #region CheckExists
            ApiResponse res = ValidationApiRepository.Get().CheckItemsExists(model.ItemERPID, model.ItemCode);
            if (res.CustomeRespons.ResponseID == "1")
            {
                if (Convert.ToInt32(res.CustomeRespons.ERPInternalID) > 0)
                {
                    errors.Add(Resources.Resource.ERPInternalIDExists);
                }
            }
            #endregion

            return errors;
        }

        public List<string> ChangePasswordValidation(FormCollection frm)
        {
            bool CheckPassword = true;
            List<string> errorsListOfStrings = new List<string>();

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