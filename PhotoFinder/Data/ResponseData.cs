namespace PhotoFinder.Data
{
    public enum RESULT
    {
        SUCCEED,
        FAIL
    }

    // 서버로부터 받은 데이터를 저장하는 클래스(성공여부와 데이터)
    class ResponseData
    {
        public RESULT Result { get; set; }
        public object Obj { get; set; }

        public ResponseData(RESULT result, object responseData)
        {
            Result = result;
            Obj = responseData;
        }
    }
}
