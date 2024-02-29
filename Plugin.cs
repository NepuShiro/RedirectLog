using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using Elements.Core;

namespace RedirectLog
{
	[BepInPlugin("org.Nep.RedirectLog", "RedirectLog", "1.0.2")]
	public class RedirectLog : BaseUnityPlugin
	{
		public static readonly string logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ResoniteLogs", "ResoniteLog-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".log"); 
		private ConfigEntry<bool> configEnableTimestamp;
		private ConfigEntry<bool> configEnableFPS;
		private static ConfigEntry<bool> configEnableFileLogging;
		private void Awake()
		{
			configEnableFileLogging = Config.Bind("General",
									"Enable File Logging",
									true,
									"Whether or not to Log to a File");
			configEnableTimestamp = Config.Bind("General",
									"Enable TimeStamps",
									true,
									"Whether or not to show TimeStamps in the Log");
			configEnableFPS = Config.Bind("General",
						"Enable FPS Log",
						false,
						"Whether or not to show the FPS in the Log");
			UniLog.OnLog += OnLogMessage;
			UniLog.OnWarning += OnWarningMessage;
			UniLog.OnError += OnErrorMessage;
		}

		private void OnLogMessage(string message)
		{
			string cleanedMessage = RemoveTimestampAndFPS(message);

			if (IsValidLog(cleanedMessage) && MatchesLogPatterns(cleanedMessage))
			{
				Console.LogToFile("[UniLog] " + message);
				FormatModLoaderLog(cleanedMessage);
			}
			else if (IsValidLog(cleanedMessage))
			{
				Console.LogToFile("[UniLog] " + message);
				Console.WriteLine("[UniLog] " + cleanedMessage, ConsoleColor.Gray);
			}
		}

		private void OnWarningMessage(string message)
		{
			string cleanedMessage = RemoveTimestampAndFPS(message);

			if (IsValidLog(cleanedMessage) && MatchesLogPatterns(cleanedMessage))
			{
				Console.LogToFile("[UniLog] [Warning] " + message);
				FormatModLoaderLog(cleanedMessage);
			}
			else if (IsValidLog(cleanedMessage))
			{
				Console.LogToFile("[UniLog] [Warning] " + message);
				Console.WriteLine("[UniLog] [Warning] " + cleanedMessage, ConsoleColor.Yellow);
			}
		}

		private void OnErrorMessage(string message)
		{
			string cleanedMessage = RemoveTimestampAndFPS(message);

			if (IsValidLog(cleanedMessage) && MatchesLogPatterns(cleanedMessage))
			{
				Console.LogToFile("[UniLog] [Error] " + message);
				FormatModLoaderLog(cleanedMessage);
			}
			else if (IsValidLog(cleanedMessage))
			{
				Console.LogToFile("[UniLog] [Error] " + message);
				Console.WriteLine("[UniLog] [Error] " + cleanedMessage, ConsoleColor.Red);
			}
		}

		private void FormatModLoaderLog(string message)
		{
			var lower = message.ToLower();
			var consoleColor = Regex.IsMatch(lower, @"\[info\]") ? ConsoleColor.Green :
							   Regex.IsMatch(lower, @"\[debug\]") ? ConsoleColor.Blue :
							   Regex.IsMatch(lower, @"\[error\]") ? ConsoleColor.Red :
							   Regex.IsMatch(lower, @"\[warn\]|updated: https") ? ConsoleColor.Yellow :
							   Regex.IsMatch(lower, @"lastmodifyinguser") ? ConsoleColor.Yellow :
							   Regex.IsMatch(lower, @"failed load: could not gather") ? ConsoleColor.Red :
							   Regex.IsMatch(lower, @"signalr|clearing expired status|status before clearing:|status after clearing:") ? ConsoleColor.DarkMagenta :
							   Regex.IsMatch(lower, @"sendstatustouser:") ? ConsoleColor.Magenta :
							   ConsoleColor.Gray;

			Console.WriteLine($"[UniLog] {message}", consoleColor);
		}

		private bool MatchesLogPatterns(string message)
		{
			var lower = message.ToLower();
			return Regex.IsMatch(lower, @"\[info\]") ||
				   Regex.IsMatch(lower, @"\[debug\]") ||
				   Regex.IsMatch(lower, @"\[error\]") ||
				   Regex.IsMatch(lower, @"\[warn\]|updated: https") ||
				   Regex.IsMatch(lower, @"lastmodifyinguser") ||
				   Regex.IsMatch(lower, @"failed load: could not gather") ||
				   Regex.IsMatch(lower, @"signalr|clearing expired status|status before clearing:|status after clearing:") ||
				   Regex.IsMatch(lower, @"sendstatustouser:");
		}

		private bool IsValidLog(string message)
		{
			string lower = message.ToLower();
			return !Regex.IsMatch(lower, "session updated, forcing status update") &&
				   !Regex.IsMatch(lower, @"\[debug\]\[resonitemodloader\] intercepting call to appdomain\.getassemblies\(\)") &&
				   !Regex.IsMatch(lower, "rebuild:");
		}

		private string RemoveTimestampAndFPS(string message)
		{
			string timestampPattern = @"\d{1,2}:\d{1,2}:\d{1,2} [APap][Mm]\.\d{1,3}\s";
			string fpsPattern = @"\(\s*-*\d+\s?FPS\s?\)\s+";

			if (!configEnableTimestamp.Value)
			{
				message = Regex.Replace(message, timestampPattern, "");
			}

			if (!configEnableFPS.Value)
			{
				message = Regex.Replace(message, fpsPattern, "");
			}

			return message;
		}

		public class Console
		{
			public static void WriteLine(string text, ConsoleColor consoleColor)
			{
				ConsoleManager.SetConsoleColor(consoleColor);
				ConsoleManager.StandardOutStream.WriteLine(text);
			}
			
			public static void LogToFile(string message)
			{
				
				string logDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ResoniteLogs");

				if (!Directory.Exists(logDirectory))
				{
					Directory.CreateDirectory(logDirectory);
				}

				if (!configEnableFileLogging.Value) return;
				
				File.AppendAllText(logFilePath, message + Environment.NewLine);
			}
		}
	}
}
