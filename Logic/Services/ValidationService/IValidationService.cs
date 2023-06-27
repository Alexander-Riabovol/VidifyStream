using Data.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Logic.Services.ValidationService
{
    public interface IValidationService
    {
        Task<ServiceResponse<ModelStateDictionary>> Validate<T>(T obj);
    }
}
