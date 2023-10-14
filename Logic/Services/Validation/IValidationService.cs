using VidifyStream.Data.Dtos;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VidifyStream.Logic.Services.Validation
{
    /// <summary>
    /// Service responsible for validating data using FluentValidation.
    /// </summary>
    public interface IValidationService
    {
        /// <summary>
        /// Validates the specified object synchronously.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>A <see cref="ServiceResponse"/> containing the validation result as a <see cref="ModelStateDictionary"/>.</returns>
        ServiceResponse<ModelStateDictionary> Validate<T>(T obj);
        /// <summary>
        /// Validates the specified object asynchronously.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <returns>A <see cref="ServiceResponse"/> containing the validation result as a <see cref="ModelStateDictionary"/>.</returns>
        Task<ServiceResponse<ModelStateDictionary>> ValidateAsync<T>(T obj);
    }
}
