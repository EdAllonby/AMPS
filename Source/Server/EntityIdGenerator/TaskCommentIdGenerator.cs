using Shared;

namespace Server.EntityIdGenerator
{
    internal class TaskCommentIdGenerator : EntityIdGenerator
    {
        public TaskCommentIdGenerator(IGeneratorStrategy generatorStrategy)
        {
            CurrentId = generatorStrategy.GetCurrentHighestId("TaskComments");
        }
    }
}