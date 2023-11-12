using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VidifyStream.Data.Dtos;

namespace VidifyStream.Logic.Services.Validation
{
    public class ValidationService : IValidationService
    {
        // These are Validators for following DTO models:
        private readonly IValidator<IFormFile> _formFileValidator;


        public ValidationService(IValidator<IFormFile> formFileValidator)
        {
            _formFileValidator = formFileValidator;
        }
        // We need a synchronous version to not allocate unneeded memory for Task objects
        public ServiceResponse<ModelStateDictionary> Validate<T>(T obj)
        {
            if (obj == null) 
                return new ServiceResponse<ModelStateDictionary>(400, "The server cannot process the request because the provided object is null.");

            dynamic validator;
            if (obj is IFormFile) { validator = _formFileValidator; }
            else return new ServiceResponse<ModelStateDictionary>(500, $"Validation error: no appropriate validator found for the {obj.GetType()} type.");

            ValidationResult validationResult = validator.Validate(obj);

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
