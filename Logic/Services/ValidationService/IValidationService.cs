using Data.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Logic.Services.ValidationService
{
    public interface IValidationService
    {
        ServiceResponse<ModelStateDictionary> Validate<T>(T obj);
        Task<ServiceResponse<ModelStateDictionary>> ValidateAsync<T>(T obj);
    }
}
