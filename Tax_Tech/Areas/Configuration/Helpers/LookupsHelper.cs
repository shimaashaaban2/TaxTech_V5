using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tax_Tech.Areas.Configuration.Repository;

namespace Tax_Tech.Areas.Configuration.Helpers
{
    public class LookupsHelper
    {
        private static LookupsHelper _instance;
        LookupApiRepository _lookupApiRepository = new LookupApiRepository();
        public static LookupsHelper GetLookups()
        {
            if (_instance == null)
            {
                _instance = new LookupsHelper();
            }
            return _instance;
        }


        public SelectList EntitiesList(int? id)
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public SelectList EntitiesListByUser(int? id, long UserID)
        {
            try
            {
                var S = AccountsApiRepository.GetAccounts().GetEntitiesListByUser(UserID).Where(q => q.IsActive == true).ToList();
                return new SelectList(S, "EntityId", "EntityTitle", id);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public SelectList RoleList(int? ID)
        {
            try
            {
                var RoleList =  _lookupApiRepository.GetRoleList();
                return new SelectList(RoleList, "RoleID", "RoleTitle", ID);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}