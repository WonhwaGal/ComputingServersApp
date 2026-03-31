using FluentAssertions;
using NetArchTest.Rules;

namespace ComputingServers.Tests.ArchitecturalTests;

internal static class TestResultExtensions
{
	internal static void ShouldBeSuccessful(this TestResult testResult)
	{
		testResult.FailingTypes?.Should().BeEmpty();
	}
}
