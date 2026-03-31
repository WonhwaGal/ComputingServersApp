using ComputingServers.Application.Abstractions;
using ComputingServers.Application.CQRS.Servers.Rent;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Entities;
using ComputingServers.Domain.Enums;
using ComputingServers.Domain.Results;
using ComputingServers.Domain.Results.Errors;
using ComputingServers.Infrastructure.Jobs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ComputingServers.Tests.UnitTest.Application;

public class RentServerTests : BaseTest
{
	private readonly RentServerCommandHandler _handler;
	protected readonly IUnitOfWork _unitOfWorkMock;
	private readonly IQuartzService _quartzService;

	public RentServerTests()
	{
		_unitOfWorkMock = Substitute.For<IUnitOfWork>();
		_quartzService = Substitute.For<IQuartzService>();

		_handler = new RentServerCommandHandler(
			_serverRepositoryMock,
			_unitOfWorkMock,
			_quartzService,
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

		var command = new RentServerCommand(server.Id);

		_serverRepositoryMock
			.GetByIdAsync(server.Id)
			.Returns((Server?)null);

		Result<string> result = await _handler.Handle(command, CancellationToken.None);

		result.Error.Should().Be(ServerErrors.NotFound(server.Id));
	}

	[Fact]
	public async Task Handle_Should_ReturnFailure_WhenServerIsNotAvailable()
	{
		var server = new Server
		{
			Id = Faker.Random.AlphaNumeric(8),
			IsAvailable = false,
			Status = ServerStatus.PoweredOn
		};

		var command = new RentServerCommand(server.Id);

		_serverRepositoryMock
			.GetByIdAsync(server.Id)
			.Returns(server);

		Result<string> result = await _handler.Handle(command, CancellationToken.None);

		result.Error.Should().Be(ServerErrors.NotAvailable(server.Id));
	}

	[Fact]
	public async Task Handle_Should_RentServer_WhenAvailableAndPoweredOn()
	{
		var server = new Server
		{
			Id = Faker.Random.AlphaNumeric(8),
			IsAvailable = true,
			Status = ServerStatus.PoweredOn
		};

		var command = new RentServerCommand(server.Id);

		_serverRepositoryMock
			.GetByIdAsync(server.Id)
			.Returns(server);

		_unitOfWorkMock
			.SaveChangesAsync()
			.Returns(Task.FromResult(1));

		var result = await _handler.Handle(command, CancellationToken.None);

		result.IsSuccess.Should().BeTrue();
		server.IsAvailable.Should().BeFalse();
		server.RentedAt.Should().NotBeNull();
		server.Status.Should().Be(ServerStatus.PoweredOn);

		await _quartzService
			.DidNotReceive()
			.ScheduleJob<ServerPowerOnJob>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>());

		await _quartzService
			.DidNotReceive()
			.ScheduleJob<ServerReleaseJob>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>());

		_loggerMock.Received(1).LogInfo(Arg.Is<string>(s => s.Contains(server.Id!)), 
			Arg.Any<string>(), 
			Arg.Any<string>(), 
			Arg.Any<int>());
	}

	[Fact]
	public async Task Handle_Should_RentServerAndScheduleJobs_WhenPoweredOff()
	{
		var server = new Server
		{
			Id = Faker.Random.AlphaNumeric(8),
			IsAvailable = true,
			Status = ServerStatus.PoweredOff
		};

		var command = new RentServerCommand(server.Id);

		_serverRepositoryMock
			.GetByIdAsync(server.Id)
			.Returns(server);
		_unitOfWorkMock
			.SaveChangesAsync()
			.Returns(Task.FromResult(1));

		var result = await _handler.Handle(command, CancellationToken.None);

		result.IsSuccess.Should().BeTrue();
		server.IsAvailable.Should().BeFalse();
		server.RentedAt.Should().NotBeNull();
		server.Status.Should().Be(ServerStatus.Booting);

		await _quartzService.Received(1)
			.ScheduleJob<ServerPowerOnJob>("ServerId", server.Id, 1);

		await _quartzService.Received(1)
			.ScheduleJob<ServerReleaseJob>("ServerId", server.Id, 2);

		_loggerMock.Received(1).LogInfo(Arg.Is<string>(s => s.Contains(server.Id!)),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<int>());
	}

	[Fact]
	public async Task Handle_Should_ReturnFailure_WhenConcurrencyExceptionOccurs()
	{
		var server = new Server
		{
			Id = Faker.Random.AlphaNumeric(8),
			IsAvailable = true,
			Status = ServerStatus.PoweredOn
		};

		_serverRepositoryMock.GetByIdAsync(server.Id).Returns(server);

		_unitOfWorkMock
			.SaveChangesAsync()
			.Throws(new DbUpdateConcurrencyException());

		var command = new RentServerCommand(server.Id);

		var result = await _handler.Handle(command, CancellationToken.None);

		result.IsSuccess.Should().BeFalse();
		result.Error.Should().Be(ServerErrors.NotAvailable(server.Id));

		_loggerMock.Received(1).LogError(Arg.Is<string>(s => s.Contains("already rented")),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<int>());
	}
}
