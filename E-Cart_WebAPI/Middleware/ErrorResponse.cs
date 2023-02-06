namespace E_Cart_WebAPI.Middleware
{
    internal class ErrorResponse
    {
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
