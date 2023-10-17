using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using BCE;
using Elements.Core;
using static OfficialAssets.Graphics;
using static UnityEngine.UIElements.StyleVariableResolver;

namespace RedirectLog
{
    [BepInPlugin("org.Nep.RedirectLog", "RedirectLog", "1.0.1")]
    public class RedirectLog : BaseUnityPlugin
    {
        private ConfigEntry<bool> configEnableTimestamp;
        private ConfigEntry<bool> configEnableFPS;
        void Awake()
        {
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

        void OnLogMessage(string message)
        {
            string cleanedMessage = RemoveTimestampAndFPS(message);

            if (IsValidLog(cleanedMessage) && cleanedMessage.Contains("ResoniteModLoader") || cleanedMessage.Contains("LastModifyingUser") || message.Contains("Failed Load: Could not gather") || message.Contains("SignalR") || message.Contains("SIGNALR"))
            {
                FormatModLoaderLog(cleanedMessage);
            }
            else if (IsValidLog(cleanedMessage))
            {
                console.WriteLine("[UniLog] " + cleanedMessage, ConsoleColor.Gray);
            }
        }

        void OnWarningMessage(string message)
        {
            string cleanedMessage = RemoveTimestampAndFPS(message);

            if (IsValidLog(cleanedMessage) && cleanedMessage.Contains("ResoniteModLoader"))
            {
                FormatModLoaderLog(cleanedMessage);
            }
            else if (IsValidLog(cleanedMessage))
            {
                console.WriteLine("[UniLog] [Warning] " + cleanedMessage, ConsoleColor.Yellow);
            }
        }

        void OnErrorMessage(string message)
        {
            string cleanedMessage = RemoveTimestampAndFPS(message);

            if (IsValidLog(cleanedMessage) && cleanedMessage.Contains("ResoniteModLoader"))
            {
                FormatModLoaderLog(cleanedMessage);
            }
            else if (IsValidLog(cleanedMessage))
            {
                console.WriteLine("[UniLog] [Error] " + cleanedMessage, ConsoleColor.Red);
            }
        }

        void FormatModLoaderLog(string message)
        {
            if (message.Contains("[INFO]"))
            {
                console.WriteLine("[UniLog] " + message, ConsoleColor.Green);
            }
            else if (message.Contains("[DEBUG]"))
            {
                console.WriteLine("[UniLog] " + message, ConsoleColor.Blue);
            }
            else if (message.Contains("[ERROR]"))
            {
                console.WriteLine("[UniLog] " + message, ConsoleColor.Red);

            }
            else if (message.Contains("[WARN]"))
            {
                console.WriteLine("[UniLog] " + message, ConsoleColor.Yellow);
            }
            else if (message.Contains("LastModifyingUser"))
            {
                console.WriteLine("[UniLog] [Warning] " + message, ConsoleColor.Yellow);
            }
            else if (message.Contains("Failed Load: Could not gather"))
            {
                console.WriteLine("[UniLog] [Error] " + message, ConsoleColor.Red);
            }
            else if (message.Contains("SignalR") || message.Contains("SIGNALR"))
            {
                console.WriteLine("[UniLog] " + message, ConsoleColor.DarkMagenta);
            }
            else
            {
                console.WriteLine("[UniLog] " + message, ConsoleColor.Gray);
            }
        }

        string RemoveTimestampAndFPS(string message)
        {
            string pattern1 = @"\d{1,2}:\d{1,2}:\d{1,2} [APap][Mm]\.\d{1,3}\s";
            string pattern2 = @"\(\s*-*\d+\s?FPS\s?\)\s+";

            if (!configEnableTimestamp.Value)
            {
                string result = Regex.Replace(message, pattern1, "");

                if (!configEnableFPS.Value)
                {
                    string result2 = Regex.Replace(result, pattern2, "");

                    return result2;
                }
                else return result;
            }
            else
            {
                if (!configEnableFPS.Value)
                {
                    string result2 = Regex.Replace(message, pattern2, "");

                    return result2;
                }
                else return message;
            }
        }

        bool IsValidLog(string message)
        {
            return !string.IsNullOrWhiteSpace(message) &&
                   !message.Contains("Session updated, forcing status update") &&
                   !message.Contains("[DEBUG][ResoniteModLoader] Intercepting call to AppDomain.GetAssemblies()") &&
                   !message.Contains("Rebuild:");
        }
    }
}
