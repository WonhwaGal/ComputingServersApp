using ComputingServers.Application;
using ComputingServers.Domain;
using ComputingServersApp;
using System.Reflection;

namespace ComputingServers.Tests.ArchitecturalTests;

public abstract class BaseTest
{
	protected static readonly Assembly ApplicationAssembly = typeof(ApplicationAssemblyReference).Assembly;

	protected static readonly Assembly DomainAssembly = typeof(DomainAssemblyReference).Assembly;

	protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.Infrastructure).Assembly;

	protected static readonly Assembly ApiAssembly = typeof(ApiAssemblyReference).Assembly;
}
