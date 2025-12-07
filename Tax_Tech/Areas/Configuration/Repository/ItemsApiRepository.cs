using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class ItemsApiRepository
    {

        public ApiResponse InsertItem(ItemViewModel model)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsInsert?ItemName={model.ItemName}&ItemType={model.ItemType}&ItemERPID={model.ItemERPID}&ItemCode={model.ItemCode}&ItemUnitID={model.ItemUnitID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}", null);

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        public bool? ItemsInsertCheckERPExists(long? entityId, string erp)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsInsertCheckERPExists?entityId={entityId}&erp={erp}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<bool?>(response.Content.ReadAsStringAsync().Result);
        }

        public bool? ItemsUpdateCheckERPExists(long? entityId, string erp, string itemSerial)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsUpdateCheckERPExists?entityId={entityId}&erp={erp}&itemSerial={itemSerial}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<bool?>(response.Content.ReadAsStringAsync().Result);
        }

        public ApiResponse UpdateItem(ItemViewModel model)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsUpdate?ItemName={model.ItemName}&ItemType={model.ItemType}&ItemERPID={model.ItemERPID}&ItemCode={model.ItemCode}&ItemUnitID={model.ItemUnitID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}&ItemSerial={model.ItemSerial}", null);

            return new Tax_Tech.Helpers.ResponseResolver<ApiResponse>().ResolveObjectResponse(result);
        }

        public IEnumerable<ItemViewModel> GetAllItems()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse("v2/WEB/Config/GetItemsList", new { });

            return new Tax_Tech.Helpers.ResponseResolver<ItemViewModel>().ResolveListResponse(response);
        }

        public IEnumerable<ItemViewModel> GetItems(bool activeList)
        {
            HttpResponseMessage result = null;

            if (activeList)
            {
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsActiveList", null);
            }
            else
            {
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsList", null);
            }

            return new Tax_Tech.Helpers.ResponseResolver<ItemViewModel>().ResolveListResponse(result);
        }

        public ApiResponse ChangeStatus(bool status, int itemId, long actionBy)
        {
            try
            {
                HttpResponseMessage result = null;

                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/SetItemsActive?ItemSerial={itemId}&IsActive={status}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ItemViewModel> GetItemById(int itemId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsByID?ItemSerial={itemId}", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New GetIteById
        public IEnumerable<ItemViewModel> NewGetItemById(int itemId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsByID?ItemSerial={itemId}", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public ApiResponse GetItemsCount()
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsCountOfAll", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ItemViewModel> GetItemsByFilter(string Filter)
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsListByFilter?Filter=" + Filter, null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ItemViewModel> GetItemsByFilter(string filter, int? pageNo, int? pageSize)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsListByFilterWithPagination?filter={filter}&pageNo={pageNo}&pageSize={pageSize}", null);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(result.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<ItemViewModel> GetItemsByFilterAndEntityID(long? entityId, string filter, int? pageNo, int? pageSize)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsListByFilterAndEntityIDWithPagination?entityId={entityId}&filter={filter}&pageNo={pageNo}&pageSize={pageSize}", null);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(result.Content.ReadAsStringAsync().Result);
        }

        #region By Entity ID
        public IEnumerable<ItemViewModel> GetItemsByFilterAndEntityID(string Filter, int? entityId)
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsListByFilterAndEntityID?entityId={entityId}&Filter={Filter}", null);

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception(result.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(result.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<ItemViewModel> GetItemsByEntityId(long? entityId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsListByEntityID?entityId={entityId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<ItemViewModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public ApiResponse GetItemsCountByEntityId(long? entityId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsCountByEntityID?entityId={entityId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
        }
        #endregion
    }
}
