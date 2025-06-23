using System.Reflection;
using Template.HostWebApi;
#if (!UseLayerArchitecture)
using Shared;

#else
using Template.Application.Contracts;
#endif

namespace Template.Architecture.Tests;

public class ApplicationContractTests
{
    private readonly IEnumerable<Assembly> _applicationAssemblies;

    /// <summary>
    /// Carga inicial buscando los assembly que contengan en el nombre "Application"
    /// </summary>
    public ApplicationContractTests()
    {
        _applicationAssemblies = [];
        Assembly? rootAssembly = typeof(Program).Assembly;

        HashSet<string> visited = [];
        Queue<Assembly> queue = new();

        queue.Enqueue(rootAssembly);

        while (queue.Any())
        {
            Assembly assembly = queue.Dequeue();
            visited.Add(assembly.FullName ?? throw new ArgumentNullException());

            AssemblyName[] references = assembly.GetReferencedAssemblies();
            foreach (AssemblyName reference in references)
            {
                if (!visited.Contains(reference.FullName))
                {
                    queue.Enqueue(Assembly.Load(reference));
                }
            }

            if (assembly.FullName.Contains("Application"))
            {
                _applicationAssemblies = _applicationAssemblies.Append(assembly);
            }
        }

        if (!_applicationAssemblies.Any())
        {
            throw new ApplicationException("No assemblies were found.");
        }
    }

    [Fact]
    public void AllApplicationInterfaces_ShouldBeSaveOn_Contracts()
    {
        foreach (Assembly applicationAssembly in _applicationAssemblies)
        {
            List<Type> interfaceList =
            [
                .. applicationAssembly.GetTypes().Where(x => x.IsInterface && x.GetMethods().Any()),
            ];

            foreach (Type interfaceType in interfaceList)
            {
                Assert.Contains(".Contracts", interfaceType.Namespace);
            }
        }
    }

    [Fact]
    public void AllApplicationInterfaces_ShouldBeOn_ApplicationLayer()
    {
        foreach (Assembly applicationAssembly in _applicationAssemblies)
        {
            foreach (
                (
                    Type interfacesType,
                    IEnumerable<Type> implementedClass
                ) classes in GetClassesImplementingInterface(applicationAssembly)
            )
            {
                foreach (Type classType in classes.implementedClass)
                {
                    Assert.Equal(classType.Assembly, classes.interfacesType.Assembly);
                }
            }
        }
    }

    [Fact]
    public void AllApplicationClass_ShouldBe_JustOnePublicMethod()
    {
        foreach (Assembly applicationAssembly in _applicationAssemblies)
        {
            foreach (
                (
                    Type interfacesType,
                    IEnumerable<Type> implementedClass
                ) implementations in GetClassesImplementingInterface(applicationAssembly)
            )
            {
                foreach (Type classImplementInterface in implementations.implementedClass)
                {
                    int publicMethods = classImplementInterface
                        .GetMethods()
                        .Count(x => x.IsPublic && x.DeclaringType == classImplementInterface);

                    Assert.True(classImplementInterface.IsPublic);
                    Assert.Equal(1, publicMethods);
                }
            }
        }
    }

    private IEnumerable<(
        Type interfacesType,
        IEnumerable<Type> implementedClass
    )> GetClassesImplementingInterface(Assembly assembly)
    {
        List<(Type, IEnumerable<Type>)> returnList = [];

        List<Type> interfaceList = assembly.GetTypes().Where(x => x.IsInterface).ToList();

        foreach (Type interfaceType in interfaceList)
        {
            bool applicationInterfaces = interfaceType
                .GetInterfaces()
                .Any(x => x.Name.Contains(nameof(IApplicationContractBase)));

            if (!applicationInterfaces)
            {
                continue;
            }

            List<Type> classesList = assembly
                .GetTypes()
                .Where(x => x.IsClass && x.GetInterface(interfaceType.Name) is not null)
                .ToList();

            returnList.Add((interfaceType, classesList));
        }

        return returnList;
    }
}
