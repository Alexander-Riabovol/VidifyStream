using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Models;
using VidifyStream.Logic.CQRS.Auth.Queries.Logout;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Users.Commands.Delete.Me
{
    public class DeleteUserMeCommandHandler :
        IRequestHandler<DeleteUserMeCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly ISender _mediator;

        public DeleteUserMeCommandHandler(DataContext dataContext,
                                          IHttpContextAccessor accessor,
                                          ISender mediator)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mediator = mediator;
        }

        public async Task<ServiceResponse> Handle(DeleteUserMeCommand request, CancellationToken cancellationToken)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            var user = await _dataContext.Users.FindAsync(idResult.Content);

            if (user == null)
            {
                return new ServiceResponse(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }
            if (user.Status == Status.Admin)
            {
                return new ServiceResponse(403, "You can't delete your own account because you are an Admin.");
            }

            user.Status = Status.SelfDeleted;

            _dataContext.Remove(user);
            await _dataContext.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new LogoutQuery(), cancellationToken);

            return ServiceResponse.OK;
        }
    }
}
