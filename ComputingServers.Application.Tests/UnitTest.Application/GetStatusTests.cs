using ComputingServers.Application.Abstractions;
using ComputingServers.Application.CQRS.Servers.GetStatus;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Entities;
using ComputingServers.Domain.Enums;
using ComputingServers.Domain.Results;
using ComputingServers.Domain.Results.Errors;
using ComputingServers.Tests.UnitTest.Application;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ComputingServers.Application.Tests.UnitTest.Application;

public class GetStatusTests : BaseTest
{
	private readonly GetStatusQueryHandler _handler;

	public GetStatusTests()
	{
		_handler = new GetStatusQueryHandler(
			_serverRepositoryMock,
			_loggerMock);
	}

	[Fact]
	public async Task Handle_Should_ReturnFailure_WhenServerIsNull()
	{
		var server = new Server
		{
			Id = Faker.Random.AlphaNumeric(8),
			IsAvailable = true,
			Status = ServerStatus.PoweredOff
		};

		var query = new GetStatusQuery(server.Id);

		_serverRepositoryMock
			.GetByIdAsync(server.Id)
			.Returns((Server?)null);

		Result<string> result = await _handler.Handle(query, CancellationToken.None);

		result.Error.Should().Be(ServerErrors.NotFound(server.Id));
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenServerExists()
	{
		var server = new Server
		{
			Id = Faker.Random.AlphaNumeric(8),
			Status = ServerStatus.PoweredOn
		};

		_serverRepositoryMock
			.GetByIdAsync(server.Id, true)
			.Returns(Task.FromResult(server));

		var query = new GetStatusQuery(server.Id);

		var result = await _handler.Handle(query, CancellationToken.None);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().Be("Server is ready.");
	}

	[Fact]
	public async Task Handle_ShouldReturnFailureAndLogError_WhenRepositoryThrows()
	{
		var serverId = Faker.Random.AlphaNumeric(8);
		_serverRepositoryMock
			.GetByIdAsync(serverId, true)
			.ThrowsAsync(new Exception("DB failure"));

		var query = new GetStatusQuery(serverId);

		var result = await _handler.Handle(query, CancellationToken.None);

		result.IsSuccess.Should().BeFalse();
		result.Error.Should().Be(ServerErrors.FailureOnStatus(serverId));

		_loggerMock.Received(1).LogError(
			Arg.Is<string>(s => s.Contains("Failure when releasing server")),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<int>()
		);
	}
}
