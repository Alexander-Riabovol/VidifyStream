using Data.Dtos.Comment;
using Data.Models;
using Mapster;

namespace Logic.Mapping
{
    /// <summary>
    /// Configuration class for mapping CommentDTOs to <see cref="Comment"/> models and vice versa.
    /// </summary>
    internal class CommentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CommentPutDTO, Comment>()
                  .Map(dest => dest.Edited, src => true);
        }
    }
}
