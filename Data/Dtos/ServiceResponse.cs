namespace Data.Dtos
{
    public record ServiceResponse
    {
        public bool IsError { get; init; } = true;
        public int StatusCode { get; init; } = 500;
        public string? Message { get; init; }
        public ServiceResponse() {}
        public ServiceResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public static readonly ServiceResponse OK = new ServiceResponse()
        {
            IsError = false,
            StatusCode = 200,
            Message = ""
        };
    }
    public record ServiceResponse<T> : ServiceResponse where T : class, new()
    {
        public T? Content { get; init; }
        public ServiceResponse() {}
        public ServiceResponse(int statusCode, string message) : base(statusCode, message) {}
        public ServiceResponse(int statusCode, string message, T content) : this(statusCode, message) 
        { 
            Content = content;
        }
        private static readonly ServiceResponse<T> _OK = (ServiceResponse<T>)ServiceResponse.OK;
        public new ServiceResponse<T> OK(T content)
        {
            return _OK with { Content = content };
        }
    }
}
