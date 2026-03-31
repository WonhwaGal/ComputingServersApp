
namespace ComputingServers.Domain.Results.Errors
{
	public static class ServerErrors
	{
		public static Error NotFound(string id) => new Error(
			"ServerNotFound", $"Failure when renting server with id {id}.", ErrorType.NotFound);

		public static Error NotAvailable(string id) => new Error(
			"ServerNotAvailable", $"Failure when renting server with id {id}.", ErrorType.Conflict);

		public static Error FailureOnRent(string id) => new Error(
			"FailureOnRent", $"Failure when renting server with id {id}.", ErrorType.Problem);

		public static Error FailureOnAvailableList => new Error(
			"FailureOnAvailableList", $"Failure when requesting available servers.", ErrorType.Failure);

		public static Error FailureOnStatus(string id) => new Error(
			"FailureOnStatus", $"Failure when getting status of server with id {id}.", ErrorType.Problem);

		public static Error FailureOnRelease(string id) => new Error(
			"FailureOnRelease", $"Failure when releasing server with id {id}.", ErrorType.Problem);
	}
}
