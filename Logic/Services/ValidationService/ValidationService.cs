using Data.Dtos;
using Data.Dtos.Notification;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Logic.Services.ValidationService
{
    public class ValidationService : IValidationService
    {
        private readonly IValidator<NotificationCreateDTO> _notificationCreateDTOValidator;

        public ValidationService(IValidator<NotificationCreateDTO> notificationCreateDTOValidator) 
        {
            _notificationCreateDTOValidator = notificationCreateDTOValidator;
        }

        // TO DO: Test preformance with ValueTask return type
        public async Task<ServiceResponse<ModelStateDictionary>> Validate<T>(T obj)
        {
            if (obj == null) 
                return new ServiceResponse<ModelStateDictionary>(400, "The server cannot process the request because the provided object is null.");
            dynamic validator;
            bool isAsync;
            if (obj is NotificationCreateDTO) { validator = _notificationCreateDTOValidator; isAsync = false; }
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
    }
}
