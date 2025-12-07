using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tax_Tech.ApiModels;

namespace Tax_Tech.Helpers
{
    public class GenerateAPIRespons
    {
        public static ApiResponse ApiResponseGenerate(int ResponseID,string Response)
        {
            CustomResponse CustomeRespons = new CustomResponse();
            ApiResponse returnResponse= new ApiResponse();
            CustomeRespons.ResponseID =Convert.ToString(ResponseID);
            CustomeRespons.ResponseMsg = Response;

            returnResponse.CustomeRespons = CustomeRespons;
            return returnResponse;
        }
    }
}