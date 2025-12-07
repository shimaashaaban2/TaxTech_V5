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
    public class BranchesApiRepository
    {
  

        public ApiResponse InsertBranch(BranchViewModel model)
        {
            try
            {
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/BranchInsert?BranchAPIType={model.BranchAPIType}&BranchName={model.BranchName}&PersonalID={model.PersonalID}&BranchCode={model.BranchCode}&CountryID={model.CountryID}&governate={model.Governate}&regionCity={model.RegionCity}&street={model.Street}&buildingNumber={model.BuildingNo}&postalCode={model.PostalCode}&floor={model.Floor}&flat={model.Flat}&landmark={model.Landmark}&additionalInformation={model.AdditionalInformation}&ERPInternalID={model.ERPInternalID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public ApiResponse UpdateBranch(BranchViewModel model)
        {
            try
            {
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/BranchUpdate?BranchAPIType={model.BranchAPIType}&BranchName={model.BranchName}&PersonalID={model.PersonalID}&BranchCode={model.BranchCode}&CountryID={model.CountryID}&governate={model.Governate}&regionCity={model.RegionCity}&street={model.Street}&buildingNumber={model.BuildingNo}&postalCode={model.PostalCode}&floor={model.Floor}&flat={model.Flat}&landmark={model.Landmark}&additionalInformation={model.AdditionalInformation}&ERPInternalID={model.ERPInternalID}&EntityID={model.EntityID}&ActionBy={model.ActionBy}&BranchID={model.BranchID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<BranchViewModel> GetBranches(bool activeList, long EntityID)
        {
            try
            {
                HttpResponseMessage result = null;

                if(activeList)
                {
                    result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetBranchesActiveList?EntityID={EntityID}", null);
                }
                else
                {
                    result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetBranchesList?EntityID={EntityID}", null);
                }

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<BranchViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ApiResponse ChangeBranchStatus(bool status, long branchId, long actionBy)
        {
            try
            {
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/SetBranchActive?BranchID={branchId}&IsActive={status}&ActionBy={actionBy}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<BranchViewModel> GetBranchById(long branchId)
        {
            try
            {
                HttpResponseMessage result =ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Config/GetBranchByID?BranchID={branchId}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<BranchViewModel>>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
