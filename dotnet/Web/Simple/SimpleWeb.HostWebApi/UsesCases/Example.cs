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
    ISqlitePoc sqlitePoc
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
}