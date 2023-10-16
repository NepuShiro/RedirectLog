using BepInEx;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using BCE;
using Elements.Core;

namespace RedirectLog
{
    [BepInPlugin("org.nep.RedirectLog", "RedirectLog", "1.0.0")]
    public class RedirectLog : BaseUnityPlugin
    {

        void Awake()
        {
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
            string pattern1 = @"\d{1,2}:\d{1,2}:\d{1,2} [APap][Mm]\.\d{1,3} \(\s*-*\d+\s?FPS\s?\)\s+";

            string pattern2 = @"\d{1,2}:\d{1,2}:\d{1,2} [APap][Mm]\.\d{1,3} \(\s*-*\d+\s?FPS\s?\)\s+";

            string result = Regex.Replace(message, pattern1, "");
            result = Regex.Replace(result, pattern2, "");

            return result;
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
