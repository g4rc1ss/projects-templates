using AuthManager.Domain.BusinessObjects;
using Infraestructure.Database.Entities;
using Riok.Mapperly.Abstractions;

namespace AuthManager.Infraestructure.Mappers;

[Mapper]
public static partial class UserManagerMapper
{
    public static partial UserEntity ToEntity(this UserData userData);

    public static partial UserData ToUserData(this UserEntity userEntity);
}