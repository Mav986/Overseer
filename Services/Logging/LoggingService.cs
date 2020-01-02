﻿using System;
using System.IO;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Overseer.Services.Logging
{
    public class LoggingService : ILogger
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string LogDirectory = "Logs";
        private const int SourcePadLength = 15;

        public LoggingService(DiscordSocketClient client, CommandService commands)
        {
            client.Log += Log;
            commands.Log += Log;

            Initialize();
        }

        public async Task Log(LogSeverity severity, string message, string method, string caller)
        {
            var newMessage = $"{method} -> {message}";
            await Log(new LogMessage(severity, caller, newMessage));
        }

        private void Initialize()
        {
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
        }

        private Task Log(LogMessage logMessage)
        {
            var filename = $"{LogDirectory}/{DateTime.Now.ToString(DateFormat)}.log";
            var text = logMessage.ToString(padSource: SourcePadLength);
            using var writer = File.AppendText(filename);

            writer.WriteLine(text);
            Console.WriteLine(text);

            return Task.CompletedTask;
        }
    }
}
