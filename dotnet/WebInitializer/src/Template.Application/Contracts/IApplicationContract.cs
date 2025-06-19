namespace Template.Application.Contracts;

public interface IApplicationContractBase;

public interface IApplicationContract<in TRequest, TResponse> : IApplicationContractBase
{
    Task<Result<TResponse>> ExecuteAsync(
        TRequest request,
        CancellationToken cancellationToken = default
    );
}

public interface IApplicationContract<in TRequest> : IApplicationContractBase
{
    Task<Result> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
}

public interface IApplicationContract : IApplicationContractBase
{
    Task<Result> ExecuteAsync(CancellationToken cancellationToken = default);
}
