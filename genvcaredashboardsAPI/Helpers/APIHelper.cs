using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace genvcaredashboardsAPI.Helpers
{
    public static class APIHelper
    {

        public static DateTime GetDOB(this int age)
        {
            DateTime dt = DateTime.Now;
            dt = dt.Date.AddYears(-age);
            return dt;
        }


        public static int GetAge(this DateTime dob)
        {
            DateTime dt = DateTime.Now;
            int age = dt.Year - dob.Year;
            return age;
        }

        public static string GenerateErrorMessage(string customErroMessage, Exception ex)
        {
            string errorMsg = string.Empty;
            if (!string.IsNullOrEmpty(customErroMessage))
            {
                errorMsg = customErroMessage;
            }
            else
            {
                errorMsg = ex.Message + " " + ex.StackTrace;

            }

            return errorMsg;
        }

        public async static Task<string> SendSMS(dynamic obj, string apiUrl, string authKey)
        {
            string result = string.Empty;
            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage
                {

                    Method = HttpMethod.Post,
                    RequestUri = new Uri(apiUrl),
                    Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json")
                };
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authkey", authKey);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application / JSON");
                var response = await httpClient.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return result;
        }


    }
}
