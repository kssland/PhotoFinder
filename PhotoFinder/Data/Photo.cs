namespace PhotoFinder.Data
{
    // 사진 1장에 대한 정보를 담는 클래스
    public class Photo
    {
        public string Id { get; set; } //사진 아이디
        public int Width { get; set; } //사진 width
        public int Height { get; set; } //사진 height
        public string UserName { get; set; } //사용자 이름
        public string Description { get; set; } //사진 설명
        public string CreatedTime { get; set; } //업로드 날짜
        public string UpdatedTime { get; set; } //수정 날짜
        public int Likes { get; set; } //좋아요 수
        public string ToolTipText { get; set; } //툴팁 텍스트
        public string DownloadUrl { get; set; } //썸네일 이미지 URL 경로
    }
}

