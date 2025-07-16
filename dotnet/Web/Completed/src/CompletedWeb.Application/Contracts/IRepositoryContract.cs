namespace CompletedWeb.Application.Contracts;

public interface IRepositoryContract<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRepositoryContract<in TRequest>
{
    Task ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IRepositoryContract
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}
