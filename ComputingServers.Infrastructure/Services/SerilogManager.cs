using ComputingServers.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Runtime.CompilerServices;

namespace ComputingServers.Infrastructure.Services;

public class SerilogManager(ILogger<SerilogManager> logger) : ILoggerManager
{

	public delegate void LogDelegate(string? message, params object?[] args);

	public void LogDebug(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
	{
		FormLog(logger.LogDebug, message, callerFile, callerMember, callerLine);
	}

	public void LogError(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
	{
		FormLog(logger.LogError, message, callerFile, callerMember, callerLine);
	}

	public void LogInfo(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
	{
		FormLog(logger.LogInformation, message, callerFile, callerMember, callerLine);
	}

	public void LogWarn(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0)
	{
		FormLog(logger.LogWarning, message, callerFile, callerMember, callerLine);
	}

	private void FormLog(LogDelegate action, string message, string callerFile, string callerMember, int callerLine)
	{
		var callerClass = Path.GetFileNameWithoutExtension(callerFile);

		using (LogContext.PushProperty("CallerClass", callerClass))
		using (LogContext.PushProperty("CallerMember", callerMember))
		using (LogContext.PushProperty("CallerLine", callerLine))
		{
			action(message);
		}
	}
}
