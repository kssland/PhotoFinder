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

        // Get 메서드, 리턴은 String
        public async Task<ResultData> GetAsync(string url)
        {            
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                object data = await response.Content.ReadAsStringAsync();
                return new ResultData(RESULT.SUCCEED, data);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                string message;
                message = (ex is HttpRequestException) ? ".NET HttpRequestException" : ".NET Exception";
                message = message + ", raw message: \n\n";
                response.Content = new StringContent(message + ex.Message);
                return new ResultData(RESULT.FAIL, response.StatusCode);
            }
        }

        // URL 이미지 다운로드를 위한 메서드
        public async Task<ResultData> GetStreamAsync(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                object data = await response.Content.ReadAsStreamAsync();
                return new ResultData(RESULT.SUCCEED, data);
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                string message;
                message = (ex is HttpRequestException) ? ".NET HttpRequestException" : ".NET Exception";
                message = message + ", raw message: \n\n";
                response.Content = new StringContent(message + ex.Message);
                return new ResultData(RESULT.FAIL, response.StatusCode);
            }
        }
    }
}
