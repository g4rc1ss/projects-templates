#if (SqlDatabase)
using SimpleWeb.HostWebApi.Database.Entities;
using SimpleWeb.HostWebApi.Database.Repository;
#endif

namespace SimpleWeb.HostWebApi.UsesCases;

public class Example(
#if (UsePostgres)
    IPostgresPoc postgresPoc,
#endif
#if (UseSqlite)
    ISqlitePoc sqlitePoc,
#endif
#if (UseMongodb)
    IMongoPoc mongoPoc,
#endif
#if (UseLitedb)
    ILitedbPoc litedbPoc
#endif
)
{
#if (UseSqlite)
    public async Task SqliteAsync()
    {
        UserEntity user = new() { Name = "Nombre" };
        UserEntity userCreated = await sqlitePoc.CreateAsync(user);
        userCreated.Name = "Nombre Modificado";

        UserEntity? result = await sqlitePoc.GetByIdAsync(userCreated.Id);
    }
#endif
    
#if (UsePostgres)
    public async Task PostgresAsync()
    {
        UserEntity user = new() { Name = "Nombre" };
        UserEntity userCreated = await postgresPoc.CreateAsync(user);
        userCreated.Name = "Nombre Modificado";

        UserEntity? result = await postgresPoc.GetByIdAsync(userCreated.Id);
    }
#endif
    
#if (UseMongodb)
    public async Task MongodbAsync()
    {
        MongoDbEntity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Creado" };
        MongoDbEntity userCreated = await mongoPoc.CreateAsync(entity);
        userCreated.Property = "Modificado";
        MongoDbEntity? result = await mongoPoc.GetByIdAsync(userCreated.Id);
    }
#endif
    
#if (UseLitedb)
    public async Task LitedbAsync()
    {
        LiteDbEntity entity = new() { Id = Guid.NewGuid().ToString(), Property = "Creado" };
        LiteDbEntity userCreated = await litedbPoc.CreateAsync(entity);
        userCreated.Property = "Modificado";
        LiteDbEntity? result = await litedbPoc.GetByIdAsync(userCreated.Id);
    }
#endif
}