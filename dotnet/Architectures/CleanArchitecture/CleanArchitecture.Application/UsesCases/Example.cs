using CleanArchitecture.Application.Contracts;
using Garciss.ROP;

namespace CleanArchitecture.Application.UsesCases;

public interface IExample : IApplicationContract;

public class Example : IExample
{
    public Task<Result> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
