using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Common interface for BotCommand
    /// </summary>
    interface IBotCommand
    {
        /// <summary>
        /// Command string '/command'
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// Process initial message (get COMMAND help info)
        /// </summary>
        /// <param name="inUpdate">Original message</param>
        /// <returns></returns>
        CommandResult ProcessInitialMessage(Update inUpdate);

        /// <summary>
        /// Process message
        /// </summary>
        /// <param name="inUpdate">Original message</param>
        /// <returns></returns>
        CommandResult ProcessMessage(Update inUpdate);
    }
}
