using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot.Messages
{
    /// <summary>
    /// Simple text response message (only text)
    /// </summary>
    class TextResponseMessage : ResponseMessage
    {
        protected string ResponseMessage;
        public TextResponseMessage(Message origMes, string inResponse = "") : base(origMes)
        {
            ResponseMessage = inResponse;
        }

        public override async Task<bool> ApplyToBot(TelegramBotClient bot)
        {
            await bot.SendTextMessageAsync(ChatId, ResponseMessage, replyToMessageId: ReplyMessageID);
            return true;
        }
    }
}
