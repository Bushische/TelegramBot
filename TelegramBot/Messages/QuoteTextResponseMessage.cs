using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot.Messages
{
    /// <summary>
    /// version of message which will send respond with quotation of original message
    /// </summary>
    class QuoteTextResponseMessage : TextResponseMessage
    {
        public QuoteTextResponseMessage(Message origMes, string inResponse) : base(origMes, inResponse)
        {
            ReplyMessageID = origMes.MessageId;
        }

    }
}
