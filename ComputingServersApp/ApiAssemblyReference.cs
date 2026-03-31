using System.Reflection;

namespace ComputingServersApp;

public static class ApiAssemblyReference
{
	public static Assembly Assembly => typeof(ApiAssemblyReference).Assembly;
}
