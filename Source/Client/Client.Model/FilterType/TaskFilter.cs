using Shared.Domain;

namespace Client.Model.FilterType
{
    /// <summary>
    /// Filter <see cref="Task" /> entities by certain completion statuses.
    /// </summary>
    public enum TaskFilter
    {
        Completed,
        Uncompleted,
        All
    }
}