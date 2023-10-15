using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Data.Dtos.Notification;
using VidifyStream.Data.Dtos.User;

namespace VidifyStream.Logic.Services.Validation
{
    public class ValidationService : IValidationService
    {
        // These are Validators for following DTO models:
        // Comment
        private readonly IValidator<CommentPostDTO> _commentPostDTOValidator;
        private readonly IValidator<CommentPutDTO> _commentPutDTOValidator;
        private readonly IValidator<ReplyPostDTO> _replyPostDTOValidator;
        // Notification
        private readonly IValidator<NotificationAdminCreateDTO> _notificationAdminCreateDTOValidator;
        // User
        private readonly IValidator<UserAdminDeleteDTO> _userAdminDeleteDTOValidator;
        private readonly IValidator<UserProfilePicturePostDTO> _userProfilePicturePostDTOValidator;
        private readonly IValidator<UserPutDTO> _userPutDTOValidator;
        // Other
        private readonly IValidator<IFormFile> _formFileValidator;


        public ValidationService(IValidator<CommentPostDTO> commentPostDTOValidator,
                                 IValidator<CommentPutDTO> commentPutDTOValidator,
                                 IValidator<ReplyPostDTO> replyPostDTOValidator,
                                 IValidator<NotificationAdminCreateDTO> notificationAdminCreateDTOValidator,
                                 IValidator<UserAdminDeleteDTO> userAdminDeleteDTOValidator,
                                 IValidator<UserProfilePicturePostDTO> userProfilePicturePostDTOValidator,
                                 IValidator<UserPutDTO> userPutDTOValidator,
                                 IValidator<IFormFile> formFileValidator)
        {
            _commentPostDTOValidator = commentPostDTOValidator;
            _commentPutDTOValidator = commentPutDTOValidator;
            _replyPostDTOValidator = replyPostDTOValidator;
            _notificationAdminCreateDTOValidator = notificationAdminCreateDTOValidator;
            _userAdminDeleteDTOValidator = userAdminDeleteDTOValidator;
            _userProfilePicturePostDTOValidator = userProfilePicturePostDTOValidator;
            _userPutDTOValidator = userPutDTOValidator;
            _formFileValidator = formFileValidator;
        }
        // We need a synchronous version to not allocate unneeded memory for Task objects
        public ServiceResponse<ModelStateDictionary> Validate<T>(T obj)
        {
            if (obj == null) 
                return new ServiceResponse<ModelStateDictionary>(400, "The server cannot process the request because the provided object is null.");

            dynamic validator;
            if (obj is CommentPutDTO) { validator = _commentPutDTOValidator; }
            else if (obj is UserAdminDeleteDTO) { validator = _userAdminDeleteDTOValidator; }
            else if (obj is UserProfilePicturePostDTO) { validator = _userProfilePicturePostDTOValidator; }
            else if (obj is UserPutDTO) { validator = _userPutDTOValidator; }
            else if (obj is IFormFile) { validator = _formFileValidator; }
            else return new ServiceResponse<ModelStateDictionary>(500, $"Validation error: no appropriate validator found for the {obj.GetType()} type.");

            ValidationResult validationResult = validator.Validate(obj);

            return processValidationResult(validationResult);
        }

        public async Task<ServiceResponse<ModelStateDictionary>> ValidateAsync<T>(T obj)
        {
            if (obj == null)
                return new ServiceResponse<ModelStateDictionary>(400, "The server cannot process the request because the provided object is null.");

            dynamic validator;
            if (obj is CommentPostDTO) { validator = _commentPostDTOValidator; }
            else if (obj is ReplyPostDTO) { validator = _replyPostDTOValidator; }
            else if (obj is NotificationAdminCreateDTO) { validator = _notificationAdminCreateDTOValidator; }
            else return new ServiceResponse<ModelStateDictionary>(500, $"Validation error: no appropriate asynchronous validator found for the {obj.GetType()} type.");

            ValidationResult validationResult = await validator.ValidateAsync(obj);

            return processValidationResult(validationResult);
        }

        private ServiceResponse<ModelStateDictionary> processValidationResult(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (var validationFailure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        validationFailure.PropertyName,
                        validationFailure.ErrorMessage);
                }

                return new ServiceResponse<ModelStateDictionary>(400, "", modelStateDictionary);
            }

            return ServiceResponse<ModelStateDictionary>.OK(null);
        }
    }
}
