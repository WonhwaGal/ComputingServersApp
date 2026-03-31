using System.Reflection;

namespace ComputingServers.Domain;

public static class DomainAssemblyReference
{
	public static Assembly Assembly => typeof(DomainAssemblyReference).Assembly;
}
