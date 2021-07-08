using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BOT_HornyGame.SettingBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _Client;
        private readonly CommandService _Commands;
        private readonly IServiceProvider _Services;

        public CommandHandler(IServiceProvider services)
        {
            _Commands = services.GetRequiredService<CommandService>();
            _Client = services.GetRequiredService<DiscordSocketClient>();
            _Services = services;

            // Event handlers
            _Client.Ready += ClientReadyAsync;
            _Client.MessageReceived += HandleCommandAsync;
            _Client.JoinedGuild += SendJoinMessageAsync;
        }

        private async Task HandleCommandAsync(SocketMessage rawMessage)
        {
            if (rawMessage.Author.IsBot || !(rawMessage is SocketUserMessage message) || message.Channel is IDMChannel)
                return;

            var context = new SocketCommandContext(_Client, message);

            int argPos = 0;

            //JObject config = Functions.GetConfig();
            //string[] prefixes = JsonConvert.DeserializeObject<string[]>(config["prefixes"].ToString());
            string[] prefixes = { "!" };

            // Check if message has any of the prefixes or mentiones the bot.
            if (prefixes.Any(x => message.HasStringPrefix(x, ref argPos)) || message.HasMentionPrefix(_Client.CurrentUser, ref argPos))
            {
                // Execute the command.
                var result = await _Commands.ExecuteAsync(context, argPos, _Services);

                if (!result.IsSuccess && result.Error.HasValue)
                    await context.Channel.SendMessageAsync($":x: {result.ErrorReason}");
            }
        }

        private async Task SendJoinMessageAsync(SocketGuild guild)
        {
            //JObject config = Functions.GetConfig();
            //string joinMessage = config["join_message"]?.Value<string>();
            string joinMessage = "hi!";

            if (string.IsNullOrEmpty(joinMessage))
                return;

            // Send the join message in the first channel where the bot can send messsages.
            foreach (var channel in guild.TextChannels.OrderBy(x => x.Position))
            {
                var botPerms = channel.GetPermissionOverwrite(_Client.CurrentUser).GetValueOrDefault();

                if (botPerms.SendMessages == PermValue.Deny)
                    continue;

                try
                {
                    await channel.SendMessageAsync(joinMessage);
                    return;
                }
                catch
                {
                    continue;
                }
            }
        }

        private async Task ClientReadyAsync()
         => await Functions.SetBotStatusAsync(_Client);

        public async Task InitializeAsync()
            => await _Commands.AddModulesAsync(Assembly.GetEntryAssembly(), _Services);
    }
}
