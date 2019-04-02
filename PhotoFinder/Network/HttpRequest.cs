using PhotoFinder.Data;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PhotoFinder.Network
{    
    class HttpRequest
    {
        private HttpClient client = new HttpClient();

        // Authorization 설정
        public void SetAuthorization(string scheme, string accessKey)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, accessKey);
        }

        // HTTP GET 메서드 - ReadAsString
        public async Task<ResponseData> GetAsync(string url)
        {            
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                object data = await response.Content.ReadAsStringAsync();
                return new ResponseData(RESULT.SUCCEED, data);
            }
            catch (Exception ex)
            {
                return new ResponseData(RESULT.FAIL, ex.Message);
            }
        }

        // HTTP GET 메서드 - ReadAsStream
        public async Task<ResponseData> GetStreamAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                object data = await response.Content.ReadAsStreamAsync();
                return new ResponseData(RESULT.SUCCEED, data);
            }
            catch (Exception ex)
            {
                return new ResponseData(RESULT.FAIL, ex.Message);
            }
        }
    }
}
