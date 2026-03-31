using ComputingServers.Application.CQRS.Servers.Add;
using ComputingServers.Application.CQRS.Servers.GetAll;
using ComputingServers.Application.CQRS.Servers.GetAvailable;
using ComputingServers.Application.CQRS.Servers.GetStatus;
using ComputingServers.Application.CQRS.Servers.Release;
using ComputingServers.Application.CQRS.Servers.Rent;
using ComputingServers.Application.Dtos;
using ComputingServers.Domain.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ComputingServersApp.Controllers;

/// <summary>
/// Контроллер для работы с серверами
/// </summary>
    [ApiController]
    [Route("servers")]
    public class ServersController(ISender sender) : ControllerBase
{
	/// <summary>
	/// Добавить новый сервер в пул
	/// </summary>
	/// <param name="serverDto"></param>
	/// <returns></returns>
	[HttpPost("add")]
        public async Task<ActionResult<Result>> Add([FromBody] AddServerCommand command)
        {
		var result = await sender.Send(command);
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

	/// <summary>
	/// Узнать какие есть свободные сервера(поиск по параметрам)
	/// </summary>
	/// <param name="serverDto"></param>
	/// <returns></returns>
	[HttpGet("find_available")]
	public async Task<ActionResult<List<FilterServerDto>>> FindAvailable([FromQuery] ServerQueryParameters queryParameters)
	{
		var result = await sender.Send(new GetAvailableQuery(queryParameters));
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	/// <summary>
	/// Получить все сервера из пула
	/// </summary>
	/// <param name="serverDto"></param>
	/// <returns></returns>
	[HttpGet("get_all")]
	public async Task<ActionResult<List<ServerFullDto>>> GetAll()
	{
		var result = await sender.Send(new GetAllQuery());
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	/// <summary>
	/// Взять сервер в аренду с указанием его ID
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[HttpPost("rent")]
	public async Task<ActionResult<Result>> Rent([FromQuery] string id)
	{
		var result = await sender.Send(new RentServerCommand(id));
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	/// <summary>
	/// Освободить сервер
	/// </summary>
	/// <param name="serverDto"></param>
	/// <returns></returns>
	[HttpPost("release")]
	public async Task<ActionResult<Result>> Release([FromQuery] string id)
	{
		var result = await sender.Send(new ReleaseServerCommand(id));
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}

	/// <summary>
	/// Проверить готовность сервера
	/// </summary>
	/// <param name="serverDto"></param>
	/// <returns></returns>
	[HttpGet("status")]
	public async Task<ActionResult<string>> GetStatus([FromQuery] string id)
	{
		var result = await sender.Send(new GetStatusQuery(id));
		return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
	}
}
