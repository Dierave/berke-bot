using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BerkeBot
{
  class Program
  {
    static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

    private DiscordSocketClient _client;
    private CommandService _commands;
    private IServiceProvider _services;

    public async Task RunBotAsync()
    {
      _client = new DiscordSocketClient();
      _commands = new CommandService();

      _services = new ServiceCollection()
        .AddSingleton(_client)
        .AddSingleton(_commands)
        .BuildServiceProvider();

      string botToken = "CONTACT_ME";

      //event subscriptions
      _client.Log += Log;
      _client.GuildMemberUpdated += AnnounceUpdatedUser;

      await RegisterCommandsAsync();

      await _client.LoginAsync(TokenType.Bot, botToken);

      await _client.StartAsync();

      await Task.Delay(-1);
    }

    public async Task AnnounceUpdatedUser(SocketUser s, SocketUser e) 
    {
    // general text chat. 
    // TODO: find a better way to get the general text channel
      var channel = _client.GetChannel(245283083832524800) as SocketTextChannel;
      
      if (e.Username == "Pyros" && e.Status == UserStatus.Online)
      {
        await channel.SendMessageAsync($"Merhaba gencler, ben bi dus alicam, 15 dakkaya gelirim");
      }
    }

    private Task Log(LogMessage arg)
    {
      Console.WriteLine(arg);

      return Task.CompletedTask;
    }

    public async Task RegisterCommandsAsync()
    {
      _client.MessageReceived += HandleCommandAsync;

      await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
      var message = arg as SocketUserMessage;

      if (message is null || message.Author.IsBot /*|| message.Author.Username.ToString() == "dierave"*/) return;

      int argPos = 0;

      if (message.HasStringPrefix("berke!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
      {
        var context = new SocketCommandContext(_client, message);

        var result = await _commands.ExecuteAsync(context, argPos, _services);

        if (!result.IsSuccess)
        {
          Console.WriteLine(result.ErrorReason);
        }
      }
    }
  }
}
