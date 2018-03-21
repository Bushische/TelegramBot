using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using TelegramBot.Commands;
using TelegramBot.Messages;

namespace TelegramBot
{
    /// <summary>
    /// Concrete bot
    /// </summary>
    class Bot
    {
        public List<string> BotCommands => commandDictionary.Keys.ToList();
        public string ApiKey { get; private set; }
        public event Action<string> OnLog;


        public Bot(string inApiKey, List<string> inCommandList)
        {
            ApiKey = inApiKey;

            userInCommandProcessing = new SortedDictionary<int, string>();

            //TODO: make read all commands from Asseblies
            //https://stackoverflow.com/questions/38443347/getting-all-types-that-implement-an-interface-in-net-core

            commandDictionary = new SortedDictionary<string, IBotCommand>();
            if (inCommandList.Contains("/echo"))
            {
                commandDictionary.Add("/echo", new EchoCommand());
            }
            if (inCommandList.Contains("/weather"))
            {
                commandDictionary.Add("/weather", new WeatherCommand());
            }
        }

        /// <summary>
        /// start listening and process messages
        /// </summary>
        public void StartListening()
        {
            // start listening and process messages
            tgBotWorker = new BackgroundWorker();
            tgBotWorker.DoWork += TgBotWorker_DoWork;
            tgBotWorker.WorkerSupportsCancellation = true;
            tgBotWorker.RunWorkerAsync();
        }

        /// <summary>
        /// stop listening
        /// </summary>
        public void StopListening()
        {
            tgBotWorker?.CancelAsync();
        }
        
        void Log(string message)
        {
            OnLog?.Invoke(message);
        }

        //------------
        BackgroundWorker tgBotWorker;

        /// <summary>
        /// Main loop of processing messages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TgBotWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var bgWorker = sender as BackgroundWorker;
                var bot = new Telegram.Bot.TelegramBotClient(this.ApiKey);
                // clear webhook field
                await bot.SetWebhookAsync("");
                int offset = 0;         //TODO: need read from file or database

                while (true)
                {
                    var updates = await bot.GetUpdatesAsync(offset, allowedUpdates: new UpdateType[] { UpdateType.MessageUpdate });
                    foreach (var update in updates)
                    {
                        var resProc = await ProcessTgUpdate(bot, update);
                        offset = update.Id + 1;
                    }

                    System.Threading.Thread.Sleep(1000);
                    if (bgWorker?.CancellationPending ?? false)
                        break;
                }
            }
            catch (Exception ex)
            {
                Log($"Exeption = {ex.ToString()}");
            }
        }


        /// <summary>
        /// Map of user which work in command
        /// </summary>
        SortedDictionary<int, string> userInCommandProcessing;
        /// <summary>
        /// Map of command and they implementation
        /// </summary>
        SortedDictionary<string, IBotCommand> commandDictionary;

        /// <summary>
        /// Process messages
        /// </summary>
        /// <param name="inUpd"></param>
        /// <returns></returns>
        private async Task<bool> ProcessTgUpdate(Telegram.Bot.TelegramBotClient bot, Update inUpd)
        {
            CommandResult comRes = null;
            Message message = inUpd.Message ?? inUpd.EditedMessage;
            if (message == null)
            {
                Log("$Ошибка обработка Update");
                return false;
            }

            if ((message?.Entities?.FirstOrDefault()?.Type == MessageEntityType.BotCommand)
                && (commandDictionary.ContainsKey( message?.Text)))
            {
                // change command
                comRes = commandDictionary[message?.Text].ProcessInitialMessage(inUpd);
                userInCommandProcessing[message.From.Id] = message.Text;    // change command for user
            }
            else if (userInCommandProcessing.ContainsKey(message.From.Id))
            {
                comRes = commandDictionary[userInCommandProcessing[message.From.Id]].ProcessMessage(inUpd);
            }

            if (comRes == null)
            {
                var tm = new TextResponseMessage(message, "Необходимо выбрать команду");
                await tm.ApplyToBot(bot);
            }
            else
            {
                if ((comRes.CommandCompleted) && (userInCommandProcessing.ContainsKey(message.From.Id)))
                {
                    userInCommandProcessing.Remove(message.From.Id);
                }
                await comRes.ResultMessage.ApplyToBot(bot);
            }
            return true;    
        }
    }//
}
