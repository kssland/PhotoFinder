using Newtonsoft.Json;
using PhotoFinder.Network;
using System.Threading.Tasks;
using System.Collections.Generic;
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

        // 키워드를 이용해 원하는 사진을 검색하기 위한 API
        public async Task<ResultData> GetPhotoListByKeyword(string keyword, int page = 1)
        {
            string url = UNSLPASH_BASE_URI + "search/photos?query=" + keyword + "&page=" + page;
            return await httpRequest.GetAsync(url);
        }

        // URL 이미지 다운로드를 위한 메서드
        public async Task<ResultData> DownloadStream(string url)
        {            
            return await httpRequest.GetStreamAsync(url);
        }

        // 검색된 사진 리스트 데이터인 Json 형태를 Photo 아이템 클래스로 변환 후 리스트로 만들어주는 메서드
        public List<Photo> MakePhotoItemList(string jsonStr)
        {
            dynamic jobj = JsonConvert.DeserializeObject(jsonStr);
            List<Photo> photoList = new List<Photo>();
            foreach (var item in jobj.results)
            {
                Photo photo = new Photo();
                photo.Id = item.id;
                photo.Description = item.description;
                photo.UserName = item.user.username;
                photo.CreatedTime = item.created_at;
                photo.UpdatedTime = item.updated_at;
                photo.ThumbnailDownloadUrl = item.urls.thumb;
                photo.Width = item.width;
                photo.Height = item.height;
                photo.Likes = item.likes;
                photo.ToolTipText = item.alt_description;
                photoList.Add(photo);
            }
            return photoList;
        }
    }
}
