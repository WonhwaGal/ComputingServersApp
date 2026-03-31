using System.Runtime.CompilerServices;

namespace ComputingServers.Application.Abstractions;

public interface ILoggerManager
{
	void LogInfo(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0);
	void LogWarn(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0);
	void LogDebug(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0);
	void LogError(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLine = 0);
}
