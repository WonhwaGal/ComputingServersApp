using NetArchTest.Rules;

namespace ComputingServers.Tests.ArchitecturalTests;

public class LayerTests : BaseTest
{
	[Fact]
	public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
	{
		Types.InAssembly(DomainAssembly)
			.Should()
			.NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
			.GetResult()
			.ShouldBeSuccessful();
	}

	[Fact]
	public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
	{
		Types
			.InAssembly(DomainAssembly)
			.Should()
			.NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
			.GetResult()
			.ShouldBeSuccessful();
	}

	[Fact]
	public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
	{
		Types.InAssembly(ApplicationAssembly)
			.Should()
			.NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
			.GetResult()
			.ShouldBeSuccessful();
	}

	[Fact]
	public void ApplicationLayer_ShouldNotHaveDependencyOn_ApiLayer()
	{
		Types.InAssembly(ApplicationAssembly)
			.Should()
			.NotHaveDependencyOn(ApiAssembly.GetName().Name)
			.GetResult()
			.ShouldBeSuccessful();
	}

	[Fact]
	public void InfrastructureLayer_ShouldNotHaveDependencyOn_ApiLayer()
	{
		Types.InAssembly(InfrastructureAssembly)
			.Should()
			.NotHaveDependencyOn(ApiAssembly.GetName().Name)
			.GetResult()
			.ShouldBeSuccessful();
	}
}
