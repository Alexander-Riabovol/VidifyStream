﻿using Data.Dtos;
using Data.Dtos.Comment;
using Data.Dtos.Notification;
using Data.Dtos.User;
using Data.Dtos.Video;
using FluentValidation;
using FluentValidation.Results;
using Logic.Validations;
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
        private readonly IValidator<VideoPostDTO> _videoPostDTOValidator;
        private readonly IValidator<UserProfilePicturePostDTO> _userProfilePicturePostDTOValidator;
        private readonly IValidator<CommentPutDTO> _commentPutDTOValidator;
        private readonly IValidator<IFormFile> _formFileValidator;


        public ValidationService(IValidator<NotificationAdminCreateDTO> notificationAdminCreateDTOValidator,
                                 IValidator<CommentPostDTO> commentPostDTOValidator,
                                 IValidator<ReplyPostDTO> replyPostDTOValidator,
                                 IValidator<UserLoginDTO> userLoginDTOValidator,
                                 IValidator<UserRegisterDTO> userRegisterDTOValidator,
                                 IValidator<VideoPostDTO> videoPostDTOValidator,
                                 IValidator<UserProfilePicturePostDTO> userProfilePicturePostDTOValidator,
                                 IValidator<CommentPutDTO> commentPutDTOValidator,
                                 IValidator<IFormFile> formFileValidator)
        {
            _notificationAdminCreateDTOValidator = notificationAdminCreateDTOValidator;
            _commentPostDTOValidator = commentPostDTOValidator;
            _replyPostDTOValidator = replyPostDTOValidator;
            _userLoginDTOValidator = userLoginDTOValidator;
            _userRegisterDTOValidator = userRegisterDTOValidator;
            _videoPostDTOValidator = videoPostDTOValidator;
            _userProfilePicturePostDTOValidator = userProfilePicturePostDTOValidator;
            _formFileValidator = formFileValidator;
            _commentPutDTOValidator = commentPutDTOValidator;
        }
        // We need a synchronous version to not allocate unneeded memory for Task objects
        public ServiceResponse<ModelStateDictionary> Validate<T>(T obj)
        {
            if (obj == null) 
                return new ServiceResponse<ModelStateDictionary>(400, "The server cannot process the request because the provided object is null.");
            dynamic validator;
            if (obj is UserLoginDTO) { validator = _userLoginDTOValidator; }
            else if (obj is UserRegisterDTO) { validator = _userRegisterDTOValidator; }
            else if (obj is UserProfilePicturePostDTO) { validator = _userProfilePicturePostDTOValidator; }
            else if (obj is VideoPostDTO) { validator = _videoPostDTOValidator; }
            else if (obj is CommentPutDTO) { validator = _commentPutDTOValidator; }
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
            if (obj is NotificationAdminCreateDTO) { validator = _notificationAdminCreateDTOValidator; }
            else if (obj is CommentPostDTO) { validator = _commentPostDTOValidator; }
            else if (obj is ReplyPostDTO) { validator = _replyPostDTOValidator; }
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
