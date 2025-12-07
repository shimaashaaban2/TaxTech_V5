using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tax_Tech.Controllers
{
    public class WhatsAppController : BaseController
    {
        private readonly string _accessToken = ConfigurationManager.AppSettings["WhatsAppAccessToken"];
        private readonly string _phoneNumberId = ConfigurationManager.AppSettings["WhatsAppPhoneNumberId"];
        private readonly string _graphVersion = ConfigurationManager.AppSettings["WhatsAppGraphVersion"] ?? "v16.0";
        private readonly string _graphBase;

        public WhatsAppController()
        {
            _graphBase = $"https://graph.facebook.com/{_graphVersion}";
        }

        [HttpGet]
        public ActionResult SendFile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendFile(string phoneNumber, HttpPostedFileBase file)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new HttpStatusCodeResult(400, "Phone number is required.");

            if (file == null || file.ContentLength == 0)
                return new HttpStatusCodeResult(400, "File is required.");

            string to = NormalizePhone(phoneNumber);
            if (string.IsNullOrEmpty(to))
                return new HttpStatusCodeResult(400, "Invalid phone number format.");

            try
            {
                using (var http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                    // STEP 1: Send a template message (to open 24h session)
                    var templatePayload = new
                    {
                        messaging_product = "whatsapp",
                        to = to,
                        type = "template",
                        template = new
                        {
                            name = "orange_billing", // لازم يكون Template approved
                            language = new { code = "en_US" }
                        }
                    };
                    var templateJson = JsonConvert.SerializeObject(templatePayload);
                    var templateResp = await http.PostAsync(
                        $"{_graphBase}/{_phoneNumberId}/messages",
                        new StringContent(templateJson, Encoding.UTF8, "application/json")
                    );
                    var templateBody = await templateResp.Content.ReadAsStringAsync();
                    if (!templateResp.IsSuccessStatusCode)
                        return new HttpStatusCodeResult((int)templateResp.StatusCode, "Template send failed: " + templateBody);

                    // STEP 2: Upload the file
                    string mediaId;
                    using (var form = new MultipartFormDataContent())
                    {
                        var streamContent = new StreamContent(file.InputStream);
                        streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(
                            string.IsNullOrEmpty(file.ContentType) ? "application/octet-stream" : file.ContentType
                        );
                        form.Add(streamContent, "file", Path.GetFileName(file.FileName));
                        form.Add(new StringContent("whatsapp"), "messaging_product");

                        var uploadUrl = $"{_graphBase}/{_phoneNumberId}/media";
                        var uploadResp = await http.PostAsync(uploadUrl, form);
                        var uploadBody = await uploadResp.Content.ReadAsStringAsync();

                        if (!uploadResp.IsSuccessStatusCode)
                            return new HttpStatusCodeResult((int)uploadResp.StatusCode, "Upload failed: " + uploadBody);

                        var uploadJson = JObject.Parse(uploadBody);
                        mediaId = (string)uploadJson["id"];
                        if (string.IsNullOrEmpty(mediaId))
                            return new HttpStatusCodeResult(500, "Media ID missing in upload response: " + uploadBody);
                    }

                    // STEP 3: Send document message using media id
                    var filePayload = new
                    {
                        messaging_product = "whatsapp",
                        to = to,
                        type = "document",
                        document = new { id = mediaId, filename = Path.GetFileName(file.FileName) }
                    };
                    var fileJson = JsonConvert.SerializeObject(filePayload);
                    var sendUrl = $"{_graphBase}/{_phoneNumberId}/messages";
                    var resp = await http.PostAsync(sendUrl, new StringContent(fileJson, Encoding.UTF8, "application/json"));
                    var respBody = await resp.Content.ReadAsStringAsync();

                    if (!resp.IsSuccessStatusCode)
                        return new HttpStatusCodeResult((int)resp.StatusCode, "Send failed: " + respBody);

                    return Content("Template sent: " + templateBody + "<br/> File sent successfully. Response: " + respBody);
                }
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, "Exception: " + ex.Message);
            }
        }


        private string NormalizePhone(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            // strip everything except digits
            var digits = Regex.Replace(raw, @"\D", "");
            return digits;
        }
    }
}