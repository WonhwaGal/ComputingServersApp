using System.Reflection;

namespace ComputingServers.Application
{
	public static class ApplicationAssemblyReference
	{
		public static Assembly Assembly => typeof(ApplicationAssemblyReference).Assembly;
	}
}
