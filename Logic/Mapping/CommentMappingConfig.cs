using Data.Dtos.Comment;
using Data.Models;
using Mapster;

namespace Logic.Mapping
{
    public class CommentMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CommentPutDTO, Comment>()
                  .Map(dest => dest.Edited, src => true);
        }
    }
}
