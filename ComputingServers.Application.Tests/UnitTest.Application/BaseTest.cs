using Bogus;
using ComputingServers.Application.Abstractions;
using ComputingServers.Application.Repositories;
using NSubstitute;

namespace ComputingServers.Tests.UnitTest.Application;

public abstract class BaseTest
{
	protected static readonly Faker Faker = new();

	protected readonly IServerRepository _serverRepositoryMock;
	protected readonly ILoggerManager _loggerMock;

	protected BaseTest()
	{
		_serverRepositoryMock = Substitute.For<IServerRepository>();
		_loggerMock = Substitute.For<ILoggerManager>();
	}
}
