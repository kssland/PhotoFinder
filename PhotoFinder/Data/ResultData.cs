namespace PhotoFinder.Data
{
    public enum RESULT
    {
        SUCCEED,
        FAIL,
    }

    class ResultData
    {
        public RESULT Result;
        public object obj;

        public ResultData(RESULT result, object responseData)
        {
            Result = result;
            obj = responseData;
        }
    }
}
