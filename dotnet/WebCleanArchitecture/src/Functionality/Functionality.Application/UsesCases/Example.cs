using Shared;

namespace Functionality.Application.UsesCases;

public interface IExample : IApplicationContract;


public class Example : IExample
{
    public Task<Result> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}