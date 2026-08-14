using Dokkan.Api.Contracts.Authentication;
using Dokkan.Api.Contracts.Category;
using Dokkan.Api.Contracts.Users;
using Dokkan.Api.Entities;
using Mapster;

namespace Dokkan.Api.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Email);

        config.NewConfig<CategoryRequest, Category>()
            .Map(dest => dest.IsActive, _ => true);
    }
}
