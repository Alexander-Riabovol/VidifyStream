using Data.Dtos.User;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Logic.Validations
{
    public class UserProfilePicturePostDTOValidator : AbstractValidator<UserProfilePicturePostDTO>
    {
        public UserProfilePicturePostDTOValidator(IValidator<IFormFile> fileValidator)
        {
            RuleFor(x => x.File).Custom((file, content) => 
                                        content.RootContextData.Add("content-type", "image"))
                                .SetValidator(fileValidator);
        }
    }
}
