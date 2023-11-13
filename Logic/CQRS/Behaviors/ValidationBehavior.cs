using FluentValidation;
using MediatR;
using VidifyStream.Data.Dtos;

namespace VidifyStream.Logic.CQRS.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : ServiceResponse, new()
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
                TRequest request,
                RequestHandlerDelegate<TResponse> next,
                CancellationToken cancellationToken)
        {
            if(_validator is null)
            {
                return await next();
            }

            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if(validationResult.IsValid)
            {
                return await next();
            }

            var errors = validationResult.Errors.Select(x => $"{x.PropertyName}: {x.ErrorMessage}");

            dynamic serviceResponse = Activator.CreateInstance(
                    typeof(TResponse), 
                    400, 
                    string.Join('\n', errors))!;

            return serviceResponse;
        }
    }
}
