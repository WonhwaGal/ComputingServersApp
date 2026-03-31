using MediatR;
using NetArchTest.Rules;

namespace ComputingServers.Tests.ArchitecturalTests;

public class ApplicationTests : BaseTest
{
	[Fact]
	public void QueriesAndCommands_Should_BeSealed()
	{
		Types.InAssembly(ApplicationAssembly)
			.That()
			.ImplementInterface(typeof(IRequest))
			.Should()
			.BeSealed()
			.GetResult()
			.ShouldBeSuccessful();
	}
}
