using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace AstolfoBot
{
    public sealed class Main
    {
        static Main()
        {
            var ClientConfig = new DiscordSocketConfig
            {
#if DEBUG
                LogLevel = LogSeverity.Debug,
#else
                LogLevel = LogSeverity.Info,
#endif
                MessageCacheSize = 1000,
                AlwaysDownloadUsers = true,
                GatewayIntents = GatewayIntents.All
            };
            Client = new DiscordSocketClient(ClientConfig);
        }
        public readonly static DiscordSocketClient Client;
        // public DiscordSocketConfig? ClientConfig;
        private string Token = "TOKEN";
        public async Task AsyncMain()
        {
#if DEBUG
            Logger.LogLevel = Logger.LogSeverity.Debug;
#else
            Logger.LogLevel = Logger.LogSeverity.Info;
#endif
            Client.Log += Logger.Log;

            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");
            if (File.Exists("Logs/latest.log"))
            {
                File.Move("Logs/latest.log", $"Logs/{File.GetLastWriteTime("Logs/latest.log"):yyyy-MM-dd_HH-mm-ss}.log");
            }
            File.Create("Logs/latest.log").Close();

            if (File.Exists("Token.txt"))
            {
                Token = File.ReadAllText("Token.txt");
            }
            else
            {
                Logger.Critical("Token.txt not found", this);
                return;
            }

            Client.Ready += () =>
            {
                Config.ConfigManager.LoadFromFile();
                Logger.Debug("Client ready", this);
                return Task.CompletedTask;
            };

            var commandHandler = new CommandHandler(Client);
            await commandHandler.InstallCommandsAsync();

            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            await Client.SetStatusAsync(UserStatus.Online);
            await Client.SetGameAsync("with 1's and 0's");

            while (true)
            {
                string? input = Console.ReadLine();
                switch (input)
                {
                    case "exit":
                        await StopAsync();
                        Logger.Debug("-----END-----", this);
                        return;
                }
            }
            // await Task.Delay(-1);
        }
        public static async Task StopAsync()
        {
            Logger.Debug("Stopping...", "Main");
            await Client.SetStatusAsync(UserStatus.Invisible);
            await Client!.StopAsync();
            await Client.LogoutAsync();
            Config.ConfigManager.SaveToFile();
            Logger.Debug("Saved Configs", "Main");
        }
    }
}