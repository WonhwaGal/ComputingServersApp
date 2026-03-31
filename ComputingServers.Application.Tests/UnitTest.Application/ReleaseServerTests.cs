using ComputingServers.Application.Abstractions;
using ComputingServers.Application.CQRS.Servers.Release;
using ComputingServers.Domain.Entities;
using ComputingServers.Domain.Enums;
using ComputingServers.Domain.Results;
using ComputingServers.Domain.Results.Errors;
using ComputingServers.Tests.UnitTest.Application;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ComputingServers.Application.Tests.UnitTest.Application;

public class ReleaseServerTests : BaseTest
{
	private readonly ReleaseServerCommandHandler _handler;
	protected readonly IUnitOfWork _unitOfWorkMock;

	public ReleaseServerTests()
	{
		_unitOfWorkMock = Substitute.For<IUnitOfWork>();

		_handler = new ReleaseServerCommandHandler(
			_serverRepositoryMock,
			_unitOfWorkMock,
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

		var command = new ReleaseServerCommand(server.Id);

		_serverRepositoryMock
			.GetByIdAsync(server.Id)
			.Returns((Server?)null);

		Result<bool> result = await _handler.Handle(command, CancellationToken.None);

		result.Error.Should().Be(ServerErrors.NotFound(server.Id));
	}

	[Fact]
	public async Task Handle_ShouldReleaseServer_WhenServerExists()
	{
		var server = new Server { 
			Id = Faker.Random.AlphaNumeric(8), 
			IsAvailable = false, 
			Status = ServerStatus.PoweredOn 
		};

		_serverRepositoryMock
			.GetByIdAsync(server.Id)
			.Returns(Task.FromResult(server));
		_unitOfWorkMock
			.SaveChangesAsync()
			.Returns(Task.FromResult(1));

		var command = new ReleaseServerCommand(server.Id);
		var result = await _handler.Handle(command, CancellationToken.None);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().BeTrue();
		server.IsAvailable.Should().BeTrue();
		server.Status.Should().Be(ServerStatus.PoweredOff);
		server.LastUsed.Should().NotBeNull();

		await _unitOfWorkMock.Received(1).SaveChangesAsync();
		_loggerMock.Received(1).LogInfo(Arg.Is<string>(s => s.Contains(server.Id)),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<int>()
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailureAndLogError_WhenExceptionOccurs()
	{
		var server = new Server { Id = Faker.Random.AlphaNumeric(8) };
		_serverRepositoryMock.GetByIdAsync(server.Id).Returns(Task.FromResult(server));
		_unitOfWorkMock.SaveChangesAsync().ThrowsAsync(new Exception("DB failure"));

		var command = new ReleaseServerCommand(server.Id);
		var result = await _handler.Handle(command, CancellationToken.None);

		result.IsSuccess.Should().BeFalse();
		result.Error.Code.Should().Be("FailureOnRelease");
		_loggerMock.Received(1).LogError(Arg.Is<string>(s => s.Contains("Failure when releasing server")),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<int>()
		);
	}
}
