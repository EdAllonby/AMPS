using Shared.Domain;

namespace Shared.Message.TaskMessage
{
    class TaskCommentRequest: IMessage
    {
        public MessageIdentifier MessageIdentifier { get; private set; }
    }
}
