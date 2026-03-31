using ComputingServers.Domain.Entities;
using System.Linq.Expressions;

namespace ComputingServers.Application.Repositories;

public interface IServerRepository
{
	/// <summary>
	/// Добавление нового сервера
	/// </summary>
	/// <param name="server"></param>
	void Add(Server server);

	/// <summary>
	/// Получение списка серверов по фильтру
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="readOnly"></param>
	/// <returns></returns>
	Task<List<Server>> GetAvailableAsync(Expression<Func<Server, bool>> filter, bool readOnly = false);

	/// <summary>
	/// Получение сервера по ID
	/// </summary>
	/// <param name="id"></param>
	/// <param name="readOnly"></param>
	/// <returns></returns>
	Task<Server> GetByIdAsync(string id, bool readOnly = false);

	/// <summary>
	/// Получение всех серверов из пула
	/// </summary>
	/// <returns></returns>
	Task<List<Server>> GetAllAsync(bool readOnly = false);
}
