using AuthManager.Domain.BusinessObjects;
using Infraestructure.AuthManagerDB.Entities;
using Riok.Mapperly.Abstractions;

namespace AuthManager.Infraestructure.Mappers;

[Mapper]
public static partial class UserManagerMapper
{
    [MapProperty(nameof(@UserData.Password), nameof(@UserEntity.PasswordHash))]
    public static partial UserEntity ToEntity(this UserData userData);


    [MapProperty(nameof(@UserEntity.PasswordHash), nameof(@UserData.Password))]
    public static partial UserData ToUserData(this UserEntity userEntity);
}