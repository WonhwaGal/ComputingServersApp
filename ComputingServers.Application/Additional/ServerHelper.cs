using ComputingServers.Domain.Enums;

namespace ComputingServers.Application.Additional
{
	public static class ServerHelper
	{
		public static string FormStatusResponse(ServerStatus status)
			=> status switch
			{
				ServerStatus.PoweredOn => "Server is ready.",
				ServerStatus.PoweredOff => "Server is currently powered off.",
				_ => "Server is booting. Please Wait."
			};
	}
}
