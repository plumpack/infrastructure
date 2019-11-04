namespace PlumPack.Web
{
    public class BaseResponse
    {
        protected BaseResponse()
        {
            Success = true;
        }
        
        public bool Success { get; set; }
        
        public string ErrorMessage { get; set; }
    }
}