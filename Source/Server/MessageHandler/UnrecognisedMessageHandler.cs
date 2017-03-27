using log4net;
using Shared;
using Shared.Message;

namespace Server.MessageHandler
{
    internal class UnrecognisedMessageHandler : IMessageHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UnrecognisedMessageHandler));

        public void HandleMessage(IMessage message)
        {
            Log.Info("Handled unrecognised message");
        }
    }
}