#if (UseIdentity)
using AuthManager.Domain.BusinessObjects;
using Infraestructure.Database.Entities;
#endif
using Riok.Mapperly.Abstractions;

namespace AuthManager.Infraestructure.Mappers;

[Mapper]
public static partial class UserManagerMapper
{
#if (UseIdentity)
    [MapProperty(nameof(@UserData.Password), nameof(@UserEntity.PasswordHash))]
    public static partial UserEntity ToEntity(this UserData userData);


    [MapProperty(nameof(@UserEntity.PasswordHash), nameof(@UserData.Password))]
    public static partial UserData ToUserData(this UserEntity userEntity);
#endif
}