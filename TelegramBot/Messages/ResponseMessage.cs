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
    /// Common interface for all response messages
    /// </summary>
    abstract class ResponseMessage
    {
        /// <summary>
        /// ChatID
        /// </summary>
        protected long ChatId { get; set; }
        /// <summary>
        /// ID of message for which will be reply (default 0 => not reply)
        /// </summary>
        protected int ReplyMessageID { get; set; }

        public ResponseMessage (Message origMes)
        {
            ChatId = origMes.Chat.Id;
            ReplyMessageID = 0;// origMes.MessageId;
        }

        /// <summary>
        /// apply message to Bot
        /// </summary>
        /// <param name="bot"></param>
        public virtual async Task<bool> ApplyToBot(TelegramBotClient bot)
        {
            return true;
        }
    }//
}
