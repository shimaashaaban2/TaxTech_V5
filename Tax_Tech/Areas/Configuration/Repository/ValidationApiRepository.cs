using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Tax_Tech.ApiModels;
using Tax_Tech.Helpers;
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class ValidationApiRepository
    {
        private static ValidationApiRepository _instance;

        public static ValidationApiRepository Get()
        {
            if (_instance == null)
            {
                _instance = new ValidationApiRepository();
            }
            return _instance;
        }

        public ApiResponse CheckVendorExists(string ERPInternalID, string PersonalID)
        {
            HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v1/WEB/Validation/CheckVendorExists?ERPInternalID={ERPInternalID}&PersonalID={PersonalID}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        public ApiResponse CheckBranchExists(string ERPInternalID, string PersonalID)
        {
            HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v1/WEB/Validation/CheckBranchExists?ERPInternalID={ERPInternalID}&PersonalID={PersonalID}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        public ApiResponse CheckItemsExists(string ERPInternalID, string Code)
        {
            HttpResponseMessage result = ApiRepository.GetAPI().PostResponse($"v1/WEB/Validation/CheckItemExists?ERPInternalID={ERPInternalID}&Code={Code}", new { });

            return new ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }
    }
}