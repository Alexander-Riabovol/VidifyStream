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
        public static readonly ServiceResponse NotModified = new ServiceResponse()
        {
            IsError = true,
            StatusCode = 304,
            Message = "Not Modified"
        };
    }
    public record ServiceResponse<T> : ServiceResponse
    {
        public T? Content { get; init; }
        public ServiceResponse() {}
        public ServiceResponse(int statusCode, string message) : base(statusCode, message) {}
        public ServiceResponse(int statusCode, string message, T content) : this(statusCode, message) 
        { 
            Content = content;
        }
        public new static ServiceResponse<T> OK(T? content)
        {
            return new ServiceResponse<T>()
            {
                IsError = false,
                StatusCode = 200,
                Message = "",
                Content = content
            };
        }
    }
}
