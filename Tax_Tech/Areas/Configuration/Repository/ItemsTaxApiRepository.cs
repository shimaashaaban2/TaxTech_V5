using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.ApiModels.ViewModels;
using Tax_Tech.Repository;
using Tax_Tech.ViewModels;

namespace Tax_Tech.Areas.Configuration.Repository
{
    public class ItemsTaxApiRepository
    {
       
        public ApiResponse InsertItemTax(ItemsTaxViewModel model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsTaxInsert?ItemID={model.ItemID}&TaxID={model.TaxID}&ActionBy={model.ActionBy}&SubTaxID={model.TaxSubTypeID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse UpdateItemTax(ItemsTaxViewModel model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsTaxUpdate?RecordID={model.RecordID}&TaxID={model.TaxID}&ActionBy={model.ActionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<ItemsTaxViewModel> GetItemsTax(bool activeList, int itemId)
        {
            try
            {
                HttpResponseMessage result = null;

                if(activeList)
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsTaxActiveList?ItemID={itemId}", null);
                }
                else
                {
                    result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetItemsTaxList", null);
                }

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<ItemsTaxViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse ChangeStatus(bool status, int recordId, long actionBy)
        {
            try
            {
                HttpResponseMessage result = null;
                result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/SetItemsTaxActive?RecordID={recordId}&IsActive={status}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<TaxType> GetTaxSubTypes(int? taxId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Lookups/GetTaxTypeSubList?TaxTypeID={taxId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<TaxType>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //Price after tax Saving repo Code here
        public ApiResponse GetPriceAfterTax(PriceAfterTaxModel model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/Update/ItemsPriceAfterTax?itemID={model.itemID}&PriceAfterTax={model.PriceAfterTax}&By={model.By}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse GetPriceAfterTaxMaster(PriceAfterTaxModelMaster model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/Update/ItemsPriceAfterTax/Master?itemID={model.itemID}&PriceAfterTax={model.PriceAfterTax}&basicPrice={model.basicPrice}&T2={model.T2}&T3={model.T3}&DistributionExpenses={model.DistributionExpenses}&MI02={model.MI02}&MI04={model.MI04}&By={model.By}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<PriceDetailsViewModel> GetPriceChanges(Int64? itemID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsPriceAfterTax?itemID={itemID}", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<PriceDetailsViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public IEnumerable<PriceDetailsViewModel> GetPriceChangesMaster(Int64? itemID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemsPriceAfterTax/Master?itemID={itemID}", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<PriceDetailsViewModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }


        public IEnumerable<ItemGroupsListModel> GetItemsGroup()
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemGroupsList", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ItemGroupsListModel>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }
       public IEnumerable<ItemGroupsListByItemID> GetItemsGroupList(long itemId)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/ItemGroupsListByItemID?ItemID={itemId}", null);

                result.EnsureSuccessStatusCode();
                var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<ItemGroupsListByItemID>>(data);
            }
            catch (Exception)
            {
                return null;
            }
        }

         public ApiResponse AddItemsGroup(AddItemGroups model)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/Add/ItemGroups?ItemGroupID={model.ItemGroupID}&ItemID={model.ItemID}&UserID={model.UserID}", null);

                result.EnsureSuccessStatusCode();
                //var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

            public ApiResponse RemoveItemgroup(long Id)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/Delete/ItemGroups?ID={Id}", null);

                result.EnsureSuccessStatusCode();
                //var data = result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        
       
    }
}
