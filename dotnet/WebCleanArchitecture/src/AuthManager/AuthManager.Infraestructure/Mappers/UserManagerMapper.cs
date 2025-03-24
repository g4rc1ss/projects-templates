using AuthManager.Domain.BusinessObjects;
using Infraestructure.Database.Entities;
using Riok.Mapperly.Abstractions;

namespace AuthManager.Infraestructure.Mappers;

[Mapper]
public static partial class UserManagerMapper
{
#if (UseIdentity)
    [MapProperty(nameof(@UserData.Password), nameof(@UserEntity.PasswordHash))]
#endif
    public static partial UserEntity ToEntity(this UserData userData);

#if (UseIdentity)
    [MapProperty(nameof(@UserEntity.PasswordHash), nameof(@UserData.Password))]
    public static partial UserData ToUserData(this UserEntity userEntity);
#endif
}