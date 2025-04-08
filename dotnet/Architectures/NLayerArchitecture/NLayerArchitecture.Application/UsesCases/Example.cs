using Microsoft.Extensions.Logging;
using NLayerArchitecture.Infraestructure;

namespace NLayerArchitecture.Application.UsesCases;

public class Example(
    ILogger<Example> logger,
    IRepository repository
)
{
}