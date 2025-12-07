using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tax_Tech.ApiModels;
using Tax_Tech.Helpers;

namespace Tax_Tech.Repository
{
    public class JobQueueApiRepository
    {

        public IEnumerable<JobQueue> GetJobQueues()
        {
            HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetJobsQueueList", null);

            return new ResponseResolver<JobQueue>().ResolveListResponse(result);
        }

        public IEnumerable<JobQueue> GetJobQueues(string jobType)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetJobsQueueListByJobType?JobType={jobType}", null);

                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<IEnumerable<JobQueue>>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<JobQueueDetails> GetJobQueueTrackByJobID(string jobID)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetJobsTrackList?JobID={jobID}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<IEnumerable<JobQueueDetails>>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ApiResponse GetJobsQueueByJobID(string jobID)
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetJobsQueueByJobID?JobID={jobID}", null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public ApiResponse InsertJobQueue(string fileName, long actionBy, string jobType, int? TemplateID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/JobsQueueInsert?CreatedBy={actionBy}&FileName={fileName}&JobType={jobType}&TemplateID={TemplateID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region New InsertJobQueue
        public ApiResponse NewInsertJobQueue(string fileName, long actionBy, string jobType, int? TemplateID)
        {
            try
            {
                HttpResponseMessage result = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/JobsQueueInsert?CreatedBy={actionBy}&FileName={fileName}&JobType={jobType}&TemplateID={TemplateID}", null);

                result.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<ApiResponse>(result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        public IEnumerable<JobQueueType> GetJobQueueTypes()
        {
            try
            {
                HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Operations/GetJobTypeList", new { });

                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<IEnumerable<JobQueueType>>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<StatusStatisticsApiModel> GetStatusStatistics()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse("v2/WEB/JobsQueue/StatusStatistics", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<StatusStatisticsApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<ImportStatsticsApiModel> GetImportStatstics(long? jobId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Jobs/ImportStatstics/Rep1?JobID={jobId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<ImportStatsticsApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<ImportStatstics2ApiModel> GetImportStatstics2(long? jobId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Jobs/ImportStatstics/Rep2?JobID={jobId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<ImportStatstics2ApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<ImportStatstics3ApiModel> GetImportStatstics3(long? jobId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Jobs/ImportStatstics/Rep3?JobID={jobId}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<ImportStatstics3ApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<RuningJobQueueApiModel> GetRunningList()
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/JobsQueue/RuningList", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<RuningJobQueueApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<JobsTrackerStatisticsApiModel> GetJobsTrackerStatistics(long? jobId, byte? logType)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/JobsQueue/JobsTrackerStatistics?JobID={jobId}&LogType={logType}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<JobsTrackerStatisticsApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<JobQueueEnhancedDetailsApiModel> GetJobsTrackerDetails(long? jobId, byte? logType)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/JobsQueue/JobsTrackerDetails?JobID={jobId}&LogType={logType}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<JobQueueEnhancedDetailsApiModel>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<DocumentBasicByMOFStatus> DocByMOFStatus(long JobID, string status)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/DocByMOFStatus?JobID={JobID}&status={status.ToLower()}", new { });

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ReadAsStringAsync().Result);
            }

            return JsonConvert.DeserializeObject<IEnumerable<DocumentBasicByMOFStatus>>(response.Content.ReadAsStringAsync().Result);
        }

        public IEnumerable<JobQueueSummaryApiModel> GetJobQueueSummary(long? jobId)
        {
            HttpResponseMessage response = ApiTokenRepository.GetAPI().PostResponse($"v2/WEB/Jobs/ImportStatstics/RepAll?jobId={jobId}", new { });

            return new ResponseResolver<JobQueueSummaryApiModel>().ResolveListResponse(response);
        }
    }
}
