using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace VidifyStream.Logic.Validators
{
    public class FormFileValidator : AbstractValidator<IFormFile>
    {
        public FormFileValidator() 
        {
            RuleFor(x => x.ContentType).NotEmpty()
            .Custom((contentType, context) =>
            {
                if (context.RootContextData.ContainsKey("content-type"))
                {
                    var expectedContentType = context.RootContextData["content-type"] as string;
                    if (expectedContentType != null && !contentType.Contains(expectedContentType))
                    {
                        context.AddFailure("ContentType",
                        $"Invalid file format. Please ensure you are uploading a file with the correct content type: {expectedContentType}.");
                    }
                }
            });
        }
    }
}
