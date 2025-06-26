using Infraestructure.Database.Contexts;
using Infraestructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infraestructure.Database.Repository;

public class PostgresPoc(PostgresContext dbContext) : IPostgresPoc
{
    public WeatherForecastEntity? Entity { get; set; }

    public async Task<WeatherForecastEntity?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        if (Entity is not null)
        {
            return Entity;
        }

        WeatherForecastEntity weatherForecast = await dbContext.WeatherForecast.FirstAsync(
            entity => entity.Id == id,
            cancellationToken
        );

        Entity = weatherForecast;

        return weatherForecast;
    }

    public async Task<WeatherForecastEntity> CreateAsync(
        WeatherForecastEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        EntityEntry<WeatherForecastEntity> user = await dbContext.WeatherForecast.AddAsync(
            entity,
            cancellationToken
        );
        await dbContext.SaveChangesAsync(cancellationToken);

        return user.Entity;
    }

    public async Task CreateManyAsync(
        IEnumerable<WeatherForecastEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        await dbContext.WeatherForecast.AddRangeAsync(entities, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        WeatherForecastEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.WeatherForecast.Update(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateManyAsync(
        IEnumerable<WeatherForecastEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.WeatherForecast.UpdateRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        WeatherForecastEntity entity,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.WeatherForecast.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteManyAsync(
        IEnumerable<WeatherForecastEntity> entities,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.WeatherForecast.RemoveRange(entities);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

public interface IPostgresPoc
    : IRepository<WeatherForecastEntity, int>,
        IManyCommandRepository<WeatherForecastEntity>;
