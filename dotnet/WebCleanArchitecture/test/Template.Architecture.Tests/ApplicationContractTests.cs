using System.Reflection;
using Template.HostWebApi;

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
            visited.Add(assembly.FullName);

            AssemblyName[] references = assembly.GetReferencedAssemblies();
            foreach (AssemblyName reference in references)
            {
                if (!visited.Contains(reference.FullName))
                    queue.Enqueue(Assembly.Load(reference));
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
    public void AllApplicationInterfaces_ShouldImplement_GenericApplicationInterface()
    {
        foreach (Assembly applicationAssembly in _applicationAssemblies)
        {
            List<Type> interfaceList = applicationAssembly.GetTypes()
                .Where(x => x.IsInterface)
                .ToList();

            foreach (Type interfaceType in interfaceList)
            {
                IEnumerable<Type> interfaces = interfaceType.GetInterfaces()
                    .Where(x => x.Name.Contains("IApplicationContractBase"));

                Assert.True(interfaces.Any());
            }
        }
    }

    [Fact]
    public void AllApplicationInterfaces_ShouldBeSaveOn_Contracts()
    {
        foreach (Assembly applicationAssembly in _applicationAssemblies)
        {
            List<Type> interfaceList = applicationAssembly.GetTypes()
                .Where(x => x.IsInterface && x.GetMethods().Any())
                .ToList();

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
            List<Type> classImplementInterfaces = applicationAssembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces()?.Any() == true)
                .ToList();

            foreach (Type classImplementInterface in classImplementInterfaces)
            {
                Type[] interfaces = classImplementInterface.GetInterfaces();

                foreach (Type @interface in interfaces)
                {
                    if (!@interface.Name.Contains("IApplicationContract"))
                    {
                        Assert.Equal(classImplementInterface.Assembly, @interface.Assembly);
                    }
                }
            }
        }
    }

    [Fact]
    public void AllApplicationClass_ShouldBe_JustOnePublicMethod()
    {
        foreach (Assembly applicationAssembly in _applicationAssemblies)
        {
            List<Type> classImplementInterfaces = applicationAssembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces()?.Any() == true)
                .ToList();

            foreach (Type classImplementInterface in classImplementInterfaces)
            {
                int publicMethods = classImplementInterface.GetMethods()
                    .Count(x => x.IsPublic && x.DeclaringType == classImplementInterface);

                Assert.True(classImplementInterface.IsPublic);
                Assert.Equal(1, publicMethods);
            }
        }
    }
}