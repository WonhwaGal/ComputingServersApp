using ComputingServers.Application.Abstractions;
using ComputingServers.Application.CQRS.Servers.GetAll;
using ComputingServers.Application.CQRS.Servers.GetAvailable;
using ComputingServers.Application.Dtos;
using ComputingServers.Application.Repositories;
using ComputingServers.Domain.Entities;
using ComputingServers.Domain.Enums;
using ComputingServers.Tests.UnitTest.Application;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;

namespace ComputingServers.Application.Tests.UnitTest.Application;

public class GetServersListsTests : BaseTest
{
	private readonly GetAvailableQueryHandler _handler;
	private readonly GetAllQueryHandler _all_handler;

	public GetServersListsTests() : base()
	{
		_handler = new GetAvailableQueryHandler(
			_serverRepositoryMock,
			_loggerMock);

		_all_handler = new GetAllQueryHandler(
			_serverRepositoryMock,
			_loggerMock);
	}

	[Fact]
	public async Task Handle_ShouldReturnAvailableServers()
	{
		var queryParams = new ServerQueryParameters(null, 16, null, null, null);
		var request = new GetAvailableQuery(queryParams);

		var servers = new List<Server>
		{
			new Server { Id = Faker.Random.AlphaNumeric(8), IsAvailable = true, Status = ServerStatus.PoweredOn },
			new Server { Id = Faker.Name.FirstName(), IsAvailable = true, Status = ServerStatus.PoweredOff }
		};

		_serverRepositoryMock
			.GetAvailableAsync(Arg.Any<Expression<Func<Server, bool>>>(), true)
			.Returns(Task.FromResult(servers));

		var result = await _handler.Handle(request, CancellationToken.None);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().HaveCount(2);
		await _serverRepositoryMock
			.Received(1)
			.GetAvailableAsync(Arg.Any<Expression<Func<Server, bool>>>(), true);
	}

	[Fact]
	public async Task Handle_ShouldReturnFailure_WhenRepositoryThrows()
	{
		var queryParams = new ServerQueryParameters(null, 16, null, null, null);
		var request = new GetAvailableQuery(queryParams);

		_serverRepositoryMock
			.GetAvailableAsync(Arg.Any<Expression<Func<Server, bool>>>(), true)
			.ThrowsAsync(new Exception("DB failure"));

		var result = await _handler.Handle(request, CancellationToken.None);

		result.IsSuccess.Should().BeFalse();
		result.Error.Code.Should().Be("FailureOnAvailableList");

		_loggerMock.Received(1).LogError(
			Arg.Is<string>(s => s.Contains("Failure when requesting available servers")),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<int>()
		);
	}

	[Fact]
	public async Task Handle_ShouldReturnAllServers_WhenRepositoryReturnsData()
	{
		var servers = new List<Server>
		{
			new Server { Id = Faker.Random.AlphaNumeric(8), OS = "Linux", MemoryCapacity = 16, DiskCapacity = 100, CpuNumber = 4, Status = ServerStatus.PoweredOn },
			new Server { Id = Faker.Random.AlphaNumeric(8), OS = "Windows", MemoryCapacity = 32, DiskCapacity = 200, CpuNumber = 8, Status = ServerStatus.PoweredOff }
		};

		_serverRepositoryMock.GetAllAsync(true).Returns(Task.FromResult(servers));

		var result = await _all_handler.Handle(new GetAllQuery(), CancellationToken.None);

		result.IsSuccess.Should().BeTrue();
		result.Value.Should().HaveCount(2);
		result.Value.Select(dto => dto.Id).Should().BeEquivalentTo(servers.Select(s => s.Id));
		
		_loggerMock
			.DidNotReceive()
			.LogError(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>());
	}

	[Fact]
	public async Task GetAllHandle_ShouldReturnFailure_WhenRepositoryThrows()
	{
		_serverRepositoryMock.GetAllAsync(true).ThrowsAsync(new Exception("DB failure"));

		var result = await _all_handler.Handle(new GetAllQuery(), CancellationToken.None);

		result.IsSuccess.Should().BeFalse();
		result.Error.Code.Should().Be("FailureOnStatus");
		_loggerMock.Received(1).LogError(
			Arg.Is<string>(s => s.Contains("Failure when getting full server list")),
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<int>()
		);
	}
}
