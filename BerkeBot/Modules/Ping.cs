using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerkeBot.Modules
{
  public class Ping : ModuleBase<SocketCommandContext>
  {
    [Command("whatsup")]

    public async Task PingAsync()
    {
      EmbedBuilder builder = new EmbedBuilder();

      //builder.AddField("Field1", "test")
      //  .AddInlineField("Field2", "test")
      //  .AddInlineField("Field3", "test");

      builder.WithTitle("Hello!")
        .WithDescription("I'm a peasant for my King")
        .WithColor(Color.Purple);

      // i.e
      // berke-bot || Str187 sent berke!whatsup in Die Übersicht
      //await ReplyAsync($"{Context.Client.CurrentUser.Mention} || {Context.User.Mention} sent {Context.Message.Content} in {Context.Guild.Name}");

      await ReplyAsync("", false, builder.Build());
      //await ReplyAsync("If you can't handle me at my worst you don't deserve me at my best!");
    }
  }
}
