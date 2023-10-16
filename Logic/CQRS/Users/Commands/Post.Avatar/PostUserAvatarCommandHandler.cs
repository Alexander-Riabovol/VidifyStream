using MediatR;
using Microsoft.AspNetCore.Http;
using VidifyStream.Data.Context;
using VidifyStream.Data.Dtos;
using VidifyStream.Logic.Extensions;
using VidifyStream.Logic.Services.Files;

namespace VidifyStream.Logic.CQRS.Users.Commands.Post.Avatar
{
    public class PostUserAvatarCommandHandler
        : IRequestHandler<PostUserAvatarCommand, ServiceResponse<string>>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _accessor;
        private readonly IFileService _fileService;

        public PostUserAvatarCommandHandler(DataContext dataContext,
                                            IHttpContextAccessor accessor,
                                            IFileService fileService)
        {
            _dataContext = dataContext;
            _accessor = accessor;
            _fileService = fileService;
        }

        public async Task<ServiceResponse<string>> Handle(PostUserAvatarCommand request, CancellationToken cancellationToken)
        {
            var idResult = _accessor.HttpContext!.RetriveUserId();
            if (idResult.IsError) return new ServiceResponse<string>(idResult.StatusCode,
                                                                     idResult.Message!);

            var user = await _dataContext.Users.FindAsync(idResult.Content);
            if (user == null)
            {
                return new ServiceResponse<string>(500, $"Unknown error occured: a user with {idResult.Content} was not found.");
            }

            var fileUploadResult = await _fileService.Upload(request.AvatarDto.File);
            if (fileUploadResult.IsError)
            {
                return new ServiceResponse<string>(fileUploadResult.StatusCode, fileUploadResult.Message!);
            }

            user.ProfilePictureUrls.Insert(0, fileUploadResult.Content!);

            _dataContext.Update(user);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return ServiceResponse<string>.OK(user.ProfilePictureUrls.First());
        }
    }
}
