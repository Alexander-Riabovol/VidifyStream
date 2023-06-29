using Data.Dtos;
using Data.Dtos.Comment;
using Data.Dtos.Notification;
using Data.Dtos.User;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Logic.Services.ValidationService
{
    public class ValidationService : IValidationService
    {
        private readonly IValidator<NotificationAdminCreateDTO> _notificationAdminCreateDTOValidator;
        private readonly IValidator<CommentPostDTO> _commentPostDTOValidator;
        private readonly IValidator<ReplyPostDTO> _replyPostDTOValidator;
        private readonly IValidator<UserLoginDTO> _userLoginDTOValidator;
        private readonly IValidator<UserRegisterDTO> _userRegisterDTOValidator;

        public ValidationService(IValidator<NotificationAdminCreateDTO> notificationAdminCreateDTOValidator,
                                 IValidator<CommentPostDTO> commentPostDTOValidator,
                                 IValidator<ReplyPostDTO> replyPostDTOValidator,
                                 IValidator<UserLoginDTO> userLoginDTOValidator,
                                 IValidator<UserRegisterDTO> userRegisterDTOValidator)
        {
            _notificationAdminCreateDTOValidator = notificationAdminCreateDTOValidator;
            _commentPostDTOValidator = commentPostDTOValidator;
            _replyPostDTOValidator = replyPostDTOValidator;
            _userLoginDTOValidator = userLoginDTOValidator;
            _userRegisterDTOValidator = userRegisterDTOValidator;
        }

        // TO DO: Test preformance with ValueTask return type
        public async Task<ServiceResponse<ModelStateDictionary>> Validate<T>(T obj)
        {
            if (obj == null) 
                return new ServiceResponse<ModelStateDictionary>(400, "The server cannot process the request because the provided object is null.");
            dynamic validator;
            bool isAsync;
            if (obj is NotificationAdminCreateDTO) { validator = _notificationAdminCreateDTOValidator; isAsync = true; }
            else if (obj is CommentPostDTO) { validator = _commentPostDTOValidator; isAsync = true; }
            else if (obj is ReplyPostDTO) { validator = _replyPostDTOValidator; isAsync = true; }
            else if (obj is UserLoginDTO) { validator = _userLoginDTOValidator; isAsync = false; }
            else if (obj is UserRegisterDTO) { validator = _userRegisterDTOValidator; isAsync = false; }
            else return new ServiceResponse<ModelStateDictionary>(500, $"Validation error: no appropriate validator found for the {obj.GetType()} type.");

            ValidationResult validationResult = isAsync ? await validator.ValidateAsync(obj) : validator.Validate(obj);

            if(!validationResult.IsValid)
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
        // If it is possible, maybe it is better to do a couple of IFormFile validator classes, 
        // and use them directly in controllers, to not rewrite logic in case of IFormFile being a part of a model.
        public ServiceResponse<ModelStateDictionary> Validate(IFormFile formFile, string requiredType)
        {
            if(!formFile.ContentType.Contains(requiredType))
            {
                var modelStateDictionary = new ModelStateDictionary();

                modelStateDictionary.AddModelError(
                    "ContentType",
                    $"Invalid file format. Please ensure you are uploading a file with the correct content type: {requiredType}.");

                return new ServiceResponse<ModelStateDictionary>(400, "", modelStateDictionary);
            }

            return ServiceResponse<ModelStateDictionary>.OK(null);
        }
    }
}
