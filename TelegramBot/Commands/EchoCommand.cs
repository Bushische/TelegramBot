using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.Messages;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Just reply with original message
    /// </summary>
    class EchoCommand : IBotCommand
    {
        public string CommandName => "/echo";

        public CommandResult ProcessInitialMessage(Update inUpdate)
        {
            var message = new TextResponseMessage(inUpdate.Message ?? inUpdate.EditedMessage, "Привет! Пиши всё, что хочешь!");
            return new CommandResult(message, false);
        }

        public CommandResult ProcessMessage(Update inUpdate)
        {
            ResponseMessage resMes = null;
            if (inUpdate.Type == Telegram.Bot.Types.Enums.UpdateType.EditedMessage)
                resMes = new QuoteTextResponseMessage(inUpdate.EditedMessage, "Не хорошо править старые сообщения");
            else if (inUpdate.Type == Telegram.Bot.Types.Enums.UpdateType.MessageUpdate)
            {
                Message message = inUpdate.Message ?? inUpdate.EditedMessage;
                if ((message != null) && (!string.IsNullOrEmpty( message?.Text)))
                    resMes = new TextResponseMessage(message, $"Эхо: {message.Text?.Substring(0, Math.Min(255, message.Text?.Length ?? 0))}");
                else
                    resMes = new TextResponseMessage(message, "Ничего не понимаю");
            }
            return new CommandResult(resMes, false);
        }
    }
}
