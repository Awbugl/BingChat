﻿using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace BingChat.Cli.Commands;

public sealed class AskCommand : AsyncCommand<AskCommand.Settings>
{
    public class Settings : ChatCommandSettings
    {
        [CommandArgument(0, "[message]")]
        [Description("The message to ask")]
        public required string[] Message { get; init; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        var client = Utils.GetClient();
        var message = string.Join(' ', settings.Message);
        var answer = string.Empty;

        Utils.WriteMessage(message, settings);
        
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.BouncingBar)
            .StartAsync("Bing is thinking...", async _ => { answer = await client.AskAsync(message); });

        Utils.WriteAnswer(answer, settings);

        return 0;
    }
}