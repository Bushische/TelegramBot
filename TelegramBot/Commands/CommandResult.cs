using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Messages;

namespace TelegramBot.Commands
{
    /// <summary>
    /// Result of processing Update by command
    /// </summary>
    class CommandResult
    {
        /// <summary>
        /// Result of command process
        /// </summary>
        public ResponseMessage ResultMessage { get; private set; }
        /// <summary>
        /// Command process is completed. Next message will no pass to command
        /// </summary>
        public bool CommandCompleted { get; private set; }

        public CommandResult(ResponseMessage result, bool completed)
        {
            ResultMessage = result;
            CommandCompleted = completed;
        }
    }
}
