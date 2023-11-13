using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Logic.Extensions;

namespace VidifyStream.Logic.CQRS.Comments.Commands.Put
{
    public class PutCommentCommandHandler :
        IRequestHandler<PutCommentCommand, ServiceResponse>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;

        public PutCommentCommandHandler(DataContext dataContext,
                                        IHttpContextAccessor accessor,
                                        IMapper mapper)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(PutCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _dataContext.Comments.FindAsync(request.CommentDto.CommentId);

            if (comment == null)
            {
                return new ServiceResponse(404, $"Comment with ID {request.CommentDto.CommentId} does not exist.");
            }

            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse(idResult.StatusCode, idResult.Message!);

            if (idResult.Content != comment.UserId)
            {
                return new ServiceResponse(403, "Forbidden");
            }

            comment = _mapper.Map(request.CommentDto, comment);

            _dataContext.Update(comment);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse.OK;
        }
    }
}
