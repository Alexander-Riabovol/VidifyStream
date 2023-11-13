using MediatR;
using VidifyStream.Data.Dtos;
using VidifyStream.Data.Dtos.Comment;
using VidifyStream.Data.Models;

namespace VidifyStream.Logic.CQRS.Comments.Queries.Get.Video
{
    /// <summary>
    /// Retrieves all <see cref="Comment"/>s associated with a specific <see cref="Video"/>.
    /// </summary>
    public record GetCommentsByVideoQuery(int VideoId) 
        : IRequest<ServiceResponse<IEnumerable<CommentGetDTO>>>;
}
