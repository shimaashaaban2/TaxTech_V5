using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Repository;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class PermissionRepository
    {
        public IEnumerable<UserPermissionApiModel> GetUserPermissions(long? userId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/ActionsPermissions/ByUserID?UserID={userId}", new { });

            return new Tax_Tech.Helpers.ResponseResolver<UserPermissionApiModel>().ResolveListResponse(response);
        }

        public ApiResponse AddActionToUser(long? actionId, long? userId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Permissions/Add?UserID={userId}&actionId={actionId}", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        public ApiResponse RemoveActionFromUser(long? actionId, long? userId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Permissions/Remove?UserID={userId}&actionId={actionId}", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        public ApiResponse HasPermission(long? actionId, long? userId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Permissions/Get?actionId={actionId}&userId={userId}", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }

        #region New HasPermission
        public ApiResponse NewHasPermission(long? actionId, long? userId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Permissions/Get?actionId={actionId}&userId={userId}", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(response);
        }
        #endregion
    }
}
