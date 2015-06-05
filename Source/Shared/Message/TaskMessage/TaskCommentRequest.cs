using System;
using Shared.Domain;

namespace Shared.Message.TaskMessage
{
    [Serializable]
    public class TaskCommentRequest : IMessage
    {
        public TaskCommentRequest(TaskComment taskComment)
        {
            TaskComment = taskComment;
        }

        public TaskComment TaskComment { get; private set; }

        public MessageIdentifier MessageIdentifier
        {
            get { return MessageIdentifier.TaskCommentRequest; }
        }
    }
}