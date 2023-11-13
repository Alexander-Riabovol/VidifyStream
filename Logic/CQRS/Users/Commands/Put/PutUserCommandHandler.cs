using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Users.Commands.Put
{
    public class PutUserCommandHandler :
        IRequestHandler<PutUserCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public PutUserCommandHandler(DataContext dataContext,
                                     IHttpContextAccessor accessor,
                                     IMapper mapper)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(PutUserCommand request, CancellationToken cancellationToken)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.FindAsync(idResult.Content);

            if (user == null)
            {
                return new ServiceResponse(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }

            user = _mapper.Map(request.UserDto, user);

            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse.OK;
        }
    }
}
