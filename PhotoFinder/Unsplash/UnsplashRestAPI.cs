using PhotoFinder.Network;
using System.Threading.Tasks;
using PhotoFinder.Data;

namespace PhotoFinder.Unsplash
{
    class UnsplashRestAPI
    {
        private static string UNSLPASH_BASE_URI = "https://api.unsplash.com/";
        private HttpRequest httpRequest;

        public UnsplashRestAPI()
        {
            httpRequest = new HttpRequest();
            httpRequest.SetAuthorization("Client-ID", "0e52051cc42824f51efa938facc5fd0472b0131fff08bed97353b066c60ebe7a");
        }

        // 키워드로 사진 검색을 위한 API
        public async Task<ResponseData> GetPhotoListByKeyword(string keyword, int page = 1)
        {
            string url = UNSLPASH_BASE_URI + "search/photos?query=" + keyword + "&page=" + page;
            return await httpRequest.GetAsync(url);
        }

        // URL 이미지 다운로드를 위한 메서드
        public async Task<ResponseData> DownloadStream(string url)
        {            
            return await httpRequest.GetStreamAsync(url);
        }
    }
}
