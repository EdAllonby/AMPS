namespace Shared.Message.TaskMessage
{
    internal class TaskCommentRequest : IMessage
    {
        public MessageIdentifier MessageIdentifier { get; private set; }
    }
}