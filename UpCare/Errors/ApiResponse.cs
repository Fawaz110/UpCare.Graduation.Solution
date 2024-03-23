namespace UpCare.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statusCode, string? message = null) 
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden access",
                404 => "Resource Not Found",
                500 => "Errors are the paths to dark side. Errors lead to anger. Anger lead to hate. Hate lead to career change",
                _ => null
            };
        }
    }
}
