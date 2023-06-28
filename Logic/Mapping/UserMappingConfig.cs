using Data.Dtos.User;
using Data.Models;
using Mapster;
using System.Runtime.CompilerServices;

namespace Logic.Mapping
{
    public class UserMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserRegisterDTO, User>()
                  // Remove all spaces inbetween words
                  .Map(dest => dest.Name, src => string.Join(' ',
                                               src.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries)))
                  // Omit time details
                  .Map(dest => dest.BirthDate, src => src.BirthDate.Date)
                  .Map(dest => dest.Status, src => Status.Unverified);
        }
    }
}
