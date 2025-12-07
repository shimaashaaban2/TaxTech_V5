using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Repository;

namespace Tax_Tech.Helpers
{
    public class LookupsHelper
    {
        private static LookupsHelper _instance;
        public static LookupsHelper GetLookups()
        {
            if (_instance == null)
            {
                _instance = new LookupsHelper();
            }
            return _instance;
        }


        public SelectList EntitiesList(long? id)
        {
            try
            {
                var S = AccountsApiRepository.GetAccounts().GetEntitiesList().Where(q=>q.IsActive==true).ToList();
                return new SelectList(S, "EntityId", "EntityTitle", id);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public SelectList ItemsCodeList(int? id)
        {
            try
            {
                InvoiceApiRepository _InvoiceApiRepository = new InvoiceApiRepository();

                var S = _InvoiceApiRepository.GetActiveItemsList().ToList();
                return new SelectList(S, "ItemSerial", "ItemERPID", id);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public SelectList ItemsNamesList(int? id)
        {
            try
            {
                InvoiceApiRepository _InvoiceApiRepository = new InvoiceApiRepository();

                var S = _InvoiceApiRepository.GetActiveItemsList().ToList();
                return new SelectList(S, "ItemSerial", "ItemName", id);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public string SubmissionSignStatus(byte code)
        {
            if (code == 0)
              return  Resources.Resource.Pending;
            else if (code == 1)
                return Resources.Resource.Signed;
            else if (code == 2)
                return Resources.Resource.Submitted;
            return "";
        }
        public SelectList SubmissionSignStatusList(long? id)
        {
            try
            {
                var List = new List<object>
                {
                    new { Id=100, status = Resources.Resource.All },
                    new { Id=0, status = Resources.Resource.Pending },
                    new { Id=1, status = Resources.Resource.Signed },
                    new { Id=2, status = Resources.Resource.Submitted},                     
                };
                return new SelectList(List, "Id", "status", id);
            }
            catch (Exception ex)
            {
                 return null;
            }
        }

        
    }
}